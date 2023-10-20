#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2020 Atif Aziz. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace MoreLinq.Test.Async
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Experimental.Async;
    using NUnit.Framework;
    using Throws = Throws;

    [TestFixture]
    public class MergeTest
    {
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-3)]
        public void InvalidMaxConcurrent(int n)
        {
            var sources = new IAsyncEnumerable<object>[0];
            void Act() => sources.Merge(n);
            Assert.That(Act, Throws.ArgumentOutOfRangeException("maxConcurrent"));
        }

        [Test]
        public async Task MergeSync()
        {
            using var ts1 = AsyncEnumerable.Range(1, 1).AsTestingSequence();
            using var ts2 = AsyncEnumerable.Range(1, 2).AsTestingSequence();
            using var ts3 = AsyncEnumerable.Range(1, 3).AsTestingSequence();
            using var ts4 = AsyncEnumerable.Range(1, 4).AsTestingSequence();
            using var ts5 = AsyncEnumerable.Range(1, 5).AsTestingSequence();

            using var ts = TestingSequence.Of(ts1, ts2, ts3, ts4, ts5);
            var result = await ts.Merge().ToListAsync();

            Assert.That(result, Is.EqualTo(new[]
            {
                1,
                1, 2,
                1, 2, 3,
                1, 2, 3, 4,
                1, 2, 3, 4, 5,
            }));
        }

        [Test]
        public async Task MergeAsyncAll()
        {
            using var ts1 = AsyncEnumerable.Range(10, 1).Yield().AsTestingSequence();
            using var ts2 = AsyncEnumerable.Range(20, 2).Yield().AsTestingSequence();
            using var ts3 = AsyncEnumerable.Range(30, 3).Yield().AsTestingSequence();
            using var ts4 = AsyncEnumerable.Range(40, 4).Yield().AsTestingSequence();
            using var ts5 = AsyncEnumerable.Range(50, 5).Yield().AsTestingSequence();

            using var ts = TestingSequence.Of(ts1, ts2, ts3, ts4, ts5);
            var result = await ts.Merge().ToListAsync();

            Assert.That(result, Is.EquivalentTo(new[]
            {
                10,
                20, 21,
                30, 31, 32,
                40, 41, 42, 43,
                50, 51, 52, 53, 54,
            }));
        }

        sealed class AsyncControl<T>
        {
            sealed record State(TaskCompletionSource<T> TaskCompletionSource, T Result);

            State? _state;

            public Task<T> Result(T result)
            {
                if (_state is not null)
                    throw new InvalidOperationException();
                _state = new State(new TaskCompletionSource<T>(), result);
                return _state.TaskCompletionSource.Task;
            }

            public void Complete()
            {
                if (_state is not { } state)
                    throw new InvalidOperationException();
                _state = null;
                state.TaskCompletionSource.SetResult(state.Result);
            }
        }

        [Test]
        public async Task MergeAsyncSome()
        {
            var ac1 = new AsyncControl<int>();
            var ac2 = new AsyncControl<int>();

            async IAsyncEnumerable<int> Source1()
            {
                yield return await ac1.Result(1);
                yield return 2;
                yield return 3;
                _ = await ac1.Result(0);
            }

            async IAsyncEnumerable<int> Source2()
            {
                yield return await ac2.Result(4);
            }

            using var ts1 = Source1().AsTestingSequence();
            using var ts2 = Source2().AsTestingSequence();
            using var sources = TestingSequence.Of(ts1, ts2);
            var e = sources.Merge().GetAsyncEnumerator();

            async Task<int> ExpectAsync(AsyncControl<int> control)
            {
                var t = e.MoveNextAsync();
                Assert.That(t.IsCompleted, Is.False);
                control.Complete();
                Assert.That(await t, Is.True);
                return e.Current;
            }

            async Task<int> ExpectSync()
            {
                var t = e.MoveNextAsync();
                Assert.That(t.IsCompleted, Is.True);
                Assert.That(await t, Is.True);
                return e.Current;
            }

            Assert.That(await ExpectAsync(ac2), Is.EqualTo(4));
            Assert.That(await ExpectAsync(ac1), Is.EqualTo(1));
            Assert.That(ts2.IsDisposed, Is.True);
            Assert.That(await ExpectSync(), Is.EqualTo(2));
            Assert.That(await ExpectSync(), Is.EqualTo(3));

            var t = e.MoveNextAsync();
            Assert.That(t.IsCompleted, Is.False);
            ac1.Complete();
            Assert.That(await t, Is.False);
        }
    }
}
