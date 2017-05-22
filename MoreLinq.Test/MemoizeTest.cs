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

using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using System.Threading;

namespace MoreLinq.Test
{
    [TestFixture]
    public class MemoizeTest
    {
        [Test]
        public void MemoizeWithNullSequence()
        {
            Assert.Throws<ArgumentNullException>(
                () => MoreEnumerable.Memoize<int>(null));
        }

        [Test]
        public void MemoizeReturningExpectedElementsWhenUsedAtInnerForeach()
        {
            var array = Enumerable.Range(1, 10).ToArray();
            var buffer = Enumerable.Range(1, 10).Memoize();

            var flowArray = InnerForeach(array);
            var flowBuffer = InnerForeach(buffer);

            flowArray.AssertSequenceEqual(flowBuffer);
        }

        public List<string> InnerForeach(IEnumerable<int> source)
        {
            var flow = new List<string>();

            var firstVisitAtInnerLoopDone = false;

            //add 1-3 to cache (so enter inner loop)
            //consume 4-5 already cached
            //add 6-7 to cache (so enter inner loop)
            //consume 8-10 already cached

            flow.Add("enter outer loop");
            foreach (var i in source)
            {
                flow.Add(i.ToString());

                if (i == 3 || i == 7)
                {
                    //consume 1-3 already cached
                    //add 4-5 to cache (so go to outer loop)
                    //consume 1-7 already cached
                    //add 8-10 to cache (so go to outer loop)

                    flow.Add("enter inner loop");
                    foreach (var j in source)
                    {
                        flow.Add(j.ToString());

                        if (!firstVisitAtInnerLoopDone && j == 5)
                        {
                            firstVisitAtInnerLoopDone = true;
                            break;
                        }
                    }
                    flow.Add("exit inner loop");
                }
            }
            flow.Add("exit outer loop");

            flow.Add("enter last loop");
            //consume 1-10 (all item were already cached)
            foreach (var k in source)
            {
                flow.Add(k.ToString());
            }
            flow.Add("exit last loop");

            return flow;
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

        [Test]
        public void MemoizeWithInMemoryCollection()
        {
            var list = Enumerable.Range(1, 10).ToList();
            Assert.That(list.Memoize(), Is.SameAs(list));
        }

        [Test]
        public void MemoizeEnumeratesOnlyOnce()
        {
            var memoized = Enumerable.Range(1, 10).AsTestingSequence().Memoize();

            Assert.IsTrue(memoized.ToList().Count == 10);
            Assert.IsTrue(memoized.ToList().Count == 10);
        }

        [Test]
        public void MemoizeDoesNotDisposeOnEarlyExitByDefault()
        {
            Assert.Throws<AssertionException>(() =>
            {
                using (var xs = new[] { 1, 2 }.AsTestingSequence())
                {
                    xs.Memoize().Take(1).Consume();
                    xs.Memoize().Take(1).Consume();
                }
            });
        }

        [Test]
        public void MemoizeWithDisposeOnEarlyExitTrue()
        {
            using (var xs = new[] { 1, 2 }.AsTestingSequence())
            {
                var memoized = xs.Memoize();
                using ((IDisposable) memoized)
                    memoized.Take(1).Consume();
            }
        }

        [Test]
        public void MemoizeDisposesAfterSourceIsIteratedEntirely()
        {
            using (var xs = new[] { 1, 2 }.AsTestingSequence())
            {
                var memoized = xs.Memoize();
                using ((IDisposable) memoized)
                    memoized.Consume();
            }
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

            Assert.That(sequence, Is.EquivalentTo(memoized));
            lists.ForEach(list => Assert.That(list, Is.EquivalentTo(memoized)));
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

            using (var reader = memoized.Read())
            {
                Assert.That(reader.Read(), Is.EqualTo(1));

                disposable.Dispose();

                var e = Assert.Throws<ObjectDisposedException>(() => reader.Read());
                Assert.That(e.ObjectName, Is.EqualTo("MemoizedEnumerable"));
            }
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

            var disposed = false;
            var xs = TestSequence().AsVerifiable().WhenDisposed(_ => disposed = true);
            var memoized = xs.Memoize();
            using ((IDisposable) memoized)
            using (var r1 = memoized.Read())
            using (var r2 = memoized.Read())
            {
                Assert.That(r1.Read(), Is.EqualTo(r2.Read()));
                var e1 = Assert.Throws<Exception>(() => r1.Read());
                Assert.That(e1, Is.SameAs(error));

                Assert.That(disposed, Is.True);

                var e2 = Assert.Throws<Exception>(() => r2.Read());
                Assert.That(e2, Is.SameAs(error));
            }
            using (var r1 = memoized.Read())
                Assert.That(r1.Read(), Is.EqualTo(123));
        }
    }
}
