using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
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
            Assert.IsTrue(permutations.Single().SequenceEqual(emptySet));
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
            Assert.IsTrue(permutations.Single().SequenceEqual(set));
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
            Assert.IsTrue(permutations.Count() == 2);
            Assert.IsTrue(permutations.First().SequenceEqual(set));
            Assert.IsTrue(permutations.Last().SequenceEqual(set.Reverse()));
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
            Assert.AreEqual(expectedPermutations.Count(), permutations.Count());
            Assert.IsTrue(permutations.All(p => expectedPermutations.Contains(p, EqualityComparerFunc<IList<int>>.As((x, y) => x.SequenceEqual(y)))));
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
            Assert.AreEqual(expectedPermutations.Count(), permutations.Count());
            Assert.IsTrue(permutations.All(p => expectedPermutations.Contains(p, EqualityComparerFunc<IList<int>>.As((x, y) => x.SequenceEqual(y)))));
        }

        /// <summary>
        /// Verify that the number of expected permutations of sets of size 5 through 10
        /// are equal to the factorial of the set size.
        /// </summary>
        [Test]
        public void TestHigherCardinalityPermutations()
        {
            // NOTE: Testing higher cardinality permutations by exhaustive comparison becomes tedious
            //       above cardiality 4 sets, as the number of permutations is N! (factorial). To provide
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
                Assert.AreEqual(Combinatorics.Factorial(set.Count()), permutationCount);
            }
        }

        /// <summary>
        /// Verify that the Permutations() extension does not begin evaluation until the
        /// resulting sequence is iterated.
        /// </summary>
        [Test]
        public void TestPermutationsIsLazy()
        {
            new BreakingSequence<int>().Permutations();
        }


        /// <summary>
        /// Verify that invoking Permutations() on a <c>null</c> sequence results in an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPermutationNullSequenceException()
        {
            const IEnumerable<int> sequence = null;
            sequence.Permutations();
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
            Assert.IsNotEmpty(listPermutations);

            for (var i = 0; i < listPermutations.Count; i++)
            {
                for (var j = 1; j < listPermutations.Count; j++)
                {
                    if (j == i) continue;
                    Assert.AreNotSame(listPermutations[i], listPermutations[j]);
                }
            }
        }
    }
}