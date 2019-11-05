#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Pierre Lando. All rights reserved.
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
    using NUnit.Framework;

    /// <summary>
    /// Tests of the Combinations() family of extension methods.
    /// </summary>
    [TestFixture]
    public class CombinationsTest
    {
        /// <summary>
        /// Verify that Combinations() behaves in a lazy manner.
        /// </summary>
        [Test]
        public void TestCombinationsIsLazy()
        {
            new BreakingSequence<int>().Combinations();
        }

        /// <summary>
        /// Verify that the only Combinations of an empty sequence is the empty sequence.
        /// </summary>
        [Test]
        public void TestEmptySequenceCombinations()
        {
            var sequence = Enumerable.Repeat(0, 0);
            var result = sequence.Combinations();

            Assert.That(result.Single(), Is.EqualTo(sequence));
        }

        /// <summary>
        /// Verify that Combinations are returned in increasing size, starting with the empty set.
        /// </summary>
        [Test]
        public void TestCombinationsInIncreasingOrder()
        {
            const int count = 5;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Combinations();

            var prevCombinations = Enumerable.Empty<int>();
            foreach (var Combinations in result)
            {
                Assert.GreaterOrEqual(Combinations.Count, prevCombinations.Count());
                prevCombinations = Combinations;
            }
        }

        /// <summary>
        /// Verify that the number of Combinations returned is correct, but don't verify the Combinations contents.
        /// </summary>
        [TestCase(0, ExpectedResult = 1)]
        [TestCase(1, ExpectedResult = 2)]
        [TestCase(2, ExpectedResult = 5)]
        [TestCase(3, ExpectedResult = 16)]
        [TestCase(4, ExpectedResult = 65)]
        public int TestAllCombinationsExpectedCount(int sourceSize)
        {
            return Enumerable.Range(1, sourceSize).Combinations().Count();
        }

        private int[][][] Expected { get; } =
        {
            new[]
            {
                new int[] { }
            },

            new[]
            {
                new[] {1}, new[] {2}, new[] {3}, new[] {4}
            },

            new[]
            {
                new[] {1, 2}, new[] {1, 3}, new[] {1, 4},
                new[] {2, 1}, new[] {2, 3}, new[] {2, 4},
                new[] {3, 1}, new[] {3, 2}, new[] {3, 4},
                new[] {4, 1}, new[] {4, 2}, new[] {4, 3}
            },

            new[]
            {
                new[] {1, 2, 3}, new[] {1, 2, 4}, new[] {1, 3, 2}, new[] {1, 3, 4}, new[] {1, 4, 2}, new[] {1, 4, 3},
                new[] {2, 1, 3}, new[] {2, 1, 4}, new[] {2, 3, 1}, new[] {2, 3, 4}, new[] {2, 4, 1}, new[] {2, 4, 3},
                new[] {3, 1, 2}, new[] {3, 1, 4}, new[] {3, 2, 1}, new[] {3, 2, 4}, new[] {3, 4, 1}, new[] {3, 4, 2},
                new[] {4, 1, 2}, new[] {4, 1, 3}, new[] {4, 2, 1}, new[] {4, 2, 3}, new[] {4, 3, 1}, new[] {4, 3, 2}
            },

            new[]
            {
                new[] {1, 2, 3, 4}, new[] {1, 2, 4, 3}, new[] {1, 3, 2, 4}, new[] {1, 3, 4, 2}, new[] {1, 4, 2, 3}, new[] {1, 4, 3, 2},
                new[] {2, 1, 3, 4}, new[] {2, 1, 4, 3}, new[] {2, 3, 1, 4}, new[] {2, 3, 4, 1}, new[] {2, 4, 1, 3}, new[] {2, 4, 3, 1},
                new[] {3, 1, 2, 4}, new[] {3, 1, 4, 2}, new[] {3, 2, 1, 4}, new[] {3, 2, 4, 1}, new[] {3, 4, 1, 2}, new[] {3, 4, 2, 1},
                new[] {4, 1, 2, 3}, new[] {4, 1, 3, 2}, new[] {4, 2, 1, 3}, new[] {4, 2, 3, 1}, new[] {4, 3, 1, 2}, new[] {4, 3, 2, 1}
            }
        };

        /// <summary>
        /// Verify that the complete Combinations results for a known set are correct.
        /// </summary>
        [Test]
        public void TestAllCombinationsExpectedResults()
        {
            var sequence = Enumerable.Range(1, 4);
            var actual = sequence.Combinations().ToList();
            var expected = Expected.SelectMany(a => a);

            CollectionAssert.AreEquivalent(expected, actual);
        }

        /// <summary>
        /// Verify that the partial Combinations results for a known set are correct.
        /// </summary>
        [Test]
        public void TestAllPartialCombinationsExpectedResults()
        {
            var sequence = Enumerable.Range(1, 4).ToList();

            var i = 1;
            foreach (var expected in Expected.Skip(1))
            {
                var actual = sequence.Combinations(i).ToList();
                CollectionAssert.AreEquivalent(expected, actual);
                i++;
            }
        }
    }
}
