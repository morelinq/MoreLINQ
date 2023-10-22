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
    using Tuple = System.ValueTuple;

    [TestFixture]
    public class EquiZipTest
    {
        [Test]
        public void BothSequencesDisposedWithUnequalLengthsAndLongerFirst()
        {
            using var longer = TestingSequence.Of(1, 2, 3);
            using var shorter = TestingSequence.Of(1, 2);

            // Yes, this will throw... but then we should still have disposed both sequences
            Assert.That(() => longer.EquiZip(shorter, (x, y) => x + y).Consume(),
                        Throws.InvalidOperationException);
        }

        [Test]
        public void BothSequencesDisposedWithUnequalLengthsAndShorterFirst()
        {
            using var longer = TestingSequence.Of(1, 2, 3);
            using var shorter = TestingSequence.Of(1, 2);

            // Yes, this will throw... but then we should still have disposed both sequences
            Assert.That(() => shorter.EquiZip(longer, (x, y) => x + y).Consume(),
                        Throws.InvalidOperationException);
        }

        [Test]
        public void ZipWithEqualLengthSequencesFailStrategy()
        {
            var zipped = new[] { 1, 2, 3 }.EquiZip(new[] { 4, 5, 6 }, Tuple.Create);
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual((1, 4), (2, 5), (3, 6));
        }

        [Test]
        public void ZipWithFirstSequenceShorterThanSecondFailStrategy()
        {
            var zipped = new[] { 1, 2 }.EquiZip(new[] { 4, 5, 6 }, Tuple.Create);
            Assert.That(zipped, Is.Not.Null);
            Assert.That(zipped.Consume, Throws.InvalidOperationException);
        }

        [Test]
        public void ZipWithFirstSequnceLongerThanSecondFailStrategy()
        {
            var zipped = new[] { 1, 2, 3 }.EquiZip(new[] { 4, 5 }, Tuple.Create);
            Assert.That(zipped, Is.Not.Null);
            Assert.That(zipped.Consume, Throws.InvalidOperationException);
        }

        [Test]
        public void ZipIsLazy()
        {
            var bs = new BreakingSequence<int>();
            _ = bs.EquiZip(bs, BreakingFunc.Of<int, int, int>());
        }

        [Test]
        public void MoveNextIsNotCalledUnnecessarily()
        {
            using var s1 = TestingSequence.Of(1, 2);
            using var s2 = TestingSequence.Of(1, 2, 3);
            using var s3 = MoreEnumerable.From(() => 1,
                                               () => 2,
                                               () => throw new TestException())
                                         .AsTestingSequence();

            Assert.That(() => s1.EquiZip(s2, s3, (x, y, z) => x + y + z).Consume(),
                        Throws.InvalidOperationException);
        }

        [Test]
        public void ZipDisposesInnerSequencesCaseGetEnumeratorThrows()
        {
            using var s1 = TestingSequence.Of(1, 2);

            Assert.That(() => s1.EquiZip(new BreakingSequence<int>(), Tuple.Create).Consume(),
                        Throws.BreakException);
        }
    }
}
