namespace MoreLinq.Test
{
    using System.Linq;
    using NUnit.Framework;

    /// <summary>
    /// Verify the behavior of the Exclude operator
    /// </summary>
    [TestFixture]
    public class ExcludeTests
    {
        /// <summary>
        /// Verify that Exclude behaves in a lazy manner
        /// </summary>
        [Test]
        public void TestExcludeIsLazy()
        {
            new BreakingSequence<int>().Exclude(0, 10);
        }

        /// <summary>
        /// Verify that a negative startIndex parameter results in an exception
        /// </summary>
        [Test]
        public void TestExcludeNegativeStartIndexException()
        {
            AssertThrowsArgument.OutOfRangeException("startIndex",() =>
                Enumerable.Range(1, 10).Exclude(-10, 10));
        }

        /// <summary>
        /// Verify that a negative count parameter results in an exception
        /// </summary>
        [Test]
        public void TestExcludeNegativeCountException()
        {
            AssertThrowsArgument.OutOfRangeException("count",() =>
                Enumerable.Range(1, 10).Exclude(0, -5));
        }

        /// <summary>
        /// Verify that excluding with count equals zero returns the original source
        /// </summary>
        [Test]
        public void TestExcludeWithCountEqualsZero()
        {
            var sequence = Enumerable.Range(1, 10);
            var resultA = sequence.Exclude(5, 0);

            Assert.That(resultA, Is.SameAs(sequence));
        }

        /// <summary>
        /// Verify that excluding from an empty sequence results in an empty sequence
        /// </summary>
        [Test]
        public void TestExcludeEmptySequence()
        {
            var sequence = Enumerable.Empty<int>();
            var resultA = sequence.Exclude(0, 0);
            var resultB = sequence.Exclude(0, 10); // shouldn't matter how many we ask for past end
            var resultC = sequence.Exclude(5, 5);  // shouldn't matter where we start
            Assert.IsTrue(resultA.SequenceEqual(sequence));
            Assert.IsTrue(resultB.SequenceEqual(sequence));
            Assert.IsTrue(resultC.SequenceEqual(sequence));
        }

        /// <summary>
        /// Verify we can exclude the beginning portion of a sequence
        /// </summary>
        [Test]
        public void TestExcludeSequenceHead()
        {
            const int count = 10;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Exclude(0, count / 2);

            Assert.IsTrue(result.SequenceEqual(sequence.Skip(count / 2)));
        }

        /// <summary>
        /// Verify we can exclude the tail portion of a sequence
        /// </summary>
        [Test]
        public void TestExcludeSequenceTail()
        {
            const int count = 10;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Exclude(count / 2, count);

            Assert.IsTrue(result.SequenceEqual(sequence.Take(count / 2)));
        }

        /// <summary>
        /// Verify we can exclude the middle portion of a sequence
        /// </summary>
        [Test]
        public void TestExcludeSequenceMiddle()
        {
            const int count = 10;
            const int startIndex = 3;
            const int excludeCount = 5;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Exclude(startIndex, excludeCount);

            Assert.IsTrue(result.SequenceEqual(sequence.Take(startIndex).Concat(sequence.Skip(startIndex + excludeCount))));
        }

        /// <summary>
        /// Verify that excluding the entire sequence results in an empty sequence
        /// </summary>
        [Test]
        public void TestExcludeEntireSequence()
        {
            const int count = 10;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Exclude(0, count);

            Assert.IsTrue(result.SequenceEqual(Enumerable.Empty<int>()));
        }

        /// <summary>
        /// Verify that excluding past the end on a sequence excludes the appropriate elements
        /// </summary>
        [Test]
        public void TestExcludeCountGreaterThanSequenceLength()
        {
            const int count = 10;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Exclude(1, count * 10);

            Assert.IsTrue(result.SequenceEqual(sequence.Take(1)));
        }

        /// <summary>
        /// Verify that beginning exclusion past the end of a sequence has no effect
        /// </summary>
        [Test]
        public void TestExcludeStartIndexGreaterThanSequenceLength()
        {
            const int count = 10;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Exclude(count + 5, count);

            Assert.IsTrue(result.SequenceEqual(sequence));
        }
    }
}