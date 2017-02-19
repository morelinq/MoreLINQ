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
    public class ExactlyTest
    {
        [Test]
        public void ExactlyWithNegativeCount()
        {
            Assert.ThrowsArgumentOutOfRangeException("count", () =>
                new[] { 1 }.Exactly(-1));
        }

        [Test]
        public void ExactlySequenceWithFirstNullSequence()
        {
            Assert.ThrowsArgumentNullException("first",
                () => MoreEnumerable.Exactly<int>(null, LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void ExactlySequenceWithSecondNullSequence()
        {
            Assert.ThrowsArgumentNullException("second",
                () => MoreEnumerable.Exactly<int>(LinqEnumerable.Empty<int>(), null));
        }

        [Test]
        public void ExactlyWithEmptySequenceHasExactlyZeroElements()
        {
            Assert.IsTrue(LinqEnumerable.Empty<int>().Exactly(0));
        }

        [Test]
        public void ExactlyWithEmptySequenceHasExactlyOneElement()
        {
            Assert.IsFalse(LinqEnumerable.Empty<int>().Exactly(1));
        }

        [Test]
        public void ExactlyWithSingleElementHasExactlyOneElements()
        {
            Assert.IsTrue(new[] { 1 }.Exactly(1));
        }

        [Test]
        public void ExactlyWithManyElementHasExactlyOneElement()
        {
            Assert.IsFalse(new[] { 1, 2, 3 }.Exactly(1));
        }

        [Test]
        public void ExactlyWithEmptyCollectionHasExactlyEmptyCollection()
        {
            Assert.IsTrue(LinqEnumerable.Empty<int>().Exactly(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void ExactlyWithEmptyCollectionHasExactlyEmptySequence()
        {
            Assert.IsTrue(LinqEnumerable.Empty<int>().Exactly(LinqEnumerable.Range(1, 0)));
        }

        [Test]
        public void ExactlyWithEmptySequenceHasExactlyEmptyCollection()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 0).Exactly(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void ExactlyWithEmptySequenceHasExactlyEmptySequence()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 0).Exactly(LinqEnumerable.Range(1, 0)));
        }

        [Test]
        public void ExactlyWithEmptyCollectionHasExactlyOneElementCollection()
        {
            Assert.IsFalse(LinqEnumerable.Empty<int>().Exactly(new[] { 1 }));
        }

        [Test]
        public void ExactlyWithEmptyCollectionHasExactlyOneElementSequence()
        {
            Assert.IsFalse(LinqEnumerable.Empty<int>().Exactly(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void ExactlyWithEmptySequenceHasExactlyOneElementCollection()
        {
            Assert.IsFalse(LinqEnumerable.Range(1, 0).Exactly(new[] { 1 }));
        }

        [Test]
        public void ExactlyWithEmptySequenceHasExactlyOneElementSequence()
        {
            Assert.IsFalse(LinqEnumerable.Range(1, 0).Exactly(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void ExactlyWithSingleElementCollectionHasExactlyOneElementCollection()
        {
            Assert.IsTrue(new[] { 1 }.Exactly(new[] { 1 }));
        }

        [Test]
        public void ExactlyWithSingleElementCollectionHasExactlyOneElementSequence()
        {
            Assert.IsTrue(new[] { 1 }.Exactly(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void ExactlyWithSingleElementSequenceHasExactlyOneElementCollection()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 1).Exactly(new[] { 1 }));
        }

        [Test]
        public void ExactlyWithSingleElementSequenceHasExactlyOneElementSequence()
        {
            Assert.IsTrue(LinqEnumerable.Range(1, 1).Exactly(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void ExactlyWithManyElementCollectionHasExactlyOneElementCollection()
        {
            Assert.IsFalse(new[] { 1, 2, 3 }.Exactly(new[] { 1 }));
        }

        [Test]
        public void ExactlyWithManyElementCollectionHasExactlyOneElementSequence()
        {
            Assert.IsFalse(new[] { 1, 2, 3 }.Exactly(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void ExactlyWithManyElementSequenceHasExactlyOneElementCollection()
        {
            Assert.IsFalse(LinqEnumerable.Range(1, 3).Exactly(new[] { 1 }));
        }

        [Test]
        public void ExactlyWithManyElementSequenceHasExactlyOneElementSequence()
        {
            Assert.IsFalse(LinqEnumerable.Range(1, 3).Exactly(LinqEnumerable.Range(1, 1)));
        }
    }
}
