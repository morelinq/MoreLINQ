namespace MoreLinq.Test
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Verify the behavior of the Interleave operator
    /// </summary>
    [TestFixture]
    public class InterleaveTests
    {
        /// <summary>
        /// Verify that Interleave behaves in a lazy manner
        /// </summary>
        [Test]
        public void TestInterleaveIsLazy()
        {
            new BreakingSequence<int>().Interleave(new BreakingSequence<int>());
        }

        /// <summary>
        /// Verify that interleaving disposes those enumerators that it managed
        /// to open successfully
        /// </summary>
        [Test]
        public void TestInterleaveDisposesOnError()
        {
            using (var sequenceA = TestingSequence.Of<int>())
            {
                Assert.Throws<InvalidOperationException>(() => // Expected and thrown by BreakingSequence
                    sequenceA.Interleave(new BreakingSequence<int>()).Consume());
            }
        }

        /// <summary>
        /// Verify that two balanced sequences will interleave all of their elements
        /// </summary>
        [Test]
        public void TestInterleaveTwoBalancedSequences()
        {
            const int count = 10;
            var sequenceA = Enumerable.Range(1, count);
            var sequenceB = Enumerable.Range(1, count);
            var result = sequenceA.Interleave(sequenceB);

            Assert.That(result, Is.EqualTo(Enumerable.Range(1, count).Select(x => new[] { x, x }).SelectMany(z => z)));
        }

        /// <summary>
        /// Verify that interleaving two empty sequences results in an empty sequence
        /// </summary>
        [Test]
        public void TestInterleaveTwoEmptySequences()
        {
            var sequenceA = Enumerable.Empty<int>();
            var sequenceB = Enumerable.Empty<int>();
            var result = sequenceA.Interleave(sequenceB);

            Assert.That(result, Is.EqualTo(Enumerable.Empty<int>()));
        }

        /// <summary>
        /// Verify that interleaving two unequal sequences with the Skip strategy results in
        /// the shorter sequence being omitted from the interleave operation when consumed
        /// </summary>
        [Test]
        public void TestInterleaveTwoImbalanceStrategySkip()
        {
            var sequenceA = new[] { 0, 0, 0, 0, 0, 0 };
            var sequenceB = new[] { 1, 1, 1, 1 };
            var result = sequenceA.Interleave(sequenceB);

            var expectedResult = new[] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 0 };

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Verify that interleaving multiple empty sequences results in an empty sequence
        /// </summary>
        [Test]
        public void TestInterleaveManyEmptySequences()
        {
            var sequenceA = Enumerable.Empty<int>();
            var sequenceB = Enumerable.Empty<int>();
            var sequenceC = Enumerable.Empty<int>();
            var sequenceD = Enumerable.Empty<int>();
            var sequenceE = Enumerable.Empty<int>();
            var result = sequenceA.Interleave(sequenceB, sequenceC, sequenceD, sequenceE);

            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Verify that interleaving multiple unequal sequences with the Skip strategy
        /// results in sequences being omitted form the interleave operation when consumed
        /// </summary>
        [Test]
        public void TestInterleaveManyImbalanceStrategySkip()
        {
            var sequenceA = new[] { 1, 5, 8, 11, 14, 16, };
            var sequenceB = new[] { 2, 6, 9, 12, };
            var sequenceC = new int[] { };
            var sequenceD = new[] { 3 };
            var sequenceE = new[] { 4, 7, 10, 13, 15, 17, };
            var result = sequenceA.Interleave(sequenceB, sequenceC, sequenceD, sequenceE);

            var expectedResult = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Verify that Interleave disposes of all iterators it creates regardless of which strategy
        /// is used to interleave the sequences
        /// </summary>
        [Test]
        public void TestInterleaveDisposesAllIterators()
        {
            const int count = 10;

            using (var sequenceA = Enumerable.Range(1, count).AsTestingSequence())
            using (var sequenceB = Enumerable.Range(1, count - 1).AsTestingSequence())
            using (var sequenceC = Enumerable.Range(1, count - 5).AsTestingSequence())
            using (var sequenceD = Enumerable.Range(1, 0).AsTestingSequence())
            {
                sequenceA.Interleave(sequenceB, sequenceC, sequenceD)
                         .Consume();
            }
        }
    }
}
