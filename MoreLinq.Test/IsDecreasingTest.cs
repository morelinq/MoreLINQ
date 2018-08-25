using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreLinq.Test
{
    [TestFixture]
    public class IsDecreasingTest
    {
        [Test]
        public void IsDecreasingWithIncreasingSequenceReturnsFalse()
        {
            var sequenceAscending = Enumerable.Range(1, 100);
            var result = sequenceAscending.IsDecreasing<int>();
            Assert.IsFalse(result);
        }

        [Test]
        public void IsDecreasingWithDecreasingSequenceReturnsTrue()
        {
            var sequenceAscending = Enumerable.Range(1, 100).Reverse();
            var result = sequenceAscending.IsDecreasing<int>();
            Assert.IsTrue(result);
        }

        [Test]
        public void IsDecreasingWithRandomOrderedSequenceReturnsFalse()
        {
            var sequenceAscending = Enumerable.Range(1, 100).Shuffle();
            var result = sequenceAscending.IsDecreasing<int>();
            Assert.IsFalse(result);
        }


        [Test]
        public void IsDecreasingWithNullThrowsException()
        {
            var sequenceAscending = Enumerable.Range(1, 100).Shuffle();
            sequenceAscending = null;
            Assert.Throws<ArgumentNullException>(() => sequenceAscending.IsDecreasing<int>());
        }
    }
}
