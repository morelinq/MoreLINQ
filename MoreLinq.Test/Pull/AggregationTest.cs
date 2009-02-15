using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoreLinq.Pull;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.Threading;
using System.Globalization;

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

        #region ToDelimitedString        
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToDelimitedStringWithNullSequence()
        {
            Aggregation.ToDelimitedString<int>(null, ",");
        }

        [Test]
        public void ToDelimitedStringWithEmptySequence()
        {
            Assert.That(Aggregation.ToDelimitedString(Enumerable.Empty<int>()), Is.Empty);
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceAndDelimiter()
        {
            var result = Aggregation.ToDelimitedString(new[] { 1, 2, 3 }, "-");
            Assert.That(result, Is.EqualTo("1-2-3"));
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceAndDefaultDelimiter()
        {
            using (new CurrentThreadCultureScope(new CultureInfo("fr-FR")))
            {
                var result = Aggregation.ToDelimitedString(new[] {1, 2, 3});
                Assert.That(result, Is.EqualTo("1;2;3"));
            }
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceAndNullDelimiter()
        {
            using (new CurrentThreadCultureScope(new CultureInfo("fr-FR")))
            {
                var result = Aggregation.ToDelimitedString(new[] { 1, 2, 3 }, null);
                Assert.That(result, Is.EqualTo("1;2;3"));
            }
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceContainingNulls()
        {
            var result = Aggregation.ToDelimitedString(new object[] { 1, null, "foo", true }, ",");
            Assert.That(result, Is.EqualTo("1,,foo,True"));
        }

        #endregion

        private abstract class Scope<T> : IDisposable
        {
            private readonly T old;

            protected Scope(T current)
            {
                old = current;
            }

            public virtual void Dispose()
            {
                Restore(old);
            }

            protected abstract void Restore(T old);
        }

        private sealed class CurrentThreadCultureScope : Scope<CultureInfo>
        {
            public CurrentThreadCultureScope(CultureInfo @new) : 
                base(Thread.CurrentThread.CurrentCulture)
            {
                Install(@new);
            }

            protected override void Restore(CultureInfo old)
            {
                Install(old);
            }

            private static void Install(CultureInfo value)
            {
                Thread.CurrentThread.CurrentCulture = value;
            }
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
