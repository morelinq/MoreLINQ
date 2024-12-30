#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2024 Andy Romero (armorynode). All rights reserved.
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
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class SkipLastWhileTest
    {
        [Test]
        public void IsLazy()
        {
            _ = new BreakingSequence<object>().SkipLastWhile(BreakingFunc.Of<object, bool>());
        }

        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        public void PredicateNeverFalse(SourceKind sourceKind)
        {
            using var sequence = TestingSequence.Of(0, 1, 2, 3, 4);

            Assert.That(sequence.ToSourceKind(sourceKind).SkipLastWhile(x => x < 5), Is.Empty);
        }

        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        public void PredicateNeverTrue(SourceKind sourceKind)
        {
            using var sequence = TestingSequence.Of(0, 1, 2, 3, 4);

            sequence.ToSourceKind(sourceKind)
                    .SkipLastWhile(x => x == 100)
                    .AssertSequenceEqual(0, 1, 2, 3, 4);
        }

        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        public void PredicateBecomesTruePartWay(SourceKind sourceKind)
        {
            using var sequence = TestingSequence.Of(0, 1, 2, 3, 4);

            sequence.ToSourceKind(sourceKind)
                    .SkipLastWhile(x => x > 2)
                    .AssertSequenceEqual(0, 1, 2);
        }

        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        public void NeverEvaluatesPredicateWhenSourceIsEmpty(SourceKind sourceKind)
        {
            using var sequence = TestingSequence.Of<int>();

            Assert.That(sequence
                .ToSourceKind(sourceKind)
                .SkipLastWhile(BreakingFunc.Of<int, bool>()), Is.Empty);
        }

        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        public void UsesCollectionCountAtIterationTime(SourceKind sourceKind)
        {
            var list = new List<int> { 1, 2, 3, 4 };
            var result = list.ToSourceKind(sourceKind).SkipLastWhile(x => x > 2);
            list.Add(5);
            result.AssertSequenceEqual(1, 2);
        }

        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        public void OptimizedForCollections(SourceKind sourceKind)
        {
            var sequence = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.ToSourceKind(sourceKind);

            sequence.SkipLastWhile(x => x > 7)
                    .AssertSequenceEqual(1, 2, 3, 4, 5, 6, 7);
        }

        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        public void KeepsNonTrailingItemsThatMatchPredicate(SourceKind sourceKind)
        {
            using var sequence = TestingSequence.Of(1, 2, 0, 0, 3, 4, 0, 0);

            sequence.ToSourceKind(sourceKind)
                    .SkipLastWhile(x => x == 0)
                    .AssertSequenceEqual(1, 2, 0, 0, 3, 4);
        }
    }
}
