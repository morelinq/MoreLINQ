using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    /// <summary>
    /// Verify the behavior of the Rank operator
    /// </summary>
    [TestFixture]
    public class RankTests
    {
        /// <summary>
        /// Verify that rank behaves in a lazy manner.
        /// </summary>
        [Test]
        public void TestRankIsLazy()
        {
            new BreakingSequence<int>().Rank();
            new BreakingSequence<int>().RankBy(x => x);
        }

        /// <summary>
        /// Verify that Rank throws an exception when invoked on a <c>null</c> sequence
        /// </summary>
        [Test]
        public void TestRankSequenceNullException()
        {
            const IEnumerable<int> sequence = null;

            AssertUtil.Throws<ArgumentNullException>(() => sequence.Rank());
            AssertUtil.Throws<ArgumentNullException>(() => sequence.Rank(Comparer<int>.Default));
            AssertUtil.Throws<ArgumentNullException>(() => sequence.RankBy(x => x));
            AssertUtil.Throws<ArgumentNullException>(() => sequence.RankBy(x => x, Comparer<int>.Default));
        }

        /// <summary>
        /// Verify that Rank throws an exception if the key selector is <c>null</c>
        /// </summary>
        [Test]
        public void TestRankKeySelectorNullException()
        {
            var sequence = Enumerable.Repeat(1, 10);

            AssertUtil.Throws<ArgumentNullException>(() => sequence.RankBy((Func<int, int>)null));
            AssertUtil.Throws<ArgumentNullException>(() => sequence.RankBy((Func<int, int>)null, null));
        }

        /// <summary>
        /// Verify that Rank uses the default comparer when comparer is <c>null</c>
        /// </summary>
        [Test]
        public void TestRankNullComparer()
        {
            var sequence = Enumerable.Repeat(1, 10);
            sequence.AsTestingSequence().Rank(null).AssertSequenceEqual(sequence.ToArray());
            sequence.AsTestingSequence().RankBy(x => x, null).AssertSequenceEqual(sequence.ToArray());
        }
        
        /// <summary>
        /// Verify that ranking a descending series of integers produces
        /// a linear, progressive rank for each value.
        /// </summary>
        [Test]
        public void TestRankDescendingSequence()
        {
            const int count = 100;
            var sequence = Enumerable.Range(456, count).Reverse();
            var result = sequence.AsTestingSequence().Rank().ToArray();
            var expectedResult = Enumerable.Range(1, count);

            Assert.AreEqual(count, result.Length);
            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        /// <summary>
        /// Verify that ranking an ascending series of integers produces
        /// a linear, regressive rank for each value.
        /// </summary>
        [Test]
        public void TestRankByAscendingSeries()
        {
            const int count = 100;
            var sequence = Enumerable.Range(456, count);
            var result = sequence.AsTestingSequence().Rank().ToArray();
            var expectedResult = Enumerable.Range(1, count).Reverse();

            Assert.AreEqual(count, result.Length);
            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        /// <summary>
        /// Verify that the rank of a sequence of the same item is always 1.
        /// </summary>
        [Test]
        public void TestRankEquivalentItems()
        {
            const int count = 100;
            var sequence = Enumerable.Repeat(1234, count);
            var result = sequence.AsTestingSequence().Rank().ToArray();

            Assert.AreEqual(count, result.Length);
            Assert.IsTrue(result.SequenceEqual(Enumerable.Repeat(1, count)));
        }

        /// <summary>
        /// Verify that the rank of equivalent items in a sequence is the same.
        /// </summary>
        [Test]
        public void TestRankGroupedItems()
        {
            const int count = 10;
            var sequence = Enumerable.Range(0, count)
                .Concat(Enumerable.Range(0, count))
                .Concat(Enumerable.Range(0, count));
            var result = sequence.AsTestingSequence().Rank().ToArray();

            Assert.AreEqual(count, result.Distinct().Count());
            Assert.IsTrue(result.SequenceEqual(sequence.Reverse().Select(x => x + 1)));
        }

        /// <summary>
        /// Verify that the highest rank (that of the largest item) is 1 (not 0).
        /// </summary>
        [Test]
        public void TestRankOfHighestItemIsOne()
        {
            const int count = 10;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.AsTestingSequence().Rank();

            Assert.AreEqual(1, result.OrderBy(x => x).First());
        }

        /// <summary>
        /// Verify that we can rank items by an arbitrary key produced from the item.
        /// </summary>
        [Test]
        public void TestRankByKeySelector()
        {
            var sequence = new[]
                               {
                                   new { Name = "Bob", Age = 24, ExpectedRank = 5 },
                                   new { Name = "Sam", Age = 51, ExpectedRank = 2 },
                                   new { Name = "Kim", Age = 18, ExpectedRank = 7 },
                                   new { Name = "Tim", Age = 23, ExpectedRank = 6 },
                                   new { Name = "Joe", Age = 31, ExpectedRank = 3 },
                                   new { Name = "Mel", Age = 28, ExpectedRank = 4 },
                                   new { Name = "Jim", Age = 74, ExpectedRank = 1 },
                                   new { Name = "Jes", Age = 11, ExpectedRank = 8 },
                               };
            var result = sequence.AsTestingSequence().RankBy(x => x.Age).ToArray();

            Assert.AreEqual(sequence.Count(), result.Length);
            Assert.IsTrue(result.SequenceEqual(sequence.Select(x => x.ExpectedRank)));
        }

        /// <summary>
        /// Verify that Rank can use a custom comparer
        /// </summary>
        [Test]
        public void TestRankCustomComparer()
        {
            const int count = 10;
            var ordinals = Enumerable.Range(1, count);
            var sequence = ordinals.Select( x => new DateTime(2010,x,20-x) );
            // invert the CompareTo operation to Rank in reverse order (ascening to descending)
            var resultA = sequence.AsTestingSequence().Rank(ComparerFunc<DateTime>.As((a, b) => -a.CompareTo(b)));
            var resultB = sequence.AsTestingSequence().RankBy(x => x.Day, ComparerFunc<int>.As((a, b) => -a.CompareTo(b)));

            Assert.IsTrue(resultA.SequenceEqual(ordinals));
            Assert.IsTrue(resultB.SequenceEqual(ordinals.Reverse()));
        }
    }
}