#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Leandro F. Vieira (leandromoh). All rights reserved.
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
    public class AtMostTest
    {
        [Test]
        public void AtMostWithNegativeCount()
        {
            Assert.ThrowsArgumentOutOfRangeException("count",
                () => new[] { 1 }.AtMost(-1));
        }

        [Test]
        public void AtMostSequenceWithFirstNullSequence()
        {
            Assert.ThrowsArgumentNullException("first",
                () => MoreEnumerable.AtMost<int, int>(null, LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void AtMostSequenceWithSecondNullSequence()
        {
            Assert.ThrowsArgumentNullException("second",
                () => MoreEnumerable.AtMost<int, int>(LinqEnumerable.Empty<int>(), null));
        }

        [Test]
        public void AtMostWithEmptySequenceHasAtMostZeroElements()
        {
            Assert.IsTrue(LinqEnumerable.Empty<int>().AtMost(0));
        }

        [Test]
        public void AtMostWithEmptySequenceHasAtMostOneElement()
        {
            Assert.IsTrue(LinqEnumerable.Empty<int>().AtMost(1));
        }

        [Test]
        public void AtMostWithSingleElementHasAtMostZeroElements()
        {
            Assert.IsFalse(new[] { 1 }.AtMost(0));
        }

        [Test]
        public void AtMostWithSingleElementHasAtMostOneElement()
        {
            Assert.IsTrue(new[] { 1 }.AtMost(1));
        }

        [Test]
        public void AtMostWithSingleElementHasAtMostManyElements()
        {
            Assert.IsTrue(new[] { 1 }.AtMost(2));
        }

        [Test]
        public void AtMostWithManyElementsHasAtMostOneElements()
        {
            Assert.IsFalse(new[] { 1, 2, 3 }.AtMost(1));
        }

        //Enumerables
        [Test]
        public void AtMostWithEmptyCollectionHasAtMostEmptyCollection()
        {
            Assert.IsTrue(LinqEnumerable.Empty<int>().AtMost(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void AtMostWithEmptyCollectionHasAtMostEmptySequence()
        {
            Assert.IsTrue(LinqEnumerable.Empty<int>().AtMost(LinqEnumerable.Range(1, 0)));
        }

        [Test]
        public void AtMostWithEmptySequenceHasAtMostEmptyCollection()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 0).AtMost(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void AtMostWithEmptySequenceHasAtMostEmptySequence()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 0).AtMost(LinqEnumerable.Range(1, 0)));
        }

        [Test]
        public void AtMostWithEmptyCollectionHasAtMostOneElementCollection()
        {
            Assert.IsTrue(LinqEnumerable.Empty<int>().AtMost(new[] { 1 }));
        }

        [Test]
        public void AtMostWithEmptyCollectionHasAtMostOneElementSequence()
        {
            Assert.IsTrue(LinqEnumerable.Empty<int>().AtMost(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void AtMostWithEmptySequenceHasAtMostOneElementCollection()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 0).AtMost(new[] { 1 }));
        }

        [Test]
        public void AtMostWithEmptySequenceHasAtMostOneElementSequence()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 0).AtMost(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void AtMostWithSingleElementCollectionHasAtMostZeroElementCollection()
        {
            Assert.IsFalse(new[] { 1 }.AtMost(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void AtMostWithSingleElementCollectionHasAtMostZeroElementSequence()
        {
            Assert.IsFalse(new[] { 1 }.AtMost(LinqEnumerable.Range(1, 0)));
        }

        [Test]
        public void AtMostWithSingleElementSequenceHasAtMostZeroElementCollection()
        {
            Assert.IsFalse(LinqEnumerable.Range(1, 1).AtMost(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void AtMostWithSingleElementSequenceHasAtMostZeroElementSequence()
        {
            Assert.IsFalse(LinqEnumerable.Range(1, 1).AtMost(LinqEnumerable.Range(1, 0)));
        }

        [Test]
        public void AtMostWithSingleElementCollectionHasAtMostOneElementCollection()
        {
            Assert.IsTrue(new[] { 1 }.AtMost(new[] { 1 }));
        }

        [Test]
        public void AtMostWithSingleElementCollectionHasAtMostOneElementSequence()
        {
            Assert.IsTrue(new[] { 1 }.AtMost(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void AtMostWithSingleElementSequenceHasAtMostOneElementCollection()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 1).AtMost(new[] { 1 }));
        }

        [Test]
        public void AtMostWithSingleElementSequenceHasAtMostOneElementSequence()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 1).AtMost(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void AtMostWithSingleElementCollectionHasAtMostManyElementCollection()
        {
            Assert.IsTrue(new[] { 1 }.AtMost(new[] { 1, 2 }));
        }

        [Test]
        public void AtMostWithSingleElementCollectionHasAtMostManyElementSequence()
        {
            Assert.IsTrue(new[] { 1 }.AtMost(LinqEnumerable.Range(1, 2)));
        }

        [Test]
        public void AtMostWithSingleElementSequenceHasAtMostManyElementCollection()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 1).AtMost(new[] { 1, 2 }));
        }

        [Test]
        public void AtMostWithSingleElementSequenceHasAtMostManyElementSequence()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 1).AtMost(LinqEnumerable.Range(1, 2)));
        }

        [Test]
        public void AtMostWithManyElementCollectionHasAtMostOneElementCollection()
        {
            Assert.IsFalse(new[] { 1, 2, 3 }.AtMost(new[] { 1 }));
        }

        [Test]
        public void AtMostWithManyElementCollectionHasAtMostOneElementSequence()
        {
            Assert.IsFalse(new[] { 1, 2, 3 }.AtMost(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void AtMostWithManyElementSequenceHasAtMostOneElementCollection()
        {
            Assert.IsFalse(LinqEnumerable.Range(1, 3).AtMost(new[] { 1 }));
        }

        [Test]
        public void AtMostWithManyElementSequenceHasAtMostOneElementSequence()
        {
            Assert.IsFalse(LinqEnumerable.Range(1, 3).AtMost(LinqEnumerable.Range(1, 1)));
        }
    }
}
