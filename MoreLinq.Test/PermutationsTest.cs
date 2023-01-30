#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2010 Leopold Bushkin. All rights reserved.
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
    using System.Collections.Generic;
    using NUnit.Framework;

    /// <summary>
    /// Tests that verify the behavior of the Permutations() operator.
    /// </summary>
    [TestFixture]
    public class PermutationsTest
    {
        /// <summary>
        /// Verify that the permutation of the empty set is the empty set.
        /// </summary>
        [Test]
        public void TestCardinalityZeroPermutation()
        {
            var emptySet = new int[] { };
            var permutations = emptySet.Permutations();

            // should contain a single result: the empty set itself
            Assert.That(permutations.Single(), Is.EqualTo(emptySet));
        }

        /// <summary>
        /// Verify that there is one permutation of a set of one item
        /// </summary>
        [Test]
        public void TestCardinalityOnePermutation()
        {
            var set = new[] { 42 };
            var permutations = set.Permutations();

            // should contain a single result: the set itself
            Assert.That(permutations.Single(), Is.EqualTo(set));
        }

        /// <summary>
        /// Verify that there are two permutations of a set of two items
        /// and confirm that the permutations are correct.
        /// </summary>
        [Test]
        public void TestCardinalityTwoPermutation()
        {
            var set = new[] { 42, 37 };
            var permutations = set.Permutations();

            // should contain two results: the set itself and its reverse
            Assert.That(permutations.Count(), Is.EqualTo(2));
            Assert.That(permutations.First(), Is.EqualTo(set));
            Assert.That(permutations.Last(), Is.EqualTo(set.Reverse()));
        }

        /// <summary>
        /// Verify that there are six (3!) permutations of a set of three items
        /// and confirm the permutations are correct.
        /// </summary>
        [Test]
        public void TestCardinalityThreePermutation()
        {
            var set = new[] { 42, 11, 100 };
            var permutations = set.Permutations();

            var expectedPermutations = new[]
                                           {
                                               new[] {42, 11, 100},
                                               new[] {42, 100, 11},
                                               new[] {11, 100, 42},
                                               new[] {11, 42, 100},
                                               new[] {100, 11, 42},
                                               new[] {100, 42, 11},
                                           };

            // should contain six permutations (as defined above)
            Assert.That(permutations.Count(), Is.EqualTo(expectedPermutations.Length));
            Assert.That(permutations.All(p => expectedPermutations.Contains(p, SequenceEqualityComparer<int>.Instance)), Is.True);
        }

        /// <summary>
        /// Verify there are 24 (4!) permutations of a set of four items
        /// and confirm the permutations are correct.
        /// </summary>
        [Test]
        public void TestCardinalityFourPermutation()
        {
            var set = new[] { 42, 11, 100, 89 };
            var permutations = set.Permutations();

            var expectedPermutations = new[]
                                           {
                                               new[] {42, 11, 100, 89},
                                               new[] {42, 100, 11, 89},
                                               new[] {11, 100, 42, 89},
                                               new[] {11, 42, 100, 89},
                                               new[] {100, 11, 42, 89},
                                               new[] {100, 42, 11, 89},
                                               new[] {42, 11, 89, 100},
                                               new[] {42, 100, 89, 11},
                                               new[] {11, 100, 89, 42},
                                               new[] {11, 42, 89, 100},
                                               new[] {100, 11, 89, 42},
                                               new[] {100, 42, 89, 11},
                                               new[] {42, 89, 11, 100},
                                               new[] {42, 89, 100, 11},
                                               new[] {11, 89, 100, 42},
                                               new[] {11, 89, 42, 100},
                                               new[] {100, 89, 11, 42},
                                               new[] {100, 89, 42, 11},
                                               new[] {89, 42, 11, 100},
                                               new[] {89, 42, 100, 11},
                                               new[] {89, 11, 100, 42},
                                               new[] {89, 11, 42, 100},
                                               new[] {89, 100, 11, 42},
                                               new[] {89, 100, 42, 11},
                                           };

            // should contain six permutations (as defined above)
            Assert.That(permutations.Count(), Is.EqualTo(expectedPermutations.Length));
            Assert.That(permutations.All(p => expectedPermutations.Contains(p, SequenceEqualityComparer<int>.Instance)), Is.True);
        }

        /// <summary>
        /// Verify that the number of expected permutations of sets of size 5 through 10
        /// are equal to the factorial of the set size.
        /// </summary>
        [Test]
        public void TestHigherCardinalityPermutations()
        {
            // NOTE: Testing higher cardinality permutations by exhaustive comparison becomes tedious
            //       above cardinality 4 sets, as the number of permutations is N! (factorial). To provide
            //       some level of verification, though, we will simply test the count of items in the
            //       permuted sets, and verify they are equal to the expected number (count!).

            // NOTE: Generating all permutations for sets larger than about 10 items is computationally
            //       expensive and generally impractical - especially since each additional step adds
            //       less and less to our confidence in the underlying implementation.
            //       We will assume that if the algorithm scales to sets of up to 10 items, it will work
            //       with any size set.
            var setsToPermute = Enumerable.Range(5, 6).Select(s => Enumerable.Range(1, s));

            foreach (var set in setsToPermute)
            {
                var permutedSet = set.Permutations();
                var permutationCount = permutedSet.Count();
                Assert.That(permutationCount, Is.EqualTo(Combinatorics.Factorial(set.Count())));
            }
        }

        /// <summary>
        /// Verify that the Permutations() extension does not begin evaluation until the
        /// resulting sequence is iterated.
        /// </summary>
        [Test]
        public void TestPermutationsIsLazy()
        {
            _ = new BreakingSequence<int>().Permutations();
        }

        /// <summary>
        /// Verify that each permutation produced is a new object, this ensures that callers
        /// can request permutations and cache or store them without them being overwritten.
        /// </summary>
        [Test]
        public void TestPermutationsAreIndependent()
        {
            var set = new[] { 10, 20, 30, 40, };
            var permutedSets = set.Permutations();

            var listPermutations = new List<IList<int>>();
            listPermutations.AddRange(permutedSets);
            Assert.That(listPermutations, Is.Not.Empty);

            for (var i = 0; i < listPermutations.Count; i++)
            {
                for (var j = 1; j < listPermutations.Count; j++)
                {
                    if (j == i) continue;
                    Assert.That(listPermutations[i], Is.Not.SameAs(listPermutations[j]));
                }
            }
        }

        static class SequenceEqualityComparer<T>
        {
            public static readonly IEqualityComparer<IEnumerable<T>> Instance =
                EqualityComparer.Create<IEnumerable<T>>((x, y) => x is { } sx && y is { } sy && sx.SequenceEqual(sy));
        }
    }
}
