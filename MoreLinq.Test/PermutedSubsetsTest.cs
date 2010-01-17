using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    /// <summary>
    /// Tests that verify the behavior of the PermutedSubsets() family of operators.
    /// </summary>
    [TestFixture]
    public class PermutedSubsetsTests
    {
        /// <summary>
        /// Verify that PermutedSubsets() behaves in a deferred, lazy manner.
        /// </summary>
        [Test]
        public void TestPermutedSubsetsIsLazy()
        {
            new BreakingSequence<int>().PermutedSubsets();
            new BreakingSequence<int>().PermutedSubsets(10);
        }

        /// <summary>
        /// Verify that negative subset size arguments results in an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestNegativeSizeArgument()
        {
            Enumerable.Range(1, 10).PermutedSubsets(-5);
        }
        
        /// <summary>
        /// Verify that invoking PermutedSubsets() on a null sequence results in an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullSequenceException()
        {
            const IEnumerable<int> sequence = null;
            sequence.PermutedSubsets();
        }

        /// <summary>
        /// Verify that invoking PermutedSubsets() on a null sequence results in an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullSequenceException2()
        {
            const IEnumerable<int> sequence = null;
            sequence.PermutedSubsets(2); // tests alternate overload that accepts subsetSize
        }

        /// <summary>
        /// Verify that the count of permuted subsets (of all sizes) is correct.
        /// </summary>
        /// <remarks>
        /// The number of subsets (of all sizes) of a given set is given by the formula:
        /// </remarks>
        [Test]
        public void TestAllSizePermutedSubsetsCount()
        {
            const int count = 9;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.PermutedSubsets();

            var expectedCount =
                Enumerable.Range(0, count + 1)
                          .Select(k => Combinatorics.Factorial(k) * Combinatorics.Binomial(count, k))
                          .Sum();

            Assert.AreEqual(expectedCount, result.Count());
        }

        /// <summary>
        /// Verify the results of generating all permuted subsets of a known sequence
        /// </summary>
        [Test]
        public void TestAllSizePermutedSubsetsResults()
        {
            var sequence = Enumerable.Range(1, 3);
            var result = sequence.PermutedSubsets();

            var expectedResult = new[]
                                     {
                                         new int[] {},
                                         new[] {1},
                                         new[] {2},
                                         new[] {3},
                                         new[] {1,2},
                                         new[] {2,1},
                                         new[] {1,3},
                                         new[] {3,1},
                                         new[] {2,3},
                                         new[] {3,2},
                                         new[] {1,2,3},
                                         new[] {1,3,2},
                                         new[] {2,1,3},
                                         new[] {2,3,1},
                                         new[] {3,1,2},
                                         new[] {3,2,1},
                                     };

            var index = 0;
            foreach (var permutedSet in result)
                Assert.IsTrue(permutedSet.SequenceEqual(expectedResult[index++]));

        }

        /// <summary>
        /// Verify the count of permuted subsets generated for a known sequence
        /// </summary>
        [Test]
        public void TestKSizePermutedSubsetsCount()
        {
            const int count = 9;
            const int subsetSize = 5;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.PermutedSubsets(subsetSize);

            var expectedCount = Combinatorics.Factorial(subsetSize) * Combinatorics.Binomial(count, subsetSize);

            Assert.Less(subsetSize, count);
            Assert.AreEqual(expectedCount, result.Count());
        }

        /// <summary>
        /// Verify the results of generating permuted subsets of a known sequence
        /// </summary>
        [Test]
        public void TestKSizePermutedSubsetsResults()
        {
            var sequence = Enumerable.Range(1, 4);
            var result = sequence.PermutedSubsets(3);

            var expectedResult = new[]
                                     {
                                         new[] {1,2,3},
                                         new[] {1,3,2},
                                         new[] {2,1,3},
                                         new[] {2,3,1},
                                         new[] {3,1,2},
                                         new[] {3,2,1},

                                         new[] {1,2,4},
                                         new[] {1,4,2},
                                         new[] {2,1,4},
                                         new[] {2,4,1},
                                         new[] {4,1,2},
                                         new[] {4,2,1},

                                         new[] {1,3,4},
                                         new[] {1,4,3},
                                         new[] {3,1,4},
                                         new[] {3,4,1},
                                         new[] {4,1,3},
                                         new[] {4,3,1},

                                         new[] {2,3,4},
                                         new[] {2,4,3},
                                         new[] {3,2,4},
                                         new[] {3,4,2},
                                         new[] {4,2,3},
                                         new[] {4,3,2},
                                     };

            var index = 0;
            foreach (var permutedSet in result)
                Assert.IsTrue(permutedSet.SequenceEqual(expectedResult[index++]));
        }

        /// <summary>
        /// Verify that all subsets of the empty set are the empty set itself.
        /// </summary>
        [Test]
        public void TestEmptySequenceAllSubsets()
        {
            var sequence = Enumerable.Empty<int>();
            var result = sequence.PermutedSubsets();
            Assert.IsTrue(result.All(s => s.SequenceEqual(sequence)));
        }

        /// <summary>
        /// Verify that all K-subsets of the empty set are the empty set itself.
        /// </summary>
        [Test]
        public void TestEmptySequenceKSubsets()
        {
            var sequence = Enumerable.Empty<int>();
            var result = sequence.PermutedSubsets(0); // only one subset size is legal: 0
            Assert.IsTrue(result.All(s => s.SequenceEqual(sequence)));
        }
    }
}