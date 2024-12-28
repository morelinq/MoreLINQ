#region License and Terms
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

namespace MoreLinq.Test
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class CountByTest
    {
        [Test]
        public void CountBySimpleTest()
        {
            var result = MoreEnumerable.CountBy([1, 2, 3, 4, 5, 6, 1, 2, 3, 1, 1, 2 ], c => c);

            result.AssertSequenceEqual(
                KeyValuePair.Create(1, 4),
                KeyValuePair.Create(2, 3),
                KeyValuePair.Create(3, 2),
                KeyValuePair.Create(4, 1),
                KeyValuePair.Create(5, 1),
                KeyValuePair.Create(6, 1));
        }

        [Test]
        public void CountByWithSecondOccurenceImmediatelyAfterFirst()
        {
            var result = MoreEnumerable.CountBy("jaffer", c => c);

            result.AssertSequenceEqual(
                KeyValuePair.Create('j', 1),
                KeyValuePair.Create('a', 1),
                KeyValuePair.Create('f', 2),
                KeyValuePair.Create('e', 1),
                KeyValuePair.Create('r', 1));
        }

        [Test]
        public void CountByEvenOddTest()
        {
            var result = MoreEnumerable.CountBy(Enumerable.Range(1, 100), c => c % 2);

            result.AssertSequenceEqual(
                KeyValuePair.Create(1, 50),
                KeyValuePair.Create(0, 50));
        }

        [Test]
        public void CountByWithEqualityComparer()
        {
            var result = MoreEnumerable.CountBy(["a", "B", "c", "A", "b", "A"], c => c, StringComparer.OrdinalIgnoreCase);

            result.AssertSequenceEqual(
                KeyValuePair.Create("a", 3),
                KeyValuePair.Create("B", 2),
                KeyValuePair.Create("c", 1));
        }

        [Test]
        public void CountByHasKeysOrderedLikeGroupBy()
        {
            var randomSequence = MoreEnumerable.Random(0, 100).Take(100).ToArray();

            var countByKeys = MoreEnumerable.CountBy(randomSequence, x => x).Select(x => x.Key);
            var groupByKeys = randomSequence.GroupBy(x => x).Select(x => x.Key);

            countByKeys.AssertSequenceEqual(groupByKeys);
        }

        [Test]
        public void CountByIsLazy()
        {
            _ = MoreEnumerable.CountBy(new BreakingSequence<string>(), BreakingFunc.Of<string, int>());
        }

        [Test]
        public void CountByWithSomeNullKeys()
        {
            var ss = new[]
            {
                "foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo"
            };
            var result = MoreEnumerable.CountBy(ss, s => s);

            result.AssertSequenceEqual(
                KeyValuePair.Create((string?)"foo", 2),
                KeyValuePair.Create((string?)null, 4),
                KeyValuePair.Create((string?)"bar", 2),
                KeyValuePair.Create((string?)"baz", 2));
        }

        [Test]
        public void CountByWithSomeNullKeysAndEqualityComparer()
        {
            string?[] source = ["a", "B", null, "c", "A", null, "b", "A"];
            var result = MoreEnumerable.CountBy(source, c => c, StringComparer.OrdinalIgnoreCase);

            result.AssertSequenceEqual(
                KeyValuePair.Create((string?)"a", 3),
                KeyValuePair.Create((string?)"B", 2),
                KeyValuePair.Create((string?)null, 2),
                KeyValuePair.Create((string?)"c", 1));
        }
    }
}
