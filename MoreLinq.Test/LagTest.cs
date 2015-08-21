using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    /// <summary>
    /// Verify the behavior of the Lag operator
    /// </summary>
    [TestFixture]
    public class LagTests
    {
        /// <summary>
        /// Verify that lag behaves in a lazy manner.
        /// </summary>
        [Test]
        public void TestLagIsLazy()
        {
            new BreakingSequence<int>().Lag(5, (a, b) => a);
            new BreakingSequence<int>().Lag(5, -1, (a, b) => a);
        }

        /// <summary>
        /// Verify that lag throws an exception if invoked on a <c>null</c> sequence
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLagNullSequenceException()
        {
            const IEnumerable<int> sequence = null;
            sequence.Lag(10, (val, lagVal) => val);
        }

        /// <summary>
        /// Verify that lagging by a negative offset results in an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestLagNegativeOffsetException()
        {
            Enumerable.Repeat(1, 10).Lag(-10, (val, lagVal) => val);
        }

        /// <summary>
        /// Verify that attempting to lag by a zero offset will result in an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestLagZeroOffset()
        {
            Enumerable.Range(1, 10).Lag(0, (val, lagVal) => val + lagVal);
        }

        /// <summary>
        /// Verify that lag can accept an propagate a default value passed to it.
        /// </summary>
        [Test]
        public void TestLagExplicitDefaultValue()
        {
            const int count = 100;
            const int lagBy = 10;
            const int lagDefault = -1;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lag(lagBy, lagDefault, (val, lagVal) => lagVal);

            Assert.AreEqual(count, result.Count());
            Assert.IsTrue(result.Take(lagBy).SequenceEqual(Enumerable.Repeat(lagDefault, lagBy)));
        }

        /// <summary>
        /// Verify that lag will use default(T) if a specific default value is not supplied for the lag value.
        /// </summary>
        [Test]
        public void TestLagImplicitDefaultValue()
        {
            const int count = 100;
            const int lagBy = 10;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lag(lagBy, (val, lagVal) => lagVal);

            Assert.AreEqual(count, result.Count());
            Assert.IsTrue(result.Take(lagBy).SequenceEqual(Enumerable.Repeat(default(int), lagBy)));
        }

        /// <summary>
        /// Verify that if the lag offset is greater than the sequence length lag
        /// still yields all of the elements of the source sequence.
        /// </summary>
        [Test]
        public void TestLagOffsetGreaterThanSequenceLength()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lag(count + 1, (a, b) => a);

            Assert.AreEqual(count, result.Count());
            Assert.IsTrue(result.SequenceEqual(sequence));
        }

        /// <summary>
        /// Verify that lag actually yields the correct pair of values from the sequence
        /// when offsetting by a single item.
        /// </summary>
        [Test]
        public void TestLagPassesCorrectLagValueOffsetBy1()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lag(1, (a, b) => new { A = a, B = b });

            Assert.AreEqual(count, result.Count());
            Assert.IsTrue(result.All(x => x.B == (x.A - 1)));
        }

        /// <summary>
        /// Verify that lag yields the correct pair of values from the sequence when
        /// offsetting by more than a single item.
        /// </summary>
        [Test]
        public void TestLagPassesCorrectLagValuesOffsetBy2()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lag(2, (a, b) => new { A = a, B = b });

            Assert.AreEqual(count, result.Count());
            Assert.IsTrue(result.Skip(2).All(x => x.B == (x.A - 2)));
            Assert.IsTrue(result.Take(2).All(x => (x.A - x.B) == x.A));
        }
    }
}