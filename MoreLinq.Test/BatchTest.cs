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
        [TestCase(0)]
        [TestCase(-1)]
        public void BatchBadSize(int size)
        {
            Assert.That(() => new object[0].Batch(size),
                        Throws.ArgumentOutOfRangeException("size"));
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
            _ = new BreakingSequence<object>().Batch(1);
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

        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        [TestCase(SourceKind.BreakingCollection)]
        public void BatchUsesCollectionCountAtIterationTime(SourceKind kind)
        {
            var list = new List<int> { 1, 2 };
            var result = list.AsSourceKind(kind).Batch(3);

            list.Add(3);
            result.AssertSequenceEqual(new[] { 1, 2, 3 });

            list.Add(4);
            // should fail trying to enumerate because count is now greater than the batch size
            Assert.That(result.Consume, Throws.TypeOf<BreakException>());
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
    public class BatchPoolTest
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void BatchBadSize(int size)
        {
            Assert.That(() => new object[0].Batch(size, ArrayPool<object>.Shared,
                                                  BreakingFunc.Of<ICurrentBuffer<object>, IEnumerable<object>>(),
                                                  BreakingFunc.Of<IEnumerable<object>, object>()),
                        Throws.ArgumentOutOfRangeException("size"));
        }

        [Test]
        public void BatchEvenlyDivisibleSequence()
        {
            using var input = TestingSequence.Of(1, 2, 3, 4, 5, 6, 7, 8, 9);
            using var pool = new TestArrayPool<int>();

            var result = input.Batch(3, pool, Enumerable.ToArray);

            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1, 2, 3);
            reader.Read().AssertSequenceEqual(4, 5, 6);
            reader.Read().AssertSequenceEqual(7, 8, 9);
            reader.ReadEnd();
        }

        [Test]
        public void BatchUnevenlyDivisibleSequence()
        {
            using var input = TestingSequence.Of(1, 2, 3, 4, 5, 6, 7, 8, 9);
            using var pool = new TestArrayPool<int>();

            var result = input.Batch(4, pool, Enumerable.ToArray);

            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1, 2, 3, 4);
            reader.Read().AssertSequenceEqual(5, 6, 7, 8);
            reader.Read().AssertSequenceEqual(9);
            reader.ReadEnd();
        }

        [Test]
        public void BatchIsLazy()
        {
            var input = new BreakingSequence<object>();
            _ = input.Batch(1, ArrayPool<object>.Shared,
                            BreakingFunc.Of<ICurrentBuffer<object>, IEnumerable<object>>(),
                            BreakingFunc.Of<IEnumerable<object>, object>());
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
            using var pool = new TestArrayPool<int>();

            var result = xs.ToSourceKind(kind)
                           .Batch(xs.Length + oversize, pool, Enumerable.ToArray);

            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1, 2, 3, 4, 5);
            reader.ReadEnd();
        }

        [Test]
        public void BatchReadOnlyCollectionSmallerThanSize()
        {
            var collection = ReadOnlyCollection.From(1, 2, 3, 4, 5);
            using var pool = new TestArrayPool<int>();

            var result = collection.Batch(collection.Count * 2, pool,
                                          Enumerable.ToArray);

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
            using var pool = new TestArrayPool<int>();

            var result = Enumerable.Empty<int>()
                                   .ToSourceKind(kind)
                                   .Batch(100, pool, Enumerable.ToArray);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void BatchFilterBucket()
        {
            const int scale = 2;
            using var input = TestingSequence.Of(1, 2, 3, 4, 5, 6, 7, 8, 9);
            using var pool = new TestArrayPool<int>();

            var result = input.Batch(3, pool,
                                     current => from n in current
                                                where n % 2 == 0
                                                select n * scale,
                                     Enumerable.ToArray);

            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(2 * scale);
            reader.Read().AssertSequenceEqual(4 * scale, 6 * scale);
            reader.Read().AssertSequenceEqual(8 * scale);
            reader.ReadEnd();
        }

        [Test]
        public void BatchSumBucket()
        {
            using var input = TestingSequence.Of(1, 2, 3, 4, 5, 6, 7, 8, 9);
            using var pool = new TestArrayPool<int>();

            var result = input.Batch(3, pool, Enumerable.Sum);

            using var reader = result.Read();
            Assert.That(reader.Read(), Is.EqualTo(1 + 2 + 3));
            Assert.That(reader.Read(), Is.EqualTo(4 + 5 + 6));
            Assert.That(reader.Read(), Is.EqualTo(7 + 8 + 9));
            reader.ReadEnd();
        }

        /// <remarks>
        /// This test does not exercise the intended usage!
        /// </remarks>

        [Test]
        public void BatchUpdatesCurrentListInPlace()
        {
            using var input = TestingSequence.Of(1, 2, 3, 4, 5, 6, 7, 8, 9);
            using var pool = new TestArrayPool<int>();

            var result = input.Batch(4, pool, current => current, current => (ICurrentBuffer<int>)current);

            using var reader = result.Read();
            var current = reader.Read();
            current.AssertSequenceEqual(1, 2, 3, 4);
            _ = reader.Read();
            current.AssertSequenceEqual(5, 6, 7, 8);
            _ = reader.Read();
            current.AssertSequenceEqual(9);

            reader.ReadEnd();

            Assert.That(current, Is.Empty);
        }

        [Test]
        public void BatchCurrentListIndexerWithBadIndexThrowsArgumentOutOfRangeException()
        {
            using var input = TestingSequence.Of(1, 2, 3, 4, 5, 6, 7, 8, 9);
            using var pool = new TestArrayPool<int>();

            var result = input.Batch(4, pool, current => current, current => (ICurrentBuffer<int>)current);

            using var reader = result.Read();
            var current = reader.Read();

            Assert.That(() => current[100], Throws.ArgumentOutOfRangeException("index"));
        }

        [Test]
        public void BatchCallsBucketSelectorBeforeIteratingSource()
        {
            var iterations = 0;
            IEnumerable<int> Source()
            {
                iterations++;
                yield break;
            }

            var input = Source();
            using var pool = new TestArrayPool<int>();
            var initIterations = -1;

            var result = input.Batch(4, pool,
                                     current =>
                                     {
                                         initIterations = iterations;
                                         return current;
                                     },
                                     _ => 0);

            using var enumerator = result.GetEnumerator();
            Assert.That(enumerator.MoveNext(), Is.False);
            Assert.That(initIterations, Is.Zero);
        }

        [Test]
        public void BatchBucketSelectorCurrentList()
        {
            using var input = TestingSequence.Of(1, 2, 3, 4, 5, 6, 7, 8, 9);
            using var pool = new TestArrayPool<int>();
            int[]? bucketSelectorItems = null;

            var result = input.Batch(4, pool, current => bucketSelectorItems = current.ToArray(), _ => 0);

            using var reader = result.Read();
            _ = reader.Read();
            Assert.That(bucketSelectorItems, Is.Not.Null);
            Assert.That(bucketSelectorItems, Is.Empty);
        }

        /// <summary>
        /// An <see cref="ArrayPool{T}"/> implementation for testing purposes that holds only
        /// one array in the pool and ensures that it is returned when the pool is disposed.
        /// </summary>

        sealed class TestArrayPool<T> : ArrayPool<T>, IDisposable
        {
            T[]? _pooledArray;
            T[]? _rentedArray;

            public override T[] Rent(int minimumLength)
            {
                if (_pooledArray is null && _rentedArray is null)
                    _pooledArray = new T[minimumLength * 2];

                (_pooledArray, _rentedArray) =
                    (null, _pooledArray ?? throw new InvalidOperationException("The pool is exhausted."));

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

            public void Dispose() =>
                Assert.That(_rentedArray, Is.Null);
        }
    }
}

#endif // NETCOREAPP3_1_OR_GREATER
