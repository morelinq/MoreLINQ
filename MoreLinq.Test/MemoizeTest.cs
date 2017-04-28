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
using System.Threading.Tasks;

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

            Assert.IsInstanceOf<List<int>>(list.Memoize());
            Assert.IsInstanceOf<List<int>>(list.Memoize(false, false));
        }

        [Test]
        public void MemoizeWithForcedBuffering()
        {
            var list = Enumerable.Range(1, 10).ToList();

            Assert.IsNotInstanceOf<List<int>>(list.Memoize(true, false));
        }

        [Test]
        public void MemoizeEnumeratesOnlyOnce()
        {
            var memoized = Enumerable.Range(1, 10).AsTestingSequence().Memoize();

            Assert.IsTrue(memoized.ToList().Count == 10);
            Assert.IsTrue(memoized.ToList().Count == 10);
        }

        [Test]
        public void MemoizeDoesNotDisponseOnEarlyExitByDefault()
        {
            Assert.Throws<AssertionException>(() =>
            {
                using (var xs = new[] { 1, 2 }.AsTestingSequence())
                {
                    xs.Memoize().Take(1).Consume();
                    xs.Memoize(false, false).Take(1).Consume();
                }
            });
        }

        [Test]
        public void MemoizeWithDisponseOnEarlyExitTrue()
        {
            using (var xs = new[] { 1, 2 }.AsTestingSequence())
                xs.Memoize(false, true).Take(1).Consume();
        }

        [Test]
        public void MemoizeDisponsesAfterSourceIsIteratedEntirely()
        {
            using (var xs = new[] { 1, 2 }.AsTestingSequence())
                xs.Memoize().Consume();
        }

        [Test]
        public static void MemoizeIsThreadSafe()
        {
            var sequence = Enumerable.Range(1, 50000);
            var memoized = sequence.AsTestingSequence().Memoize();
            var taskConsumed = new List<List<int>>();

            var tasks = Enumerable.Range(1, 5).Select(_ =>
            {
                var list = new List<int>();
                taskConsumed.Add(list);

                return new Task(() =>
                {
                    foreach (var x in memoized)
                        list.Add(x);
                });
            }).ToArray();

            tasks.ForEach(t => t.Start());

            Task.WaitAll(tasks);

            sequence.AssertSequenceEqual(memoized);
            taskConsumed.ForEach(consumed => consumed.AssertSequenceEqual(memoized));
        }

        [Test]
        public void MemoizeAvoidsRevaluation()
        {
            var memoized = Enumerable.Range(0, 9).Memoize();
            var reevaluated = memoized.Memoize();

            Assert.That(memoized, Is.SameAs(reevaluated));
        }
    }
}
