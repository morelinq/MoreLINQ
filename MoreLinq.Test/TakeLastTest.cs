#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2009 Atif Aziz. All rights reserved.
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
    using System;
    using static MoreLinq.Extensions.TakeLastExtension;

    [TestFixture]
    public class TakeLastTest
    {
        [Test]
        public void TakeLast()
        {
            AssertTakeLast(new[] { 12, 34, 56, 78, 910, 1112 },
                           3,
                           result => result.AssertSequenceEqual(78, 910, 1112));
        }

        [Test]
        public void TakeLastOnSequenceShortOfCount()
        {
            AssertTakeLast(new[] { 12, 34, 56 },
                           5,
                           result => result.AssertSequenceEqual(12, 34, 56));
        }

        [Test]
        public void TakeLastWithNegativeCount()
        {
            AssertTakeLast(new[] { 12, 34, 56 },
                           -2,
                           result => Assert.That(result, Is.Empty));
        }

        [Test]
        public void TakeLastIsLazy()
        {
            _ = new BreakingSequence<object>().TakeLast(1);
        }

        [Test]
        public void TakeLastDisposesSequenceEnumerator()
        {
            using var seq = TestingSequence.Of(1, 2, 3);
            seq.TakeLast(1).Consume();
        }

        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        public void TakeLastOptimizedForCollections(SourceKind sourceKind)
        {
            var sequence = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.ToSourceKind(sourceKind);

            sequence.TakeLast(3).AssertSequenceEqual(8, 9, 10);
        }

        [Test]
        public void TakeLastUsesCollectionCountAtIterationTime()
        {
            var list = new List<int> { 1, 2, 3, 4 };
            var result = list.TakeLast(3);
            list.Add(5);
            result.AssertSequenceEqual(3, 4, 5);
        }

        static void AssertTakeLast<T>(ICollection<T> input, int count, Action<IEnumerable<T>> action)
        {
            // Test that the behaviour does not change whether a collection
            // or a sequence is used as the source.

            action(input.TakeLast(count));
            action(input.Select(x => x).TakeLast(count));
        }
    }
}
