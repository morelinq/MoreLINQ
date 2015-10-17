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
        [ExpectedException(typeof(ArgumentNullException))]
        public void AtLeastWithNullSequence()
        {
            IEnumerable<int> sequence = null;
            sequence.AtLeast(1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AtLeastWithNegativeCount()
        {
            new[] { 1 }.AtLeast(-1);
        }

        private static IEnumerable<int> getSequence()
        {
            int i = 1;
            while(true)
            {
                yield return i++;
            }
        }
        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastZeroElements()
        {
            Assert.That(getEmptySequence().AtLeast(0));
        }
        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastOneElement()
        {
            Assert.That(!getEmptySequence().AtLeast(1));
        }
        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastManyElements()
        {
            Assert.That(!getEmptySequence().AtLeast(2));
        }
        private static IEnumerable<int> getEmptySequence()
        {
            return LinqEnumerable.Empty<int>();
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastZeroElements()
        {
            Assert.That(getSingleElementSequence().AtLeast(0));
        }
        [Test]
        public void AtLeastWithSingleElementHasAtLeastOneElement()
        {
            Assert.That(getSingleElementSequence().AtLeast(1));
        }
        [Test]
        public void AtLeastWithSingleElementHasAtLeastManyElements()
        {
            Assert.That(!getSingleElementSequence().AtLeast(2));
        }
        private static IEnumerable<int> getSingleElementSequence()
        {
            return getSequence().Take(1);
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastZeroElements()
        {
            Assert.That(getManyElementSequence().AtLeast(0));
        }
        [Test]
        public void AtLeastWithManyElementsHasAtLeastOneElement()
        {
            Assert.That(getManyElementSequence().AtLeast(1));
        }
        [Test]
        public void AtLeastWithManyElementsHasAtLeastManyElements()
        {
            Assert.That(getManyElementSequence().AtLeast(2));
        }
        private static IEnumerable<int> getManyElementSequence()
        {
            return getSequence().Take(3);
        }

        //ICollection<T> Optimization Tests
        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastZeroElementsForCollections()
        {
            Assert.That(getEmptyArray().AtLeast(0));
        }
        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastOneElementForCollections()
        {
            Assert.That(!getEmptyArray().AtLeast(1));
        }
        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastManyElementsForCollections()
        {
            Assert.That(!getEmptyArray().AtLeast(2));
        }
        private static IEnumerable<int> getEmptyArray()
        {
            return new int[] { };
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastZeroElementsForCollections()
        {
            Assert.That(getSingleElementArray().AtLeast(0));
        }
        [Test]
        public void AtLeastWithSingleElementHasAtLeastOneElementForCollections()
        {
            Assert.That(getSingleElementArray().AtLeast(1));
        }
        [Test]
        public void AtLeastWithSingleElementHasAtLeastManyElementsForCollections()
        {
            Assert.That(!getSingleElementArray().AtLeast(2));
        }
        private static IEnumerable<int> getSingleElementArray()
        {
            return getSingleElementSequence().ToArray();
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastZeroElementsForCollections()
        {
            Assert.That(getManyElementArray().AtLeast(0));
        }
        [Test]
        public void AtLeastWithManyElementsHasAtLeastOneElementForCollections()
        {
            Assert.That(getManyElementArray().AtLeast(1));
        }
        [Test]
        public void AtLeastWithManyElementsHasAtLeastManyElementsForCollections()
        {
            Assert.That(getManyElementArray().AtLeast(2));
        }
        private static IEnumerable<int> getManyElementArray()
        {
            return getManyElementSequence().ToArray();
        }
    }
}
