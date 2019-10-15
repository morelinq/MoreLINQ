#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 James Webster. All rights reserved.
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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class TrySingleTest
    {
        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        [TestCase(SourceKind.BreakingCollection)]
        [TestCase(SourceKind.BreakingReadOnlyCollection)]
        public void TrySingleWithEmptySource(SourceKind kind)
        {
            var arrayWithNone = new int?[0].ToSourceKind(kind);

            var (cardinality, value) = arrayWithNone.TrySingle("zero", "one", "many");

            Assert.AreEqual(cardinality, "zero");
            Assert.IsNull(value);
        }

        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        public void TrySingleWithSingleton(SourceKind kind)
        {
            var arrayWithOne = new int?[] { 10 }.ToSourceKind(kind);

            var (cardinality, value) = arrayWithOne.TrySingle("zero", "one", "many");

            Assert.AreEqual(cardinality, "one");
            Assert.AreEqual(value, 10);
        }

        [TestCaseSource(nameof(_singletonCollectionTestCases))]
        public void TrySingleWithSingletonCollections<T>(IEnumerable<T> collection, T result)
        {
            var (cardinality, value) = collection.TrySingle("zero", "one", "many");

            Assert.AreEqual(cardinality, "one");
            Assert.AreEqual(value, result);
        }

        private static object[] _singletonCollectionTestCases =
        {
            new object[] { new BreakingSingleElementCollection<int>(10), 10 },
            new object[] { new BreakingSingleElementReadOnlyCollection<int>(20), 20 }
        };

        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        [TestCase(SourceKind.BreakingCollection)]
        [TestCase(SourceKind.BreakingReadOnlyCollection)]
        public void TrySingleWithMoreThanOne(SourceKind kind)
        {
            var arrayWithMultiple = new int?[] { 10, 20 }.ToSourceKind(kind);

            var (cardinality, value) = arrayWithMultiple.TrySingle("zero", "one", "many");

            Assert.AreEqual(cardinality, "many");
            Assert.IsNull(value);
        }

        [Test]
        public void TrySingleDoesNotConsumeMoreThanTwoElementsFromTheSequence()
        {
            IEnumerable<int> TestSequence()
            {
                yield return 1;
                yield return 2;
                throw new Exception(nameof(MoreEnumerable.TrySingle) + " should not have attempted to consume a third element.");
            }

            var (cardinality, value) = TestSequence().TrySingle("zero", "one", "many");
            Assert.AreEqual("many", cardinality);
            Assert.AreEqual(default(int), value);
        }

        [Test]
        public void TrySingleOptimizesForList()
        {
            var list = new BreakingList<int>();

            var (cardinality, value) = list.TrySingle("zero", "one", "many");
            Assert.AreEqual("zero", cardinality);
            Assert.AreEqual(default(int), value);

            list = new BreakingList<int>(new List<int> { 1 });
            (cardinality, value) = list.TrySingle("zero", "one", "many");
            Assert.AreEqual("one", cardinality);
            Assert.AreEqual(1, value);

            list = new BreakingList<int>(new List<int> { 1, 2 });
            (cardinality, value) = list.TrySingle("zero", "one", "many");
            Assert.AreEqual("many", cardinality);
            Assert.AreEqual(default(int), value);
        }

        [Test]
        public void TrySingleOptimizesForCollection()
        {
            var coll = new BreakingCollection<int>();

            var (cardinality, value) = coll.TrySingle("zero", "one", "many");
            Assert.AreEqual("zero", cardinality);
            Assert.AreEqual(default(int), value);

            coll = new BreakingCollection<int>(new List<int> {1, 2});

            (cardinality, value) = coll.TrySingle("zero", "one", "many");
            Assert.AreEqual("many", cardinality);
            Assert.AreEqual(default(int), value);
        }

        [TestCase(0, "zero")]
        [TestCase(1, "one")]
        [TestCase(2, "many")]
        public void TrySingleEnumeratesOnceOnlyAndDisposes(int numberOfElements, string expectedCardinality)
        {
            using (var seq = Enumerable.Range(1, numberOfElements).AsTestingSequence())
            {
                var (cardinality, _) = seq.TrySingle("zero", "one", "many");
                Assert.AreEqual(expectedCardinality, cardinality);
            }
        }

        private class BreakingSingleElementCollection<T> : ICollection<T>
        {
            private readonly T _element;

            public BreakingSingleElementCollection(T element)
            {
                _element = element;
            }

            public IEnumerator<T> GetEnumerator()
            {
                yield return _element;
                throw new Exception($"{nameof(MoreEnumerable.TrySingle)} should not have attempted to consume a second element.");
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public void Add(T item) => throw new NotImplementedException();
            public void Clear() => throw new NotImplementedException();
            public bool Contains(T item) => throw new NotImplementedException();
            public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();
            public bool Remove(T item) => throw new NotImplementedException();
            public int Count => 1;
            public bool IsReadOnly => true;
        }

        private class BreakingSingleElementReadOnlyCollection<T> : IReadOnlyCollection<T>
        {
            readonly T _element;

            public BreakingSingleElementReadOnlyCollection(T element)
            {
                _element = element;
            }

            public IEnumerator<T> GetEnumerator()
            {
                yield return _element;
                throw new Exception($"{nameof(MoreEnumerable.TrySingle)} should not have attempted to consume a second element.");
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public int Count => 1;
        }
    }
}
