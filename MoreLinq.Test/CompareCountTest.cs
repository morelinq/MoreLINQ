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
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class CompareCountTest
    {
        [TestCase(0, 0,  0)]
        [TestCase(0, 1, -1)]
        [TestCase(1, 0,  1)]
        [TestCase(1, 1,  0)]
        public void CompareCount(int s1Count,
            int s2Count,
            int expectedCompareCount)
        {
            var s1 = Enumerable.Range(1, s1Count);
            var s2 = Enumerable.Range(1, s2Count);

            foreach (var (xs, ys) in ArrangeCollectionTestCases(s1, s2))
                Assert.AreEqual(expectedCompareCount, xs.CompareCount(ys));
        }

        [TestCase(0, 0,  0, 1)]
        [TestCase(0, 1, -1, 1)]
        [TestCase(1, 0,  1, 1)]
        [TestCase(1, 1,  0, 2)]
        public void CompareCountWithCollectionAndSequence(int collectionCount,
            int sequenceCount,
            int expectedCompareCount,
            int expectedMoveNextCallCount)
        {
            var collection = new BreakingCollection<int>(collectionCount);

            using (var seq = Enumerable.Range(0, sequenceCount).AsTestingSequence())
            {
                Assert.AreEqual(expectedCompareCount, collection.CompareCount(seq));
                Assert.AreEqual(expectedMoveNextCallCount, seq.MoveNextCallCount);
            }
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
            var collection = new BreakingCollection<int>(collectionCount);

            using (var seq = Enumerable.Range(0, sequenceCount).AsTestingSequence())
            {
                Assert.AreEqual(expectedCompareCount, seq.CompareCount(collection));
                Assert.AreEqual(expectedMoveNextCallCount, seq.MoveNextCallCount);
            }
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
            using (var seq1 = Enumerable.Range(0, sequenceCount1).AsTestingSequence())
            using (var seq2 = Enumerable.Range(0, sequenceCount2).AsTestingSequence())
            {
                Assert.AreEqual(expectedCompareCount, seq1.CompareCount(seq2));
                Assert.AreEqual(expectedMoveNextCallCount, seq1.MoveNextCallCount);
                Assert.AreEqual(expectedMoveNextCallCount, seq2.MoveNextCallCount);
            }
        }

        [Test]
        public void CompareCountDisposesSequenceEnumerators()
        {
            using (var seq1 = TestingSequence.Of<int>())
            using (var seq2 = TestingSequence.Of<int>())
            {
                Assert.AreEqual(0, seq1.CompareCount(seq2));
            }
        }

        [Test]
        public void CompareCountDisposesFirstEnumerator()
        {
            var collection = new BreakingCollection<int>(0);

            using (var seq = TestingSequence.Of<int>())
            {
                Assert.AreEqual(0, seq.CompareCount(collection));
            }
        }

        [Test]
        public void CompareCountDisposesSecondEnumerator()
        {
            var collection = new BreakingCollection<int>(0);

            using (var seq = TestingSequence.Of<int>())
            {
                Assert.AreEqual(0, collection.CompareCount(seq));
            }
        }

        [Test]
        public void CompareCountNotIterateUnnecessaryElements()
        {
            var seq1 = MoreEnumerable.From(() => 1,
                                           () => 2,
                                           () => 3,
                                           () => 4,
                                           () => throw new InvalidOperationException());

            var seq2 = Enumerable.Range(1, 3);

            Assert.AreEqual( 1, seq1.CompareCount(seq2));
            Assert.AreEqual(-1, seq2.CompareCount(seq1));
        }

        static IEnumerable<(IEnumerable<T>, IEnumerable<T>)> ArrangeCollectionTestCases<T>(IEnumerable<T> s1, IEnumerable<T> s2)
        {
            // Test that the operator is optimized for collections

            var s1Seq = s1.Select(x => x);
            var s2Seq = s2.Select(x => x);

            var s1Col = s1.ToBreakingCollection(false);
            var s2Col = s2.ToBreakingCollection(false);

            var s1ReadOnlyCol = s1.ToBreakingCollection(true);
            var s2ReadOnlyCol = s2.ToBreakingCollection(true);

            // sequences
            yield return (s1Seq, s2Seq);

            // sequences and collections
            yield return (s1Seq, s2Col);
            yield return (s1Col, s2Seq);
            yield return (s1Col, s2Col);

            // sequences and readOnlyCollections
            yield return (s1Seq, s2ReadOnlyCol);
            yield return (s1ReadOnlyCol, s2Seq);
            yield return (s1ReadOnlyCol, s2ReadOnlyCol);
        }
    }
}
