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
    using LinqEnumerable = System.Linq.Enumerable;

    [TestFixture]
    public class AtLeastTest
    {
        [Test]
        public void AtLeastWithNegativeCount()
        {
            Assert.ThrowsArgumentOutOfRangeException("count", () =>
                new[] { 1 }.AtLeast(-1));
        }

        [Test]
        public void AtLeastSequenceWithFirstNullSequence()
        {
            Assert.ThrowsArgumentNullException("first", () =>
                MoreEnumerable.AtLeast<int, int>(null, LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void AtLeastSequenceWithSecondNullSequence()
        {
            Assert.ThrowsArgumentNullException("second", () =>
                MoreEnumerable.AtLeast<int, int>(LinqEnumerable.Empty<int>(), null));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastZeroElements()
        {
            Assert.IsTrue(LinqEnumerable.Empty<int>().AtLeast(0));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastOneElement()
        {
            Assert.IsFalse(LinqEnumerable.Empty<int>().AtLeast(1));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastManyElements()
        {
            Assert.IsFalse(LinqEnumerable.Empty<int>().AtLeast(2));
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastZeroElements()
        {
            Assert.IsTrue(new[] { 1 }.AtLeast(0));
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastOneElement()
        {
            Assert.IsTrue(new[] { 1 }.AtLeast(1));
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastManyElements()
        {
            Assert.IsFalse(new[] { 1 }.AtLeast(2));
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastZeroElements()
        {
            Assert.IsTrue(new[] { 1, 2, 3 }.AtLeast(0));
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastOneElement()
        {
            Assert.IsTrue(new[] { 1, 2, 3 }.AtLeast(1));
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastManyElements()
        {
            Assert.IsTrue(new[] { 1, 2, 3 }.AtLeast(2));
        }

        [Test]
        public void AtLeastWithEmptyCollectionHasAtLeastEmptyCollection()
        {
            Assert.IsTrue(LinqEnumerable.Empty<int>().AtLeast(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastEmptyCollection()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 0).AtLeast(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void AtLeastWithEmptyCollectionHasAtLeastEmptySequence()
        {
            Assert.IsTrue(LinqEnumerable.Empty<int>().AtLeast(LinqEnumerable.Range(1, 0)));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastEmptySequence()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 0).AtLeast(LinqEnumerable.Range(1, 0)));
        }

        [Test]
        public void AtLeastWithEmptyCollectionHasAtLeastCollectionWithOneElement()
        {
            Assert.IsFalse(LinqEnumerable.Empty<int>().AtLeast(new[] { 1 }));
        }

        [Test]
        public void AtLeastWithEmptyCollectionHasAtLeastSequenceWithOneElement()
        {
            Assert.IsFalse(LinqEnumerable.Empty<int>().AtLeast(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastCollectionWithOneElement()
        {
            Assert.IsFalse(LinqEnumerable.Range(1, 0).AtLeast(new[] { 1 }));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastSequenceWithOneElement()
        {
            Assert.IsFalse(LinqEnumerable.Range(1, 0).AtLeast(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void AtLeastWithEmptyCollectionHasAtLeastCollectionWithManyElements()
        {
            Assert.IsFalse(LinqEnumerable.Empty<int>().AtLeast(new[] { 1, 2 }));
        }

        [Test]
        public void AtLeastWithEmptyCollectionHasAtLeastSequenceWithManyElements()
        {
            Assert.IsFalse(LinqEnumerable.Empty<int>().AtLeast(LinqEnumerable.Range(1, 2)));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastCollectionWithManyElements()
        {
            Assert.IsFalse(LinqEnumerable.Range(1, 0).AtLeast(new[] { 1, 2 }));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastSequenceWithManyElements()
        {
            Assert.IsFalse(LinqEnumerable.Range(1, 0).AtLeast(LinqEnumerable.Range(1, 2)));
        }

        [Test]
        public void AtLeastWithSingleElementCollectionHasAtLeastEmptyCollection()
        {
            Assert.IsTrue(new[] { 1 }.AtLeast(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void AtLeastWithSingleElementCollectionHasAtLeastEmptySequence()
        {
            Assert.IsTrue(new[] { 1 }.AtLeast(LinqEnumerable.Range(1, 0)));
        }

        [Test]
        public void AtLeastWithSingleElementSequenceHasAtLeastEmptyCollection()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 1).AtLeast(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void AtLeastWithSingleElementSequenceHasAtLeastEmptySequence()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 1).AtLeast(LinqEnumerable.Range(1, 0)));
        }

        [Test]
        public void AtLeastWithSingleElementCollectionHasAtLeastSingleElementCollection()
        {
            Assert.IsTrue(new[] { 1 }.AtLeast(new[] { 1 }));
        }

        [Test]
        public void AtLeastWithSingleElementCollectionHasAtLeastSingleElementSequence()
        {
            Assert.IsTrue(new[] { 1 }.AtLeast(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void AtLeastWithSingleElementSequenceHasAtLeastSingleElementCollection()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 1).AtLeast(new[] { 1 }));
        }

        [Test]
        public void AtLeastWithSingleElementSequenceHasAtLeastSingleElementSequence()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 1).AtLeast(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void AtLeastWithSingleElementCollectionHasAtLeastManyElementCollection()
        {
            Assert.IsFalse(new[] { 1 }.AtLeast(new[] { 1, 2 }));
        }

        [Test]
        public void AtLeastWithSingleElementCollectionHasAtLeastManyElementSequence()
        {
            Assert.IsFalse(new[] { 1 }.AtLeast(LinqEnumerable.Range(1, 2)));
        }

        [Test]
        public void AtLeastWithSingleElementSequenceHasAtLeastManyElementCollection()
        {
            Assert.IsFalse(LinqEnumerable.Range(1, 1).AtLeast(new[] { 1, 2 }));
        }

        [Test]
        public void AtLeastWithSingleElementSequenceHasAtLeastManyElementSequence()
        {
            Assert.IsFalse(LinqEnumerable.Range(1, 1).AtLeast(LinqEnumerable.Range(1, 2)));
        }

        [Test]
        public void AtLeastWithManyElementCollectionHasAtLeastEmptyCollection()
        {
            Assert.IsTrue(new[] { 1, 2, 3 }.AtLeast(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void AtLeastWithManyElementCollectionHasAtLeastEmptySequence()
        {
            Assert.IsTrue(new[] { 1, 2, 3 }.AtLeast(LinqEnumerable.Range(1, 0)));
        }

        [Test]
        public void AtLeastWithManyElementSequenceHasAtLeastEmptyCollection()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 3).AtLeast(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void AtLeastWithManyElementSequenceHasAtLeastEmptySequence()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 3).AtLeast(LinqEnumerable.Range(1, 0)));
        }

        [Test]
        public void AtLeastWithManyElementCollectionHasAtLeastOneElementCollection()
        {
            Assert.IsTrue(new[] { 1, 2, 3 }.AtLeast(new[] { 1 }));
        }

        [Test]
        public void AtLeastWithManyElementCollectionHasAtLeastOneElementSequence()
        {
            Assert.IsTrue(new[] { 1, 2, 3 }.AtLeast(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void AtLeastWithManyElementSequenceHasAtLeastOneElementCollection()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 3).AtLeast(new[] { 1 }));
        }

        [Test]
        public void AtLeastWithManyElementSequenceHasAtLeastOneElementSequence()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 3).AtLeast(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void AtLeastWithManyElementCollectionHasAtLeastManyElementCollection()
        {
            Assert.IsTrue(new[] { 1, 2, 3 }.AtLeast(new[] { 1, 2 }));
        }

        [Test]
        public void AtLeastWithManyElementCollectionHasAtLeastManyElementSequence()
        {
            Assert.IsTrue(new[] { 1, 2, 3 }.AtLeast(LinqEnumerable.Range(1, 2)));
        }

        [Test]
        public void AtLeastWithManyElementSequenceHasAtLeastManyElementCollection()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 3).AtLeast(new[] { 1, 2 }));
        }

        [Test]
        public void AtLeastWithManyElementSequenceHasAtLeastManyElementSequence()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 3).AtLeast(LinqEnumerable.Range(1, 2)));
        }

        //ICollection<T> Optimization Tests
        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastZeroElementsForCollections()
        {
            Assert.IsTrue(new int[] { }.AtLeast(0));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastOneElementForCollections()
        {
            Assert.IsFalse(new int[] { }.AtLeast(1));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastManyElementsForCollections()
        {
            Assert.IsFalse(new int[] { }.AtLeast(2));
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastZeroElementsForCollections()
        {
            Assert.IsTrue(new[] { 1 }.AtLeast(0));
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastOneElementForCollections()
        {
            Assert.IsTrue(new[] { 1 }.AtLeast(1));
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastManyElementsForCollections()
        {
            Assert.IsFalse(new[] { 1 }.AtLeast(2));
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastZeroElementsForCollections()
        {
            Assert.IsTrue(new[] { 1, 2, 3 }.AtLeast(0));
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastOneElementForCollections()
        {
            Assert.IsTrue(new[] { 1, 2, 3 }.AtLeast(1));
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastManyElementsForCollections()
        {
            Assert.IsTrue(new[] { 1, 2, 3 }.AtLeast(2));
        }

        [Test]
        public void AtLeastShouldBeNotEnumerateSequenceForImplementersOfICollection()
        {
            var sequence = new UnenumerableList<int>();
            sequence.AtLeast(3);
        }
    }
}
