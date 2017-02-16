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

using NUnit.Framework;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class AtLeastTest
    {
        [Test]
        public void AtLeastWithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source", () =>
                MoreEnumerable.AtLeast<int>(null, 1));
        }

        [Test]
        public void AtLeastWithNegativeCount()
        {
            Assert.ThrowsArgumentOutOfRangeException("count", () =>
                new[] { 1 }.AtLeast(-1));
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
            Assert.IsTrue(new int[] { 1 }.AtLeast(0));
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastOneElementForCollections()
        {
            Assert.IsTrue(new int[] { 1 }.AtLeast(1));
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastManyElementsForCollections()
        {
            Assert.IsFalse(new int[] { 1 }.AtLeast(2));
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
