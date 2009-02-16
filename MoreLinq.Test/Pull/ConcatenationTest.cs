using System;
using System.Collections.Generic;
using MoreLinq.Pull;
using NUnit.Framework;

namespace MoreLinq.Test.Pull
{
    [TestFixture]
    public class ConcatenationTest
    {
        #region Concat with single head and tail sequence

        // NOTE: Concat with single head and tail sequence is now 
        // implemented in terms of Prepend so the tests are identical. 

        [Test]
        public void ConcatWithNonEmptyTailSequence()
        {
            string[] tail = { "second", "third" };
            string head = "first";
            IEnumerable<string> whole = Concatenation.Concat(head, tail);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void ConcatWithEmptyTailSequence()
        {
            string[] tail = { };
            string head = "first";
            IEnumerable<string> whole = Concatenation.Concat(head, tail);
            whole.AssertSequenceEqual("first");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConcatWithNullTailSequence()
        {
            Concatenation.Concat("head", null);
        }

        [Test]
        public void ConcatWithNullHead()
        {
            string[] tail = { "second", "third" };
            string head = null;
            IEnumerable<string> whole = Concatenation.Concat(head, tail);
            whole.AssertSequenceEqual(null, "second", "third");
        }

        [Test]
        public void ConcatIsLazyInTailSequence()
        {
            Concatenation.Concat("head", new BreakingSequence<string>());
        }
        #endregion

        #region Concat with single head and tail sequence
        [Test]
        public void ConcatWithNonEmptyHeadSequence()
        {
            string[] head = { "first", "second" };
            string tail = "third";
            IEnumerable<string> whole = Concatenation.Concat(head, tail);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void ConcatWithEmptyHeadSequence()
        {
            string[] head = { };
            string tail = "first";
            IEnumerable<string> whole = Concatenation.Concat(head, tail);
            whole.AssertSequenceEqual("first");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConcatWithNullHeadSequence()
        {
            Concatenation.Concat(null, "tail");
        }

        [Test]
        public void ConcatWithNullTail()
        {
            string[] head = { "first", "second" };
            string tail = null;
            IEnumerable<string> whole = Concatenation.Concat(head, tail);
            whole.AssertSequenceEqual("first", "second", null);
        }

        [Test]
        public void ConcatIsLazyInHeadSequence()
        {
            Concatenation.Concat(new BreakingSequence<string>(), "tail");
        }
        #endregion

        #region Pad

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PadNullSource()
        {
            Concatenation.Pad<object>(null, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void PadNegativeWidth()
        {
            Concatenation.Pad(new object[0], -1);
        }

        [Test]
        public void PadIsLazy()
        {
            Concatenation.Pad(new BreakingSequence<object>(), 0);
        }

        [Test]
        public void PadWithFillerIsLazy()
        {
            Concatenation.Pad(new BreakingSequence<object>(), 0, new object());
        }

        [Test]
        public void PadWideSourceSequence()
        {
            var result = Concatenation.Pad(new[] { 123, 456, 789 }, 2);
            result.AssertSequenceEqual(new[] { 123, 456, 789 });
        }

        [Test]
        public void PadEqualSourceSequence()
        {
            var result = Concatenation.Pad(new[] { 123, 456, 789 }, 3);
            result.AssertSequenceEqual(new[] { 123, 456, 789 });
        }

        [Test]
        public void PadNarrowSourceSequenceWithDefaultPadding()
        {
            var result = Concatenation.Pad(new[] { 123, 456, 789 }, 5);
            result.AssertSequenceEqual(new[] { 123, 456, 789, 0, 0 });
        }

        [Test]
        public void PadNarrowSourceSequenceWithNonDefaultPadding()
        {
            var result = Concatenation.Pad(new[] { 123, 456, 789 }, 5, -1);
            result.AssertSequenceEqual(new[] { 123, 456, 789, -1, -1 });
        }

        [Test]
        public void PadNarrowSourceSequenceWithDynamicPadding()
        {
            var result = Concatenation.Pad("hello".ToCharArray(), 15, i => i % 2 == 0 ? '+' : '-');
            result.AssertSequenceEqual("hello-+-+-+".ToCharArray());
        }

        #endregion

        #region Prepend
        [Test]
        public void PrependWithNonEmptyTailSequence()
        {
            string[] tail = { "second", "third" };
            string head = "first";
            IEnumerable<string> whole = Concatenation.Prepend(tail, head);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void PrependWithEmptyTailSequence()
        {
            string[] tail = { };
            string head = "first";
            IEnumerable<string> whole = Concatenation.Prepend(tail, head);
            whole.AssertSequenceEqual("first");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PrependWithNullTailSequence()
        {
            Concatenation.Prepend(null, "head");
        }

        [Test]
        public void PrependWithNullHead()
        {
            string[] tail = { "second", "third" };
            string head = null;
            IEnumerable<string> whole = Concatenation.Prepend(tail, head);
            whole.AssertSequenceEqual(null, "second", "third");
        }

        [Test]
        public void PrependIsLazyInTailSequence()
        {
            Concatenation.Prepend(new BreakingSequence<string>(), "head");
        }
        #endregion
    }
}
