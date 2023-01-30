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
    using NUnit.Framework;

    [TestFixture]
    public class TakeUntilTest
    {
        [Test]
        public void TakeUntilPredicateNeverFalse()
        {
            var sequence = Enumerable.Range(0, 5).TakeUntil(x => x != 100);
            sequence.AssertSequenceEqual(0);
        }

        [Test]
        public void TakeUntilPredicateNeverTrue()
        {
            var sequence = Enumerable.Range(0, 5).TakeUntil(x => x == 100);
            sequence.AssertSequenceEqual(0, 1, 2, 3, 4);
        }

        [Test]
        public void TakeUntilPredicateBecomesTrueHalfWay()
        {
            var sequence = Enumerable.Range(0, 5).TakeUntil(x => x == 2);
            sequence.AssertSequenceEqual(0, 1, 2);
        }

        [Test]
        public void TakeUntilEvaluatesSourceLazily()
        {
            _ = new BreakingSequence<string>().TakeUntil(x => x.Length == 0);
        }

        [Test]
        public void TakeUntilEvaluatesPredicateLazily()
        {
            // Predicate would explode at x == 0, but we never need to evaluate it due to the Take call.
            var sequence = Enumerable.Range(-2, 5).TakeUntil(x => 1 / x == 1).Take(3);
            sequence.AssertSequenceEqual(-2, -1, 0);
        }
    }
}
