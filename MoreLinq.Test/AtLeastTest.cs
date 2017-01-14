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
using System;
using System.Collections.Generic;
using System.Linq;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class AtLeastTest
    {
        [Test]
        public void AtLeastWithNullSequence()
        {
            IEnumerable<int> sequence = null;
            Assert.ThrowsArgumentNullException("source", () =>
                sequence.AtLeast(1));
        }

        [Test]
        public void AtLeastWithNegativeCount()
        {
            Assert.ThrowsArgumentOutOfRangeException("count", () =>
                new[] { 1 }.AtLeast(-1));
        }

        private static IEnumerable<int> GetSequence()
        {
            return new InfiniteSequence<int>(0);
        }
        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastZeroElements()
        {
            Assert.IsTrue(GetEmptySequence().AtLeast(0));
        }
        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastOneElement()
        {
            Assert.IsFalse(GetEmptySequence().AtLeast(1));
        }
        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastManyElements()
        {
            Assert.IsFalse(GetEmptySequence().AtLeast(2));
        }
        private static IEnumerable<int> GetEmptySequence()
        {
            return LinqEnumerable.Empty<int>();
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastZeroElements()
        {
            Assert.IsTrue(GetSingleElementSequence().AtLeast(0));
        }
        [Test]
        public void AtLeastWithSingleElementHasAtLeastOneElement()
        {
            Assert.IsTrue(GetSingleElementSequence().AtLeast(1));
        }
        [Test]
        public void AtLeastWithSingleElementHasAtLeastManyElements()
        {
            Assert.IsFalse(GetSingleElementSequence().AtLeast(2));
        }
        private static IEnumerable<int> GetSingleElementSequence()
        {
            return GetSequence().Take(1);
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastZeroElements()
        {
            Assert.IsTrue(GetManyElementSequence().AtLeast(0));
        }
        [Test]
        public void AtLeastWithManyElementsHasAtLeastOneElement()
        {
            Assert.IsTrue(GetManyElementSequence().AtLeast(1));
        }
        [Test]
        public void AtLeastWithManyElementsHasAtLeastManyElements()
        {
            Assert.IsTrue(GetManyElementSequence().AtLeast(2));
        }
        private static IEnumerable<int> GetManyElementSequence()
        {
            return GetSequence().Take(3);
        }

        //ICollection<T> Optimization Tests
        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastZeroElementsForCollections()
        {
            Assert.IsTrue(GetEmptyArray().AtLeast(0));
        }
        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastOneElementForCollections()
        {
            Assert.IsFalse(GetEmptyArray().AtLeast(1));
        }
        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastManyElementsForCollections()
        {
            Assert.IsFalse(GetEmptyArray().AtLeast(2));
        }
        private static IEnumerable<int> GetEmptyArray()
        {
            return new int[] { };
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastZeroElementsForCollections()
        {
            Assert.IsTrue(GetSingleElementArray().AtLeast(0));
        }
        [Test]
        public void AtLeastWithSingleElementHasAtLeastOneElementForCollections()
        {
            Assert.IsTrue(GetSingleElementArray().AtLeast(1));
        }
        [Test]
        public void AtLeastWithSingleElementHasAtLeastManyElementsForCollections()
        {
            Assert.IsFalse(GetSingleElementArray().AtLeast(2));
        }
        private static IEnumerable<int> GetSingleElementArray()
        {
            return GetSingleElementSequence().ToArray();
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastZeroElementsForCollections()
        {
            Assert.IsTrue(GetManyElementArray().AtLeast(0));
        }
        [Test]
        public void AtLeastWithManyElementsHasAtLeastOneElementForCollections()
        {
            Assert.IsTrue(GetManyElementArray().AtLeast(1));
        }
        [Test]
        public void AtLeastWithManyElementsHasAtLeastManyElementsForCollections()
        {
            Assert.IsTrue(GetManyElementArray().AtLeast(2));
        }
        private static IEnumerable<int> GetManyElementArray()
        {
            return GetManyElementSequence().ToArray();
        }

        [Test]
        public void AtLeastShouldBeNotEnumerateSequenceForImplementersOfICollection()
        {
            var sequence = new UnenumerableList<int>();
            sequence.AtLeast(3);
        }
    }
}
