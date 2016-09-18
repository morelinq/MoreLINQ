#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Jonas Nyrup. All rights reserved.
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
    public class AtMostTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AtMostWithNullSequence()
        {
            IEnumerable<int> sequence = null;
            sequence.AtMost(1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AtMostWithNegativeCount()
        {
            new[] { 1 }.AtMost(-1);
        }

        private static IEnumerable<int> GetSequence()
        {
            return new InfiniteSequence<int>(0);
        }
        [Test]
        public void AtMostWithEmptySequenceHasAtMostZeroElements()
        {
            Assert.IsTrue(GetEmptySequence().AtMost(0));
        }
        [Test]
        public void AtMostWithEmptySequenceHasAtMostOneElement()
        {
            Assert.IsTrue(GetEmptySequence().AtMost(1));
        }
        private static IEnumerable<int> GetEmptySequence()
        {
            return LinqEnumerable.Empty<int>();
        }

        [Test]
        public void AtMostWithSingleElementHasAtMostZeroElements()
        {
            Assert.IsFalse(GetSingleElementSequence().AtMost(0));
        }
        [Test]
        public void AtMostWithSingleElementHasAtMostOneElement()
        {
            Assert.IsTrue(GetSingleElementSequence().AtMost(1));
        }
        [Test]
        public void AtMostWithSingleElementHasAtMostTwoElements()
        {
            Assert.IsTrue(GetSingleElementSequence().AtMost(2));
        }
        private static IEnumerable<int> GetSingleElementSequence()
        {
            return GetSequence().Take(1);
        }

        [Test]
        public void AtMostWithManyElementsHasAtMostZeroElements()
        {
            Assert.IsFalse(GetManyElementSequence().AtMost(0));
        }
        [Test]
        public void AtMostWithManyElementsHasAtMostOneElement()
        {
            Assert.IsFalse(GetManyElementSequence().AtMost(1));
        }
        [Test]
        public void AtMostWithManyElementsHasAtMostThreeElements()
        {
            Assert.IsTrue(GetManyElementSequence().AtMost(3));
        }
        [Test]
        public void AtMostWithManyElementsHasAtMostManyElements()
        {
            Assert.IsTrue(GetManyElementSequence().AtMost(4));
        }
        private static IEnumerable<int> GetManyElementSequence()
        {
            return GetSequence().Take(3);
        }

        //ICollection<T> Optimization Tests
        [Test]
        public void AtMostWithEmptySequenceHasAtMostZeroElementsForCollections()
        {
            Assert.IsTrue(GetEmptyArray().AtMost(0));
        }
        [Test]
        public void AtMostWithEmptySequenceHasAtMostOneElementForCollections()
        {
            Assert.IsTrue(GetEmptyArray().AtMost(1));
        }
        private static IEnumerable<int> GetEmptyArray()
        {
            return new int[] { };
        }

        [Test]
        public void AtMostWithSingleElementHasAtMostZeroElementsForCollections()
        {
            Assert.IsFalse(GetSingleElementArray().AtMost(0));
        }
        [Test]
        public void AtMostWithSingleElementHasAtMostOneElementForCollections()
        {
            Assert.IsTrue(GetSingleElementArray().AtMost(1));
        }
        [Test]
        public void AtMostWithSingleElementHasAtMostManyElementsForCollections()
        {
            Assert.IsTrue(GetSingleElementArray().AtMost(2));
        }
        private static IEnumerable<int> GetSingleElementArray()
        {
            return GetSingleElementSequence().ToArray();
        }

        [Test]
        public void AtMostWithManyElementsHasAtMostZeroElementsForCollections()
        {
            Assert.IsFalse(GetManyElementArray().AtMost(0));
        }
        [Test]
        public void AtMostWithManyElementsHasAtMostOneElementForCollections()
        {
            Assert.IsFalse(GetManyElementArray().AtMost(1));
        }
        [Test]
        public void AtMostWithManyElementsHasAtMostThreeElementsForCollections()
        {
            Assert.IsTrue(GetManyElementArray().AtMost(3));
        }
        [Test]
        public void AtMostWithManyElementsHasAtMostManyElementsForCollections()
        {
            Assert.IsTrue(GetManyElementArray().AtMost(4));
        }
        private static IEnumerable<int> GetManyElementArray()
        {
            return GetManyElementSequence().ToArray();
        }

        [Test]
        public void AtMostShouldBeNotEnumerateSequenceForImplementersOfICollection()
        {
            var sequence = new UnenumerableList<int>();
            sequence.AtMost(3);
        }
    }
}