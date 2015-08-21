using System;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    /// <summary>
    /// Tests of the Subset() family of extension methods.
    /// </summary>
    [TestFixture]
    public class SubsetTest
    {
        /// <summary>
        /// Verify that Subsets() behaves in a lazy manner.
        /// </summary>
        [Test]
        public void TestSubsetsIsLazy()
        {
            new BreakingSequence<int>().Subsets();
            new BreakingSequence<int>().Subsets(5);
        }

        /// <summary>
        /// Verify that negative subset sizes result in an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestNegativeSubsetSize()
        {
            const int count = 10;
            var sequence = Enumerable.Range(1, count);
            sequence.Subsets(-5);
        }

        /// <summary>
        /// Verify that requesting subsets larger than the original sequence length result in an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSubsetLargerThanSequence()
        {
            const int count = 10;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Subsets(count + 5);
            result.Count(); // this particular exception is deferred until sequence evaluation
        }

        /// <summary>
        /// Verify that the only subset of an empty sequence is the empty sequence.
        /// </summary>
        [Test]
        public void TestEmptySequenceSubsets()
        {
            var sequence = Enumerable.Repeat(0, 0);
            var result = sequence.Subsets();

            Assert.IsTrue(result.Single().SequenceEqual(sequence));
        }

        /// <summary>
        /// Verify that subsets are returned in increasing size, starting with the empty set.
        /// </summary>
        [Test]
        public void TestSubsetsInIncreasingOrder()
        {
            const int count = 10;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Subsets();

            var prevSubset = Enumerable.Empty<int>();
            foreach (var subset in result)
            {
                Assert.GreaterOrEqual(subset.Count(), prevSubset.Count());
                prevSubset = subset;
            }
        }

        /// <summary>
        /// Verify that the number of subsets returned is correct, but don't verify the subset contents.
        /// </summary>
        [Test]
        public void TestAllSubsetsExpectedCount()
        {
            const int count = 20;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Subsets();

            var expectedCount = Math.Pow(2, count);

            Assert.AreEqual(expectedCount, result.Count());
        }

        /// <summary>
        /// Verify that the complete subset results for a known set are correct.
        /// </summary>
        [Test]
        public void TestAllSubsetsExpectedResults()
        {
            var sequence = Enumerable.Range(1, 4);
            var result = sequence.Subsets();

            var expectedSubsets = new[]
                                      {
                                          new int[] {},
                                          new[] {1}, new[] {2}, new[] {3}, new[] {4},
                                          new[] {1,2}, new[] {1,3}, new[] {1,4}, new[] {2,3}, new[] {2,4}, new[] {3,4},
                                          new[] {1,2,3}, new[] {1,2,4}, new[] {1,3,4}, new[] {2,3,4},
                                          new[] {1,2,3,4}
                                      };

            var index = 0;
            foreach (var subset in result)
                Assert.IsTrue(subset.SequenceEqual(expectedSubsets[index++]));
        }

        /// <summary>
        /// Verify that the number of subsets for a given subset-size is correct.
        /// </summary>
        [Test]
        public void TestKSubsetExpectedCount()
        {
            const int count = 20;
            const int subsetSize = 10;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Subsets(subsetSize);

            // number of subsets of a given size is defined by the binomial coefficient: c! / ((c-s)!*s!)
            var expectedSubsetCount = Combinatorics.Binomial(count, subsetSize);

            Assert.AreEqual(expectedSubsetCount, result.Count());
        }

        /// <summary>
        /// Verify that k-subsets of a given set are in the correct order and contain the correct elements.
        /// </summary>
        [Test]
        public void TestKSubsetExpectedResult()
        {
            var sequence = Enumerable.Range(1, 6);
            var result = sequence.Subsets(4);

            var expectedSubsets = new[]
                                      {
                                          new[] {1,2,3,4},
                                          new[] {1,2,3,5},
                                          new[] {1,2,3,6},
                                          new[] {1,2,4,5},
                                          new[] {1,2,4,6},
                                          new[] {1,2,5,6},
                                          new[] {1,3,4,5},
                                          new[] {1,3,4,6},
                                          new[] {1,3,5,6},
                                          new[] {1,4,5,6},
                                          new[] {2,3,4,5},
                                          new[] {2,3,4,6},
                                          new[] {2,3,5,6},
                                          new[] {2,4,5,6},
                                          new[] {3,4,5,6},
                                      };

            var index = 0;
            foreach (var subset in result)
                Assert.IsTrue(subset.SequenceEqual(expectedSubsets[index++]));
        }
    }
}