#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Leandro F. Vieira (leandromoh). All rights reserved.
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
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class CountBetweenTest
    {
        [Test]
        public void CountBetweenWithNegativeMin()
        {
            AssertThrowsArgument.OutOfRangeException("min", () =>
                new[] { 1 }.CountBetween(-1, 0));
        }

        [Test]
        public void CountBetweenWithNegativeMax()
        {
            AssertThrowsArgument.OutOfRangeException("max", () =>
               new[] { 1 }.CountBetween(0, -1));
        }

        [Test]
        public void CountBetweenWithMaxLesserThanMin()
        {
            AssertThrowsArgument.OutOfRangeException("max", () =>
                new[] { 1 }.CountBetween(1, 0));
        }

        [TestCaseSource(nameof(CountBetweenData))]
        public bool CountBetween(SourceKind sourceKind, int count, int min, int max)
        {
            return Enumerable.Range(0, count).ToSourceKind(sourceKind).CountBetween(min, max);
        }

        static IEnumerable<TestCaseData> CountBetweenData()
        {
            return (
                from count in new[]
                {
                    (1, 1, 1),
                    (1, 2, 4),
                    (2, 2, 4),
                    (3, 2, 4),
                    (4, 2, 4),
                    (5, 2, 4)
                }
                from type in new[] { SourceKind.Sequence, SourceKind.BreakingCollection, SourceKind.BreakingReadOnlyCollection }
                select new TestCaseData(type, count.Item1, count.Item2, count.Item3)
                    .Returns(count.Item1 >= count.Item2 && count.Item1 <= count.Item3)
                    .SetName($"{{m}}({type}[{count.Item1}], {count.Item2}-{count.Item3}")
            );
        }

        [Test]
        public void CountBetweenDoesNotIterateUnnecessaryElements()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => 2,
                                             () => 3,
                                             () => 4,
                                             () => throw new TestException());
            Assert.False(source.CountBetween(2, 3));
        }
    }
}
