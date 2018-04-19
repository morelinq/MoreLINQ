#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Atif Aziz. All rights reserved.
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
    public class CountDownTest
    {
        [Test]
        public void IsLazy()
        {
            new BreakingSequence<object>()
                .CountDown(42, BreakingFunc.Of<object, int?, object>());
        }

        [TestCase(-1, ExpectedResult = new[] { -1, -1, -1, -1, -1 })]
        [TestCase( 0, ExpectedResult = new[] { -1, -1, -1, -1, -1 })]
        [TestCase( 1, ExpectedResult = new[] { -1, -1, -1, -1,  0 })]
        [TestCase( 2, ExpectedResult = new[] { -1, -1, -1,  1,  0 })]
        [TestCase( 3, ExpectedResult = new[] { -1, -1,  2,  1,  0 })]
        [TestCase( 4, ExpectedResult = new[] { -1,  3,  2,  1,  0 })]
        [TestCase( 5, ExpectedResult = new[] {  4,  3,  2,  1,  0 })]
        [TestCase( 6, ExpectedResult = new[] {  4,  3,  2,  1,  0 })]
        [TestCase( 7, ExpectedResult = new[] {  4,  3,  2,  1,  0 })]
        public IEnumerable<int> CountDown(int count)
        {
            var xs = Enumerable.Range(1, 5);

            var result = xs.CountDown(count, (x, cd) => new
            {
                X = x,
                Countdown = cd ?? -1
            });

            result.Select(e => e.X).AssertSequenceEqual(xs);
            return result.Select(e => e.Countdown);
        }
    }
}
