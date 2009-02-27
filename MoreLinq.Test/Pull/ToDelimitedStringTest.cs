namespace MoreLinq.Test.Pull
{
    using System;
    using System.Globalization;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using Enumerable = MoreLinq.Pull.Enumerable;
    using LinqEnumerable = System.Linq.Enumerable;

    partial class EnumerableTest
    {
        [Test, Category("Aggregation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToDelimitedStringWithNullSequence()
        {
            Enumerable.ToDelimitedString<int>(null, ",");
        }

        [Test, Category("Aggregation")]
        public void ToDelimitedStringWithEmptySequence()
        {
            Assert.That(Enumerable.ToDelimitedString(LinqEnumerable.Empty<int>()), Is.Empty);
        }

        [Test, Category("Aggregation")]
        public void ToDelimitedStringWithNonEmptySequenceAndDelimiter()
        {
            var result = Enumerable.ToDelimitedString(new[] { 1, 2, 3 }, "-");
            Assert.That(result, Is.EqualTo("1-2-3"));
        }

        [Test, Category("Aggregation")]
        public void ToDelimitedStringWithNonEmptySequenceAndDefaultDelimiter()
        {
            using (new CurrentThreadCultureScope(new CultureInfo("fr-FR")))
            {
                var result = Enumerable.ToDelimitedString(new[] {1, 2, 3});
                Assert.That(result, Is.EqualTo("1;2;3"));
            }
        }

        [Test, Category("Aggregation")]
        public void ToDelimitedStringWithNonEmptySequenceAndNullDelimiter()
        {
            using (new CurrentThreadCultureScope(new CultureInfo("fr-FR")))
            {
                var result = Enumerable.ToDelimitedString(new[] { 1, 2, 3 }, null);
                Assert.That(result, Is.EqualTo("1;2;3"));
            }
        }

        [Test, Category("Aggregation")]
        public void ToDelimitedStringWithNonEmptySequenceContainingNulls()
        {
            var result = Enumerable.ToDelimitedString(new object[] { 1, null, "foo", true }, ",");
            Assert.That(result, Is.EqualTo("1,,foo,True"));
        }
    }
}
