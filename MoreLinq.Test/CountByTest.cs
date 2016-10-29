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
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountByWithNullSequence()
        {
            IEnumerable<int> sequence = null;
            sequence.CountBy(x => x % 2 == 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountByWithNullProjection()
        {
            Func<int, bool> projection = null;
            Enumerable.Range(1, 10).CountBy(projection);
        }

        [Test]
        public void CountBySimpleTest()
        {
            IEnumerable<KeyValuePair<int, int>> result = new[] { 1, 2, 3, 4, 5, 6, 1, 2, 3, 1, 1, 2 }.CountBy(c => c);

            IEnumerable<KeyValuePair<int, int>> expecteds = new Dictionary<int, int>() { { 1, 4 }, { 2, 3 }, { 3, 2 }, { 4, 1 }, { 5, 1 }, { 6, 1 } };

            result.AssertSequenceEqual(expecteds);
        }

        [Test]
        public void CountByEvenOddTest()
        {
            IEnumerable<KeyValuePair<int, int>> result = Enumerable.Range(1, 100).CountBy(c => c % 2);

            IEnumerable<KeyValuePair<int, int>> expecteds = new Dictionary<int, int>() { { 1, 50 }, { 0, 50 } };

            result.AssertSequenceEqual(expecteds);
        }

        [Test]
        public void CountByWithEqualityComparer()
        {
            IEnumerable<KeyValuePair<string, int>> result = new[] { "a", "B", "c", "A", "b", "A" }.CountBy(c => c, StringComparer.OrdinalIgnoreCase);

            IEnumerable<KeyValuePair<string, int>> expecteds = new Dictionary<string, int>() { { "a", 3 }, { "B", 2 }, { "c", 1 } };

            result.AssertSequenceEqual(expecteds);
        }
    }
}
