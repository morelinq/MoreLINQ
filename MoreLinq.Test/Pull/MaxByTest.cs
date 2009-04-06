namespace MoreLinq.Test.Pull
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using MoreLinq.Pull;
    using NUnit.Framework;

    partial class EnumerableTest
    {
        private static readonly IEnumerable<string> Strings = new ReadOnlyCollection<string>(
            new[] { "ax", "hello", "world", "aa", "ab", "ay", "az" });

        [Test, Category("Aggregation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MaxByNullSequence()
        {
            ((IEnumerable<string>)null).MaxBy(x => x.Length);
        }

        [Test, Category("Aggregation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MaxByNullSelector()
        {
            Strings.MaxBy<string, int>(null);
        }

        [Test, Category("Aggregation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MaxByNullComparer()
        {
            Strings.MaxBy(x => x.Length, null);
        }

        [Test, Category("Aggregation")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MaxByEmptySequence()
        {
            new string[0].MaxBy(x => x.Length);
        }

        [Test, Category("Aggregation")]
        public void MaxByWithNaturalComparer()
        {
            Assert.AreEqual("az", Strings.MaxBy(x => x[1]));
        }

        [Test, Category("Aggregation")]
        public void MaxByWithComparer()
        {
            Assert.AreEqual("aa", Strings.MaxBy(x => x[1], new ReverseCharComparer()));
        }

        [Test, Category("Aggregation")]
        public void MaxByReturnsFirstOfEquals()
        {
            Assert.AreEqual("hello", Strings.MaxBy(x => x.Length));
        }

        private class ReverseCharComparer : IComparer<char>
        {
            public int Compare(char x, char y)
            {
                return y.CompareTo(x);
            }
        }
    }
}
