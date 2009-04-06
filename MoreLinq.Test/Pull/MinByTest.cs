namespace MoreLinq.Test.Pull
{
    using System;
    using System.Collections.Generic;
    using MoreLinq.Pull;
    using NUnit.Framework;

    partial class EnumerableTest
    {
        [Test, Category("Aggregation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MinByNullSequence()
        {
            ((IEnumerable<string>)null).MinBy(x => x.Length);
        }

        [Test, Category("Aggregation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MinByNullSelector()
        {
            Strings.MinBy<string, int>(null);
        }

        [Test, Category("Aggregation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MinByNullComparer()
        {
            Strings.MinBy(x => x.Length, null);
        }

        [Test, Category("Aggregation")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MinByEmptySequence()
        {
            new string[0].MinBy(x => x.Length);
        }

        [Test, Category("Aggregation")]
        public void MinByWithNaturalComparer()
        {
            Assert.AreEqual("aa", Strings.MinBy(x => x[1]));
        }

        [Test, Category("Aggregation")]
        public void MinByWithComparer()
        {
            Assert.AreEqual("az", Strings.MinBy(x => x[1], new ReverseCharComparer()));
        }

        [Test, Category("Aggregation")]
        public void MinByReturnsFirstOfEquals()
        {
            Assert.AreEqual("ax", Strings.MinBy(x => x.Length));
        }
    }
}
