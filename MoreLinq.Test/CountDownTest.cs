#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Atif Aziz. All rights reserved.
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
    public class CountDownTest
    {
        [Test]
        public void IsLazy()
        {
            var bs = new BreakingSequence<object>();
            _ = bs.CountDown(42, BreakingFunc.Of<object, int?, object>());
        }

        [Test]
        public void WithNegativeCount()
        {
            const int count = 10;
            Enumerable.Range(1, count)
                      .CountDown(-1000, (_, cd) => cd)
                      .AssertSequenceEqual(Enumerable.Repeat((int?)null, count));
        }

        static IEnumerable<T> GetData<T>(Func<int[], int, int?[], T> selector)
        {
            var xs = Enumerable.Range(0, 5).ToArray();
            yield return selector(xs, -1, new int?[] { null, null, null, null, null });
            yield return selector(xs,  0, new int?[] { null, null, null, null, null });
            yield return selector(xs,  1, new int?[] { null, null, null, null,    0 });
            yield return selector(xs,  2, new int?[] { null, null, null,    1,    0 });
            yield return selector(xs,  3, new int?[] { null, null,    2,    1,    0 });
            yield return selector(xs,  4, new int?[] { null,    3,    2,    1,    0 });
            yield return selector(xs,  5, new int?[] {    4,    3,    2,    1,    0 });
            yield return selector(xs,  6, new int?[] {    4,    3,    2,    1,    0 });
            yield return selector(xs,  7, new int?[] {    4,    3,    2,    1,    0 });
        }

        static readonly IEnumerable<TestCaseData> SequenceData =
            from e in GetData((xs, count, countdown) => new
            {
                Source = xs, Count = count, Countdown = countdown
            })
            select new TestCaseData(e.Source, e.Count)
                .Returns(e.Source.Zip(e.Countdown, ValueTuple.Create))
                .SetName($"{nameof(WithSequence)}({{ {string.Join(", ", e.Source)} }}, {e.Count})");

        [TestCaseSource(nameof(SequenceData))]
        public IEnumerable<(int, int?)> WithSequence(int[] xs, int count)
        {
            using var ts = xs.Select(x => x).AsTestingSequence();
            foreach (var e in ts.CountDown(count, ValueTuple.Create))
                yield return e;
        }

        static readonly IEnumerable<TestCaseData> ListData =
            from e in GetData((xs, count, countdown) => new
            {
                Source = xs, Count = count, Countdown = countdown
            })
            from kind in SourceKinds.List
            select new TestCaseData(e.Source.ToSourceKind(kind), e.Count)
                .Returns(e.Source.Zip(e.Countdown, ValueTuple.Create))
                .SetName($"{nameof(WithList)}({kind} {{ {string.Join(", ", e.Source)} }}, {e.Count})");

        [TestCaseSource(nameof(ListData))]
        public IEnumerable<(int, int?)> WithList(IEnumerable<int> xs, int count) =>
            xs.CountDown(count, ValueTuple.Create);

        static readonly IEnumerable<TestCaseData> CollectionData =
            from e in GetData((xs, count, countdown) => new
            {
                Source = xs, Count = count, Countdown = countdown
            })
            from isReadOnly in new[] { true, false }
            select new TestCaseData(e.Source, isReadOnly, e.Count)
                .Returns(e.Source.Zip(e.Countdown, ValueTuple.Create))
                .SetName($"{nameof(WithCollection)}({{ {string.Join(", ", e.Source)} }}, {isReadOnly}, {e.Count})");

        [TestCaseSource(nameof(CollectionData))]
        public IEnumerable<(int, int?)> WithCollection(int[] xs, bool isReadOnly, int count)
        {
            var moves = 0;
            var disposed = false;

            IEnumerator<T> Watch<T>(IEnumerator<T> e)
            {
                moves = 0;
                disposed = false;
                var te = e.AsWatchable();
                te.Disposed += delegate { disposed = true; };
                te.MoveNextCalled += delegate { moves++; };
                return te;
            }

            var ts = isReadOnly
                   ? TestCollection.CreateReadOnly(xs, Watch)
                   : TestCollection.Create(xs, Watch).AsEnumerable();

            foreach (var e in ts.CountDown(count, ValueTuple.Create).Index(1))
            {
                // For a collection, CountDown doesn't do any buffering
                // so check that as each result becomes available, the
                // source hasn't been "pulled" on more.

                Assert.That(moves, Is.EqualTo(e.Key));
                yield return e.Value;
            }

            Assert.That(disposed, Is.True);
        }

        static class TestCollection
        {
            public static ICollection<T>
                Create<T>(ICollection<T> collection,
                             Func<IEnumerator<T>, IEnumerator<T>>? em = null)
            {
                return new Collection<T>(collection, em);
            }

            public static IReadOnlyCollection<T>
                CreateReadOnly<T>(ICollection<T> collection,
                            Func<IEnumerator<T>, IEnumerator<T>>? em = null)
            {
                return new ReadOnlyCollection<T>(collection, em);
            }

            /// <summary>
            /// A sequence that permits its enumerator to be substituted
            /// for another.
            /// </summary>

            abstract class Sequence<T> : IEnumerable<T>
            {
                readonly Func<IEnumerator<T>, IEnumerator<T>> _em;

                protected Sequence(Func<IEnumerator<T>, IEnumerator<T>>? em) =>
                    _em = em ?? (e => e);

                public IEnumerator<T> GetEnumerator() =>
                    _em(Items.GetEnumerator());

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                protected abstract IEnumerable<T> Items { get; }
            }

            /// <summary>
            /// A collection that wraps another but which also permits its
            /// enumerator to be substituted for another.
            /// </summary>

            sealed class Collection<T> : Sequence<T>, ICollection<T>
            {
                readonly ICollection<T> _collection;

                public Collection(ICollection<T> collection,
                                  Func<IEnumerator<T>, IEnumerator<T>>? em = null) :
                    base(em) =>
                    _collection = collection ?? throw new ArgumentNullException(nameof(collection));

                public int Count => _collection.Count;
                public bool IsReadOnly => _collection.IsReadOnly;

                protected override IEnumerable<T> Items => _collection;

                public bool Contains(T item) => _collection.Contains(item);
                public void CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

                public void Add(T item) => throw new NotImplementedException();
                public void Clear() => throw new NotImplementedException();
                public bool Remove(T item) => throw new NotImplementedException();
            }

            /// <summary>
            /// A read-only collection that wraps another collection but which
            /// also permits its enumerator to be substituted for another.
            /// </summary>

            sealed class ReadOnlyCollection<T> : Sequence<T>, IReadOnlyCollection<T>
            {
                readonly ICollection<T> _collection;

                public ReadOnlyCollection(ICollection<T> collection,
                                          Func<IEnumerator<T>, IEnumerator<T>>? em = null) :
                    base(em) =>
                    _collection = collection ?? throw new ArgumentNullException(nameof(collection));

                public int Count => _collection.Count;

                protected override IEnumerable<T> Items => _collection;
            }
        }

        [Test]
        public void UsesCollectionCountAtIterationTime()
        {
            var stack = new Stack<int>(Enumerable.Range(1, 3));
            var result = stack.CountDown(2, (_, cd) => cd);
            stack.Push(4);
            result.AssertSequenceEqual(null, null, 1, 0);
        }
    }
}
