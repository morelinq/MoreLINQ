using System;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    /// <summary>
    /// Tests that verify the behavior of the NestedLoops extension method.
    /// </summary>
    [TestFixture]
    public class NestedLoopTest
    {
        private static void DoNothing() { }

        private static readonly Action EmptyLoopBody = DoNothing;

        /// <summary>
        /// Verify that passing an empty loop count sequence results in an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullLoopCountsException()
        {
            EmptyLoopBody.NestedLoops(null);
        }

        /// <summary>
        /// Verify that passing negative loop counts results in an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNegativeLoopCountsException()
        {
            EmptyLoopBody.NestedLoops(Enumerable.Range(-10, 10));
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
            var nestedLoops = loopBody.NestedLoops(loopCounts.AsTestingSequence());

            nestedLoops.ForEach( act => act() ); // perform all actions

            Assert.AreEqual( expectedCount, i );
            Assert.AreEqual( expectedCount, nestedLoops.Count() );
            Assert.IsTrue( nestedLoops.All( act => act == loopBody ));
        }
    }
}
