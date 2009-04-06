using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MoreLinq.Test
{
    [TestFixture]
    public class TakeEveryTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TakeEveryNullSequence()
        {
            Enumerable.TakeEvery<object>(null, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TakeEveryNegativeSkip()
        {
            Enumerable.TakeEvery(new object[0], -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TakeEveryOutOfRangeZeroStep()
        {
            Enumerable.TakeEvery(new object[0], 0);
        }

        [Test]
        public void TakeEveryEmptySequence()
        {
            Assert.That(Enumerable.TakeEvery(new object[0], 1).GetEnumerator().MoveNext(), Is.False);
        }

        [Test]
        public void TakeEveryNonEmptySequence()
        {
            var result = Enumerable.TakeEvery(new[] { 1, 2, 3, 4, 5 }, 1);
            result.AssertSequenceEqual(1, 2, 3, 4, 5);
        }

        [Test]
        public void TakeEveryOtherOnNonEmptySequence()
        {
            var result = Enumerable.TakeEvery(new[] { 1, 2, 3, 4, 5 }, 2);
            result.AssertSequenceEqual(1, 3, 5);
        }

        [Test]
        public void TakeEveryThirdOnNonEmptySequence()
        {
            var result = Enumerable.TakeEvery(new[] { 1, 2, 3, 4, 5 }, 3);
            result.AssertSequenceEqual(1, 4);
        }

        [Test]
        public void TakeEveryIsLazy()
        {
            new BreakingSequence<object>().TakeEvery(1);
        }
    }
}