using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class ConcatTest
    {
        #region Concat with single head and tail sequence

        // NOTE: Concat with single head and tail sequence is now 
        // implemented in terms of Prepend so the tests are identical. 

        [Test]
        public void ConcatWithNonEmptyTailSequence()
        {
            string[] tail = { "second", "third" };
            string head = "first";
            IEnumerable<string> whole = MoreEnumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void ConcatWithEmptyTailSequence()
        {
            string[] tail = { };
            string head = "first";
            IEnumerable<string> whole = MoreEnumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConcatWithNullTailSequence()
        {
            MoreEnumerable.Concat("head", null);
        }

        [Test]
        public void ConcatWithNullHead()
        {
            string[] tail = { "second", "third" };
            string head = null;
            IEnumerable<string> whole = MoreEnumerable.Concat(head, tail);
            whole.AssertSequenceEqual(null, "second", "third");
        }

        [Test]
        public void ConcatIsLazyInTailSequence()
        {
            MoreEnumerable.Concat("head", new BreakingSequence<string>());
        }
        #endregion

        #region Concat with single head and tail sequence
        [Test]
        public void ConcatWithNonEmptyHeadSequence()
        {
            string[] head = { "first", "second" };
            string tail = "third";
            IEnumerable<string> whole = MoreEnumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void ConcatWithEmptyHeadSequence()
        {
            string[] head = { };
            string tail = "first";
            IEnumerable<string> whole = MoreEnumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConcatWithNullHeadSequence()
        {
            MoreEnumerable.Concat(null, "tail");
        }

        [Test]
        public void ConcatWithNullTail()
        {
            string[] head = { "first", "second" };
            string tail = null;
            IEnumerable<string> whole = MoreEnumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first", "second", null);
        }

        [Test]
        public void ConcatIsLazyInHeadSequence()
        {
            MoreEnumerable.Concat(new BreakingSequence<string>(), "tail");
        }
        #endregion
    }
}
