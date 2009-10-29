using System;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class TakeLastTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TakeLastNullSource()
        {
            MoreEnumerable.TakeLast<object>(null, 0);
        }

        [Test]
        public void TakeLast()
        {
            var result = new[]{ 12, 34, 56, 78, 910, 1112 }.TakeLast(3);
            result.AssertSequenceEqual(78, 910, 1112);
        }

        [Test]
        public void TakeLastOnSequenceShortOfCount()
        {
            var result = new[] { 12, 34, 56 }.TakeLast(5);
            result.AssertSequenceEqual(12, 34, 56);
        }

        [Test]
        public void TakeLastWithNegativeCount()
        {
            var result = new[] { 12, 34, 56 }.TakeLast(-2);
            Assert.IsFalse(result.GetEnumerator().MoveNext());
        }

        [Test]
        public void TakeLastIsLazy()
        {
            new BreakingSequence<object>().TakeLast(1);
        }
    }
}
