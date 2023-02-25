#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Jonas Nyrup. All rights reserved.
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
    public class CompareCountTest
    {
        static IEnumerable<TestCaseData> CompareCountData =>
            from e in new[]
            {
                (Count1: 0, Count2: 0, Comparison:  0 ),
                (Count1: 0, Count2: 1, Comparison: -1 ),
                (Count1: 1, Count2: 0, Comparison:  1 ),
                (Count1: 1, Count2: 1, Comparison:  0 )
            }
            from firstKind in SourceKinds.Sequence.Concat(SourceKinds.Collection)
            from secondKind in SourceKinds.Sequence.Concat(SourceKinds.Collection)
            select new TestCaseData(
                    Enumerable.Range(1, e.Count1).ToSourceKind(firstKind),
                    Enumerable.Range(1, e.Count2).ToSourceKind(secondKind))
                .Returns(e.Comparison)
                .SetName($"{{m}}({firstKind}[{e.Count1}], {secondKind}[{e.Count2}]) = {e.Comparison}");


        [TestCaseSource(nameof(CompareCountData))]
        public int CompareCount(IEnumerable<int> xs, IEnumerable<int> ys) =>
            xs.CompareCount(ys);

        [TestCase(0, 0,  0, 1)]
        [TestCase(0, 1, -1, 1)]
        [TestCase(1, 0,  1, 1)]
        [TestCase(1, 1,  0, 2)]
        public void CompareCountWithCollectionAndSequence(int collectionCount,
            int sequenceCount,
            int expectedCompareCount,
            int expectedMoveNextCallCount)
        {
            var collection = new BreakingCollection<int>(new int[collectionCount]);

            using var seq = Enumerable.Range(0, sequenceCount).AsTestingSequence();

            Assert.That(collection.CompareCount(seq), Is.EqualTo(expectedCompareCount));
            Assert.That(seq.MoveNextCallCount, Is.EqualTo(expectedMoveNextCallCount));
        }

        [TestCase(0, 0,  0, 1)]
        [TestCase(0, 1, -1, 1)]
        [TestCase(1, 0,  1, 1)]
        [TestCase(1, 1,  0, 2)]
        public void CompareCountWithSequenceAndCollection(int sequenceCount,
            int collectionCount,
            int expectedCompareCount,
            int expectedMoveNextCallCount)
        {
            var collection = new BreakingCollection<int>(new int[collectionCount]);

            using var seq = Enumerable.Range(0, sequenceCount).AsTestingSequence();

            Assert.That(seq.CompareCount(collection), Is.EqualTo(expectedCompareCount));
            Assert.That(seq.MoveNextCallCount, Is.EqualTo(expectedMoveNextCallCount));
        }

        [TestCase(0, 0,  0, 1)]
        [TestCase(0, 1, -1, 1)]
        [TestCase(1, 0,  1, 1)]
        [TestCase(1, 1,  0, 2)]
        public void CompareCountWithSequenceAndSequence(int sequenceCount1,
            int sequenceCount2,
            int expectedCompareCount,
            int expectedMoveNextCallCount)
        {
            using var seq1 = Enumerable.Range(0, sequenceCount1).AsTestingSequence();
            using var seq2 = Enumerable.Range(0, sequenceCount2).AsTestingSequence();

            Assert.That(seq1.CompareCount(seq2), Is.EqualTo(expectedCompareCount));
            Assert.That(seq1.MoveNextCallCount, Is.EqualTo(expectedMoveNextCallCount));
            Assert.That(seq2.MoveNextCallCount, Is.EqualTo(expectedMoveNextCallCount));
        }

        [Test]
        public void CompareCountDisposesSequenceEnumerators()
        {
            using var seq1 = TestingSequence.Of<int>();
            using var seq2 = TestingSequence.Of<int>();

            Assert.That(seq1.CompareCount(seq2), Is.Zero);
        }

        [Test]
        public void CompareCountDisposesFirstEnumerator()
        {
            var collection = new BreakingCollection<int>();

            using var seq = TestingSequence.Of<int>();

            Assert.That(seq.CompareCount(collection), Is.Zero);
        }

        [Test]
        public void CompareCountDisposesSecondEnumerator()
        {
            var collection = new BreakingCollection<int>();

            using var seq = TestingSequence.Of<int>();

            Assert.That(collection.CompareCount(seq), Is.Zero);
        }

        [Test]
        public void CompareCountDoesNotIterateUnnecessaryElements()
        {
            var seq1 = MoreEnumerable.From(() => 1,
                                           () => 2,
                                           () => 3,
                                           () => 4,
                                           () => throw new TestException());

            var seq2 = Enumerable.Range(1, 3);

            Assert.That(seq1.CompareCount(seq2), Is.EqualTo( 1));
            Assert.That(seq2.CompareCount(seq1), Is.EqualTo(-1));
        }
    }
}
