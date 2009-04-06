using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class PrependTest
    {
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
    }
}
