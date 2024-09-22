#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2015 Felipe Sateler. All rights reserved.
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
    using System.Collections.Generic;
    using NUnit.Framework;
    using static FullGroupJoinTest.OverloadCase;

    [TestFixture]
    public class FullGroupJoinTest
    {
        public enum OverloadCase { CustomResult, TupleResult }

        [Test]
        public void FullGroupIsLazy()
        {
            var bs = new BreakingSequence<int>();
            var bf = BreakingFunc.Of<int, int>();
            var bfg = BreakingFunc.Of<int, IEnumerable<int>, IEnumerable<int>, int>();

            _ = bs.FullGroupJoin(bs, bf, bf, bfg);
        }

        [TestCase(CustomResult)]
        [TestCase(TupleResult)]
        public void FullGroupJoinsResults(OverloadCase overloadCase)
        {
            var listA = new[] { 1, 2 };
            var listB = new[] { 2, 3 };

            var result = FullGroupJoin(overloadCase, listA, listB, x => x).ToDictionary(a => a.Key);

            Assert.That(result.Keys.Count, Is.EqualTo(3));

            Assert.That(result[1].Second, Is.Empty);
            result[1].First.AssertSequenceEqual(1);

            Assert.That(result[3].First, Is.Empty);
            result[3].Second.AssertSequenceEqual(3);

            result[2].First.AssertSequenceEqual(2);
            result[2].Second.AssertSequenceEqual(2);
        }

        [TestCase(CustomResult)]
        [TestCase(TupleResult)]
        public void FullGroupJoinsEmptyLeft(OverloadCase overloadCase)
        {
            var listA = new int[] { };
            var listB = new[] { 2, 3 };

            var result = FullGroupJoin(overloadCase, listA, listB, x => x).ToDictionary(a => a.Key);

            Assert.That(result.Keys.Count, Is.EqualTo(2));

            Assert.That(result[2].First, Is.Empty);
            Assert.That(result[2].Second.Single(), Is.EqualTo(2));

            Assert.That(result[3].First, Is.Empty);
            Assert.That(result[3].Second.Single(), Is.EqualTo(3));
        }

        [TestCase(CustomResult)]
        [TestCase(TupleResult)]
        public void FullGroupJoinsEmptyRight(OverloadCase overloadCase)
        {
            var listA = new[] { 2, 3 };
            var listB = new int[] { };

            var result = FullGroupJoin(overloadCase, listA, listB, x => x).ToDictionary(a => a.Key);

            Assert.That(result.Keys.Count, Is.EqualTo(2));

            Assert.That(result[2].First.Single(), Is.EqualTo(2));
            Assert.That(result[2].Second, Is.Empty);

            Assert.That(result[3].First.Single(), Is.EqualTo(3));
            Assert.That(result[3].Second, Is.Empty);
        }

        [TestCase(CustomResult)]
        [TestCase(TupleResult)]
        public void FullGroupPreservesOrder(OverloadCase overloadCase)
        {
            var listA = new[]
            {
                (3, 1),
                (1, 1),
                (2, 1),
                (1, 2),
                (1, 3),
                (3, 2),
                (1, 4),
                (3, 3),
            };
            var listB = new[]
            {
                (4, 1),
                (3, 1),
                (2, 1),
                (0, 1),
                (3, 0),
            };

            var result = FullGroupJoin(overloadCase, listA, listB, x => x.Item1).ToList();

            // Order of keys is preserved
            result.Select(x => x.Key).AssertSequenceEqual(3, 1, 2, 4, 0);

            // Order of joined elements is preserved
            foreach (var (key, first, second) in result)
            {
                first.AssertSequenceEqual(listA.Where(t => t.Item1 == key).ToArray());
                second.AssertSequenceEqual(listB.Where(t => t.Item1 == key).ToArray());
            }
        }

        static IEnumerable<(int Key, IEnumerable<T> First, IEnumerable<T> Second)> FullGroupJoin<T>(OverloadCase overloadCase, IEnumerable<T> listA, IEnumerable<T> listB, Func<T, int> getKey) =>
            overloadCase switch
            {
                CustomResult => listA.FullGroupJoin(listB, getKey, getKey, ValueTuple.Create, comparer: null),
                TupleResult => listA.FullGroupJoin(listB, getKey, getKey),
                _ => throw new ArgumentOutOfRangeException(nameof(overloadCase))
            };
    }
}
