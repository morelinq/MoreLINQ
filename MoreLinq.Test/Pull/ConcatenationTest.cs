using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MoreLinq.Pull;

namespace MoreLinq.Test.Pull
{
    [TestFixture]
    public class ConcatenationTest
    {
        #region Concat with single head and tail sequence
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
    }
}
