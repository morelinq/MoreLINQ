using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using LinqEnumerable = System.Linq.Enumerable;
using System.Text;

namespace MoreLinq.Test
{
    [TestFixture]
    public class CountByTest
    {
        private IEnumerable<int> inputSequence = new int[] { 97, 98, 98, 99, 99, 99, 100, 100, 100, 100, 101, 101, 101, 101, 101 };

        private char convertIntToChar(int int_)
        {
            return (char)int_;
        }

        /// <summary>
        /// Verify that RandomSubset() behaves in a lazy manner.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CountByIsLazyTest()
        {
            new BreakingSequence<int>().CountBy(convertIntToChar);
        }

        /// <summary>
        /// Verify that invoking CountBy on a <c>null</c> sequence results in an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountByWithNullSequenceTest()
        {
            IEnumerable<int> inputSequence = null;
            inputSequence.CountBy(this.convertIntToChar);
        }

        /// <summary>
        /// Verify that CountBy correctly projects values from the original input sequence.
        /// </summary>
        [Test]
        public void CountByProjectedValuesAreCorrectTest()
        {
            IDictionary<char, int> projectedValuesByCount = this.inputSequence.CountBy(convertIntToChar);
            char[] projectedValuesOrderedAlphabetically = projectedValuesByCount.Keys.OrderBy(char_ => char_).ToArray();

            Assert.IsTrue(projectedValuesOrderedAlphabetically.SequenceEqual(new char[] { 'a', 'b', 'c', 'd', 'e' }));
        }

        /// <summary>
        /// Verify that CountBy correctly counts the number of occurrences of new projected values.
        /// </summary>
        [Test]
        public void CountByProjectedValuesAreCountedCorrectlyTest()
        {
            IDictionary<char, int> projectedValuesByCount = this.inputSequence.CountBy(convertIntToChar);
            int[] orderedCountsOfProjectedValues = projectedValuesByCount.Values.OrderBy(count => count).ToArray();

            Assert.IsTrue(orderedCountsOfProjectedValues.SequenceEqual(new int[] { 1, 2, 3, 4, 5 }));
        }
    }
}
