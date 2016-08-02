#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2015 sholland. All rights reserved.
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
    public class ExactlyTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExactlyWithNullSequence()
        {
            IEnumerable<int> sequence = null;
            sequence.Exactly(1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExactlyWithNegativeCount()
        {
            new[] { 1 }.Exactly(-1);
        }

        private static IEnumerable<int> GetSequence()
        {
            return new InfiniteSequence<int>(0);
        }
        [Test]
        public void ExactlyWithEmptySequenceHasExactlyZeroElements()
        {
            Assert.IsTrue(GetEmptySequence().Exactly(0));
        }
        [Test]
        public void ExactlyWithEmptySequenceHasExactlyOneElement()
        {
            Assert.IsFalse(GetEmptySequence().Exactly(1));
        }
        [Test]
        public void ExactlyWithEmptySequenceHasExactlyManyElements()
        {
            Assert.IsFalse(GetEmptySequence().Exactly(2));
        }
        private static IEnumerable<int> GetEmptySequence()
        {
            return LinqEnumerable.Empty<int>();
        }

        [Test]
        public void ExactlyWithSingleElementHasExactlyZeroElements()
        {
            Assert.IsFalse(GetSingleElementSequence().Exactly(0));
        }
        [Test]
        public void ExactlyWithSingleElementHasExactlyOneElement()
        {
            Assert.IsTrue(GetSingleElementSequence().Exactly(1));
        }
        [Test]
        public void ExactlyWithSingleElementHasExactlyManyElements()
        {
            Assert.IsFalse(GetSingleElementSequence().Exactly(2));
        }
        private static IEnumerable<int> GetSingleElementSequence()
        {
            return GetSequence().Take(1);
        }

        [Test]
        public void ExactlyWithThreeElementsHasExactlyZeroElements()
        {
            Assert.IsFalse(GetThreeElementSequence().Exactly(0));
        }
        [Test]
        public void ExactlyWithThreeElementsHasExactlyOneElement()
        {
            Assert.IsFalse(GetThreeElementSequence().Exactly(1));
        }
        [Test]
        public void ExactlyWithThreeElementsHasExactlyManyElements()
        {
            Assert.IsTrue(GetThreeElementSequence().Exactly(3));
        }
        private static IEnumerable<int> GetThreeElementSequence()
        {
            return GetSequence().Take(3);
        }

        //ICollection<T> Optimization Tests
        [Test]
        public void ExactlyWithEmptySequenceHasExactlyZeroElementsForCollections()
        {
            Assert.IsTrue(GetEmptyArray().Exactly(0));
        }
        [Test]
        public void ExactlyWithEmptySequenceHasExactlyOneElementForCollections()
        {
            Assert.IsFalse(GetEmptyArray().Exactly(1));
        }
        [Test]
        public void ExactlyWithEmptySequenceHasExactlyManyElementsForCollections()
        {
            Assert.IsFalse(GetEmptyArray().Exactly(2));
        }
        private static IEnumerable<int> GetEmptyArray()
        {
            return new int[] { };
        }

        [Test]
        public void ExactlyWithSingleElementHasExactlyZeroElementsForCollections()
        {
            Assert.IsFalse(GetSingleElementArray().Exactly(0));
        }
        [Test]
        public void ExactlyWithSingleElementHasExactlyOneElementForCollections()
        {
            Assert.IsTrue(GetSingleElementArray().Exactly(1));
        }
        [Test]
        public void ExactlyWithSingleElementHasExactlyManyElementsForCollections()
        {
            Assert.IsFalse(GetSingleElementArray().Exactly(2));
        }
        private static IEnumerable<int> GetSingleElementArray()
        {
            return GetSingleElementSequence().ToArray();
        }

        [Test]
        public void ExactlyWithThreeElementsHasExactlyZeroElementsForCollections()
        {
            Assert.IsFalse(GetThreeElementArray().Exactly(0));
        }
        [Test]
        public void ExactlyWithThreeElementsHasExactlyOneElementForCollections()
        {
            Assert.IsFalse(GetThreeElementArray().Exactly(1));
        }
        [Test]
        public void ExactlyWithThreeElementsHasExactlyManyElementsForCollections()
        {
            Assert.IsTrue(GetThreeElementArray().Exactly(3));
        }
        private static IEnumerable<int> GetThreeElementArray()
        {
            return GetThreeElementSequence().ToArray();
        }

        [Test]
        public void ExactlyShouldBeNotEnumerateSequenceForImplementersOfICollection()
        {
            var sequence = new UnenumerableList<int>();
            sequence.Exactly(3);
        }
    }
}
