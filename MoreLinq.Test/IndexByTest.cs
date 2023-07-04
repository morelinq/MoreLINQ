#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Leandro F. Vieira (leandromoh). All rights reserved.
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
    public class IndexByTest
    {
        [Test]
        public void IndexBySimpleTest()
        {
            var source = new[] { "ana", "beatriz", "carla", "bob", "davi", "adriano", "angelo", "carlos" };
            var result = source.IndexBy(x => x.First());

            result.AssertSequenceEqual(
                KeyValuePair.Create(0, "ana"    ),
                KeyValuePair.Create(0, "beatriz"),
                KeyValuePair.Create(0, "carla"  ),
                KeyValuePair.Create(1, "bob"    ),
                KeyValuePair.Create(0, "davi"   ),
                KeyValuePair.Create(1, "adriano"),
                KeyValuePair.Create(2, "angelo" ),
                KeyValuePair.Create(1, "carlos" ));
        }

        [Test]
        public void IndexByWithSecondOccurenceImmediatelyAfterFirst()
        {
            var result = "jaffer".IndexBy(c => c);

            result.AssertSequenceEqual(
                KeyValuePair.Create(0, 'j'),
                KeyValuePair.Create(0, 'a'),
                KeyValuePair.Create(0, 'f'),
                KeyValuePair.Create(1, 'f'),
                KeyValuePair.Create(0, 'e'),
                KeyValuePair.Create(0, 'r'));
        }

        [Test]
        public void IndexByWithEqualityComparer()
        {
            var source = new[] { "a", "B", "c", "A", "b", "A" };
            var result = source.IndexBy(c => c, StringComparer.OrdinalIgnoreCase);

            result.AssertSequenceEqual(
               KeyValuePair.Create(0, "a"),
               KeyValuePair.Create(0, "B"),
               KeyValuePair.Create(0, "c"),
               KeyValuePair.Create(1, "A"),
               KeyValuePair.Create(1, "b"),
               KeyValuePair.Create(2, "A"));
        }

        [Test]
        public void IndexByIsLazy()
        {
            _ = new BreakingSequence<string>().IndexBy(BreakingFunc.Of<string, char>());
        }

        [Test]
        public void IndexByWithSomeNullKeys()
        {
            var source = new[] { "foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo" };
            var result = source.IndexBy(c => c);

            result.AssertSequenceEqual(
                KeyValuePair.Create(0, (string?)"foo"),
                KeyValuePair.Create(0, (string?)null),
                KeyValuePair.Create(0, (string?)"bar"),
                KeyValuePair.Create(0, (string?)"baz"),
                KeyValuePair.Create(1, (string?)null),
                KeyValuePair.Create(2, (string?)null),
                KeyValuePair.Create(1, (string?)"baz"),
                KeyValuePair.Create(1, (string?)"bar"),
                KeyValuePair.Create(3, (string?)null),
                KeyValuePair.Create(1, (string?)"foo"));
        }

        [Test]
        public void IndexBytDoesNotIterateUnnecessaryElements()
        {
            var source = MoreEnumerable.From(() => "ana",
                                             () => "beatriz",
                                             () => "carla",
                                             () => "bob",
                                             () => "davi",
                                             () => throw new TestException(),
                                             () => "angelo",
                                             () => "carlos");

            var result = source.IndexBy(x => x.First());

            result.Take(5).AssertSequenceEqual(
                KeyValuePair.Create(0, "ana"    ),
                KeyValuePair.Create(0, "beatriz"),
                KeyValuePair.Create(0, "carla"  ),
                KeyValuePair.Create(1, "bob"    ),
                KeyValuePair.Create(0, "davi"   ));

            Assert.That(() => result.ElementAt(5),
                        Throws.TypeOf<TestException>());
        }
    }
}
