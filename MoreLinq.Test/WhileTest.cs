using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    /// <summary>
    /// Verify the behavior of the While operator.
    /// </summary>
    [TestFixture]
    public class WhileTests
    {
        /// <summary>
        /// Verify that the While operator behaves in a lazy manner.
        /// </summary>
        [Test]
        public void TestWhileIsLazy()
        {
            var sequence = new BreakingSequence<int>();
            sequence.While((prevValue, value, index) => true, x => x);
        }

        /// <summary>
        /// Verify that invoking While on a <c>null</c> sequence results in an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWhileNullSequenceException()
        {
            const IEnumerable<int> sequence = null;
            var result = sequence.While((prev, curr, index) => true, x => x);
        }

        /// <summary>
        /// Verify that passing a <c>null</c> condition to While results in an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWhileNullConditionException()
        {
            Enumerable.Repeat(1, 10).While(null, x => x);
        }

        /// <summary>
        /// Verify that passing a <c>null</c> projection to While results in an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWhileNullProjectionException()
        {
            Enumerable.Repeat(1, 10).While<int,int>((prev,curr,index) => true, null);
        }

        /// <summary>
        /// Verify that While can correctly evaluate an incremental condition
        /// </summary>
        [Test]
        public void TestWhileIncrementalCondition()
        {
            var sequence = new[] { 1, 2, 4, 8, 16, 32, 64, 128 };
            var result = sequence.While((prev, curr, index) => (curr - prev) < 30, x => x);
            Assert.IsTrue(result.SequenceEqual(sequence.Where(x => x < 64)));
        }
    }
}