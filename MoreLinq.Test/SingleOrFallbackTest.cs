using System;
using NUnit.Framework;
using System.Linq;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class SingleOrFallbackTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrFallbackWithNullSequence()
        {
            MoreEnumerable.SingleOrFallback(null, BreakingFunc.Of<int>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrFallbackWithNullFallback()
        {
            MoreEnumerable.SingleOrFallback(new[] {1}, null);
        }

        [Test]
        public void SingleOrFallbackWithEmptySequence()
        {
            Assert.AreEqual(5, LinqEnumerable.Empty<int>().Select(x => x).SingleOrFallback(() => 5));
        }
        [Test]
        public void SingleOrFallbackWithEmptySequenceIListOptimized()
        {
            Assert.AreEqual(5, LinqEnumerable.Empty<int>().SingleOrFallback(() => 5));
        }

        [Test]
        public void SingleOrFallbackWithSingleElementSequence()
        {
            Assert.AreEqual(10, new[]{10}.Select(x => x).SingleOrFallback(BreakingFunc.Of<int>()));
        }
        [Test]
        public void SingleOrFallbackWithSingleElementSequenceIListOptimized()
        {
            Assert.AreEqual(10, new[] { 10 }.SingleOrFallback(BreakingFunc.Of<int>()));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SingleOrFallbackWithLongSequence()
        {
            new[] { 10, 20, 30 }.Select(x => x).SingleOrFallback(BreakingFunc.Of<int>());
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SingleOrFallbackWithLongSequenceIListOptimized()
        {
            new[] { 10, 20, 30 }.SingleOrFallback(BreakingFunc.Of<int>());
        }
    }
}
