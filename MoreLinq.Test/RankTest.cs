namespace MoreLinq.Test
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

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
        }

        /// <summary>
        /// Verify that rank behaves in a lazy manner.
        /// </summary>
        [Test]
        public void TestRankByIsLazy()
        {
            new BreakingSequence<int>().RankBy(x => x);
        }

        /// <summary>
        /// Verify that Rank uses the default comparer when comparer is <c>null</c>
        /// </summary>
        [Test]
        public void TestRankNullComparer()
        {
            var sequence = Enumerable.Repeat(1, 10);
            sequence.UsingTestingSequence(ts => ts.Rank(null))
                    .AssertSequenceEqual(sequence);
        }

        /// <summary>
        /// Verify that Rank uses the default comparer when comparer is <c>null</c>
        /// </summary>
        [Test]
        public void TestRankByNullComparer()
        {
            var sequence = Enumerable.Repeat(1, 10);
            sequence.UsingTestingSequence(ts => ts.RankBy(x => x, null))
                    .AssertSequenceEqual(sequence);
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

            var result = sequence.UsingTestingSequence(ts => ts.Rank());

            Assert.AreEqual(count, result.Length);
            Assert.IsTrue(result.SequenceEqual(Enumerable.Range(1, count)));
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

            var result = sequence.UsingTestingSequence(ts => ts.Rank());

            Assert.AreEqual(count, result.Length);
            Assert.IsTrue(result.SequenceEqual(Enumerable.Range(1, count).Reverse()));
        }

        /// <summary>
        /// Verify that the rank of a sequence of the same item is always 1.
        /// </summary>
        [Test]
        public void TestRankEquivalentItems()
        {
            const int count = 100;
            var sequence = Enumerable.Repeat(1234, count);
            var result = sequence.UsingTestingSequence(ts => ts.Rank());

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

            var result = sequence.UsingTestingSequence(ts => ts.Rank());

            Assert.AreEqual(count, result.Distinct().Count());
            Assert.IsTrue(result.SequenceEqual(sequence.Reverse().Select(x => x + 1)));
        }

        /// <summary>
        /// Verify that the highest rank (that of the largest item) is 1 (not 0).
        /// </summary>
        [Test]
        public void TestRankOfHighestItemIsOne()
        {
            using (var ts = Enumerable.Range(1, 10).AsTestingSequence())
                Assert.AreEqual(1, ts.Rank().OrderBy(x => x).First());
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
            var result = sequence.UsingTestingSequence(ts =>
                ts.RankBy(x => x.Age));

            Assert.AreEqual(sequence.Length, result.Length);
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
            var resultA = sequence.UsingTestingSequence(ts => ts.Rank(Comparer.Create<DateTime>((a, b) => -a.CompareTo(b))));
            var resultB = sequence.UsingTestingSequence(ts => ts.RankBy(x => x.Day, Comparer.Create<int>((a, b) => -a.CompareTo(b))));

            Assert.IsTrue(resultA.SequenceEqual(ordinals));
            Assert.IsTrue(resultB.SequenceEqual(ordinals.Reverse()));
        }
    }
}
