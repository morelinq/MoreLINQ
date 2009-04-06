namespace MoreLinq.Test.Pull
{
    using System;
    using MoreLinq.Pull;
    using NUnit.Framework;

    partial class EnumerableTest
    {
        [Test, Category("Concatenation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PadNullSource()
        {
            Enumerable.Pad<object>(null, 0);
        }

        [Test, Category("Concatenation")]
        [ExpectedException(typeof(ArgumentException))]
        public void PadNegativeWidth()
        {
            Enumerable.Pad(new object[0], -1);
        }

        [Test, Category("Concatenation")]
        public void PadIsLazy()
        {
            Enumerable.Pad(new BreakingSequence<object>(), 0);
        }

        [Test, Category("Concatenation")]
        public void PadWithFillerIsLazy()
        {
            Enumerable.Pad(new BreakingSequence<object>(), 0, new object());
        }

        [Test, Category("Concatenation")]
        public void PadWideSourceSequence()
        {
            var result = Enumerable.Pad(new[] { 123, 456, 789 }, 2);
            result.AssertSequenceEqual(new[] { 123, 456, 789 });
        }

        [Test, Category("Concatenation")]
        public void PadEqualSourceSequence()
        {
            var result = Enumerable.Pad(new[] { 123, 456, 789 }, 3);
            result.AssertSequenceEqual(new[] { 123, 456, 789 });
        }

        [Test, Category("Concatenation")]
        public void PadNarrowSourceSequenceWithDefaultPadding()
        {
            var result = Enumerable.Pad(new[] { 123, 456, 789 }, 5);
            result.AssertSequenceEqual(new[] { 123, 456, 789, 0, 0 });
        }

        [Test, Category("Concatenation")]
        public void PadNarrowSourceSequenceWithNonDefaultPadding()
        {
            var result = Enumerable.Pad(new[] { 123, 456, 789 }, 5, -1);
            result.AssertSequenceEqual(new[] { 123, 456, 789, -1, -1 });
        }

        [Test, Category("Concatenation")]
        public void PadNarrowSourceSequenceWithDynamicPadding()
        {
            var result = Enumerable.Pad("hello".ToCharArray(), 15, i => i % 2 == 0 ? '+' : '-');
            result.AssertSequenceEqual("hello-+-+-+-+-+".ToCharArray());
        }
    }
}
