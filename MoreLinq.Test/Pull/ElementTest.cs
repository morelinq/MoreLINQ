using System;
using NUnit.Framework;
using MoreLinq.Pull;
using System.Linq;

namespace MoreLinq.Test.Pull
{
    [TestFixture]
    public class ElementTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrFallbackWithNullSequence()
        {
            Element.SingleOrFallback<int>(null, BreakingFunc.Of<int>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrFallbackWithNullFallback()
        {
            Element.SingleOrFallback(new[] {1}, null);
        }

        [Test]
        public void SingleOrFallbackWithEmptySequence()
        {
            Assert.AreEqual(5, Enumerable.Empty<int>().SingleOrFallback(() => 5));
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
