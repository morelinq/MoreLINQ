namespace MoreLinq.Test
{
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
            var result = sequence.Repeat(repeatCount);

            var expectedResult = Enumerable.Empty<int>();
            for (var i = 0; i < repeatCount; i++)
                expectedResult = expectedResult.Concat(sequence);

            Assert.AreEqual(count * repeatCount, result.Count());
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

        [Test]
        public void RepeatDoesNotEnumerateSourceMoreThanOnce()
        {
            var source = Enumerable.Range(1, 10);
            var expectations = source.Concat(source);

            using (var test = source.AsTestingSequence())
            {
                var result = test.Repeat(2);
                Assert.IsTrue(result.SequenceEqual(expectations));
            }
        }

        [Test]
        public void RepeatDisposeSourceWithPartialIteration()
        {
            var source = Enumerable.Range(1, 10);
            var expectations = source.Concat(source);

            using (var test = source.AsTestingSequence())
            {
                test.Repeat(2).Take(5).Consume();
            }
        }

        /// <summary>
        /// Verify applying Repeat without passing count produces a circular sequence
        /// </summary>
        [Test]
        public void TestRepeatForeverBehaviorSingleElementList()
        {
            var value = 3;

            var result = new[] { value }.Repeat();

            Assert.IsTrue(result.Take(100).All(x => x == value));
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
            var result = sequence.Repeat();

            var expectedResult = Enumerable.Empty<int>();
            for (var i = 0; i < repeatCount; i++)
                expectedResult = expectedResult.Concat(sequence);

            Assert.That(expectedResult, Is.EquivalentTo(result.Take(takeCount)));
        }

        /// <summary>
        /// Verify that the repeat method returns results in a lazy manner.
        /// </summary>
        [Test]
        public void TestRepeatForeverIsLazy()
        {
            new BreakingSequence<int>().Repeat();
        }

        [Test]
        public void RepeatForeverDoesNotEnumerateSourceMoreThanOnce()
        {
            var source = new[] { 3 };

            using (var test = source.AsTestingSequence())
            {
                var result = test.Repeat();
                Assert.IsTrue(result.Take(100).All(x => x == 3));
            }
        }
    }
}
