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
    public class ScanByTest
    {
        [Test]
        public void ScanByIsLazy()
        {
            var bs = new BreakingSequence<string>();
            _ = bs.ScanBy(BreakingFunc.Of<string, int>(),
                          BreakingFunc.Of<int, char>(),
                          BreakingFunc.Of<char, int, string, char>());
        }

        [Test]
        public void ScanBy()
        {
            var source = new[]
            {
                "ana",
                "beatriz",
                "carla",
                "bob",
                "davi",
                "adriano",
                "angelo",
                "carlos"
            };

            var result =
                    source.ScanBy(
                        item => item.First(),
                        key => (Element: string.Empty, Key: key, State: key - 1),
                        (state, key, item) => (item, char.ToUpperInvariant(key), state.State + 1));

            result.AssertSequenceEqual(
               KeyValuePair.Create('a', ("ana",     'A', 97)),
               KeyValuePair.Create('b', ("beatriz", 'B', 98)),
               KeyValuePair.Create('c', ("carla",   'C', 99)),
               KeyValuePair.Create('b', ("bob",     'B', 99)),
               KeyValuePair.Create('d', ("davi",    'D', 100)),
               KeyValuePair.Create('a', ("adriano", 'A', 98)),
               KeyValuePair.Create('a', ("angelo",  'A', 99)),
               KeyValuePair.Create('c', ("carlos",  'C', 100)));
        }

        [Test]
        public void ScanByWithSecondOccurenceImmediatelyAfterFirst()
        {
            var result = "jaffer".ScanBy(c => c, _ => -1, (i, _, _) => i + 1);

            result.AssertSequenceEqual(
                KeyValuePair.Create('j', 0),
                KeyValuePair.Create('a', 0),
                KeyValuePair.Create('f', 0),
                KeyValuePair.Create('f', 1),
                KeyValuePair.Create('e', 0),
                KeyValuePair.Create('r', 0));
        }

        [Test]
        public void ScanByWithEqualityComparer()
        {
            var source = new[] { "a", "B", "c", "A", "b", "A" };
            var result = source.ScanBy(c => c,
                                       _ => -1,
                                       (i, _, _) => i + 1,
                                       StringComparer.OrdinalIgnoreCase);

            result.AssertSequenceEqual(
               KeyValuePair.Create("a", 0),
               KeyValuePair.Create("B", 0),
               KeyValuePair.Create("c", 0),
               KeyValuePair.Create("A", 1),
               KeyValuePair.Create("b", 1),
               KeyValuePair.Create("A", 2));
        }

        [Test]
        public void ScanByWithSomeNullKeys()
        {
            var source = new[] { "foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo" };
            var result = source.ScanBy(c => c, _ => -1, (i, _, _) => i + 1);

            result.AssertSequenceEqual(
                KeyValuePair.Create((string?)"foo", 0),
                KeyValuePair.Create((string?)null , 0),
                KeyValuePair.Create((string?)"bar", 0),
                KeyValuePair.Create((string?)"baz", 0),
                KeyValuePair.Create((string?)null , 1),
                KeyValuePair.Create((string?)null , 2),
                KeyValuePair.Create((string?)"baz", 1),
                KeyValuePair.Create((string?)"bar", 1),
                KeyValuePair.Create((string?)null , 3),
                KeyValuePair.Create((string?)"foo", 1));
        }

        [Test]
        public void ScanByWithNullSeed()
        {
            var nil = (object?)null;
            var source = new[] { "foo", null, "bar", null, "baz" };
            var result = source.ScanBy(c => c, _ => nil, (_, _, _) => nil);

            result.AssertSequenceEqual(
                KeyValuePair.Create((string?)"foo", nil),
                KeyValuePair.Create((string?)null , nil),
                KeyValuePair.Create((string?)"bar", nil),
                KeyValuePair.Create((string?)null , nil),
                KeyValuePair.Create((string?)"baz", nil));
        }

        [Test]
        public void ScanByDoesNotIterateUnnecessaryElements()
        {
            var source = MoreEnumerable.From(() => "ana",
                                             () => "beatriz",
                                             () => "carla",
                                             () => "bob",
                                             () => "davi",
                                             () => throw new TestException(),
                                             () => "angelo",
                                             () => "carlos");

            var result = source.ScanBy(c => c.First(), _ => -1, (i, _, _) => i + 1);

            result.Take(5).AssertSequenceEqual(
                KeyValuePair.Create('a', 0),
                KeyValuePair.Create('b', 0),
                KeyValuePair.Create('c', 0),
                KeyValuePair.Create('b', 1),
                KeyValuePair.Create('d', 0));

            Assert.That(() => result.ElementAt(5),
                        Throws.TypeOf<TestException>());
        }
    }
}
