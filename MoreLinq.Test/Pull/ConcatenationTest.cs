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
            IEnumerable<string> whole = Enumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void ConcatWithEmptyTailSequence()
        {
            string[] tail = { };
            string head = "first";
            IEnumerable<string> whole = Enumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConcatWithNullTailSequence()
        {
            Enumerable.Concat("head", null);
        }

        [Test]
        public void ConcatWithNullHead()
        {
            string[] tail = { "second", "third" };
            string head = null;
            IEnumerable<string> whole = Enumerable.Concat(head, tail);
            whole.AssertSequenceEqual(null, "second", "third");
        }

        [Test]
        public void ConcatIsLazyInTailSequence()
        {
            Enumerable.Concat("head", new BreakingSequence<string>());
        }
        #endregion

        #region Concat with single head and tail sequence
        [Test]
        public void ConcatWithNonEmptyHeadSequence()
        {
            string[] head = { "first", "second" };
            string tail = "third";
            IEnumerable<string> whole = Enumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void ConcatWithEmptyHeadSequence()
        {
            string[] head = { };
            string tail = "first";
            IEnumerable<string> whole = Enumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConcatWithNullHeadSequence()
        {
            Enumerable.Concat(null, "tail");
        }

        [Test]
        public void ConcatWithNullTail()
        {
            string[] head = { "first", "second" };
            string tail = null;
            IEnumerable<string> whole = Enumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first", "second", null);
        }

        [Test]
        public void ConcatIsLazyInHeadSequence()
        {
            Enumerable.Concat(new BreakingSequence<string>(), "tail");
        }
        #endregion

        #region Pad

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

        #endregion

        #region Prepend
        [Test]
        public void PrependWithNonEmptyTailSequence()
        {
            string[] tail = { "second", "third" };
            string head = "first";
            IEnumerable<string> whole = Enumerable.Prepend(tail, head);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void PrependWithEmptyTailSequence()
        {
            string[] tail = { };
            string head = "first";
            IEnumerable<string> whole = Enumerable.Prepend(tail, head);
            whole.AssertSequenceEqual("first");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PrependWithNullTailSequence()
        {
            Enumerable.Prepend(null, "head");
        }

        [Test]
        public void PrependWithNullHead()
        {
            string[] tail = { "second", "third" };
            string head = null;
            IEnumerable<string> whole = Enumerable.Prepend(tail, head);
            whole.AssertSequenceEqual(null, "second", "third");
        }

        [Test]
        public void PrependIsLazyInTailSequence()
        {
            Enumerable.Prepend(new BreakingSequence<string>(), "head");
        }
        #endregion
    }
}
