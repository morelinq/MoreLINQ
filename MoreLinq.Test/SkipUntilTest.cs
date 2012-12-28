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

using System;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class SkipUntilTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SkipUntilNullSource()
        {
            MoreEnumerable.SkipUntil<string>(null, x => x.Length == 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SkipUntilNullPredicate()
        {
            MoreEnumerable.SkipUntil<string>(null, x => x.Length == 1);
        }

        [Test]
        public void SkipUntilPredicateNeverFalse()
        {
            var sequence = Enumerable.Range(0, 5).SkipUntil(x => x != 100);
            sequence.AssertSequenceEqual(1, 2, 3, 4);
        }

        [Test]
        public void SkipUntilPredicateNeverTrue()
        {
            var sequence = Enumerable.Range(0, 5).SkipUntil(x => x == 100);
            sequence.AssertSequenceEqual();
        }

        [Test]
        public void SkipUntilPredicateBecomesTrueHalfWay()
        {
            var sequence = Enumerable.Range(0, 5).SkipUntil(x => x == 2);
            sequence.AssertSequenceEqual(3, 4);
        }

        [Test]
        public void SkipUntilEvaluatesSourceLazily()
        {
            MoreEnumerable.SkipUntil(new BreakingSequence<string>(), x => x.Length == 0);
        }

        [Test]
        public void SkipUntilEvaluatesPredicateLazily()
        {
            // Predicate would explode at x == 0, but we never need to evaluate it as we've
            // started returning items after -1.
            var sequence = Enumerable.Range(-2, 5).SkipUntil(x => 1 / x == -1);
            sequence.AssertSequenceEqual(0, 1, 2);
        }
    }
}
