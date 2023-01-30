#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2010 Leopold Bushkin. All rights reserved.
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

    /// <summary>
    /// Verify the behavior of the Rank operator
    /// </summary>
    [TestFixture]
    public class RankTests
    {
        /// <summary>
        /// Verify that rank behaves in a lazy manner.
        /// </summary>
        [Test]
        public void TestRankIsLazy()
        {
            _ = new BreakingSequence<int>().Rank();
        }

        /// <summary>
        /// Verify that rank behaves in a lazy manner.
        /// </summary>
        [Test]
        public void TestRankByIsLazy()
        {
            _ = new BreakingSequence<int>().RankBy(BreakingFunc.Of<int, int>());
        }

        /// <summary>
        /// Verify that Rank uses the default comparer when comparer is <c>null</c>
        /// </summary>
        [Test]
        public void TestRankNullComparer()
        {
            var sequence = Enumerable.Repeat(1, 10);
            using var ts = sequence.AsTestingSequence();
            ts.Rank(null).AssertSequenceEqual(sequence);
        }

        /// <summary>
        /// Verify that Rank uses the default comparer when comparer is <c>null</c>
        /// </summary>
        [Test]
        public void TestRankByNullComparer()
        {
            var sequence = Enumerable.Repeat(1, 10);
            using var ts = sequence.AsTestingSequence();
            ts.RankBy(x => x, null).AssertSequenceEqual(sequence);
        }

        /// <summary>
        /// Verify that ranking a descending series of integers produces
        /// a linear, progressive rank for each value.
        /// </summary>
        [Test]
        public void TestRankDescendingSequence()
        {
            const int count = 100;
            using var sequence = Enumerable.Range(456, count).Reverse().AsTestingSequence();
            var result = sequence.Rank().ToArray();
            var expectedResult = Enumerable.Range(1, count);

            Assert.That(result.Length, Is.EqualTo(count));
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Verify that ranking an ascending series of integers produces
        /// a linear, regressive rank for each value.
        /// </summary>
        [Test]
        public void TestRankByAscendingSeries()
        {
            const int count = 100;
            using var sequence = Enumerable.Range(456, count).AsTestingSequence();
            var result = sequence.Rank().ToArray();
            var expectedResult = Enumerable.Range(1, count).Reverse();

            Assert.That(result.Length, Is.EqualTo(count));
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Verify that the rank of a sequence of the same item is always 1.
        /// </summary>
        [Test]
        public void TestRankEquivalentItems()
        {
            const int count = 100;
            using var sequence = Enumerable.Repeat(1234, count).AsTestingSequence();
            var result = sequence.Rank().ToArray();

            Assert.That(result.Length, Is.EqualTo(count));
            Assert.That(result, Is.EqualTo(Enumerable.Repeat(1, count)));
        }

        /// <summary>
        /// Verify that the rank of equivalent items in a sequence is the same.
        /// </summary>
        [Test]
        public void TestRankGroupedItems()
        {
            const int count = 10;
            var sequence = Enumerable.Range(0, count)
                .Concat(Enumerable.Range(0, count))
                .Concat(Enumerable.Range(0, count));
            using var ts = sequence.AsTestingSequence();
            var result = ts.Rank();

            Assert.That(result.Distinct().Count(), Is.EqualTo(count));
            Assert.That(result, Is.EqualTo(sequence.Reverse().Select(x => x + 1)));
        }

        /// <summary>
        /// Verify that the highest rank (that of the largest item) is 1 (not 0).
        /// </summary>
        [Test]
        public void TestRankOfHighestItemIsOne()
        {
            const int count = 10;
            using var sequence = Enumerable.Range(1, count).AsTestingSequence();
            var result = sequence.Rank();

            Assert.That(result.OrderBy(x => x).First(), Is.EqualTo(1));
        }

        /// <summary>
        /// Verify that we can rank items by an arbitrary key produced from the item.
        /// </summary>
        [Test]
        public void TestRankByKeySelector()
        {
            var sequence = new[]
                               {
                                   new { Name = "Bob", Age = 24, ExpectedRank = 5 },
                                   new { Name = "Sam", Age = 51, ExpectedRank = 2 },
                                   new { Name = "Kim", Age = 18, ExpectedRank = 7 },
                                   new { Name = "Tim", Age = 23, ExpectedRank = 6 },
                                   new { Name = "Joe", Age = 31, ExpectedRank = 3 },
                                   new { Name = "Mel", Age = 28, ExpectedRank = 4 },
                                   new { Name = "Jim", Age = 74, ExpectedRank = 1 },
                                   new { Name = "Jes", Age = 11, ExpectedRank = 8 },
                               };
            using var ts = sequence.AsTestingSequence();
            var result = ts.RankBy(x => x.Age).ToArray();

            Assert.That(result.Length, Is.EqualTo(sequence.Length));
            Assert.That(result, Is.EqualTo(sequence.Select(x => x.ExpectedRank)));
        }

        /// <summary>
        /// Verify that Rank can use a custom comparer
        /// </summary>
        [Test]
        public void TestRankCustomComparer()
        {
            const int count = 10;
            var ordinals = Enumerable.Range(1, count);
            var sequence = ordinals.Select(x => new DateTime(2010, x, 20 - x));
            // invert the CompareTo operation to Rank in reverse order (ascending to descending)
            using var tsA = sequence.AsTestingSequence();
            var resultA = tsA.Rank(Comparer<DateTime>.Create((a, b) => -a.CompareTo(b)));
            using var tsB = sequence.AsTestingSequence();
            var resultB = tsB.RankBy(x => x.Day, Comparer<int>.Create((a, b) => -a.CompareTo(b)));

            Assert.That(resultA, Is.EqualTo(ordinals));
            Assert.That(resultB, Is.EqualTo(ordinals.Reverse()));
        }
    }
}
