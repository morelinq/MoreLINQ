#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2026 MoreLINQ contributors. All rights reserved.
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
    public class RankByTest
    {
        [TestCase(new[] { "aaa", "xyz", "aaaa" }, new[] { 2, 2, 1 })]
        [TestCase(new[] { "a", "bbb", "c", "dd" }, new[] { 3, 1, 3, 2 })]
        [TestCase(new[] { "a", "b", "c" }, new[] { 1, 1, 1 })]
        [TestCase(new string[0], new int[0])]
        public void RankByEquivalentKeys(string[] source, int[] expected)
        {
            using var sequence = source.AsTestingSequence();
            sequence.RankBy(s => s.Length).AssertSequenceEqual(expected);
        }

        [Test]
        public void RankByComparerEquivalentKeys()
        {
            using var sequence = new[] { "a", "B", "A", "c", "b" }.AsTestingSequence();
            sequence.RankBy(s => s, StringComparer.OrdinalIgnoreCase)
                    .AssertSequenceEqual(3, 2, 3, 1, 2);
        }

        [Test]
        public void RankComparerEquivalentItems()
        {
            using var sequence = new[] { "a", "B", "A", "c", "b" }.AsTestingSequence();
            sequence.Rank(StringComparer.OrdinalIgnoreCase)
                    .AssertSequenceEqual(3, 2, 3, 1, 2);
        }

        [Test]
        public void RankByNullKeys()
        {
            using var sequence = new[] { "a", "b", "cc", "ddd", "d" }.AsTestingSequence();
            sequence.RankBy(s => s.Length == 1 ? null : s, StringComparer.Ordinal)
                    .AssertSequenceEqual(3, 3, 2, 1, 3);
        }

        [Test]
        public void RankNullItems()
        {
            using var sequence = new[] { null, "b", null, "a" }.AsTestingSequence();
            sequence.Rank(StringComparer.Ordinal).AssertSequenceEqual(3, 1, 3, 2);
        }
    }
}
