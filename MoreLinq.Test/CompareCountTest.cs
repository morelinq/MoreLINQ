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
        [Test]
        public void CompareCountWithEmptyCollectionHasCompareCountEmptyCollection()
        {
            var firstCollection = new TrackingTestCollection<int>(new int[] { });
            var secondCollection = new TrackingTestCollection<int>(new int[] { });

            Assert.AreEqual(0, firstCollection.CompareCount(secondCollection));

            Assert.AreEqual(0, firstCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, firstCollection.Enumerator.MoveNextCount);

            Assert.AreEqual(0, secondCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, secondCollection.Enumerator.MoveNextCount);
        }

        [Test]
        public void CompareCountWithEmptySequenceHasCompareCountEmptyCollection()
        {
            var firstSequence = new TrackingTestEnumerable<int>(new int[] { });
            var secondCollection = new TrackingTestCollection<int>(new int[] { });

            Assert.AreEqual(0, firstSequence.CompareCount(secondCollection));

            Assert.AreEqual(1, firstSequence.Enumerator.LoopCount);
            Assert.AreEqual(1, firstSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(firstSequence.Enumerator.Disposed);

            Assert.AreEqual(0, secondCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, secondCollection.Enumerator.MoveNextCount);
        }

        [Test]
        public void CompareCountWithEmptyCollectionHasCompareCountEmptySequence()
        {
            var firstCollection = new TrackingTestCollection<int>(new int[] { });
            var secondSequence = new TrackingTestEnumerable<int>(new int[] { });

            Assert.AreEqual(0, firstCollection.CompareCount(secondSequence));

            Assert.AreEqual(0, firstCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, firstCollection.Enumerator.MoveNextCount);

            Assert.AreEqual(1, secondSequence.Enumerator.LoopCount);
            Assert.AreEqual(1, secondSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(secondSequence.Enumerator.Disposed);
        }

        [Test]
        public void CompareCountWithEmptySequenceHasCompareCountEmptySequence()
        {
            var firstSequence = new TrackingTestEnumerable<int>(new int[] { });
            var secondSequence = new TrackingTestEnumerable<int>(new int[] { });

            Assert.AreEqual(0, firstSequence.CompareCount(secondSequence));

            Assert.AreEqual(1, firstSequence.Enumerator.LoopCount);
            Assert.AreEqual(1, firstSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(firstSequence.Enumerator.Disposed);

            Assert.AreEqual(1, secondSequence.Enumerator.LoopCount);
            Assert.AreEqual(1, secondSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(secondSequence.Enumerator.Disposed);
        }

        [Test]
        public void CompareCountWithSingleElementCollectionHasCompareCountSingleElementCollection()
        {
            var firstCollection = new TrackingTestCollection<int>(new int[] { 1 });
            var secondCollection = new TrackingTestCollection<int>(new int[] { 1 });

            Assert.AreEqual(0, firstCollection.CompareCount(secondCollection));

            Assert.AreEqual(0, firstCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, firstCollection.Enumerator.MoveNextCount);

            Assert.AreEqual(0, secondCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, secondCollection.Enumerator.MoveNextCount);
        }

        [Test]
        public void CompareCountWithSingleElementCollectionHasCompareCountSingleElementSequence()
        {
            var firstCollection = new TrackingTestCollection<int>(new int[] { 1 });
            var secondSequence = new TrackingTestEnumerable<int>(new int[] { 1 });

            Assert.AreEqual(0, firstCollection.CompareCount(secondSequence));

            Assert.AreEqual(0, firstCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, firstCollection.Enumerator.MoveNextCount);

            Assert.AreEqual(1, secondSequence.Enumerator.LoopCount);
            Assert.AreEqual(2, secondSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(secondSequence.Enumerator.Disposed);
        }

        [Test]
        public void CompareCountWithSingleElementSequenceHasCompareCountSingleElementCollection()
        {
            var firstSequence = new TrackingTestEnumerable<int>(new int[] { 1 });
            var secondCollection = new TrackingTestCollection<int>(new int[] { 1 });

            Assert.AreEqual(0, firstSequence.CompareCount(secondCollection));

            Assert.AreEqual(1, firstSequence.Enumerator.LoopCount);
            Assert.AreEqual(2, firstSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(firstSequence.Enumerator.Disposed);

            Assert.AreEqual(0, secondCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, secondCollection.Enumerator.MoveNextCount);
        }

        [Test]
        public void CompareCountWithSingleElementSequenceHasCompareCountSingleElementSequence()
        {
            var firstSequence = new TrackingTestEnumerable<int>(new int[] { 1 });
            var secondSequence = new TrackingTestEnumerable<int>(new int[] { 1 });

            Assert.AreEqual(0, firstSequence.CompareCount(secondSequence));

            Assert.AreEqual(1, firstSequence.Enumerator.LoopCount);
            Assert.AreEqual(2, firstSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(firstSequence.Enumerator.Disposed);

            Assert.AreEqual(1, secondSequence.Enumerator.LoopCount);
            Assert.AreEqual(2, secondSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(secondSequence.Enumerator.Disposed);
        }

        [Test]
        public void CompareCountWithEmptyCollectionHasCompareCountCollectionWithOneElement()
        {
            var firstCollection = new TrackingTestCollection<int>(new int[] { });
            var secondCollection = new TrackingTestCollection<int>(new int[] { 1 });

            Assert.AreEqual(-1, firstCollection.CompareCount(secondCollection));

            Assert.AreEqual(0, firstCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, firstCollection.Enumerator.MoveNextCount);

            Assert.AreEqual(0, secondCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, secondCollection.Enumerator.MoveNextCount);
        }

        [Test]
        public void CompareCountWithEmptyCollectionHasCompareCountSequenceWithOneElement()
        {
            var firstCollection = new TrackingTestCollection<int>(new int[] { });
            var secondSequence = new TrackingTestEnumerable<int>(new int[] { 1 });

            Assert.AreEqual(-1, firstCollection.CompareCount(secondSequence));

            Assert.AreEqual(0, firstCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, firstCollection.Enumerator.MoveNextCount);

            Assert.AreEqual(1, secondSequence.Enumerator.LoopCount);
            Assert.AreEqual(1, secondSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(secondSequence.Enumerator.Disposed);
        }

        [Test]
        public void CompareCountWithEmptySequenceHasCompareCountCollectionWithOneElement()
        {
            var firstSequence = new TrackingTestEnumerable<int>(new int[] { });
            var secondCollection = new TrackingTestCollection<int>(new int[] { 1 });

            Assert.AreEqual(-1, firstSequence.CompareCount(secondCollection));

            Assert.AreEqual(1, firstSequence.Enumerator.LoopCount);
            Assert.AreEqual(1, firstSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(firstSequence.Enumerator.Disposed);

            Assert.AreEqual(0, secondCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, secondCollection.Enumerator.MoveNextCount);
        }

        [Test]
        public void CompareCountWithEmptySequenceHasCompareCountSequenceWithOneElement()
        {
            var firstSequence = new TrackingTestEnumerable<int>(new int[] { });
            var secondSequence = new TrackingTestEnumerable<int>(new int[] { 1 });

            Assert.AreEqual(-1, firstSequence.CompareCount(secondSequence));

            Assert.AreEqual(1, firstSequence.Enumerator.LoopCount);
            Assert.AreEqual(1, firstSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(firstSequence.Enumerator.Disposed);

            Assert.AreEqual(1, secondSequence.Enumerator.LoopCount);
            Assert.AreEqual(1, secondSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(secondSequence.Enumerator.Disposed);
        }

        [Test]
        public void CompareCountWithSingleElementCollectionHasCompareCountEmptyCollection()
        {
            var firstCollection = new TrackingTestCollection<int>(new int[] { 1 });
            var secondCollection = new TrackingTestCollection<int>(new int[] { });

            Assert.AreEqual(1, firstCollection.CompareCount(secondCollection));

            Assert.AreEqual(0, firstCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, firstCollection.Enumerator.MoveNextCount);

            Assert.AreEqual(0, secondCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, secondCollection.Enumerator.MoveNextCount);
        }

        [Test]
        public void CompareCountWithSingleElementCollectionHasCompareCountEmptySequence()
        {
            var firstCollection = new TrackingTestCollection<int>(new int[] { 1 });
            var secondSequence = new TrackingTestEnumerable<int>(new int[] { });

            Assert.AreEqual(1, firstCollection.CompareCount(secondSequence));

            Assert.AreEqual(0, firstCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, firstCollection.Enumerator.MoveNextCount);

            Assert.AreEqual(1, secondSequence.Enumerator.LoopCount);
            Assert.AreEqual(1, secondSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(secondSequence.Enumerator.Disposed);
        }

        [Test]
        public void CompareCountWithSingleElementSequenceHasCompareCountEmptyCollection()
        {
            var firstSequence = new TrackingTestEnumerable<int>(new int[] { 1 });
            var secondCollection = new TrackingTestCollection<int>(new int[] { });

            Assert.AreEqual(1, firstSequence.CompareCount(secondCollection));

            Assert.AreEqual(1, firstSequence.Enumerator.LoopCount);
            Assert.AreEqual(1, firstSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(firstSequence.Enumerator.Disposed);

            Assert.AreEqual(0, secondCollection.Enumerator.LoopCount);
            Assert.AreEqual(0, secondCollection.Enumerator.MoveNextCount);
        }

        [Test]
        public void CompareCountWithSingleElementSequenceHasCompareCountEmptySequence()
        {
            var firstSequence = new TrackingTestEnumerable<int>(new int[] { 1 });
            var secondSequence = new TrackingTestEnumerable<int>(new int[] { });

            Assert.AreEqual(1, firstSequence.CompareCount(secondSequence));

            Assert.AreEqual(1, firstSequence.Enumerator.LoopCount);
            Assert.AreEqual(1, firstSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(firstSequence.Enumerator.Disposed);

            Assert.AreEqual(1, secondSequence.Enumerator.LoopCount);
            Assert.AreEqual(1, secondSequence.Enumerator.MoveNextCount);
            Assert.IsTrue(secondSequence.Enumerator.Disposed);
        }
    }
}
