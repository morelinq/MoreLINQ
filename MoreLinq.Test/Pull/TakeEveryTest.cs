using System;
using MoreLinq.Pull;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MoreLinq.Test.Pull
{
    partial class EnumerableTest
    {
        [Test, Category("Partition")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EveryNullSequence()
        {
            Enumerable.TakeEvery<object>(null, 1);
        }

        [Test, Category("Partition")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void EveryNegativeSkip()
        {
            Enumerable.TakeEvery(new object[0], -1);
        }

        [Test, Category("Partition")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void EveryOutOfRangeZeroStep()
        {
            Enumerable.TakeEvery(new object[0], 0);
        }

        [Test, Category("Partition")]
        public void EveryEmptySequence()
        {
            Assert.That(Enumerable.TakeEvery(new object[0], 1).GetEnumerator().MoveNext(), Is.False);
        }

        [Test, Category("Partition")]
        public void EveryNonEmptySequence()
        {
            var result = Enumerable.TakeEvery(new[] { 1, 2, 3, 4, 5 }, 1);
            result.AssertSequenceEqual(1, 2, 3, 4, 5);
        }

        [Test, Category("Partition")]
        public void EveryOtherOnNonEmptySequence()
        {
            var result = Enumerable.TakeEvery(new[] { 1, 2, 3, 4, 5 }, 2);
            result.AssertSequenceEqual(1, 3, 5);
        }

        [Test, Category("Partition")]
        public void EveryThirdOnNonEmptySequence()
        {
            var result = Enumerable.TakeEvery(new[] { 1, 2, 3, 4, 5 }, 3);
            result.AssertSequenceEqual(1, 4);
        }

        [Test, Category("Partition")]
        public void EveryIsLazy()
        {
            new BreakingSequence<object>().TakeEvery(1);
        }
    }
}