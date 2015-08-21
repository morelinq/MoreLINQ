using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    /// <summary>
    /// Verify the behavior of the SlidingWindow operator
    /// </summary>
    [TestFixture]
    public class WindowedTests
    {
        /// <summary>
        /// Verify that SlidingWindow behaves in a lazy manner
        /// </summary>
        [Test]
        public void TestSlidingWindowIsLazy()
        {
            new BreakingSequence<int>().Windowed(1);
        }

        /// <summary>
        /// Verify that invoking SlidingWindow on a <c>null</c> sequence results in an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSlidingWindowNullSequenceException()
        {
            const IEnumerable<int> sequence = null;
            sequence.Windowed(10);
        }

        /// <summary>
        /// Verify that a negative window size results in an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSlidingWindowNegativeWindowSizeException()
        {
            var sequence = Enumerable.Repeat(1, 10);
            sequence.Windowed(-5);
        }

        /// <summary>
        /// Verify that a sliding window of an any size over an empty sequence is a single empty sequence
        /// </summary>
        [Test]
        public void TestSlidingWindowEmptySequence()
        {
            var sequence = Enumerable.Empty<int>();
            var result = sequence.Windowed(5);

            Assert.IsTrue( result.Single().SequenceEqual(sequence) );
        }

        /// <summary>
        /// Verify that decomposing a sequence into windows of a single item
        /// degenerates to the original sequence.
        /// </summary>
        [Test]
        public void TestSlidingWindowOfSingleElement()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Windowed(1);

            // number of windows should be equal to the source sequence length
            Assert.AreEqual(count, result.Count());
            // each window should contain single item consistent of element at that offset
            var index = -1;
            foreach (var window in result)
                Assert.AreEqual(sequence.ElementAt(++index), window.Single());
        }

        /// <summary>
        /// Verify that asking for a window large than the source sequence results
        /// in a single sequence whose content is the same as the source sequence.
        /// </summary>
        [Test]
        public void TestSlidingWindowLargerThanSequence()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Windowed(count + 1);

            // there should only be one window whose contents is the same
            // as the source sequence
            Assert.IsTrue(result.Single().SequenceEqual(sequence));
        }

        /// <summary>
        /// Verify that asking for a window smaller than the source sequence results
        /// in N sequences, where N = (source.Count() - windowSize) + 1.
        /// </summary>
        [Test]
        public void TestSlidingWindowSmallerThanSequence()
        {
            const int count = 100;
            const int windowSize = count / 3;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Windowed(windowSize);

            // ensure that the number of windows is correct
            Assert.AreEqual(count - windowSize + 1, result.Count());
            // ensure each window contains the correct set of items
            var index = -1;
            foreach (var window in result)
                Assert.IsTrue(window.SequenceEqual(sequence.Skip(++index).Take(windowSize)));
        }
    }
}