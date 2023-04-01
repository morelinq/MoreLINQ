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
    using Tuple = System.ValueTuple;

    [TestFixture]
    public class ZipShortestTest
    {
        [Test]
        public void BothSequencesDisposedWithUnequalLengthsAndLongerFirst()
        {
            using var longer = TestingSequence.Of(1, 2, 3);
            using var shorter = TestingSequence.Of(1, 2);

            longer.ZipShortest(shorter, (x, y) => x + y).Consume();
        }

        [Test]
        public void BothSequencesDisposedWithUnequalLengthsAndShorterFirst()
        {
            using var longer = TestingSequence.Of(1, 2, 3);
            using var shorter = TestingSequence.Of(1, 2);

            shorter.ZipShortest(longer, (x, y) => x + y).Consume();
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
        public void ZipShortestIsLazy()
        {
            var bs = new BreakingSequence<int>();
            _ = bs.ZipShortest(bs, BreakingFunc.Of<int, int, int>());
        }

        [Test]
        public void MoveNextIsNotCalledUnnecessarilyWhenFirstIsShorter()
        {
            using var s1 = TestingSequence.Of(1, 2);
            using var s2 = MoreEnumerable.From(() => 4,
                                               () => 5,
                                               () => throw new TestException())
                                         .AsTestingSequence();

            var zipped = s1.ZipShortest(s2, Tuple.Create);

            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual((1, 4), (2, 5));
        }

        [Test]
        public void ZipShortestNotIterateUnnecessaryElements()
        {
            using var s1 = MoreEnumerable.From(() => 4,
                                               () => 5,
                                               () => 6,
                                               () => throw new TestException())
                                         .AsTestingSequence();
            using var s2 = TestingSequence.Of(1, 2);

            var zipped = s1.ZipShortest(s2, Tuple.Create);

            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual((4, 1), (5, 2));
        }

        [Test]
        public void ZipShortestDisposesInnerSequencesCaseGetEnumeratorThrows()
        {
            using var s1 = TestingSequence.Of(1, 2);

            Assert.That(() => s1.ZipShortest(new BreakingSequence<int>(), Tuple.Create).Consume(),
                        Throws.BreakException);
        }
    }
}
