#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Atif Aziz. All rights reserved.
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

#if !NO_ASYNC

namespace MoreLinq.Test
{
    using System.Threading.Tasks;
    using Experimental;
    using NUnit.Framework;

    [TestFixture, Explicit]
    public class AwaitTest
    {
        [Test]
        public void Unordered()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            var tasks =
                from tcs in new[] { tcs1, tcs2, tcs3 }
                select tcs.Task;

            using (var results = tasks.AsTestingSequence())
            using (var reader = results.Await().Read())
            {
                const int a = 123, b = 456, c = 789;

                tcs2.SetResult(b); Assert.That(reader.Read(), Is.EqualTo(b));
                tcs1.SetResult(a); Assert.That(reader.Read(), Is.EqualTo(a));
                tcs3.SetResult(c); Assert.That(reader.Read(), Is.EqualTo(c));

                reader.ReadEnd();
            }
        }

        [Test]
        public void UnorderedWithErroneousTask()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            const int a = 123, b = 456;

            var tasks =
                from tcs in new[] { tcs1, tcs2, tcs3 }
                select tcs.Task;

            using (var results = tasks.AsTestingSequence())
            using (var reader = results.Await().Read())
            {
                tcs2.SetResult(b); Assert.That(reader.Read(), Is.EqualTo(b));
                tcs1.SetResult(a); Assert.That(reader.Read(), Is.EqualTo(a));

                var te = new TestException();
                tcs3.SetException(te);
                var ex = Assert.Throws<TestException>(() => reader.Read());
                Assert.That(ex, Is.SameAs(te));
            }
        }

        [Test]
        public void Ordered()
        {
            var xs = Enumerable.Range(1, 10);

            var tcss = xs.Select(_ => new TaskCompletionSource<int>())
                         .ToArray();

            using (var tasks = tcss.Select(tcs => tcs.Task)
                                   .AsTestingSequence())
            {
                var results = tasks.Await().AsOrdered();

                tcss.Index(xs.First())
                    .Reverse()
                    .ForEach(e => e.Value.SetResult(e.Key));

                results.ToArray().AssertSequenceEqual(xs);
            }
        }

        [Test]
        public void OrderedWithErroneousTask()
        {
            var xs = Enumerable.Range(1, 10);

            var tcss = xs.Select(_ => new TaskCompletionSource<int>())
                         .ToArray();

            using (var tasks = tcss.Select(tcs => tcs.Task)
                                   .AsTestingSequence())
            {
                var results = tasks.Await().AsOrdered();

                var te = new TestException();
                foreach (var e in tcss.Index(xs.First())
                                      .Reverse()
                                      .TagFirstLast((e, fst, _) => new
                                      {
                                          Result = e.Key,
                                          TaskCompletionSource = e.Value,
                                          Error = fst ? te : null,
                                      }))
                {
                    if (e.Error != null)
                        e.TaskCompletionSource.SetException(te);
                    else
                        e.TaskCompletionSource.SetResult(e.Result);
                }

                using (var reader = results.Read())
                {
                    foreach (var x in xs.SkipLast(1))
                        Assert.That(reader.Read(), Is.EqualTo(x));

                    var ex = Assert.Throws<TestException>(() => reader.Read());
                    Assert.That(ex, Is.SameAs(te));
                }
            }
        }
    }
}

#endif // !NO_ASYNC
