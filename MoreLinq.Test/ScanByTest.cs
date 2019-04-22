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
            new BreakingSequence<string>().ScanBy(
                BreakingFunc.Of<string, int>(),
                BreakingFunc.Of<int, char>(),
                BreakingFunc.Of<string, int, char, char>(),
                BreakingFunc.Of<string, int, char, bool>());
        }

        [Test]
        public void ScanBy()
        {
            var sourceIndex = 0;
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

            var uniqueKeysIndex = 0;
            var uniqueKeys = source.Select(name => name.First())
                                   .Distinct()
                                   .ToArray();

            var result = source.ScanBy(
                item =>
                {
                    Assert.That(item, Is.EqualTo(source[sourceIndex]));
                    return item.First();
                },
                firstLetter =>
                {
                    Assert.That(firstLetter, Is.EqualTo(uniqueKeys[uniqueKeysIndex]));
                    uniqueKeysIndex++;
                    return -1;
                },
                (item, key, state) =>
                {
                    Assert.That(item, Is.EqualTo(source[sourceIndex]));
                    Assert.That(key, Is.EqualTo(item.First()));
                    sourceIndex++;
                    return state + 1;
                },
                ValueTuple.Create);

            result.AssertSequenceEqual(
                ("ana", 'a', 0),
                ("beatriz", 'b', 0),
                ("carla", 'c', 0),
                ("bob", 'b', 1),
                ("davi", 'd', 0),
                ("adriano", 'a', 1),
                ("angelo", 'a', 2),
                ("carlos", 'c', 1));
        }

        [Test]
        public void ScanByWithSecondOccurenceImmediatelyAfterFirst()
        {
            var result = "jaffer".ScanBy(c => c, k => -1, (e, k, i) => i + 1, ValueTuple.Create);

            result.AssertSequenceEqual(
                ('j', 'j', 0),
                ('a', 'a', 0),
                ('f', 'f', 0),
                ('f', 'f', 1),
                ('e', 'e', 0),
                ('r', 'r', 0));
        }

        [Test]
        public void ScanByWithEqualityComparer()
        {
            var source = new[] { "a", "B", "c", "A", "b", "A" };
            var result = source.ScanBy(c => c,
                                       k => -1,
                                       (e, k, i) => i + 1,
                                       ValueTuple.Create,
                                       StringComparer.OrdinalIgnoreCase);

            result.AssertSequenceEqual(
               ("a", "a", 0),
               ("B", "B", 0),
               ("c", "c", 0),
               ("A", "A", 1),
               ("b", "b", 1),
               ("A", "A", 2));
        }

        [Test]
        public void ScanByWithSomeNullKeys()
        {
            var source = new[] { "foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo" };
            var result = source.ScanBy(c => c, k => -1, (e, k, i) => i + 1, ValueTuple.Create);

            result.AssertSequenceEqual(
                ("foo", "foo", 0),
                (null,  null,  0),
                ("bar", "bar", 0),
                ("baz", "baz", 0),
                (null,  null,  1),
                (null,  null,  2),
                ("baz", "baz", 1),
                ("bar", "bar", 1),
                (null,  null,  3),
                ("foo", "foo", 1));
        }
    }
}
