using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    /// <summary>
    /// Verify the behavior of the Incremental operator.
    /// </summary>
    [TestFixture]
    public class IncrementalTests
    {
        /// <summary>
        /// Verify that Incremental behaves in a lazy manner.
        /// </summary>
        [Test]
        public void TestIncrementalIsLazy()
        {
            new BreakingSequence<int>().Incremental((prev, next) => prev + next);
            new BreakingSequence<int>().Incremental((prev, next, index) => next - prev);
        }

        /// <summary>
        /// Verify that invoking Incremental on a <c>null</c> sequence result in an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullSequenceException1()
        {
            const IEnumerable<int> sequence = null;
            sequence.Incremental((prev, next) => prev + next);
        }

        /// <summary>
        /// Verify that invoking Incremental on a <c>null</c> sequence result in an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullSequenceException2()
        {
            const IEnumerable<int> sequence = null;
            sequence.Incremental((prev, next, i) => prev + next);
        }

        /// <summary>
        /// Verify that the result of applying Incremental to an empty sequence is an empty sequence.
        /// </summary>
        [Test]
        public void TestIncrementalEmptySequence()
        {
            var sequence = Enumerable.Empty<int>();
            var resultA = sequence.Incremental((prev, next) => prev + next);
            var resultB = sequence.Incremental((prev, next, index) => prev + next);

            Assert.IsTrue(resultA.SequenceEqual(sequence));
            Assert.IsTrue(resultB.SequenceEqual(sequence));
        }

        /// <summary>
        /// Verify that the result of applying Incremental to a single-item sequence is an empty sequence.
        /// </summary>
        [Test]
        public void TestIncrementalSingleItemSequence()
        {
            var sequence = Enumerable.Repeat(1, 1);
            var expectedResult = Enumerable.Empty<int>();
            var resultA = sequence.Incremental((prev, next) => prev + next);
            var resultB = sequence.Incremental((prev, next, index) => prev + next);

            Assert.IsTrue(resultA.SequenceEqual(expectedResult));
            Assert.IsTrue(resultB.SequenceEqual(expectedResult));
        }

        /// <summary>
        /// Verify that the number of items in the resulting sequence is one-less than in the source sequence
        /// </summary>
        [Test]
        public void TestIncrementResultLengthInvariant()
        {
            const int count = 1000;
            var sequence = Enumerable.Repeat(0, count);
            var resultA = sequence.Incremental((prev, next) => prev + next);
            var resultB = sequence.Incremental((prev, next, index) => prev + next);

            Assert.AreEqual(count - 1, resultA.Count());
            Assert.AreEqual(count - 1, resultB.Count());
        }

        /// <summary>
        /// Verify that Increment passes the correct indexes into the projection method
        /// </summary>
        [Test]
        public void TestIncrementProvidesCorrectIndexes()
        {
            const int count = 1000;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Incremental((prev, next, index) => index);

            Assert.IsTrue(result.SequenceEqual(sequence.Take(count - 1)));
        }

        /// <summary>
        /// Verify that Incremental computes the right results for a simple running difference
        /// </summary>
        [Test]
        public void TestIncrementalRunningDifference()
        {
            var sequence = Enumerable.Range(1, 10).Select(x => 2 * x);
            var result = sequence.Incremental((a, b) => b - a);
            Assert.IsTrue(result.All(x => x == 2));
            Assert.AreEqual(sequence.Count() - 1, result.Count());
        }

        /// <summary>
        /// Verify that Incremental visits every adjacent pair of elements
        /// </summary>
        [Test]
        public void TestIncrementalVisitsEveryAdjacentPair()
        {
            var sequence = Enumerable.Range(1, 10);
            var result = sequence.Incremental((a, b) => new { A = a, B = b });
            Assert.IsTrue(result.All(x => x.B == (x.A + 1)));
            Assert.IsTrue(result.Select(x => x.B).SequenceEqual(Enumerable.Range(2, 9)));
        }
    }
}