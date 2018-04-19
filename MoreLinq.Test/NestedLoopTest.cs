namespace MoreLinq.Test
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Tests that verify the behavior of the NestedLoops extension method.
    /// </summary>
    [TestFixture]
    public class NestedLoopTest
    {
        [Test]
        public void NestedLoopsIsLazy()
        {
            BreakingAction.WithoutArguments.NestedLoops(new BreakingSequence<int>());
        }

        /// <summary>
        /// Verify that passing negative loop counts results in an exception
        /// </summary>
        [Test]
        public void NestedLoopWithFirstElementNegative()
        {
            AssertThrowsArgument.Exception("loopCounts", () =>
                BreakingAction.WithoutArguments.NestedLoops(Enumerable.Range(-10, 10))
                                               .ElementAt(0));
        }

        [Test]
        public void NestedLoopWithLastElementNegative()
        {
            AssertThrowsArgument.Exception("loopCounts", () =>
                BreakingAction.WithoutArguments.NestedLoops(MoreEnumerable.Sequence(10, -1))
                                               .ElementAt(0));
        }

        /// <summary>
        /// Verify that the number of elements in a NestedLoop sequence is equal
        /// to the product of the loop count values.
        /// </summary>
        [Test]
        public void TestNestedLoopSequenceLength()
        {
            var i = 0;
            Action loopBody = () => ++i;

            const int count = 6;
            var expectedCount = Combinatorics.Factorial(count);

            var loopCounts = Enumerable.Range(1, count);
            var nestedLoops = loopBody.NestedLoops(loopCounts.AsTestingSequence()).ToList();

            nestedLoops.ForEach( act => act() ); // perform all actions

            Assert.AreEqual( expectedCount, i );
            Assert.AreEqual( expectedCount, nestedLoops.Count );
            Assert.IsTrue( nestedLoops.All( act => act == loopBody ));
        }

        [Test]
        public void NestedLoopWithEmptySequence()
        {
            var result = BreakingAction.WithoutArguments.NestedLoops(new int[0]);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void NestedLoopContainingZero()
        {
            var result = BreakingAction.WithoutArguments.NestedLoops(new[] { 3, 2, 1, 0, 4 });

            Assert.That(result, Is.Empty);
        }
    }
}
