namespace MoreLinq.Test
{
    using System.Collections.Generic;
    using NUnit.Framework;

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

            int[] result;
            using (var ts = sequence.AsTestingSequence())
                result = ts.Repeat(repeatCount).ToArray();

            var expectedResult = Enumerable.Empty<int>();
            for (var i = 0; i < repeatCount; i++)
                expectedResult = expectedResult.Concat(sequence);

            Assert.That(result.Length, Is.EqualTo(count * repeatCount));
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Verify that repeat throws an exception when the repeat count is negative.
        /// </summary>
        [Test]
        public void TestNegativeRepeatCount()
        {
            AssertThrowsArgument.OutOfRangeException("count", () =>
                 Enumerable.Range(1, 10).Repeat(-3));
        }

        /// <summary>
        /// Verify applying Repeat without passing count produces a circular sequence
        /// </summary>
        [Test]
        public void TestRepeatForeverBehaviorSingleElementList()
        {
            const int value = 3;
            using (var sequence = new[] { value }.AsTestingSequence())
            {
                var result = sequence.Repeat();
                Assert.IsTrue(result.Take(100).All(x => x == value));
            }
        }

        /// <summary>
        /// Verify applying Repeat without passing count produces a circular sequence
        /// </summary>
        [Test]
        public void TestRepeatForeverBehaviorManyElementsList()
        {
            const int repeatCount = 30;
            const int rangeCount = 10;
            const int takeCount = repeatCount * rangeCount;

            var sequence = Enumerable.Range(1, rangeCount);

            int[] result;
            using (var ts = sequence.AsTestingSequence())
                result = ts.Repeat().Take(takeCount).ToArray();

            var expectedResult = Enumerable.Empty<int>();
            for (var i = 0; i < repeatCount; i++)
                expectedResult = expectedResult.Concat(sequence);

            Assert.That(expectedResult, Is.EquivalentTo(result));
        }

        /// <summary>
        /// Verify that the repeat method returns results in a lazy manner.
        /// </summary>
        [Test]
        public void TestRepeatForeverIsLazy()
        {
            new BreakingSequence<int>().Repeat();
        }
    }
}
