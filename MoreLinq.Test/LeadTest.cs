using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    /// <summary>
    /// Verify the behavior of the Lead operator.
    /// </summary>
    [TestFixture]
    public class LeadTests
    {
        /// <summary>
        /// Verify that Lead() behaves in a lazy manner.
        /// </summary>
        [Test]
        public void TestLeadIsLazy()
        {
            new BreakingSequence<int>().Lead(5, (a, b) => a);
            new BreakingSequence<int>().Lead(5, -1, (a, b) => a);
        }

        /// <summary>
        /// Verify that Lead throws an exception if invoked on a <c>null</c> sequence.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLeadNullSequenceException()
        {
            const IEnumerable<int> sequence = null;
            sequence.Lead(5, (val, leadVal) => val);
        }

        /// <summary>
        /// Verify that attempting to lead by a negative offset will result in an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestLeadNegativeOffset()
        {
            Enumerable.Range(1, 100).Lead(-5, (val, leadVal) => val + leadVal);
        }

        /// <summary>
        /// Verify that attempting to lead by a zero offset will result in an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestLeadZeroOffset()
        {
            Enumerable.Range(1, 100).Lead(0, (val, leadVal) => val + leadVal);
        }

        /// <summary>
        /// Verify that lead can accept and propagate a default value passed to it.  
        /// </summary>
        [Test]
        public void TestLeadExplicitDefaultValue()
        {
            const int count = 100;
            const int leadBy = 10;
            const int leadDefault = -1;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lead(leadBy, leadDefault, (val, leadVal) => leadVal);

            Assert.AreEqual(count, result.Count());
            Assert.IsTrue(result.Skip(count - leadBy).SequenceEqual(Enumerable.Repeat(leadDefault, leadBy)));
        }

        /// <summary>
        /// Verify that Lead() willuse default(T) if a specific default value is not supplied for the lead value.
        /// </summary>
        [Test]
        public void TestLeadImplicitDefaultValue()
        {
            const int count = 100;
            const int leadBy = 10;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lead(leadBy, (val, leadVal) => leadVal);

            Assert.AreEqual(count, result.Count());
            Assert.IsTrue(result.Skip(count - leadBy).SequenceEqual(Enumerable.Repeat(default(int), leadBy)));
        }

        /// <summary>
        /// Verify that if the lead offset is greater than the length of the sequence
        /// Lead() still yield all of the elements of the source sequence.
        /// </summary>
        [Test]
        public void TestLeadOffsetGreaterThanSequenceLength()
        {
            const int count = 100;
            const int leadDefault = -1;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lead(count + 1, leadDefault, (val, leadVal) => new { A = val, B = leadVal });

            Assert.AreEqual(count, result.Count());
            Assert.IsTrue(result.SequenceEqual(sequence.Select(x => new { A = x, B = leadDefault })));
        }

        /// <summary>
        /// Verify that Lead() actually yields the correct pair of values from the sequence
        /// when the lead offset is 1.
        /// </summary>
        [Test]
        public void TestLeadPassesCorrectValueOffsetBy1()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lead(1, count + 1, (val, leadVal) => new { A = val, B = leadVal });

            Assert.AreEqual(count, result.Count());
            Assert.IsTrue(result.All(x => x.B == (x.A + 1)));
        }

        /// <summary>
        /// Verify that Lead() yields the correct pair of values from the sequence
        /// when the lead offset is greater than 1.
        /// </summary>
        [Test]
        public void TestLeadPassesCorrectValueOffsetBy2()
        {
            const int count = 100;
            const int leadDefault = count + 1;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lead(2, leadDefault, (val, leadVal) => new { A = val, B = leadVal });

            Assert.AreEqual(count, result.Count());
            Assert.IsTrue(result.Take(count - 2).All(x => x.B == (x.A + 2)));
            Assert.IsTrue(result.Skip(count - 2).All(x => x.B == leadDefault && (x.A == count || x.A == count - 1)));
        }
    }
}