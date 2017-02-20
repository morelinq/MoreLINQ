﻿#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Leandro F. Vieira (leandromoh). All rights reserved.
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

namespace MoreLinq.Test
{
    [TestFixture]
    public class MemoizeTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MemoizeWithNullSequence()
        {
            (null as IEnumerable<int>).Memoize();
        }

        [Test]
        public void MemoizeWithInnerForeach()
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

            bool firstVisitAtInnerLoopDone = false;

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
        public void MemoizeCompleteJustInSecondIteration()
        {
            var buffer = Enumerable.Range(0, 10).Memoize();

            buffer.Take(5).AssertSequenceEqual(Enumerable.Range(0, 5));
            buffer.AssertSequenceEqual(Enumerable.Range(0, 10));
        }

        [Test]
        public void MemoizeIsLazy()
        {
            var buffer = Enumerable.Range(1, 10).Select(i =>
            {
                if (i > 5)
                    throw new InvalidOperationException();

                return i;
            }).Memoize();

            buffer.Take(5).ToList();
        }

        [Test]
        public void MemoizeWithInMemoryCollection()
        {
            List<int> list = Enumerable.Range(1, 10).ToList();

            Assert.IsInstanceOf<List<int>.Enumerator>(list.Memoize().GetEnumerator());
            Assert.IsInstanceOf<List<int>.Enumerator>(list.Memoize(false).GetEnumerator());
        }

        [Test]
        public void MemoizeForcingBufferingICollection()
        {
            List<int> list = Enumerable.Range(1, 10).ToList();

            Assert.IsNotInstanceOf<List<int>.Enumerator>(list.Memoize(true).GetEnumerator());
        }
    }
}
