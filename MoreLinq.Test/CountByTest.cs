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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class CountByTest
    {
        [Test]
        public void CountByWithNullSequence()
        {
            IEnumerable<int> sequence = null;
            Assert.ThrowsArgumentNullException("source", () =>
                sequence.CountBy(x => x % 2 == 0));
        }

        [Test]
        public void CountByWithNullProjection()
        {
            Func<int, bool> projection = null;
            Assert.ThrowsArgumentNullException("keySelector",() =>
                Enumerable.Range(1, 10).CountBy(projection));
        }

        [Test]
        public void CountBySimpleTest()
        {
            var result = new[] { 1, 2, 3, 4, 5, 6, 1, 2, 3, 1, 1, 2 }.CountBy(c => c);

            var expectations = new List<KeyValuePair<int, int>>()
            {
                { 1, 4 },
                { 2, 3 },
                { 3, 2 },
                { 4, 1 },
                { 5, 1 },
                { 6, 1 },
            };

            result.AssertSequenceEqual(expectations);
        }

        [Test]
        public void CountByEvenOddTest()
        {
            var result = Enumerable.Range(1, 100).CountBy(c => c % 2);

            var expectations = new List<KeyValuePair<int, int>>()
            {
                { 1, 50 },
                { 0, 50 },
            };

            result.AssertSequenceEqual(expectations);
        }

        [Test]
        public void CountByWithEqualityComparer()
        {
            var result = new[] { "a", "B", "c", "A", "b", "A" }.CountBy(c => c, StringComparer.OrdinalIgnoreCase);

            var expectations = new List<KeyValuePair<string, int>>()
            {
                { "a", 3 },
                { "B", 2 },
                { "c", 1 },
            };

            result.AssertSequenceEqual(expectations);
        }
        
        [Test]
        public void CountByHasKeysOrderedLikeGroupBy()
        {
            var randomSequence = MoreEnumerable.Random(0, 100).Take(100).ToArray();

            var countByKeys = randomSequence.CountBy(x => x).Select(x => x.Key);
            var groupByKeys = randomSequence.GroupBy(x => x).Select(x => x.Key);

            countByKeys.AssertSequenceEqual(groupByKeys);
        }
        
        [Test]
        public void CountByIsLazy()
        {
            new BreakingSequence<string>().CountBy(x => x.Length);
        }
    }
}
