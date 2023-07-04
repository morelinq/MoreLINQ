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
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

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

            _ = sequenceA.SortedMerge(OrderByDirection.Ascending, sequenceB);
        }

        /// <summary>
        /// Verify that SortedMerge disposes those enumerators that it managed
        /// to open successfully
        /// </summary>
        [Test]
        public void TestSortedMergeDisposesOnError()
        {
            using var sequenceA = TestingSequence.Of<int>();

            // Expected and thrown by BreakingSequence
            Assert.That(() => sequenceA.SortedMerge(OrderByDirection.Ascending, new BreakingSequence<int>())
                                       .Consume(),
                        Throws.BreakException);
        }

        /// <summary>
        /// Verify that SortedMerge do not call MoveNext method eagerly
        /// </summary>
        [Test]
        public void TestSortedMergeDoNotCallMoveNextEagerly()
        {
            using var sequenceA = TestingSequence.Of(1, 3);
            using var sequenceB = MoreEnumerable.From(() => 2, () => throw new TestException())
                                                .AsTestingSequence();

            var result = sequenceA.SortedMerge(OrderByDirection.Ascending, sequenceB).Take(2);

            Assert.That(() => result.Consume(), Throws.Nothing);
        }

        /// <summary>
        /// Verify that SortedMerge throws an exception if invoked on a <c>null</c> sequence.
        /// </summary>
        [Test]
        public void TestSortedMergeComparerNull()
        {
            var sequenceA = Enumerable.Range(1, 3);
            var sequenceB = Enumerable.Range(4, 3);
            var result = sequenceA.SortedMerge(OrderByDirection.Ascending, (IComparer<int>?)null, sequenceB);

            Assert.That(result, Is.EqualTo(sequenceA.Concat(sequenceB)));
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

            Assert.That(result, Is.EqualTo(sequenceA));
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

            Assert.That(result, Is.EqualTo(sequenceA));
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

            Assert.That(result, Is.EqualTo(expectedResult));
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

            Assert.That(result, Is.EqualTo(expectedResult));
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

            Assert.That(result, Is.EqualTo(expectedResult));
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

            Assert.That(result, Is.EqualTo(expectedResult));
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
            var comparer = StringComparer.InvariantCultureIgnoreCase;
            var expectedResult = sequenceA.Concat(sequenceB).Concat(sequenceC)
                                          .OrderBy(a => a, comparer);
            var result = sequenceA.SortedMerge(OrderByDirection.Ascending, comparer, sequenceB, sequenceC);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Verify that sorted merge disposes enumerators of all sequences that are passed to it.
        /// </summary>
        [Test]
        public void TestSortedMergeAllSequencesDisposed()
        {
            const int count = 10;

            using var sequenceA = Enumerable.Range(1, count).AsTestingSequence();
            using var sequenceB = Enumerable.Range(1, count - 1).AsTestingSequence();
            using var sequenceC = Enumerable.Range(1, count - 5).AsTestingSequence();
            using var sequenceD = Enumerable.Range(1, 0).AsTestingSequence();

            sequenceA.SortedMerge(OrderByDirection.Ascending, sequenceB, sequenceC, sequenceD)
                     .Consume(); // ensures the sequences are actually merged and iterators are obtained
        }
    }
}
