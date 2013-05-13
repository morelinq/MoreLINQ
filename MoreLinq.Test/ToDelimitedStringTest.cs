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

using System;
using System.Globalization;
using NUnit.Framework;
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
