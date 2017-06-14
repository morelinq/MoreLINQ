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
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class SplitAtTest
    {
        [Test]
        public void SplitAtIsLazy2()
        {
            var (first, second) = new BreakingSequence<object>().SplitAt(0);
            Assert.That(first, Is.Not.Null);
            Assert.That(second, Is.Not.Null);
            Assert.That(first, Is.Not.SameAs(second));
        }

        [Test]
        public void SplitAtWithResultSelectorIsLazy()
        {
            var (first, second) = new BreakingSequence<object>().SplitAt(0, ValueTuple.Create);
            Assert.That(first, Is.Not.Null);
            Assert.That(second, Is.Not.Null);
            Assert.That(first, Is.Not.SameAs(second));
        }

        [TestCase( 0, new int[0]                             , new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        [TestCase( 2, new[] { 1, 2                          }, new[] { 3, 4, 5, 6, 7, 8, 9, 10       })]
        [TestCase( 5, new[] { 1, 2, 3, 4, 5                 }, new[] { 6, 7, 8, 9, 10                })]
        [TestCase(10, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new int[0]                             )]
        [TestCase(20, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new int[0]                             )]
        [TestCase(-5, new int[0]                             , new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        public void SplitAt(int index, int[] expected1, int[] expected2)
        {
            var xs = Enumerable.Range(1, 10).ToArray();

            AssertParts(xs.AsTestingList()    , input => input.SplitAt(index, ValueTuple.Create));
            AssertParts(xs.AsTestingSequence(), input => input.SplitAt(index, ValueTuple.Create));
            AssertParts(xs.AsTestingList()    , input => input.SplitAt(index));
            AssertParts(xs.AsTestingSequence(), input => input.SplitAt(index));

            void AssertParts<T>(T input, Func<IEnumerable<int>, (IEnumerable<int>, IEnumerable<int>)> splitter)
                where T : IEnumerable<int>, IDisposable
            {
                using (input)
                {
                    var (part1, part2) = splitter(input);
                    part1.AssertSequenceEqual(expected1);
                    part2.AssertSequenceEqual(expected2);
                }
            }
        }

        [TestCase( 0)]
        [TestCase(-1)]
        public void SplitAtWithIndexZeroOrLessReturnsSourceAsSecond(int index)
        {
            Assert.That(index, Is.LessThanOrEqualTo(0));

            var xs = Enumerable.Range(1, 10).ToArray();

            AssertParts(xs.AsTestingList(), input => input.SplitAt(index, ValueTuple.Create));
            AssertParts(xs.AsTestingList(), input => input.SplitAt(index));

            void AssertParts<T>(T input, Func<IEnumerable<int>, (IEnumerable<int>, IEnumerable<int>)> splitter)
                where T : IEnumerable<int>, IDisposable
            {
                using (input)
                {
                    var (part1, part2) = splitter(input);
                    Assert.That(part1, Is.Empty);
                    Assert.That(part2, Is.SameAs(input));
                }
            }
        }

        [TestCase(10)]
        [TestCase(11)]
        [TestCase(20)]
        public void SplitAtWithIndexGreaterOrEqualToSourceLengthReturnsSourceAsFirst(int index)
        {
            var xs = Enumerable.Range(1, 10).ToArray();

            AssertParts(xs.AsTestingList(), input => input.SplitAt(index, ValueTuple.Create));
            AssertParts(xs.AsTestingList(), input => input.SplitAt(index));

            void AssertParts<T>(T input, Func<IEnumerable<int>, (IEnumerable<int>, IEnumerable<int>)> splitter)
                where T : IEnumerable<int>, IDisposable
            {
                using (input)
                {
                    var (part1, part2) = splitter(input);
                    Assert.That(part1, Is.SameAs(input));
                    Assert.That(part2, Is.Empty);
                }
            }
        }
    }
}
