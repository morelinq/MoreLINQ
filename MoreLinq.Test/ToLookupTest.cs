#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All rights reserved.
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
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class ToLookupTest
    {
        [Test]
        public void ToLookupWithKeyValuePairs()
        {
            var pairs = new[]
            {
                KeyValuePair.Create("foo", 1),
                KeyValuePair.Create("bar", 3),
                KeyValuePair.Create("baz", 4),
                KeyValuePair.Create("foo", 2),
                KeyValuePair.Create("baz", 5),
                KeyValuePair.Create("baz", 6),
            };

            var dict = pairs.ToLookup();

            Assert.That(dict.Count, Is.EqualTo(3));
            Assert.That(dict["foo"], Is.EqualTo(new[] { 1, 2 }));
            Assert.That(dict["bar"], Is.EqualTo(new[] { 3 }));
            Assert.That(dict["baz"], Is.EqualTo(new[] { 4, 5, 6 }));
        }

        [Test]
        public void ToLookupWithCouples()
        {
            var pairs = new[]
            {
                ("foo", 1),
                ("bar", 3),
                ("baz", 4),
                ("foo", 2),
                ("baz", 5),
                ("baz", 6),
            };

            var dict = pairs.ToLookup();

            Assert.That(dict.Count, Is.EqualTo(3));
            Assert.That(dict["foo"], Is.EqualTo(new[] { 1, 2 }));
            Assert.That(dict["bar"], Is.EqualTo(new[] { 3 }));
            Assert.That(dict["baz"], Is.EqualTo(new[] { 4, 5, 6 }));
        }

        [Test]
        public void ToLookupWithKeyValuePairsWithComparer()
        {
            var pairs = new[]
            {
                KeyValuePair.Create("foo", 1),
                KeyValuePair.Create("bar", 3),
                KeyValuePair.Create("baz", 4),
                KeyValuePair.Create("foo", 2),
                KeyValuePair.Create("baz", 5),
                KeyValuePair.Create("baz", 6),
            };

            var dict = pairs.ToLookup(StringComparer.OrdinalIgnoreCase);

            Assert.That(dict.Count, Is.EqualTo(3));
            Assert.That(dict["FOO"], Is.EqualTo(new[] { 1, 2 }));
            Assert.That(dict["BAR"], Is.EqualTo(new[] { 3 }));
            Assert.That(dict["BAZ"], Is.EqualTo(new[] { 4, 5, 6 }));
        }

        [Test]
        public void ToLookupWithCouplesWithComparer()
        {
            var pairs = new[]
            {
                ("foo", 1),
                ("bar", 3),
                ("baz", 4),
                ("foo", 2),
                ("baz", 5),
                ("baz", 6),
            };

            var dict = pairs.ToLookup(StringComparer.OrdinalIgnoreCase);

            Assert.That(dict.Count, Is.EqualTo(3));
            Assert.That(dict["FOO"], Is.EqualTo(new[] { 1, 2 }));
            Assert.That(dict["BAR"], Is.EqualTo(new[] { 3 }));
            Assert.That(dict["BAZ"], Is.EqualTo(new[] { 4, 5, 6 }));
        }
    }
}
