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

using NUnit.Framework;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class CompareCountTest
    {
        [Test]
        public void CompareCountSequenceWithFirstNullSequence()
        {
            Assert.ThrowsArgumentNullException("first", () =>
                MoreEnumerable.CompareCount<int, int>(null, LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void CompareCountSequenceWithSecondNullSequence()
        {
            Assert.ThrowsArgumentNullException("second", () =>
                MoreEnumerable.CompareCount<int, int>(LinqEnumerable.Empty<int>(), null));
        }

        [Test]
        public void CompareCountWithEmptyCollectionHasCompareCountEmptyCollection()
        {
            Assert.AreEqual(0, LinqEnumerable.Empty<int>().CompareCount(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void CompareCountWithEmptySequenceHasCompareCountEmptyCollection()
        {
            Assert.AreEqual(0, LinqEnumerable.Range(1, 0).CompareCount(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void CompareCountWithEmptyCollectionHasCompareCountEmptySequence()
        {
            Assert.AreEqual(0, LinqEnumerable.Empty<int>().CompareCount(LinqEnumerable.Range(1, 0)));
        }

        [Test]
        public void CompareCountWithEmptySequenceHasCompareCountEmptySequence()
        {
            Assert.AreEqual(0, LinqEnumerable.Range(1, 0).CompareCount(LinqEnumerable.Range(1, 0)));
        }

        [Test]
        public void CompareCountWithSingleElementCollectionHasCompareCountSingleElementCollection()
        {
            Assert.AreEqual(0, new[] { 1 }.CompareCount(new[] { 1 }));
        }

        [Test]
        public void CompareCountWithSingleElementCollectionHasCompareCountSingleElementSequence()
        {
            Assert.AreEqual(0, new[] { 1 }.CompareCount(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void CompareCountWithSingleElementSequenceHasCompareCountSingleElementCollection()
        {
            Assert.AreEqual(0, LinqEnumerable.Range(1, 1).CompareCount(new[] { 1 }));
        }

        [Test]
        public void CompareCountWithSingleElementSequenceHasCompareCountSingleElementSequence()
        {
            Assert.AreEqual(0, LinqEnumerable.Range(1, 1).CompareCount(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void CompareCountWithEmptyCollectionHasCompareCountCollectionWithOneElement()
        {
            Assert.AreEqual(-1, LinqEnumerable.Empty<int>().CompareCount(new[] { 1 }));
        }

        [Test]
        public void CompareCountWithEmptyCollectionHasCompareCountSequenceWithOneElement()
        {
            Assert.AreEqual(-1, LinqEnumerable.Empty<int>().CompareCount(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void CompareCountWithEmptySequenceHasCompareCountCollectionWithOneElement()
        {
            Assert.AreEqual(-1, LinqEnumerable.Range(1, 0).CompareCount(new[] { 1 }));
        }

        [Test]
        public void CompareCountWithEmptySequenceHasCompareCountSequenceWithOneElement()
        {
            Assert.AreEqual(-1, LinqEnumerable.Range(1, 0).CompareCount(LinqEnumerable.Range(1, 1)));
        }

        [Test]
        public void CompareCountWithSingleElementCollectionHasCompareCountEmptyCollection()
        {
            Assert.AreEqual(1, new[] { 1 }.CompareCount(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void CompareCountWithSingleElementCollectionHasCompareCountEmptySequence()
        {
            Assert.AreEqual(1, new[] { 1 }.CompareCount(LinqEnumerable.Range(1, 0)));
        }

        [Test]
        public void CompareCountWithSingleElementSequenceHasCompareCountEmptyCollection()
        {
            Assert.AreEqual(1, LinqEnumerable.Range(1, 1).CompareCount(LinqEnumerable.Empty<int>()));
        }

        [Test]
        public void CompareCountWithSingleElementSequenceHasCompareCountEmptySequence()
        {
            Assert.AreEqual(1, LinqEnumerable.Range(1, 1).CompareCount(LinqEnumerable.Range(1, 0)));
        }
    }
}
