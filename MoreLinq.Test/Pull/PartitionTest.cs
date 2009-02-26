using System;
using MoreLinq.Pull;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MoreLinq.Test.Pull
{

    [TestFixture]
    public class PartitionTest
    {
        #region TakeEvery

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EveryNullSequence()
        {
            Enumerable.TakeEvery<object>(null, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void EveryNegativeSkip()
        {
            Enumerable.TakeEvery(new object[0], -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void EveryOutOfRangeZeroStep()
        {
            Enumerable.TakeEvery(new object[0], 0);
        }

        [Test]
        public void EveryEmptySequence()
        {
            Assert.That(Enumerable.TakeEvery(new object[0], 1).GetEnumerator().MoveNext(), Is.False);
        }

        [Test]
        public void EveryNonEmptySequence()
        {
            var result = Enumerable.TakeEvery(new[] { 1, 2, 3, 4, 5 }, 1);
            result.AssertSequenceEqual(1, 2, 3, 4, 5);
        }

        [Test]
        public void EveryOtherOnNonEmptySequence()
        {
            var result = Enumerable.TakeEvery(new[] { 1, 2, 3, 4, 5 }, 2);
            result.AssertSequenceEqual(1, 3, 5);
        }

        [Test]
        public void EveryThirdOnNonEmptySequence()
        {
            var result = Enumerable.TakeEvery(new[] { 1, 2, 3, 4, 5 }, 3);
            result.AssertSequenceEqual(1, 4);
        }

        [Test]
        public void EveryIsLazy()
        {
            new BreakingSequence<object>().TakeEvery(1);
        }

        #endregion
    }
}