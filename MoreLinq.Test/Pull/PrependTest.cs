namespace MoreLinq.Test.Pull
{
    using System;
    using System.Collections.Generic;
    using MoreLinq.Pull;
    using NUnit.Framework;

    partial class EnumerableTest
    {
        [Test, Category("Concatenation")]
        public void PrependWithNonEmptyTailSequence()
        {
            string[] tail = { "second", "third" };
            string head = "first";
            IEnumerable<string> whole = Enumerable.Prepend(tail, head);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test, Category("Concatenation")]
        public void PrependWithEmptyTailSequence()
        {
            string[] tail = { };
            string head = "first";
            IEnumerable<string> whole = Enumerable.Prepend(tail, head);
            whole.AssertSequenceEqual("first");
        }

        [Test, Category("Concatenation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PrependWithNullTailSequence()
        {
            Enumerable.Prepend(null, "head");
        }

        [Test, Category("Concatenation")]
        public void PrependWithNullHead()
        {
            string[] tail = { "second", "third" };
            string head = null;
            IEnumerable<string> whole = Enumerable.Prepend(tail, head);
            whole.AssertSequenceEqual(null, "second", "third");
        }

        [Test, Category("Concatenation")]
        public void PrependIsLazyInTailSequence()
        {
            Enumerable.Prepend(new BreakingSequence<string>(), "head");
        }
    }
}
