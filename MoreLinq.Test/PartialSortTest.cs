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
    using static MoreLinq.Extensions.AppendExtension;

    [TestFixture]
    public class PartialSortTests
    {
        [Test]
        public void PartialSort()
        {
            var sorted = Enumerable.Range(1, 10)
                                   .Reverse()
                                   .Append(0)
                                   .PartialSort(5);

            sorted.AssertSequenceEqual(Enumerable.Range(0, 5));
        }

        [Test]
        public void PartialSortWithOrder()
        {
            var sorted = Enumerable.Range(1, 10)
                                    .Reverse()
                                    .Append(0)
                                    .PartialSort(5, OrderByDirection.Ascending);

            sorted.AssertSequenceEqual(Enumerable.Range(0, 5));
            sorted = Enumerable.Range(1, 10)
                                .Reverse()
                                .Append(0)
                                .PartialSort(5, OrderByDirection.Descending);
            sorted.AssertSequenceEqual(Enumerable.Range(6, 5).Reverse());
        }

        [Test]
        public void PartialSortWithDuplicates()
        {
            var sorted = Enumerable.Range(1, 10)
                                   .Reverse()
                                   .Concat(Enumerable.Repeat(3, 3))
                                   .PartialSort(5);

            sorted.AssertSequenceEqual(1, 2, 3, 3, 3);
        }

        [Test]
        public void PartialSortWithComparer()
        {
            var alphabet = Enumerable.Range(0, 26)
                                     .Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
                                     .ToArray();

            var sorted = alphabet.PartialSort(5, StringComparer.Ordinal);

            sorted.Select(s => s[0]).AssertSequenceEqual('A', 'C', 'E', 'G', 'I');
        }

        [Test]
        public void PartialSortIsLazy()
        {
            _ = new BreakingSequence<object>().PartialSort(1);
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

            var sorted = foobars.PartialSortBy(5, s => s.Length);

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
