#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2015 Jonathan Skeet. All rights reserved.
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
using System.Linq;
using NUnit.Framework;


namespace MoreLinq.Test
{
    [TestFixture]
    public class FullGroupJoinTest
    {
        [Test]
        public void FullGroupFirstNull()
        {
            Assert.ThrowsArgumentNullException("first", () =>
                ((IEnumerable<string>)null).FullGroupJoin(Enumerable.Empty<string>(), x => x, x => x, DummySelector));
        }

        [Test]
        public void FullGroupSecondNull()
        {
            Assert.ThrowsArgumentNullException("second", () =>
                Enumerable.Empty<string>().FullGroupJoin((IEnumerable<string>)null, x => x, x => x, DummySelector));
        }

        [Test]
        public void FullGroupFirstKeyNull()
        {
            Assert.ThrowsArgumentNullException("firstKeySelector",() =>
                Enumerable.Empty<string>().FullGroupJoin(Enumerable.Empty<string>(), (Func<string, string>)null, x => x, DummySelector));
        }

        [Test]
        public void FullGroupSecondKeyNull()
        {
            Assert.ThrowsArgumentNullException("secondKeySelector",() =>
                Enumerable.Empty<string>().FullGroupJoin(Enumerable.Empty<string>(), x => x, (Func<string, string>)null, DummySelector));
        }

        [Test]
        public void FullGroupResultSelectorNull()
        {
            Assert.ThrowsArgumentNullException("resultSelector", () =>
                Enumerable.Empty<string>().FullGroupJoin(Enumerable.Empty<string>(), x => x, x => x, (Func<string, IEnumerable<string>, IEnumerable<string>, string>)null));
        }

        [Test]
        public void FullGroupIsLazy()
        {
            var listA = new BreakingSequence<int>();
            var listB = new BreakingSequence<int>();

            listA.FullGroupJoin(listB, x => x, x => x, DummySelector);
            Assert.True(true);
        }

        [Test]
        public void FullGroupJoinsResults()
        {
            var listA = new[] { 1, 2 };
            var listB = new[] { 2, 3 };

            var result = listA.FullGroupJoin(listB, x => x, x => x, (key, first, second) => new { key, first, second }).ToDictionary(a => a.key);

            Assert.AreEqual(3, result.Keys.Count);

            Assert.IsEmpty(result[1].second);
            Assert.AreEqual(1, result[1].first.Single());

            Assert.IsEmpty(result[3].first);
            Assert.AreEqual(3, result[3].second.Single());

            Assert.IsNotEmpty(result[2].first);
            Assert.AreEqual(2, result[2].first.Single());
            Assert.IsNotEmpty(result[2].second);
            Assert.AreEqual(2, result[2].second.Single());
        }

        [Test]
        public void FullGroupJoinsEmptyLeft()
        {
            var listA = new int[] { };
            var listB = new[] { 2, 3 };

            var result = listA.FullGroupJoin(listB, x => x, x => x, (key, first, second) => new { key, first, second }).ToDictionary(a => a.key);

            Assert.AreEqual(2, result.Keys.Count);

            Assert.IsEmpty(result[2].first);
            Assert.AreEqual(2, result[2].second.Single());

            Assert.IsEmpty(result[3].first);
            Assert.AreEqual(3, result[3].second.Single());
        }

        [Test]
        public void FullGroupJoinsEmptyRight()
        {
            var listA = new[] { 2, 3 };
            var listB = new int[] { };

            var result = listA.FullGroupJoin(listB, x => x, x => x, (key, first, second) => new { key, first, second }).ToDictionary(a => a.key);

            Assert.AreEqual(2, result.Keys.Count);

            Assert.AreEqual(2, result[2].first.Single());
            Assert.IsEmpty(result[2].second);

            Assert.AreEqual(3, result[3].first.Single());
            Assert.IsEmpty(result[3].second);
        }

        [Test]
        public void FullGroupPreservesOrder()
        {
            var listA = new[] {
                Tuple.Create(3, 1),
                Tuple.Create(1, 1),
                Tuple.Create(2, 1),
                Tuple.Create(1, 2),
                Tuple.Create(1, 3),
                Tuple.Create(3, 2),
                Tuple.Create(1, 4),
                Tuple.Create(3, 3),
            };
            var listB = new[] {
                Tuple.Create(4, 1),
                Tuple.Create(3, 1),
                Tuple.Create(2, 1),
                Tuple.Create(0, 1),
                Tuple.Create(3, 0),
            };

            var result = listA.FullGroupJoin(listB, x => x.Item1, x => x.Item1, (key, first, second) => new { key, first, second }).ToList();

            // Order of keys is preserved
            result.Select(x => x.key).AssertSequenceEqual(3, 1, 2, 4, 0);

            // Order of joined elements is preserved
            foreach (var res in result) {
                res.first.AssertSequenceEqual(listA.Where(t => t.Item1 == res.key).ToArray());
                res.second.AssertSequenceEqual(listB.Where(t => t.Item1 == res.key).ToArray());
            }
        }

        private static T1 DummySelector<T1, T2, T3>(T1 t1, T2 t2, T3 t3)
        {
            return t1;
        }
    }
}

