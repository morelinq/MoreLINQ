#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Leandro F. Vieira (leandromoh). All rights reserved.
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
    using NUnit.Framework;

    [TestFixture]
    public class MoveTest
    {
        [Test]
        public void MoveWithNegativeFromIndex()
        {
            AssertThrowsArgument.OutOfRangeException("fromIndex", () =>
                new[] { 1 }.Move(-1, 0, 0));
        }

        [Test]
        public void MoveWithNegativeCount()
        {
            AssertThrowsArgument.OutOfRangeException("count", () =>
                new[] { 1 }.Move(0, -1, 0));
        }

        [Test]
        public void MoveWithNegativeToIndex()
        {
            AssertThrowsArgument.OutOfRangeException("toIndex", () =>
                new[] { 1 }.Move(0, 0, -1));
        }

        [Test]
        public void MoveIsLazy()
        {
            new BreakingSequence<int>().Move(0, 0, 0);
        }

        [TestCaseSource(nameof(MoveSource))]
        public void Move(int length, int fromIndex, int count, int toIndex)
        {
            var source = Enumerable.Range(0, length);

            var exclude = source.Exclude(fromIndex, count);
            var slice = source.Slice(fromIndex, count);
            var expectations = exclude.Take(toIndex).Concat(slice).Concat(exclude.Skip(toIndex));

            using (var test = source.AsTestingSequence())
            {
                var result = test.Move(fromIndex, count, toIndex);
                Assert.That(result, Is.EquivalentTo(expectations));
            }
        }

        public static IEnumerable<object> MoveSource()
        {
            const int length = 10;
            return from index in Enumerable.Range(0, length)
                   from count in Enumerable.Range(0, length + 1)
                   from tcd in new[]
                   {
                       new TestCaseData(length, index, count, Math.Max(0, index - 1)),
                       new TestCaseData(length, index, count, index + 1),
                   }
                   select tcd;
        }

        [TestCaseSource(nameof(MoveWithSequenceShorterThanToIndexSource))]
        public void MoveWithSequenceShorterThanToIndex(int length, int fromIndex, int count, int toIndex)
        {
            var source = Enumerable.Range(0, length);

            var expectations = source.Exclude(fromIndex, count).Concat(source.Slice(fromIndex, count));

            using (var test = source.AsTestingSequence())
            {
                var result = test.Move(fromIndex, count, toIndex);
                Assert.That(result, Is.EquivalentTo(expectations));
            }
        }

        public static IEnumerable<object> MoveWithSequenceShorterThanToIndexSource()
        {
            const int length = 10;

            return Enumerable.Range(length, length + 5)
                             .Select(toIndex => new TestCaseData(length, 5, 2, toIndex));
        }

        [Test]
        public void MoveWithFromIndexEqualsToIndex()
        {
            var source = Enumerable.Range(0, 10);
            var result = source.Move(5, 999, 5);

            Assert.That(source, Is.SameAs(result));
        }

        [Test]
        public void MoveWithCountEqualsZero()
        {
            var source = Enumerable.Range(0, 10);
            var result = source.Move(5, 0, 999);

            Assert.That(source, Is.SameAs(result));
        }
    }
}
