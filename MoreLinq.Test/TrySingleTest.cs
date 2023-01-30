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
    using NUnit.Framework.Interfaces;
    using Experimental;

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
            var source = new int?[0].ToSourceKind(kind);

            var (cardinality, value) = source.TrySingle("zero", "one", "many");

            Assert.That(cardinality, Is.EqualTo("zero"));
            Assert.That(value, Is.Null);
        }

        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        public void TrySingleWithSingleton(SourceKind kind)
        {
            var source = new int?[] { 10 }.ToSourceKind(kind);

            var (cardinality, value) = source.TrySingle("zero", "one", "many");

            Assert.That(cardinality, Is.EqualTo("one"));
            Assert.That(value, Is.EqualTo(10));
        }

        [TestCaseSource(nameof(SingletonCollectionTestCases))]
        public void TrySingleWithSingletonCollection<T>(IEnumerable<T> source, T result)
        {
            var (cardinality, value) = source.TrySingle("zero", "one", "many");

            Assert.That(cardinality, Is.EqualTo("one"));
            Assert.That(value, Is.EqualTo(result));
        }

        static readonly ITestCaseData[] SingletonCollectionTestCases =
        {
            new TestCaseData(new BreakingSingleElementCollection<int>(10), 10),
            new TestCaseData(new BreakingSingleElementReadOnlyCollection<int>(20), 20)
        };

        class BreakingSingleElementCollectionBase<T> : IEnumerable<T>
        {
            readonly T _element;

            protected BreakingSingleElementCollectionBase(T element) => _element = element;

#pragma warning disable CA1822 // Mark members as static
            public int Count => 1;
#pragma warning restore CA1822 // Mark members as static

            public IEnumerator<T> GetEnumerator()
            {
                yield return _element;
                Assert.Fail($"{nameof(ExperimentalEnumerable.TrySingle)} should not have attempted to consume a second element.");
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        sealed class BreakingSingleElementCollection<T> :
            BreakingSingleElementCollectionBase<T>, ICollection<T>
        {
            public BreakingSingleElementCollection(T element) : base(element) { }

            public void Add(T item) => throw new NotImplementedException();
            public void Clear() => throw new NotImplementedException();
            public bool Contains(T item) => throw new NotImplementedException();
            public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();
            public bool Remove(T item) => throw new NotImplementedException();
            public bool IsReadOnly => true;
        }

        sealed class BreakingSingleElementReadOnlyCollection<T> :
            BreakingSingleElementCollectionBase<T>, IReadOnlyCollection<T>
        {
            public BreakingSingleElementReadOnlyCollection(T element) : base(element) { }
        }

        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        [TestCase(SourceKind.BreakingCollection)]
        [TestCase(SourceKind.BreakingReadOnlyCollection)]
        public void TrySingleWithMoreThanOne(SourceKind kind)
        {
            var source = new int?[] { 10, 20 }.ToSourceKind(kind);

            var (cardinality, value) = source.TrySingle("zero", "one", "many");

            Assert.That(cardinality, Is.EqualTo("many"));
            Assert.That(value, Is.Null);
        }

        [Test]
        public void TrySingleDoesNotConsumeMoreThanTwoElementsFromTheSequence()
        {
            static IEnumerable<int> TestSequence()
            {
                yield return 1;
                yield return 2;
                Assert.Fail(nameof(ExperimentalEnumerable.TrySingle) + " should not have attempted to consume a third element.");
            }

            var (cardinality, value) = TestSequence().TrySingle("zero", "one", "many");

            Assert.That(cardinality, Is.EqualTo("many"));
            Assert.That(value, Is.EqualTo(0));
        }

        [TestCase(0, "zero")]
        [TestCase(1, "one")]
        [TestCase(2, "many")]
        public void TrySingleEnumeratesOnceOnlyAndDisposes(int numberOfElements, string expectedCardinality)
        {
            using var seq = Enumerable.Range(1, numberOfElements).AsTestingSequence();
            var (cardinality, _) = seq.TrySingle("zero", "one", "many");
            Assert.That(cardinality, Is.EqualTo(expectedCardinality));
        }
    }
}
