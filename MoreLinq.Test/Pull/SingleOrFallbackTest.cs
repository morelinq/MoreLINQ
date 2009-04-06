namespace MoreLinq.Test.Pull
{
    using System;
    using NUnit.Framework;
    using MoreLinq.Pull;
    using LinqEnumerable = System.Linq.Enumerable;

    partial class EnumerableTest
    {
        [Test, Category("Element")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrFallbackWithNullSequence()
        {
            Enumerable.SingleOrFallback(null, BreakingFunc.Of<int>());
        }

        [Test, Category("Element")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrFallbackWithNullFallback()
        {
            Enumerable.SingleOrFallback(new[] {1}, null);
        }

        [Test, Category("Element")]
        public void SingleOrFallbackWithEmptySequence()
        {
            Assert.AreEqual(5, LinqEnumerable.Empty<int>().SingleOrFallback(() => 5));
        }

        [Test, Category("Element")]
        public void SingleOrFallbackWithSingleElementSequence()
        {
            Assert.AreEqual(10, new[]{10}.SingleOrFallback(BreakingFunc.Of<int>()));
        }

        [Test, Category("Element")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SingleOrFallbackWithLongSequence()
        {
            new[] { 10, 20, 30 }.SingleOrFallback(BreakingFunc.Of<int>());
        }
    }
}
