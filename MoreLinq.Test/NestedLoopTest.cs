namespace MoreLinq.Test
{
    using System;
    using NUnit.Framework;
    using static MoreEnumerable;

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
        public void TestNegativeLoopCountsException()
        {
            AssertThrowsArgument.Exception("loopCounts", () =>
                BreakingAction.WithoutArguments.NestedLoops(Enumerable.Range(-10, 10))
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
        public void TestNestedLoopConsumesSequenceLazily()
        {
            var i = 0;
            Action loopBody = () => ++i;

            const int count = 4;
            var expectedCount = (int) Combinatorics.Factorial(count);

            var loopCounts = Enumerable.Range(1, count)
                                       .Concat(From<int>(() => throw new TestException()));

            var nestedLoops = loopBody.NestedLoops(loopCounts);

            Assert.AreEqual( expectedCount, nestedLoops.Take(expectedCount).Count() );
            Assert.Throws<TestException>(() => nestedLoops.ElementAt(expectedCount));
        }
    }
}
