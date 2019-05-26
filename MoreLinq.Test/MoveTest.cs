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
        public IEnumerable<int> Move(IEnumerable<int> source, int fromIndex, int count, int toIndex)
        {
            using (var test = source.AsTestingSequence())
            {
                return test.Move(fromIndex, count, toIndex);
            }
        }

        public static IEnumerable<object> MoveSource()
        {
            const int length = 10;
            var source = Enumerable.Range(0, length);

            return from index in source
                   from count in Enumerable.Range(0, length + 1)
                   from tcd in new[]
                   {
                       CreateTestCaseData(source, index, count, Math.Max(0, index - 1)),
                       CreateTestCaseData(source, index, count, index + 1),
                   }
                   select tcd;
        }

        [TestCaseSource(nameof(MoveWithSequenceShorterThanToIndexSource))]
        public IEnumerable<int> MoveWithSequenceShorterThanToIndex(IEnumerable<int> source, int fromIndex, int count, int toIndex)
        {
            using (var test = source.AsTestingSequence())
            {
                return test.Move(fromIndex, count, toIndex);
            }
        }

        public static IEnumerable<object> MoveWithSequenceShorterThanToIndexSource()
        {
            const int length = 10;
            var source = Enumerable.Range(0, length);

            return Enumerable.Range(length, length + 5)
                             .Select(toIndex => CreateTestCaseData(source, 5, 2, toIndex));
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

        private static TestCaseData CreateTestCaseData(IEnumerable<int> source, int fromIndex, int count, int toIndex)
        {
            var exclude = source.Exclude(fromIndex, count);
            var slice = source.Slice(fromIndex, count);
            var expectations = exclude.Take(toIndex).Concat(slice).Concat(exclude.Skip(toIndex));

            return new TestCaseData(source, fromIndex, count, toIndex)
                    .Returns(expectations)
                    .SetName($"source = [{string.Join(", ", source)}], " +
                             $"fromIndex = {fromIndex}, " +
                             $"count = {count}, " +
                             $"toIndex = {toIndex}, " +
                             $"expectations = [{string.Join(", ", expectations)}]");
        }
    }
}
