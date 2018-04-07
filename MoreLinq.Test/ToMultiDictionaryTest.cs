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
    using System;
    using NUnit.Framework;
    using MoreLinq;
    using System.Collections.Generic;

    [TestFixture]
    public class ToMultiDictionaryTest
    {
        [Test]
        public void TestListString()
        {
            var list = new List<string>
            {
                "foo", "bar", "foo", "bar", "bar", "baz", "test1", "test2"
            };
            var dict = list.ToMultiDictionary(x => x);

            Assert.That(dict["baz"].Count(), Is.EqualTo(1));
            Assert.That(dict["foo"].Count(), Is.EqualTo(2));
            Assert.That(dict["bar"].Count(), Is.EqualTo(3));

        }

        [Test]
        public void TestListStringWithComparer()
        {
            var list = new List<string>
            {
                "foo", "bAr", "Foo", "Bar", "bAR", "BAR", "baz", "test1", "Test1", "Test3", "test2", "fOO"
            };
            var dict = list.ToMultiDictionary(x => x, StringComparer.OrdinalIgnoreCase);

            Assert.That(dict["bar"].Count(),
                Is.EqualTo(dict["BAR"].Count()).And
                .EqualTo(dict["BaR"].Count()).And
                .EqualTo(dict["Bar"].Count()).And
                .EqualTo(4));
            Assert.That(dict["foo"].Count(),
                Is.EqualTo(dict["FOO"].Count()).And
                .EqualTo(3));

        }


        class Dummy
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }

        [Test]
        public void TestListObject()
        {
            var list = new List<Dummy>
            {
                new Dummy { Id = 1, Value = "val1" },
                new Dummy { Id = 2, Value = "val2" },
                new Dummy { Id = 2, Value = "val3" },
                new Dummy { Id = 2, Value = "val4" },
                new Dummy { Id = 3, Value = "val5" },
                new Dummy { Id = 3, Value = "val6" },
            };
            var dict = list.ToMultiDictionary(x => x.Id, x => x.Value);

            Assert.That(dict[1].Count(), Is.EqualTo(1));
            Assert.That(dict[2].Count(), Is.EqualTo(3));
            Assert.That(dict[3].Count(), Is.EqualTo(2));

            Assert.That(dict[1].Contains("val1"), Is.True);
            Assert.That(dict[2].Contains("val2"), Is.True);
            Assert.That(dict[2].Contains("val5"), Is.False);
            Assert.That(dict[2].Contains("val3"), Is.True);
            Assert.That(dict[3].Contains("val6"), Is.True);
        }
    }
}
