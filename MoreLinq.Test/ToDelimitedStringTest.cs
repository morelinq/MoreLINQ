#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

#pragma warning disable 612 // 'ToDelimitedString' is obsolete

using System.Globalization;
using NUnit.Framework;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class ToDelimitedStringTest
    {
        [Test]
        public void ToDelimitedStringWithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source",() =>
                MoreEnumerable.ToDelimitedString<int>(null, ","));
        }

        [Test]
        public void ToDelimitedStringWithEmptySequence()
        {
            Assert.That(LinqEnumerable.Empty<int>().ToDelimitedString(), Is.Empty);
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceAndDelimiter()
        {
            var result = new[] { 1, 2, 3 }.ToDelimitedString("-");
            Assert.That(result, Is.EqualTo("1-2-3"));
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceAndDefaultDelimiter()
        {
            using (new CurrentThreadCultureScope(new CultureInfo("fr-FR")))
            {
                var xs = new[] { 1, 2, 3 };
                var result = xs.ToDelimitedString();
                var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
                Assert.That(result, Is.EqualTo(string.Join(separator, xs)));
            }
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceAndNullDelimiter()
        {
            using (new CurrentThreadCultureScope(new CultureInfo("fr-FR")))
            {
                var xs = new[] { 1, 2, 3 };
                var result = xs.ToDelimitedString(null);
                var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
                Assert.That(result, Is.EqualTo(string.Join(separator, xs)));
            }
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceContainingNulls()
        {
            var result = new object[] { 1, null, "foo", true }.ToDelimitedString(",");
            Assert.That(result, Is.EqualTo("1,,foo,True"));
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceContainingNullsAtStart()
        {
            // See: http://code.google.com/p/morelinq/issues/detail?id=43
            var result = new object[] { null, null, "foo" }.ToDelimitedString(",");
            Assert.That(result, Is.EqualTo(",,foo"));
        }
    }
}
