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
    using NUnit.Framework;

    [TestFixture]
    public class CompareCountTest
    {
        [TestCase(0, 0, 0)]
        [TestCase(0, 1, -1)]
        [TestCase(1, 0, 1)]
        [TestCase(1, 1, 0)]
        public void CompareCountWithCollectionAndCollection(int collectionCount1, int collectionCount2, int expectedCompareCount)
        {
            var firstCollection = new BreakingCollection<int>(collectionCount1);
            var secondCollection = new BreakingCollection<int>(collectionCount2);

            Assert.AreEqual(expectedCompareCount, firstCollection.CompareCount(secondCollection));
        }

        [TestCase(0, 0, 0, 1)]
        [TestCase(0, 1, -1, 1)]
        [TestCase(1, 0, 1, 1)]
        [TestCase(1, 1, 0, 2)]
        public void CompareCountWithCollectionAndSequence(int collectionCount, int sequenceCount, int expectedCompareCount, int expectedMoveNextCount)
        {
            var collection = new BreakingCollection<int>(collectionCount);

            using (var seq = Enumerable.Range(0, sequenceCount).AsTestingSequence())
            {
                Assert.AreEqual(expectedCompareCount, collection.CompareCount(seq));
                Assert.AreEqual(expectedMoveNextCount, seq.MoveNextCounter);
            }
        }

        [TestCase(0, 0, 0, 1)]
        [TestCase(0, 1, -1, 1)]
        [TestCase(1, 0, 1, 1)]
        [TestCase(1, 1, 0, 2)]
        public void CompareCountWithSequenceAndCollection(int sequenceCount, int collectionCount, int expectedCompareCount, int expectedMoveNextCount)
        {
            var collection = new BreakingCollection<int>(collectionCount);

            using (var seq = Enumerable.Range(0, sequenceCount).AsTestingSequence())
            {
                Assert.AreEqual(expectedCompareCount, seq.CompareCount(collection));
                Assert.AreEqual(expectedMoveNextCount, seq.MoveNextCounter);
            }
        }

        [TestCase(0, 0, 0, 1)]
        [TestCase(0, 1, -1, 1)]
        [TestCase(1, 0, 1, 1)]
        [TestCase(1, 1, 0, 2)]
        public void CompareCountWithSequenceAndSequence(int sequenceCount1, int sequenceCount2, int expectedCompareCount, int expectedMoveNextCount)
        {
            using (var seq1 = Enumerable.Range(0, sequenceCount1).AsTestingSequence())
            using (var seq2 = Enumerable.Range(0, sequenceCount2).AsTestingSequence())
            {
                Assert.AreEqual(expectedCompareCount, seq1.CompareCount(seq2));
                Assert.AreEqual(expectedMoveNextCount, seq1.MoveNextCounter);
                Assert.AreEqual(expectedMoveNextCount, seq2.MoveNextCounter);
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
    }
}
