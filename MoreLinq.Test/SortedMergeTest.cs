using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    /// <summary>
    /// Tests that verify the behavior of the SortedMerge operator.
    /// </summary>
    [TestFixture]
    public class SortedMergeTests
    {
        /// <summary>
        /// Verify that SortedMerge behaves in a lazy manner.
        /// </summary>
        [Test]
        public void TestSortedMergeIsLazy()
        {
            var sequenceA = new BreakingSequence<int>();
            var sequenceB = new BreakingSequence<int>();

            sequenceA.SortedMerge(OrderByDirection.Ascending, sequenceB);
        }

        /// <summary>
        /// Verify that SortedMerge throws an exception if invoked on a <c>null</c> sequence.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSortedMergeSequenceNullException()
        {
            const IEnumerable<int> sequenceA = null;
            var sequenceB = new BreakingSequence<int>();

            sequenceA.SortedMerge(OrderByDirection.Ascending, sequenceB);
        }

        /// <summary>
        /// Verify that SortedMerge throws an exception if invoked with a <c>null</c> <c>otherSequences</c> argument.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSortedMergeOtherSequencesNullException()
        {
            var sequenceA = new BreakingSequence<int>();
            sequenceA.SortedMerge(OrderByDirection.Ascending, (IEnumerable<int>[])null);
        }

        /// <summary>
        /// Verify that SortedMerge disposes those enumerators that it managed 
        /// to open successfully
        /// </summary>
        [Test]
        public void TestSortedMergeDisposesOnError()
        {
            using (var sequenceA = TestingSequence.Of<int>())
            {
                try
                {
                    sequenceA.SortedMerge(OrderByDirection.Ascending, new BreakingSequence<int>()).ToArray();
                    Assert.Fail("{0} was expected", typeof(InvalidOperationException));
                }
                catch (InvalidOperationException)
                {
                    // Expected and thrown by BreakingSequence
                }
            }
        }

        /// <summary>
        /// Verify that SortedMerge throws an exception if invoked on a <c>null</c> sequence.
        /// </summary>
        [Test]
        public void TestSortedMergeComparerNull()
        {
            var sequenceA = Enumerable.Range(1, 3);
            var sequenceB = Enumerable.Range(4, 3);
            var result = sequenceA.SortedMerge(OrderByDirection.Ascending, (IComparer<int>)null, sequenceB);

            Assert.IsTrue(result.SequenceEqual(sequenceA.Concat(sequenceB)));
        }

        /// <summary>
        /// Verify that if <c>otherSequences</c> is empty, SortedMerge yields the contents of <c>sequence</c>
        /// </summary>
        [Test]
        public void TestSortedMergeOtherSequencesEmpty()
        {
            const int count = 10;
            var sequenceA = Enumerable.Range(1, count);
            var result = sequenceA.SortedMerge(OrderByDirection.Ascending);

            Assert.IsTrue(result.SequenceEqual(sequenceA));
        }

        /// <summary>
        /// Verify that if all sequences passed to SortedMerge are empty, the result is an empty sequence.
        /// </summary>
        [Test]
        public void TestSortedMergeAllSequencesEmpty()
        {
            var sequenceA = Enumerable.Empty<int>();
            var sequenceB = Enumerable.Empty<int>();
            var sequenceC = Enumerable.Empty<int>();
            var result = sequenceA.SortedMerge(OrderByDirection.Ascending, sequenceB, sequenceC);

            Assert.IsTrue(result.SequenceEqual(sequenceA));
        }

        /// <summary>
        /// Verify that if the primary sequence is empty, SortedMerge correctly merges <c>otherSequences</c>
        /// </summary>
        [Test]
        public void TestSortedMergeFirstSequenceEmpty()
        {
            var sequenceA = Enumerable.Empty<int>();
            var sequenceB = new[] { 1, 3, 5, 7, 9, 11 };
            var sequenceC = new[] { 2, 4, 6, 8, 10, 12 };
            var expectedResult = Enumerable.Range(1, 12);
            var result = sequenceA.SortedMerge(OrderByDirection.Ascending, sequenceB, sequenceC);

            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        /// <summary>
        /// Verify that SortedMerge correctly merges sequences of equal length.
        /// </summary>
        [Test]
        public void TestSortedMergeEqualLengthSequences()
        {
            const int count = 10;
            var sequenceA = Enumerable.Range(0, count).Select(x => x * 3 + 0);
            var sequenceB = Enumerable.Range(0, count).Select(x => x * 3 + 1);
            var sequenceC = Enumerable.Range(0, count).Select(x => x * 3 + 2);
            var expectedResult = Enumerable.Range(0, count * 3);
            var result = sequenceA.SortedMerge(OrderByDirection.Ascending, sequenceB, sequenceC);

            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        /// <summary>
        /// Verify that sorted merge correctly merges sequences of unequal length.
        /// </summary>
        [Test]
        public void TestSortedMergeUnequalLengthSequences()
        {
            const int count = 30;
            var sequenceA = Enumerable.Range(0, count).Select(x => x * 3 + 0);
            var sequenceB = Enumerable.Range(0, count).Select(x => x * 3 + 1).Take(count / 2);
            var sequenceC = Enumerable.Range(0, count).Select(x => x * 3 + 2).Take(count / 3);
            var expectedResult = sequenceA.Concat(sequenceB).Concat(sequenceC).OrderBy(x => x);
            var result = sequenceA.SortedMerge(OrderByDirection.Ascending, sequenceB, sequenceC);

            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        /// <summary>
        /// Verify that sorted merge correctly merges descending-ordered sequences.
        /// </summary>
        [Test]
        public void TestSortedMergeDescendingOrder()
        {
            const int count = 10;
            var sequenceA = Enumerable.Range(0, count).Select(x => x * 3 + 0).Reverse();
            var sequenceB = Enumerable.Range(0, count).Select(x => x * 3 + 1).Reverse();
            var sequenceC = Enumerable.Range(0, count).Select(x => x * 3 + 2).Reverse();
            var expectedResult = Enumerable.Range(0, count * 3).Reverse();
            var result = sequenceA.SortedMerge(OrderByDirection.Descending, sequenceB, sequenceC);

            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        /// <summary>
        /// Verify that sorted merge correctly uses a custom comparer supplied to it.
        /// </summary>
        [Test]
        public void TestSortedMergeCustomComparer()
        {
            var sequenceA = new[] { "a", "D", "G", "h", "i", "J", "O", "t", "z" };
            var sequenceB = new[] { "b", "E", "k", "q", "r", "u", "V", "x", "Y" };
            var sequenceC = new[] { "C", "F", "l", "m", "N", "P", "s", "w" };
            var expectedResult = sequenceA.Concat(sequenceB).Concat(sequenceC)
                                          .OrderBy(a => a, StringComparer.CurrentCultureIgnoreCase);
            var result = sequenceA.SortedMerge(OrderByDirection.Ascending, sequenceB, sequenceC);

            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        /// <summary>
        /// Verify that sorted merge disposes enumerators of all sequences that are passed to it.
        /// </summary>
        [Test]
        public void TestSortedMergeAllSequencesDisposed()
        {
            var disposedSequenceA = false;
            var disposedSequenceB = false;
            var disposedSequenceC = false;
            var disposedSequenceD = false;

            const int count = 10;
            var sequenceA = Enumerable.Range(1, count).AsVerifiable().WhenDisposed(s => disposedSequenceA = true);
            var sequenceB = Enumerable.Range(1, count - 1).AsVerifiable().WhenDisposed(s => disposedSequenceB = true);
            var sequenceC = Enumerable.Range(1, count - 5).AsVerifiable().WhenDisposed(s => disposedSequenceC = true);
            var sequenceD = Enumerable.Range(1, 0).AsVerifiable().WhenDisposed(s => disposedSequenceD = true);

            var result = sequenceA.SortedMerge(OrderByDirection.Ascending, sequenceB, sequenceC, sequenceD);
            result.Count(); // ensures the sequences are actually merged and iterators are obtained

            Assert.IsTrue(disposedSequenceA && disposedSequenceB && disposedSequenceC && disposedSequenceD);
        }
    }
}