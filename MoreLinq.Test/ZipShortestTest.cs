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
using NUnit.Framework;
using Tuple = System.ValueTuple;

namespace MoreLinq.Test
{
    [TestFixture]
    public class ZipShortestTest
    {
        [Test]
        public void BothSequencesDisposedWithUnequalLengthsAndLongerFirst()
        {
            using (var longer = TestingSequence.Of(1, 2, 3))
            using (var shorter = TestingSequence.Of(1, 2))
            {
                longer.ZipShortest(shorter, (x, y) => x + y).Consume();
            }
        }

        [Test]
        public void BothSequencesDisposedWithUnequalLengthsAndShorterFirst()
        {
            using (var longer = TestingSequence.Of(1, 2, 3))
            using (var shorter = TestingSequence.Of(1, 2))
            {
                shorter.ZipShortest(longer, (x, y) => x + y).Consume();
            }
        }

        [Test]
        public void ZipShortestWithEqualLengthSequences()
        {
            var zipped = new[] { 1, 2, 3 }.ZipShortest(new[] { 4, 5, 6 }, Tuple.Create);
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual((1, 4), (2, 5), (3, 6));
        }

        [Test]
        public void ZipShortestWithFirstSequenceShorterThanSecond()
        {
            var zipped = new[] { 1, 2 }.ZipShortest(new[] { 4, 5, 6 }, Tuple.Create);
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual((1, 4), (2, 5));
        }

        [Test]
        public void ZipShortestWithFirstSequnceLongerThanSecond()
        {
            var zipped = new[] { 1, 2, 3 }.ZipShortest(new[] { 4, 5 }, Tuple.Create);
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual((1, 4), (2, 5));
        }

        [Test]
        public void ZipShortestWithNullFirstSequence()
        {
            Assert.ThrowsArgumentNullException("first", () =>
                MoreEnumerable.ZipShortest(null, new[] { 4, 5, 6 }, BreakingFunc.Of<int, int, int>()));
        }

        [Test]
        public void ZipShortestWithNullSecondSequence()
        {
            Assert.ThrowsArgumentNullException("second", () =>
                new[] { 1, 2, 3 }.ZipShortest(null, BreakingFunc.Of<int, int, int>()));
        }

        [Test]
        public void ZipShortestWithNullResultSelector()
        {
            Assert.ThrowsArgumentNullException("resultSelector",() =>
                new[] { 1, 2, 3 }.ZipShortest<int, int, int>(new[] { 4, 5, 6 }, null));
        }

        [Test]
        public void ZipShortestIsLazy()
        {
            var bs = new BreakingSequence<int>();
            bs.ZipShortest<int, int, int>(bs, delegate { throw new NotImplementedException(); });
        }
    }
}
