using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreLinq.Test
{
    [TestFixture]
    public class IsIncreasingTest
    {
        [Test]
        public void IsIncreasingWithIncreasingSequenceReturnsTrue()
        {
            var sequenceAscending = Enumerable.Range(1, 100);
            var result = sequenceAscending.IsIncreasing<int>();
            Assert.IsTrue(result);
        }

        [Test]
        public void IsIncreasingWithDecreasingSequenceReturnsFalse()
        {
            var sequenceAscending = Enumerable.Range(1, 100).Reverse();
            var result = sequenceAscending.IsIncreasing<int>();
            Assert.IsFalse(result);
        }

        [Test]
        public void IsIncreasingWithRandomOrderedSequenceReturnsFalse()
        {
            var sequenceAscending = Enumerable.Range(1, 100).Shuffle();
            var result = sequenceAscending.IsIncreasing<int>();
            Assert.IsFalse(result);
        }


        [Test]
        public void IsIncreasingWithNullThrowsException()
        {
            var sequenceAscending = Enumerable.Range(1, 100).Shuffle();
            sequenceAscending = null;
            Assert.Throws<ArgumentNullException>(() => sequenceAscending.IsIncreasing<int>()); 
        }
    }
}
