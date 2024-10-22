﻿#region License and Terms
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
    using static MoreLinq.Extensions.SkipLastWhileExtension;

    [TestFixture]
    public class SkipLastWhileTest
    {
        [Test]
        public void SkipLastWhilePredicateNeverFalse()
        {
            using var sequence = TestingSequence.Of(0, 1, 2, 3, 4);

            sequence.SkipLastWhile(x => x < 5)
                    .AssertSequenceEqual(Enumerable.Empty<int>());
        }

        [Test]
        public void SkipLastWhilePredicateNeverTrue()
        {
            using var sequence = TestingSequence.Of(0, 1, 2, 3, 4);

            sequence.SkipLastWhile(x => x == 100)
                    .AssertSequenceEqual(0, 1, 2, 3, 4);
        }

        [Test]
        public void SkipLastWhilePredicateBecomesTrueHalfWay()
        {
            using var sequence = TestingSequence.Of(0, 1, 2, 3, 4);

            sequence.SkipLastWhile(x => x > 2)
                    .AssertSequenceEqual(0, 1, 2);
        }

        [Test]
        public void SkipLastWhileReturnsEmptySequenceWhenSourceIsEmpty()
        {
            using var sequence = TestingSequence.Of<int>();

            sequence.SkipLastWhile(_ => true)
                    .AssertSequenceEqual(Enumerable.Empty<int>());
        }

        [Test]
        public void SkipLastWhileReturnsEntireSequenceWhenPredicateIsAlwaysFalse()
        {
            using var sequence = TestingSequence.Of(0, 0, 0, 0);

            sequence.SkipLastWhile(_ => false)
                    .AssertSequenceEqual(0, 0, 0, 0);
        }

        [Test]
        public void SkipLastWhileEvaluatesSourceLazily()
        {
            _ = new BreakingSequence<object>().SkipLastWhile(_ => true);
        }

        [Test]
        public void SkipLastWhileEvaluatesPredicateLazily()
        {
            using var sequence = TestingSequence.Of(0, 1, 2, 3, 4);

            sequence.SkipLastWhile(x => 1 / x != 1)
                    .AssertSequenceEqual(0, 1);
        }

        [Test]
        public void SkipLastWhileUsesCollectionCountAtIterationTime()
        {
            var list = new List<int> { 1, 2, 3, 4 };
            var result = list.SkipLastWhile(x => x > 2);
            list.Add(5);
            result.AssertSequenceEqual(1, 2);
        }
    }
}
