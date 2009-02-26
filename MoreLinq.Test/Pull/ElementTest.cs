namespace MoreLinq.Test.Pull
{
    using System;
    using NUnit.Framework;
    using MoreLinq.Pull;
    using LinqEnumerable = System.Linq.Enumerable;
    using Enumerable = MoreLinq.Pull.Enumerable;

    [TestFixture]
    public class ElementTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrFallbackWithNullSequence()
        {
            Enumerable.SingleOrFallback(null, BreakingFunc.Of<int>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrFallbackWithNullFallback()
        {
            Enumerable.SingleOrFallback(new[] {1}, null);
        }

        [Test]
        public void SingleOrFallbackWithEmptySequence()
        {
            Assert.AreEqual(5, LinqEnumerable.Empty<int>().SingleOrFallback(() => 5));
        }

        [Test]
        public void SingleOrFallbackWithSingleElementSequence()
        {
            Assert.AreEqual(10, new[]{10}.SingleOrFallback(BreakingFunc.Of<int>()));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SingleOrFallbackWithLongSequence()
        {
            new[] { 10, 20, 30 }.SingleOrFallback(BreakingFunc.Of<int>());
        }
    }
}
