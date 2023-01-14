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

namespace MoreLinq.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class ToDelimitedStringTest
    {
        [Test]
        public void ToDelimitedStringWithNonEmptySequenceAndDelimiter()
        {
            var result = new[] { 1, 2, 3 }.ToDelimitedString("-");
            Assert.That(result, Is.EqualTo("1-2-3"));
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceContainingNulls()
        {
            var result = new object?[] { 1, null, "foo", true }.ToDelimitedString(",");
            Assert.That(result, Is.EqualTo("1,,foo,True"));
        }

        [Test]
        public void ToDelimitedStringWithNonEmptySequenceContainingNullsAtStart()
        {
            // See: https://github.com/morelinq/MoreLINQ/issues/43
            var result = new object?[] { null, null, "foo" }.ToDelimitedString(",");
            Assert.That(result, Is.EqualTo(",,foo"));
        }
    }
}
