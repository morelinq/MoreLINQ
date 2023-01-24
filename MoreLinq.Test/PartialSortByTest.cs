#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Atif Aziz. All rights reserved.
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
    public class PartialSortByTests
    {
        [Test]
        public void PartialSortBy()
        {
            var ns = MoreEnumerable.RandomDouble().Take(10).ToArray();

            const int count = 5;
            var sorted = ns.Select((n, i) => KeyValuePair.Create(i, n))
                           .Reverse()
                           .PartialSortBy(count, e => e.Key);

            sorted.Select(e => e.Value).AssertSequenceEqual(ns.Take(count));
        }

        [Test]
        public void PartialSortWithOrder()
        {
            var ns = MoreEnumerable.RandomDouble().Take(10).ToArray();

            const int count = 5;
            var sorted = ns.Select((n, i) => KeyValuePair.Create(i, n))
                            .Reverse()
                            .PartialSortBy(count, e => e.Key, OrderByDirection.Ascending);

            sorted.Select(e => e.Value).AssertSequenceEqual(ns.Take(count));

            sorted = ns.Select((n, i) => KeyValuePair.Create(i, n))
                        .Reverse()
                        .PartialSortBy(count, e => e.Key, OrderByDirection.Descending);

            sorted.Select(e => e.Value).AssertSequenceEqual(ns.Reverse().Take(count));
        }

        [Test]
        public void PartialSortWithComparer()
        {
            var alphabet = Enumerable.Range(0, 26)
                                     .Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
                                     .ToArray();

            var ns = alphabet.Zip(MoreEnumerable.RandomDouble(), KeyValuePair.Create).ToArray();
            var sorted = ns.PartialSortBy(5, e => e.Key, StringComparer.Ordinal);

            sorted.Select(e => e.Key[0]).AssertSequenceEqual('A', 'C', 'E', 'G', 'I');
        }

        [Test]
        public void PartialSortByIsLazy()
        {
            _ = new BreakingSequence<object>().PartialSortBy(1, BreakingFunc.Of<object, object>());
        }

        [Test, Ignore("TODO")]
        public void PartialSortByIsStable()
        {
            // Force creation of same strings to avoid reference equality at
            // start via interned literals.

            var foobar = "foobar".ToCharArray();
            var foobars = Enumerable.Repeat(foobar, 10)
                                    .Select(chars => new string(chars))
                                    .ToArray();

            var sorted = foobars.PartialSort(5);

            // Pair expected and actual by index and then check
            // reference equality, finding the first mismatch.

            var mismatchIndex =
                foobars.Index()
                       .Zip(sorted, (expected, actual) => new
                       {
                           Index = expected.Key,
                           Pass = ReferenceEquals(expected.Value, actual)
                       })
                       .FirstOrDefault(e => !e.Pass)?.Index;

            Assert.That(mismatchIndex, Is.Null, "Mismatch index");
        }
    }
}
