using System;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class PadTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PadNullSource()
        {
            Enumerable.Pad<object>(null, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void PadNegativeWidth()
        {
            Enumerable.Pad(new object[0], -1);
        }

        [Test]
        public void PadIsLazy()
        {
            Enumerable.Pad(new BreakingSequence<object>(), 0);
        }

        [Test]
        public void PadWithFillerIsLazy()
        {
            Enumerable.Pad(new BreakingSequence<object>(), 0, new object());
        }

        [Test]
        public void PadWideSourceSequence()
        {
            var result = Enumerable.Pad(new[] { 123, 456, 789 }, 2);
            result.AssertSequenceEqual(new[] { 123, 456, 789 });
        }

        [Test]
        public void PadEqualSourceSequence()
        {
            var result = Enumerable.Pad(new[] { 123, 456, 789 }, 3);
            result.AssertSequenceEqual(new[] { 123, 456, 789 });
        }

        [Test]
        public void PadNarrowSourceSequenceWithDefaultPadding()
        {
            var result = Enumerable.Pad(new[] { 123, 456, 789 }, 5);
            result.AssertSequenceEqual(new[] { 123, 456, 789, 0, 0 });
        }

        [Test]
        public void PadNarrowSourceSequenceWithNonDefaultPadding()
        {
            var result = Enumerable.Pad(new[] { 123, 456, 789 }, 5, -1);
            result.AssertSequenceEqual(new[] { 123, 456, 789, -1, -1 });
        }

        [Test]
        public void PadNarrowSourceSequenceWithDynamicPadding()
        {
            var result = Enumerable.Pad("hello".ToCharArray(), 15, i => i % 2 == 0 ? '+' : '-');
            result.AssertSequenceEqual("hello-+-+-+-+-+".ToCharArray());
        }
    }
}
