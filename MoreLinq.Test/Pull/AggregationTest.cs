using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MoreLinq.Pull;
using NUnit.Framework;

namespace MoreLinq.Test.Pull
{
    [TestFixture]
    public class AggregationTest
    {
        private static readonly IEnumerable<string> Strings = new ReadOnlyCollection<string>(
            new[] { "ax", "hello", "world", "aa", "ab", "ay", "az" });

        #region MaxBy
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MaxByNullSequence()
        {
            ((IEnumerable<string>)null).MaxBy(x => x.Length);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MaxByNullSelector()
        {
            Strings.MaxBy<string, int>(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MaxByNullComparer()
        {
            Strings.MaxBy(x => x.Length, null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MaxByEmptySequence()
        {
            new string[0].MaxBy(x => x.Length);
        }

        [Test]
        public void MaxByWithNaturalComparer()
        {
            Assert.AreEqual("az", Strings.MaxBy(x => x[1]));
        }

        [Test]
        public void MaxByWithComparer()
        {
            Assert.AreEqual("aa", Strings.MaxBy(x => x[1], new ReverseCharComparer()));
        }

        [Test]
        public void MaxByReturnsFirstOfEquals()
        {
            Assert.AreEqual("hello", Strings.MaxBy(x => x.Length));
        }
        #endregion

        #region MinBy
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MinByNullSequence()
        {
            ((IEnumerable<string>)null).MinBy(x => x.Length);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MinByNullSelector()
        {
            Strings.MinBy<string, int>(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MinByNullComparer()
        {
            Strings.MinBy(x => x.Length, null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MinByEmptySequence()
        {
            new string[0].MinBy(x => x.Length);
        }

        [Test]
        public void MinByWithNaturalComparer()
        {
            Assert.AreEqual("aa", Strings.MinBy(x => x[1]));
        }

        [Test]
        public void MinByWithComparer()
        {
            Assert.AreEqual("az", Strings.MinBy(x => x[1], new ReverseCharComparer()));
        }

        [Test]
        public void MinByReturnsFirstOfEquals()
        {
            Assert.AreEqual("ax", Strings.MinBy(x => x.Length));
        }
        #endregion

        private class ReverseCharComparer : IComparer<char>
        {
            public int Compare(char x, char y)
            {
                return y.CompareTo(x);
            }
        }
    }
}
