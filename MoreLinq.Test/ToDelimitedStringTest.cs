using System;
using System.Globalization;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class ToDelimitedStringTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToDelimitedStringWithNullSequence()
        {
            MoreEnumerable.ToDelimitedString<int>(null, ",");
        }

        [Test]
        public void ToDelimitedStringWithEmptySequence()
        {
            Assert.That(MoreEnumerable.ToDelimitedString(LinqEnumerable.Empty<int>()), Is.Empty);
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceAndDelimiter()
        {
            var result = MoreEnumerable.ToDelimitedString(new[] { 1, 2, 3 }, "-");
            Assert.That(result, Is.EqualTo("1-2-3"));
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceAndDefaultDelimiter()
        {
            using (new CurrentThreadCultureScope(new CultureInfo("fr-FR")))
            {
                var result = MoreEnumerable.ToDelimitedString(new[] {1, 2, 3});
                Assert.That(result, Is.EqualTo("1;2;3"));
            }
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceAndNullDelimiter()
        {
            using (new CurrentThreadCultureScope(new CultureInfo("fr-FR")))
            {
                var result = MoreEnumerable.ToDelimitedString(new[] { 1, 2, 3 }, null);
                Assert.That(result, Is.EqualTo("1;2;3"));
            }
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceContainingNulls()
        {
            var result = MoreEnumerable.ToDelimitedString(new object[] { 1, null, "foo", true }, ",");
            Assert.That(result, Is.EqualTo("1,,foo,True"));
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceContainingNullsAtStart()
        {
            // See: http://code.google.com/p/morelinq/issues/detail?id=43
            var result = MoreEnumerable.ToDelimitedString(new object[] { null, null, "foo" }, ",");
            Assert.That(result, Is.EqualTo(",,foo"));
        }
    }
}
