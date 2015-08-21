using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    /// <summary>
    /// Verify the behavior of the Cartesian operator
    /// </summary>
    [TestFixture]
    public class CartesianTests
    {
        /// <summary>
        /// Verify that the Cartesian product is evaluated in a lazy fashion on demand.
        /// </summary>
        [Test]
        public void TestCartesianIsLazy()
        {
            new BreakingSequence<int>().Cartesian(new BreakingSequence<int>(), (a, b) => new { A = a, B = b });
        }

        /// <summary>
        /// Verify applying Cartesian to a <c>null</c> sequence results in an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCartesianSequenceANullException()
        {
            const IEnumerable<int> sequence = null;
            sequence.Cartesian(Enumerable.Repeat(1, 10), (a, b) => a + b);
        }

        /// <summary>
        /// Verify passing a <c>null</c> second sequence to Cartesian results in an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCartesianSequenceBNullException()
        {
            var sequence = Enumerable.Repeat(1,10);
            sequence.Cartesian<int,int,int>(null, (a, b) => a + b);
        }

        /// <summary>
        /// Verify that passing a <c>null</c> projection function to Cartesian results in an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCartesianResultSelectorNullException()
        {
            var sequence = Enumerable.Repeat(1, 10);
            sequence.Cartesian(sequence, (Func<int, int, int>) null);
        }

        /// <summary>
        /// Verify that the Cartesian product of two empty sequences is an empty sequence
        /// </summary>
        [Test]
        public void TestCartesianOfEmptySequences()
        {
            var sequenceA = Enumerable.Empty<int>();
            var sequenceB = Enumerable.Empty<int>();
            var result = sequenceA.Cartesian(sequenceB, (a, b) => a + b);

            Assert.IsTrue(result.SequenceEqual(sequenceA));
        }

        /// <summary>
        /// Verify that the Cartesian product of an empty and non-empty sequence is an empty sequence
        /// </summary>
        [Test]
        public void TestCartesianOfEmptyAndNonEmpty()
        {
            var sequenceA = Enumerable.Empty<int>();
            var sequenceB = Enumerable.Repeat(1,10);
            var resultA = sequenceA.Cartesian(sequenceB, (a, b) => a + b);
            var resultB = sequenceB.Cartesian(sequenceA, (a, b) => a + b);

            Assert.IsTrue(resultA.SequenceEqual(sequenceA));
            Assert.IsTrue(resultB.SequenceEqual(sequenceA));
        }

        /// <summary>
        /// Verify that the number of elements in a Cartesian product is the product of the number of elements of each sequence
        /// </summary>
        [Test]
        public void TestCartesianProductCount()
        {
            const int countA = 100;
            const int countB = 75;
            const int expectedCount = countA*countB;
            var sequenceA = Enumerable.Range(1, countA);
            var sequenceB = Enumerable.Range(1, countB);
            var result = sequenceA.Cartesian(sequenceB, (a, b) => a + b);

            Assert.AreEqual( expectedCount, result.Count() );
        }
        
        /// <summary>
        /// Verify that each combination is produced in the Cartesian product
        /// </summary>
        [Test]
        public void TestCartesianProductCombinations()
        {
            var sequenceA = Enumerable.Range(0, 5);
            var sequenceB = Enumerable.Range(0, 5);
            var expectedSet = new[]
                                  {
                                      Enumerable.Repeat(false, 5).ToArray(),
                                      Enumerable.Repeat(false, 5).ToArray(),
                                      Enumerable.Repeat(false, 5).ToArray(),
                                      Enumerable.Repeat(false, 5).ToArray(),
                                      Enumerable.Repeat(false, 5).ToArray()
                                  };

            var result = sequenceA.Cartesian(sequenceB, (a, b) => new { A = a, B = b });

            // verify that the expected number of results is correct
            Assert.AreEqual(sequenceA.Count() * sequenceB.Count(), result.Count());

            // ensure that all "cells" were visited by the cartesian product
            foreach (var coord in result)
                expectedSet[coord.A][coord.B] = true;
            Assert.IsTrue(expectedSet.SelectMany(x => x).All(z => z));
        }

        /// <summary>
        /// Verify that if either sequence passed to Cartesian is empty, the result
        /// is an empty sequence.
        /// </summary>
        [Test]
        public void TestEmptyCartesianEvaluation()
        {
            var sequence = Enumerable.Range(0, 5);

            var resultA = sequence.Cartesian(Enumerable.Empty<int>(), (a, b) => new { A = a, B = b });
            var resultB = Enumerable.Empty<int>().Cartesian(sequence, (a, b) => new { A = a, B = b });
            var resultC = Enumerable.Empty<int>().Cartesian(Enumerable.Empty<int>(), (a, b) => new { A = a, B = b });

            Assert.AreEqual(0, resultA.Count());
            Assert.AreEqual(0, resultB.Count());
            Assert.AreEqual(0, resultC.Count());
        }
    }
}