namespace MoreLinq.Test.Pull
{
    using System;
    using System.Collections.Generic;
    using MoreLinq.Pull;
    using NUnit.Framework;

    partial class EnumerableTest
    {
        #region Concat with single head and tail sequence

        // NOTE: Concat with single head and tail sequence is now 
        // implemented in terms of Prepend so the tests are identical. 

        [Test, Category("Concatenation")]
        public void ConcatWithNonEmptyTailSequence()
        {
            string[] tail = { "second", "third" };
            string head = "first";
            IEnumerable<string> whole = Enumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test, Category("Concatenation")]
        public void ConcatWithEmptyTailSequence()
        {
            string[] tail = { };
            string head = "first";
            IEnumerable<string> whole = Enumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first");
        }

        [Test, Category("Concatenation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConcatWithNullTailSequence()
        {
            Enumerable.Concat("head", null);
        }

        [Test, Category("Concatenation")]
        public void ConcatWithNullHead()
        {
            string[] tail = { "second", "third" };
            string head = null;
            IEnumerable<string> whole = Enumerable.Concat(head, tail);
            whole.AssertSequenceEqual(null, "second", "third");
        }

        [Test, Category("Concatenation")]
        public void ConcatIsLazyInTailSequence()
        {
            Enumerable.Concat("head", new BreakingSequence<string>());
        }
        #endregion

        #region Concat with single head and tail sequence
        [Test, Category("Concatenation")]
        public void ConcatWithNonEmptyHeadSequence()
        {
            string[] head = { "first", "second" };
            string tail = "third";
            IEnumerable<string> whole = Enumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test, Category("Concatenation")]
        public void ConcatWithEmptyHeadSequence()
        {
            string[] head = { };
            string tail = "first";
            IEnumerable<string> whole = Enumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first");
        }

        [Test, Category("Concatenation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConcatWithNullHeadSequence()
        {
            Enumerable.Concat(null, "tail");
        }

        [Test, Category("Concatenation")]
        public void ConcatWithNullTail()
        {
            string[] head = { "first", "second" };
            string tail = null;
            IEnumerable<string> whole = Enumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first", "second", null);
        }

        [Test, Category("Concatenation")]
        public void ConcatIsLazyInHeadSequence()
        {
            Enumerable.Concat(new BreakingSequence<string>(), "tail");
        }
        #endregion
    }
}
