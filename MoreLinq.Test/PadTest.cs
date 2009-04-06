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
            MoreEnumerable.Pad<object>(null, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void PadNegativeWidth()
        {
            MoreEnumerable.Pad(new object[0], -1);
        }

        [Test]
        public void PadIsLazy()
        {
            MoreEnumerable.Pad(new BreakingSequence<object>(), 0);
        }

        [Test]
        public void PadWithFillerIsLazy()
        {
            MoreEnumerable.Pad(new BreakingSequence<object>(), 0, new object());
        }

        [Test]
        public void PadWideSourceSequence()
        {
            var result = MoreEnumerable.Pad(new[] { 123, 456, 789 }, 2);
            result.AssertSequenceEqual(new[] { 123, 456, 789 });
        }

        [Test]
        public void PadEqualSourceSequence()
        {
            var result = MoreEnumerable.Pad(new[] { 123, 456, 789 }, 3);
            result.AssertSequenceEqual(new[] { 123, 456, 789 });
        }

        [Test]
        public void PadNarrowSourceSequenceWithDefaultPadding()
        {
            var result = MoreEnumerable.Pad(new[] { 123, 456, 789 }, 5);
            result.AssertSequenceEqual(new[] { 123, 456, 789, 0, 0 });
        }

        [Test]
        public void PadNarrowSourceSequenceWithNonDefaultPadding()
        {
            var result = MoreEnumerable.Pad(new[] { 123, 456, 789 }, 5, -1);
            result.AssertSequenceEqual(new[] { 123, 456, 789, -1, -1 });
        }

        [Test]
        public void PadNarrowSourceSequenceWithDynamicPadding()
        {
            var result = MoreEnumerable.Pad("hello".ToCharArray(), 15, i => i % 2 == 0 ? '+' : '-');
            result.AssertSequenceEqual("hello-+-+-+-+-+".ToCharArray());
        }
    }
}
