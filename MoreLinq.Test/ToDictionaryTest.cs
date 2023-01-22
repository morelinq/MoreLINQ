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
    public class ToDictionaryTest
    {
        [Test]
        public void ToDictionaryWithKeyValuePairs()
        {
            var pairs = new[]
            {
                KeyValuePair.Create("foo", 123),
                KeyValuePair.Create("bar", 456),
                KeyValuePair.Create("baz", 789),
            };

            var dict = pairs.ToDictionary();

            Assert.That(dict["foo"], Is.EqualTo(123));
            Assert.That(dict["bar"], Is.EqualTo(456));
            Assert.That(dict["baz"], Is.EqualTo(789));
        }

        [Test]
        public void ToDictionaryWithCouples()
        {
            var pairs = new[]
            {
                ("foo", 123),
                ("bar", 456),
                ("baz", 789),
            };

            var dict = pairs.ToDictionary();

            Assert.That(dict["foo"], Is.EqualTo(123));
            Assert.That(dict["bar"], Is.EqualTo(456));
            Assert.That(dict["baz"], Is.EqualTo(789));
        }

        [Test]
        public void ToDictionaryWithKeyValuePairsWithComparer()
        {
            var pairs = new[]
            {
                KeyValuePair.Create("foo", 123),
                KeyValuePair.Create("bar", 456),
                KeyValuePair.Create("baz", 789),
            };

            var dict = pairs.ToDictionary(StringComparer.OrdinalIgnoreCase);

            Assert.That(dict["FOO"], Is.EqualTo(123));
            Assert.That(dict["BAR"], Is.EqualTo(456));
            Assert.That(dict["BAZ"], Is.EqualTo(789));
        }

        [Test]
        public void ToDictionaryWithCouplesWithComparer()
        {
            var pairs = new[]
            {
                ("foo", 123),
                ("bar", 456),
                ("baz", 789),
            };

            var dict = pairs.ToDictionary(StringComparer.OrdinalIgnoreCase);

            Assert.That(dict["FOO"], Is.EqualTo(123));
            Assert.That(dict["BAR"], Is.EqualTo(456));
            Assert.That(dict["BAZ"], Is.EqualTo(789));
        }
    }
}
