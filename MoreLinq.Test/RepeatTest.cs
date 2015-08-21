using System;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    /// <summary>
    /// Tests that verify the Repeat() extension method.
    /// </summary>
    [TestFixture]
    public class RepeatTest
    {
        /// <summary>
        /// Verify that the repeat method returns results in a lazy manner.
        /// </summary>
        [Test]
        public void TestRepeatIsLazy()
        {
            new BreakingSequence<int>().Repeat(5);
        }

        /// <summary>
        /// Verify that repeat will yield the expected number of items.
        /// </summary>
        [Test]
        public void TestRepeatBehavior()
        {
            const int count = 10;
            const int repeatCount = 3;
            var sequence = Enumerable.Range(1, 10);
            var result = sequence.Repeat(repeatCount);

            var expectedResult = Enumerable.Empty<int>();
            for (var i = 0; i < repeatCount; i++)
                expectedResult = expectedResult.Concat(sequence);

            Assert.AreEqual(count * repeatCount, result.Count());
            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        /// <summary>
        /// Verify that repeat throws an exception when the repeat count is negative.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestNegativeRepeatCount()
        {
            Enumerable.Range(1, 10).Repeat(-3);
        }

        /// <summary>
        /// Verify applying Repeat to a <c>null</c> sequence results in an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRepeatSequenceANullException()
        {
            MoreEnumerable.Repeat<object>(null, 42);
        }
    }
}