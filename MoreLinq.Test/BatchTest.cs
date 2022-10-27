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

namespace MoreLinq.Test
{
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class BatchTest
    {
        [Test]
        public void BatchZeroSize()
        {
            AssertThrowsArgument.OutOfRangeException("size",() =>
                new object[0].Batch(0));
        }

        [Test]
        public void BatchNegativeSize()
        {
            AssertThrowsArgument.OutOfRangeException("size",() =>
                new object[0].Batch(-1));
        }

        [Test]
        public void BatchEvenlyDivisibleSequence()
        {
            var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Batch(3);

            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1, 2, 3);
            reader.Read().AssertSequenceEqual(4, 5, 6);
            reader.Read().AssertSequenceEqual(7, 8, 9);
            reader.ReadEnd();
        }

        [Test]
        public void BatchUnevenlyDivisibleSequence()
        {
            var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Batch(4);

            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1, 2, 3, 4);
            reader.Read().AssertSequenceEqual(5, 6, 7, 8);
            reader.Read().AssertSequenceEqual(9);
            reader.ReadEnd();
        }

        [Test]
        public void BatchSequenceTransformingResult()
        {
            var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Batch(4, batch => batch.Sum());
            result.AssertSequenceEqual(10, 26, 9);
        }

        [Test]
        public void BatchSequenceYieldsListsOfBatches()
        {
            var result = new[] { 1, 2, 3 }.Batch(2);

            using var reader = result.Read();
            Assert.That(reader.Read(), Is.InstanceOf(typeof(IList<int>)));
            Assert.That(reader.Read(), Is.InstanceOf(typeof(IList<int>)));
            reader.ReadEnd();
        }

        [Test]
        public void BatchSequencesAreIndependentInstances()
        {
            var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Batch(4);

            using var reader = result.Read();
            var first = reader.Read();
            var second = reader.Read();
            var third = reader.Read();
            reader.ReadEnd();

            first.AssertSequenceEqual(1, 2, 3, 4);
            second.AssertSequenceEqual(5, 6, 7, 8);
            third.AssertSequenceEqual(9);
        }

        [Test]
        public void BatchIsLazy()
        {
            new BreakingSequence<object>().Batch(1);
        }

        [TestCase(SourceKind.BreakingCollection  , 0)]
        [TestCase(SourceKind.BreakingList        , 0)]
        [TestCase(SourceKind.BreakingReadOnlyList, 0)]
        [TestCase(SourceKind.BreakingCollection  , 1)]
        [TestCase(SourceKind.BreakingList        , 1)]
        [TestCase(SourceKind.BreakingReadOnlyList, 1)]
        [TestCase(SourceKind.BreakingCollection  , 2)]
        [TestCase(SourceKind.BreakingList        , 2)]
        [TestCase(SourceKind.BreakingReadOnlyList, 2)]
        public void BatchCollectionSmallerThanSize(SourceKind kind, int oversize)
        {
            var xs = new[] { 1, 2, 3, 4, 5 };
            var result = xs.ToSourceKind(kind).Batch(xs.Length + oversize);
            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1, 2, 3, 4, 5);
            reader.ReadEnd();
        }

        [Test]
        public void BatchReadOnlyCollectionSmallerThanSize()
        {
            var collection = ReadOnlyCollection.From(1, 2, 3, 4, 5);
            var result = collection.Batch(collection.Count * 2);
            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1, 2, 3, 4, 5);
            reader.ReadEnd();
        }

        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        [TestCase(SourceKind.BreakingReadOnlyCollection)]
        [TestCase(SourceKind.BreakingCollection)]
        public void BatchEmptySource(SourceKind kind)
        {
            var batches = Enumerable.Empty<int>().ToSourceKind(kind).Batch(100);
            Assert.That(batches, Is.Empty);
        }
    }
}

#if NETCOREAPP3_1_OR_GREATER

namespace MoreLinq.Test
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using MoreLinq.Experimental;
    using NUnit.Framework;

    [TestFixture]
    public abstract class BatchPoolTest
    {
        protected abstract void AssertBatch<T>(IEnumerable<T> source, int size, Action<IListView<T>> asserter);

        void Batch<T>(IEnumerable<T> source, int size) => Batch(source, size, delegate { });

        void Batch<T>(IEnumerable<T> source, int size, Action<IListView<T>> asserter) =>
            AssertBatch(source, size, asserter + (bucket => Assert.That(bucket.MoveNext(), Is.False)));

        [Test]
        public void BatchZeroSize()
        {
            AssertThrowsArgument.OutOfRangeException("size", () => Batch(new object[0], 0));
        }

        [Test]
        public void BatchNegativeSize()
        {
            AssertThrowsArgument.OutOfRangeException("size", () => Batch(new object[0], -1));
        }

        static Action<IListView<T>> AssertNext<T>(params T[] items) => bucket =>
        {
            Assert.That(bucket.MoveNext(), Is.True);

            Assert.That(bucket.Count, Is.EqualTo(items.Length));

            foreach (var (i, item) in items.Index())
            {
                Assert.That(bucket.Contains(item));
                Assert.That(bucket.IndexOf(item), Is.EqualTo(i));
            }

            bucket.AssertSequenceEqual(items);
            bucket.AsSpan().ToArray().SequenceEqual(items);
        };

        [Test]
        public void BatchEvenlyDivisibleSequence()
        {
            using var input = TestingSequence.Of(1, 2, 3, 4, 5, 6, 7, 8, 9);
            Batch(input, 3, AssertNext(1, 2, 3) + AssertNext(4, 5, 6) + AssertNext(7, 8, 9));
        }

        [Test]
        public void BatchUnevenlyDivisibleSequence()
        {
            using var input = TestingSequence.Of(1, 2, 3, 4, 5, 6, 7, 8, 9);
            Batch(input, 4, AssertNext(1, 2, 3, 4) + AssertNext(5, 6, 7, 8) + AssertNext(9));
        }

        [Test]
        public void BatchDisposeMidway()
        {
            using var input = TestingSequence.Of(1, 2, 3, 4, 5, 6, 7, 8, 9);
            Batch(input, 4, AssertNext(1, 2, 3, 4) + (result => result.Dispose()));
        }


        [Test]
        public void BatchIsLazy()
        {
            new BreakingSequence<object>().Batch(1);
        }

        [TestCase(SourceKind.BreakingList        , 0)]
        [TestCase(SourceKind.BreakingReadOnlyList, 0)]
        [TestCase(SourceKind.BreakingList        , 1)]
        [TestCase(SourceKind.BreakingReadOnlyList, 1)]
        [TestCase(SourceKind.BreakingList        , 2)]
        [TestCase(SourceKind.BreakingReadOnlyList, 2)]
        public void BatchCollectionSmallerThanSize(SourceKind kind, int oversize)
        {
            var xs = new[] { 1, 2, 3, 4, 5 };
            Batch(xs.ToSourceKind(kind), xs.Length + oversize, AssertNext(1, 2, 3, 4, 5));
        }

        [Test]
        public void BatchReadOnlyCollectionSmallerThanSize()
        {
            var collection = ReadOnlyCollection.From(1, 2, 3, 4, 5);
            Batch(collection, collection.Count * 2, AssertNext(1, 2, 3, 4, 5));
        }

        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        [TestCase(SourceKind.BreakingReadOnlyCollection)]
        [TestCase(SourceKind.BreakingCollection)]
        public void BatchEmptySource(SourceKind kind)
        {
            Batch(Enumerable.Empty<int>().ToSourceKind(kind), 100, delegate { });
        }

        [Test]
        public void BatchResultUpdatesInPlaceOnEachMoveNext()
        {
            var input = TestingSequence.Of(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Batch(input, 3, result =>
            {
                const int scale = 2;

                var query =
                    from n in result
                    where n % 2 == 0
                    select n * scale;

                Assert.That(result.MoveNext(), Is.True);
                query.AssertSequenceEqual(2 * scale);

                Assert.That(result.MoveNext(), Is.True);
                query.AssertSequenceEqual(4 * scale, 6 * scale);

                Assert.That(result.MoveNext(), Is.True);
                query.AssertSequenceEqual(8 * scale);

                Assert.That(result.MoveNext(), Is.False);
            });
        }
    }

    public class BatchPooledArrayTest : BatchPoolTest
    {
        protected override void AssertBatch<T>(IEnumerable<T> source, int size, Action<IListView<T>> asserter)
        {
            var pool = new TestArrayPool<T>();
            asserter(source.Batch(size, pool));
            Assert.That(pool.HasRented, Is.False);
        }
    }

    public class BatchPooledMemoryTest : BatchPoolTest
    {
        protected override void AssertBatch<T>(IEnumerable<T> source, int size, Action<IListView<T>> asserter)
        {
            var pool = new TestMemoryPool<T>();
            asserter(source.Batch(size, pool));
            Assert.That(pool.HasRented, Is.False);
        }

        sealed class TestMemoryPool<T> : MemoryPool<T>
        {
            readonly TestArrayPool<T> _pool = new();

            public bool HasRented => _pool.HasRented;

            protected override void Dispose(bool disposing) { } // NOP

            public override IMemoryOwner<T> Rent(int minBufferSize = -1) =>
                minBufferSize >= 0
                ? new MemoryOwner(_pool, _pool.Rent(minBufferSize))
                : throw new NotSupportedException();

            public override int MaxBufferSize =>
                // https://github.com/dotnet/runtime/blob/v7.0.0-rc.2.22472.3/src/libraries/System.Memory/src/System/Buffers/ArrayMemoryPool.cs#L10
                2_147_483_591;

            sealed class MemoryOwner : IMemoryOwner<T>
            {
                ArrayPool<T> _pool;
                T[] _rental;

                public MemoryOwner(ArrayPool<T> pool, T[] rental) =>
                    (_pool, _rental) = (pool, rental);

                public Memory<T> Memory => _rental is { } rental ? new Memory<T>(rental)
                                         : throw new ObjectDisposedException(null);

                public void Dispose()
                {
                    if (_rental is { } array && _pool is { } pool)
                    {
                        _rental = null;
                        _pool = null;
                        pool.Return(array);
                    }
                }
            }
        }
    }

    /// <summary>
    /// An <see cref="ArrayPool{T}"/> implementation for testing purposes that holds only
    /// one array in the pool.
    /// </summary>

    sealed class TestArrayPool<T> : ArrayPool<T>
    {
        T[] _pooledArray;
        T[] _rentedArray;

        public bool HasRented => _rentedArray is not null;

        public override T[] Rent(int minimumLength)
        {
            if (_pooledArray is null && _rentedArray is null)
                _pooledArray = new T[minimumLength * 2];

            if (_pooledArray is null)
                throw new InvalidOperationException("The pool is exhausted.");

            (_pooledArray, _rentedArray) = (null, _pooledArray);

            return _rentedArray;
        }

        public override void Return(T[] array, bool clearArray = false)
        {
            if (_rentedArray is null)
                throw new InvalidOperationException("Cannot return when nothing has been rented from this pool.");

            if (array != _rentedArray)
                throw new InvalidOperationException("Cannot return what has not been rented from this pool.");

            _pooledArray = array;
            _rentedArray = null;
        }
    }
}

#endif // NETCOREAPP3_1_OR_GREATER
