using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MoreLinq.Test
{
    [TestFixture]
    public class BatchTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BatchNullSequence()
        {
            Enumerable.Batch<object>(null, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchZeroSize()
        {
            Enumerable.Batch(new object[0], 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchNegativeSize()
        {
            Enumerable.Batch(new object[0], -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BatcWithhNullResultSelector()
        {
            Enumerable.Batch<object, object>(new object[0], 1, null);
        }

        [Test]
        public void BatchEvenlyDivisibleSequence()
        {
            var result = Enumerable.Batch(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 3);
            using (var reader = Read(result))
            {
                reader.Read().AssertSequenceEqual(1, 2, 3);
                reader.Read().AssertSequenceEqual(4, 5, 6);
                reader.Read().AssertSequenceEqual(7, 8, 9);
                reader.ReadEnd();
            }
        }

        [Test]
        public void BatchUnevenlyDivisbleSequence()
        {
            var result = Enumerable.Batch(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 4);
            using (var reader = Read(result))
            {
                reader.Read().AssertSequenceEqual(1, 2, 3, 4);
                reader.Read().AssertSequenceEqual(5, 6, 7, 8);
                reader.Read().AssertSequenceEqual(9);
                reader.ReadEnd();
            }
        }

        [Test]
        public void BatchSequenceTransformingResult()
        {
            var result = Enumerable.Batch(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 4, batch => batch.Sum());
            result.AssertSequenceEqual(10, 26, 9);
        }

        [Test]
        public void BatchSequenceYieldsBatches()
        {
            var result = Enumerable.Batch(new[] { 1, 2, 3 }, 2);
            using (var reader = Read(result))
            {
                Assert.That(reader.Read(), Is.Not.InstanceOfType(typeof(ICollection<int>)));
                Assert.That(reader.Read(), Is.Not.InstanceOfType(typeof(ICollection<int>)));
                reader.ReadEnd();
            }
        }

        [Test]
        public void BatchIsLazy()
        {
            Enumerable.Batch(new BreakingSequence<object>(), 1);
        }

        private static SequenceReader<T> Read<T>(IEnumerable<T> source)
        {
            return new SequenceReader<T>(source);
        }
    }
}
