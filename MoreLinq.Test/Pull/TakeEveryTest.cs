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
        public void TakeEveryNullSequence()
        {
            Enumerable.TakeEvery<object>(null, 1);
        }

        [Test, Category("Partition")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TakeEveryNegativeSkip()
        {
            Enumerable.TakeEvery(new object[0], -1);
        }

        [Test, Category("Partition")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TakeEveryOutOfRangeZeroStep()
        {
            Enumerable.TakeEvery(new object[0], 0);
        }

        [Test, Category("Partition")]
        public void TakeEveryEmptySequence()
        {
            Assert.That(Enumerable.TakeEvery(new object[0], 1).GetEnumerator().MoveNext(), Is.False);
        }

        [Test, Category("Partition")]
        public void TakeEveryNonEmptySequence()
        {
            var result = Enumerable.TakeEvery(new[] { 1, 2, 3, 4, 5 }, 1);
            result.AssertSequenceEqual(1, 2, 3, 4, 5);
        }

        [Test, Category("Partition")]
        public void TakeEveryOtherOnNonEmptySequence()
        {
            var result = Enumerable.TakeEvery(new[] { 1, 2, 3, 4, 5 }, 2);
            result.AssertSequenceEqual(1, 3, 5);
        }

        [Test, Category("Partition")]
        public void TakeEveryThirdOnNonEmptySequence()
        {
            var result = Enumerable.TakeEvery(new[] { 1, 2, 3, 4, 5 }, 3);
            result.AssertSequenceEqual(1, 4);
        }

        [Test, Category("Partition")]
        public void TakeEveryIsLazy()
        {
            new BreakingSequence<object>().TakeEvery(1);
        }
    }
}