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
            var result = source.IndexBy(x => x.First(), (item, key, index) => (item, key, index));

            result.AssertSequenceEqual(
                ( item: "ana",     key: 'a', index: 0 ),
                ( item: "beatriz", key: 'b', index: 0 ),
                ( item: "carla",   key: 'c', index: 0 ),
                ( item: "bob",     key: 'b', index: 1 ),
                ( item: "davi",    key: 'd', index: 0 ),
                ( item: "adriano", key: 'a', index: 1 ),
                ( item: "angelo",  key: 'a', index: 2 ),
                ( item: "carlos",  key: 'c', index: 1 ));
        }

        [Test]
        public void IndexByWithSecondOccurenceImmediatelyAfterFirst()
        {
            var result = "jaffer".IndexBy(c => c, (item, key, index) => (item, key, index));

            result.AssertSequenceEqual(
                ( item: 'j', key: 'j', index: 0 ),
                ( item: 'a', key: 'a', index: 0 ),
                ( item: 'f', key: 'f', index: 0 ),
                ( item: 'f', key: 'f', index: 1 ),
                ( item: 'e', key: 'e', index: 0 ),
                ( item: 'r', key: 'r', index: 0 ));
        }

        [Test]
        public void IndexByWithEqualityComparer()
        {
            var result = new[] { "a", "B", "c", "A", "b", "A" }.IndexBy(c => c, (item, key, index) => (item, key, index), StringComparer.OrdinalIgnoreCase);

            result.AssertSequenceEqual(
               ( item: "a", key: "a", index: 0 ),
               ( item: "B", key: "B", index: 0 ),
               ( item: "c", key: "c", index: 0 ),
               ( item: "A", key: "A", index: 1 ),
               ( item: "b", key: "b", index: 1 ),
               ( item: "A", key: "A", index: 2 ));
        }

        [Test]
        public void IndexByIsLazy()
        {
            new BreakingSequence<string>().IndexBy(BreakingFunc.Of<string, char>(), BreakingFunc.Of<string, char, int, bool>());
        }

        [Test]
        public void IndexByWithSomeNullKeys()
        {
            var source = new[]
            {
                "foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo"
            };

            var result = source.IndexBy(c => c, (item, key, index) => (item, key, index));

            result.AssertSequenceEqual(
                ( item: "foo", key: "foo", index: 0 ),
                ( item: null,  key: null,  index: 0 ),
                ( item: "bar", key: "bar", index: 0 ),
                ( item: "baz", key: "baz", index: 0 ),
                ( item: null,  key: null,  index: 1 ),
                ( item: null,  key: null,  index: 2 ),
                ( item: "baz", key: "baz", index: 1 ),
                ( item: "bar", key: "bar", index: 1 ),
                ( item: null,  key: null,  index: 3 ),
                ( item: "foo", key: "foo", index: 1 ));
        }
    }
}
