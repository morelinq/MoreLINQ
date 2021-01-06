#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Leandro F. Vieira (leandromoh). All rights reserved.
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

namespace MoreLinq.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using Delegate = Delegating.Delegate;
    using NUnit.Framework;
    using Experimental;

    [TestFixture]
    public class MemoizeTest
    {
        [Test]
        public void MemoizeReturningExpectedElementsWhenUsedAtInnerForEach()
        {
            var array = Enumerable.Range(1, 10).ToArray();
            var buffer = Enumerable.Range(1, 10).Memoize();

            var flowArray = InnerForEach(array);
            var flowBuffer = InnerForEach(buffer);

            flowArray.AssertSequenceEqual(flowBuffer);

            IEnumerable<object> InnerForEach(IEnumerable<int> source)
            {
                var firstVisitAtInnerLoopDone = false;

                //add 1-3 to cache (so enter inner loop)
                //consume 4-5 already cached
                //add 6-7 to cache (so enter inner loop)
                //consume 8-10 already cached

                yield return "enter outer loop";
                foreach (var i in source)
                {
                    yield return i;

                    if (i == 3 || i == 7)
                    {
                        //consume 1-3 already cached
                        //add 4-5 to cache (so go to outer loop)
                        //consume 1-7 already cached
                        //add 8-10 to cache (so go to outer loop)

                        yield return "enter inner loop";
                        foreach (var j in source)
                        {
                            yield return j;

                            if (!firstVisitAtInnerLoopDone && j == 5)
                            {
                                firstVisitAtInnerLoopDone = true;
                                break;
                            }
                        }
                        yield return "exit inner loop";
                    }
                }
                yield return "exit outer loop";

                yield return "enter last loop";
                //consume 1-10 (all item were already cached)
                foreach (var k in source)
                {
                    yield return k;
                }
                yield return "exit last loop";
            }
        }

        [Test]
        public void MemoizeWithPartialIterationBeforeCompleteIteration()
        {
            var buffer = Enumerable.Range(0, 10).Memoize();

            buffer.Take(5).AssertSequenceEqual(Enumerable.Range(0, 5));
            buffer.AssertSequenceEqual(Enumerable.Range(0, 10));
        }

        [Test]
        public void MemoizeIsLazy()
        {
            new BreakingSequence<int>().Memoize();
        }

        [TestCase(SourceKind.BreakingCollection)]
        [TestCase(SourceKind.BreakingReadOnlyCollection)]
        public void MemoizeWithInMemoryCollection(SourceKind sourceKind)
        {
            var collection = Enumerable.Empty<int>().ToSourceKind(sourceKind);
            Assert.That(collection.Memoize(), Is.SameAs(collection));
        }

        [Test]
        public void MemoizeEnumeratesOnlyOnce()
        {
            const int count = 10;
            using var ts = Enumerable.Range(1, count).AsTestingSequence();

            var memoized = ts.Memoize();

            using ((IDisposable)memoized)
            {
                Assert.That(memoized.ToList().Count, Is.EqualTo(count));
                Assert.That(memoized.ToList().Count, Is.EqualTo(count));
            }
        }

        [Test]
        public void MemoizeDoesNotDisposeOnEarlyExitByDefault()
        {
            Assert.Throws<AssertionException>(() =>
            {
                using var xs = new[] { 1, 2 }.AsTestingSequence();

                xs.Memoize().Take(1).Consume();
                xs.Memoize().Take(1).Consume();
            });
        }

        [Test]
        public void MemoizeWithDisposeOnEarlyExitTrue()
        {
            using var xs = new[] { 1, 2 }.AsTestingSequence();

            var memoized = xs.Memoize();

            using ((IDisposable) memoized)
                memoized.Take(1).Consume();
        }

        [Test]
        public void MemoizeDisposesAfterSourceIsIteratedEntirely()
        {
            using var xs = new[] { 1, 2 }.AsTestingSequence();
            xs.Memoize().Consume();
        }

        [Test, Explicit]
        public static void MemoizeIsThreadSafe()
        {
            var sequence = Enumerable.Range(1, 50000);
            var memoized = sequence.AsTestingSequence().Memoize();

            var lists = Enumerable.Range(0, Environment.ProcessorCount * 2)
                                  .Select(_ => new List<int>())
                                  .ToArray();

            var start = new Barrier(lists.Length);

            var threads =
                from list in lists
                select new Thread(() =>
                {
                    start.SignalAndWait();
                    list.AddRange(memoized);
                });

            threads.Pipe(t => t.Start())
                   .ToArray() // start all before joining
                   .ForEach(t => t.Join());

            Assert.That(sequence, Is.EqualTo(memoized));
            lists.ForEach(list => Assert.That(list, Is.EqualTo(memoized)));
        }

        [Test]
        public static void MemoizeRestartsAfterDisposal()
        {
            var starts = 0;

            IEnumerable<int> TestSequence()
            {
                starts++;
                yield return 1;
                yield return 2;
            }

            var memoized = TestSequence().Memoize();

            void Run()
            {
                using ((IDisposable) memoized)
                    memoized.Take(1).Consume();
            }

            Run();
            Assert.That(starts, Is.EqualTo(1));
            Run();
            Assert.That(starts, Is.EqualTo(2));
        }

        [Test]
        public static void MemoizeIteratorThrowsWhenCacheDisposedDuringIteration()
        {
            var sequence = Enumerable.Range(1, 10);
            var memoized = sequence.Memoize();
            var disposable = (IDisposable) memoized;

            using var reader = memoized.Read();
            Assert.That(reader.Read(), Is.EqualTo(1));

            disposable.Dispose();

            var e = Assert.Throws<ObjectDisposedException>(() => reader.Read());
            Assert.That(e.ObjectName, Is.EqualTo("MemoizedEnumerable"));
        }

        [Test]
        public void MemoizeWithMemoizedSourceReturnsSame()
        {
            var memoized = Enumerable.Range(0, 9).Memoize();
            var memoizedAgain = memoized.Memoize();

            Assert.That(memoized, Is.SameAs(memoizedAgain));
        }

        [Test]
        public void MemoizeRethrowsErrorDuringIterationToAllIteratorsUntilDisposed()
        {
            var error = new Exception("This is a test exception.");

            IEnumerable<int> TestSequence()
            {
                yield return 123;
                throw error;
            }

            var xs = new DisposalTrackingSequence<int>(TestSequence());
            var memoized = xs.Memoize();
            using ((IDisposable) memoized)
            using (var r1 = memoized.Read())
            using (var r2 = memoized.Read())
            {
                Assert.That(r1.Read(), Is.EqualTo(r2.Read()));
                var e1 = Assert.Throws<Exception>(() => r1.Read());
                Assert.That(e1, Is.SameAs(error));

                Assert.That(xs.IsDisposed, Is.True);

                var e2 = Assert.Throws<Exception>(() => r2.Read());
                Assert.That(e2, Is.SameAs(error));
            }
            using (var r1 = memoized.Read())
                Assert.That(r1.Read(), Is.EqualTo(123));
        }

        [Test]
        public void MemoizeRethrowsErrorDuringIterationStartToAllIteratorsUntilDisposed()
        {
            var error = new Exception("This is a test exception.");

            var i = 0;
            IEnumerable<int> TestSequence()
            {
                if (0 == i++) // throw at start for first iteration only
                    throw error;
                yield return 42;
            }

            var xs = new DisposalTrackingSequence<int>(TestSequence());
            var memoized = xs.Memoize();
            using ((IDisposable) memoized)
            using (var r1 = memoized.Read())
            using (var r2 = memoized.Read())
            {
                var e1 = Assert.Throws<Exception>(() => r1.Read());
                Assert.That(e1, Is.SameAs(error));

                Assert.That(xs.IsDisposed, Is.True);

                var e2 = Assert.Throws<Exception>(() => r2.Read());
                Assert.That(e2, Is.SameAs(error));
            }

            using (var r1 = memoized.Read())
            using (var r2 = memoized.Read())
                Assert.That(r1.Read(), Is.EqualTo(r2.Read()));
        }

        [Test]
        public void MemoizeRethrowsErrorDuringFirstIterationStartToAllIterationsUntilDisposed()
        {
            var error = new Exception("An error on the first call!");
            var obj = new object();
            var calls = 0;
            var source = Delegate.Enumerable(() => 0 == calls++
                                                 ? throw error
                                                 : Enumerable.Repeat(obj, 1).GetEnumerator());

            var memo = source.Memoize();

            for (var i = 0; i < 2; i++)
            {
                var e = Assert.Throws<Exception>(() => memo.First());
                Assert.That(e, Is.SameAs(error));
            }

            ((IDisposable) memo).Dispose();
            Assert.That(memo.Single(), Is.EqualTo(obj));
        }

        // TODO Consolidate with MoreLinq.Test.TestingSequence<T>?

        sealed class DisposalTrackingSequence<T> : IEnumerable<T>, IDisposable
        {
            readonly IEnumerable<T> _sequence;

            internal DisposalTrackingSequence(IEnumerable<T> sequence) =>
                _sequence = sequence;

            public bool? IsDisposed { get; private set; }

            void IDisposable.Dispose() => IsDisposed = null;

            public IEnumerator<T> GetEnumerator()
            {
                var enumerator = new DisposeTestingSequenceEnumerator(_sequence.GetEnumerator());
                IsDisposed = false;
                enumerator.Disposed += delegate { IsDisposed = true; };
                return enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            sealed class DisposeTestingSequenceEnumerator : IEnumerator<T>
            {
                readonly IEnumerator<T> _sequence;

                public event EventHandler Disposed;

                public DisposeTestingSequenceEnumerator(IEnumerator<T> sequence) =>
                    _sequence = sequence;

                public T Current => _sequence.Current;
                object IEnumerator.Current => Current;
                public void Reset() => _sequence.Reset();
                public bool MoveNext() => _sequence.MoveNext();

                public void Dispose()
                {
                    _sequence.Dispose();
                    Disposed?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
