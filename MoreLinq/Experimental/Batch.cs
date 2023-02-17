#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2022 Atif Aziz. All rights reserved.
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

#if !NO_BUFFERS

namespace MoreLinq.Experimental
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    static partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Batches the source sequence into sized buckets using an array pool
        /// to rent arrays to back each bucket and returns a sequence of
        /// elements projected from each bucket.
        /// </summary>
        /// <typeparam name="TSource">
        /// Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">
        /// Type of elements of the resulting sequence.
        /// </typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of buckets.</param>
        /// <param name="pool">The pool used to rent the array for each bucket.</param>
        /// <param name="resultSelector">A function that projects a result from
        /// the current bucket.</param>
        /// <returns>
        /// A sequence whose elements are projected from each bucket (returned by
        /// <paramref name="resultSelector"/>).
        /// </returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results
        /// (however, each bucket provided to <paramref name="resultSelector"/> is
        /// buffered).</para>
        /// <para>
        /// <para>
        /// Each bucket is backed by a rented array that may be at least
        /// <paramref name="size"/> in length.
        /// </para>
        /// <para>
        /// When more than one bucket is produced, all buckets except the last
        /// is guaranteed to have <paramref name="size"/> elements. The last
        /// bucket may be smaller depending on the remaining elements in the
        /// <paramref name="source"/> sequence.</para>
        /// Each bucket is pre-allocated to <paramref name="size"/> elements.
        /// If <paramref name="size"/> is set to a very large value, e.g.
        /// <see cref="int.MaxValue"/> to effectively disable batching by just
        /// hoping for a single bucket, then it can lead to memory exhaustion
        /// (<see cref="OutOfMemoryException"/>).
        /// </para>
        /// </remarks>

        public static IEnumerable<TResult>
            Batch<TSource, TResult>(this IEnumerable<TSource> source, int size,
                                    ArrayPool<TSource> pool,
                                    Func<ICurrentBuffer<TSource>, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (pool == null) throw new ArgumentNullException(nameof(pool));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.Batch(size, pool, current => current,
                                current => resultSelector((ICurrentBuffer<TSource>)current));
        }

        /// <summary>
        /// Batches the source sequence into sized buckets using an array pool
        /// to rent arrays to back each bucket and returns a sequence of
        /// elements projected from each bucket.
        /// </summary>
        /// <typeparam name="TSource">
        /// Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TBucket">
        /// Type of elements in the sequence returned by <paramref name="bucketProjectionSelector"/>.</typeparam>
        /// <typeparam name="TResult">
        /// Type of elements of the resulting sequence.
        /// </typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of buckets.</param>
        /// <param name="pool">The pool used to rent the array for each bucket.</param>
        /// <param name="bucketProjectionSelector">A function that returns a
        /// sequence projection to use for each bucket. It is called initially
        /// before iterating over <paramref name="source"/>, but the resulting
        /// projection is evaluated for each bucket. This has the same effect as
        /// calling <paramref name="bucketProjectionSelector"/> for each bucket,
        /// but allows initialization of the transformation to happen only once.
        /// </param>
        /// <param name="resultSelector">A function that projects a result from
        /// the input sequence produced over a bucket.</param>
        /// <returns>
        /// A sequence whose elements are projected from each bucket (returned by
        /// <paramref name="resultSelector"/>).
        /// </returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results
        /// (however, each bucket is buffered).</para>
        /// <para>
        /// <para>
        /// Each bucket is backed by a rented array that may be at least
        /// <paramref name="size"/> in length.
        /// </para>
        /// <para>
        /// When more than one bucket is produced, all buckets except the last
        /// is guaranteed to have <paramref name="size"/> elements. The last
        /// bucket may be smaller depending on the remaining elements in the
        /// <paramref name="source"/> sequence.</para>
        /// Each bucket is pre-allocated to <paramref name="size"/> elements.
        /// If <paramref name="size"/> is set to a very large value, e.g.
        /// <see cref="int.MaxValue"/> to effectively disable batching by just
        /// hoping for a single bucket, then it can lead to memory exhaustion
        /// (<see cref="OutOfMemoryException"/>).
        /// </para>
        /// </remarks>

        public static IEnumerable<TResult>
            Batch<TSource, TBucket, TResult>(
                this IEnumerable<TSource> source, int size, ArrayPool<TSource> pool,
                Func<ICurrentBuffer<TSource>, IEnumerable<TBucket>> bucketProjectionSelector,
                Func<IEnumerable<TBucket>, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (pool == null) throw new ArgumentNullException(nameof(pool));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
            if (bucketProjectionSelector == null) throw new ArgumentNullException(nameof(bucketProjectionSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var batch = source.Batch(size, pool);
                var bucket = bucketProjectionSelector(batch.CurrentBuffer);
                while (batch.UpdateWithNext())
                    yield return resultSelector(bucket);
            }
        }

        static ICurrentBufferProvider<T>
            Batch<T>(this IEnumerable<T> source, int size, ArrayPool<T> pool)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (pool == null) throw new ArgumentNullException(nameof(pool));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            ICurrentBufferProvider<T> Cursor(IEnumerator<(T[], int)> source) =>
                new CurrentPoolArrayProvider<T>(source, pool);

            switch (source)
            {
                case ICollection<T> { Count: 0 }:
                {
                    return Cursor(Enumerable.Empty<(T[], int)>().GetEnumerator());
                }
                case ICollection<T> collection when collection.Count <= size:
                {
                    var bucket = pool.Rent(collection.Count);
                    collection.CopyTo(bucket, 0);
                    return Cursor(MoreEnumerable.Return((bucket, collection.Count)).GetEnumerator());
                }
                case IReadOnlyCollection<T> { Count: 0 }:
                {
                    return Cursor(Enumerable.Empty<(T[], int)>().GetEnumerator());
                }
                case IReadOnlyList<T> list when list.Count <= size:
                {
                    return Cursor(_()); IEnumerator<(T[], int)> _()
                    {
                        var bucket = pool.Rent(list.Count);
                        for (var i = 0; i < list.Count; i++)
                            bucket[i] = list[i];
                        yield return (bucket, list.Count);
                    }
                }
                case IReadOnlyCollection<T> collection when collection.Count <= size:
                {
                    return Cursor(Batch(collection.Count));
                }
                default:
                {
                    return Cursor(Batch(size));
                }
            }

            IEnumerator<(T[], int)> Batch(int size)
            {
                T[]? bucket = null;
                var count = 0;

                foreach (var item in source)
                {
                    bucket ??= pool.Rent(size);
                    bucket[count++] = item;

                    // The bucket is fully buffered before it's yielded
                    if (count != size)
                        continue;

                    yield return (bucket, size);

                    bucket = null;
                    count = 0;
                }

                // Return the last bucket with all remaining elements
                if (bucket is { } someBucket && count > 0)
                    yield return (someBucket, count);
            }
        }

        sealed class CurrentPoolArrayProvider<T> : CurrentBuffer<T>, ICurrentBufferProvider<T>
        {
            bool _rented;
            T[] _array = Array.Empty<T>();
            int _count;
            IEnumerator<(T[], int)>? _rental;
            ArrayPool<T>? _pool;

            public CurrentPoolArrayProvider(IEnumerator<(T[], int)> rental, ArrayPool<T> pool) =>
                (_rental, _pool) = (rental, pool);

            ICurrentBuffer<T> ICurrentBufferProvider<T>.CurrentBuffer => this;

            public bool UpdateWithNext()
            {
                if (_rental is { Current: var (array, _) } rental)
                {
                    Debug.Assert(_pool is not null);
                    if (_rented)
                    {
                        _pool.Return(array);
                        _rented = false;
                    }

                    if (!rental.MoveNext())
                    {
                        Dispose();
                        return false;
                    }

                    _rented = true;
                    (_array, _count) = rental.Current;
                    return true;
                }

                return false;
            }

            public override int Count => _count;

            public override T this[int index]
            {
                get => index >= 0 && index < Count ? _array[index] : throw new ArgumentOutOfRangeException(nameof(index));
                set => throw new NotSupportedException();
            }

            public void Dispose()
            {
                if (_rental is { Current: var (array, _) } enumerator)
                {
                    Debug.Assert(_pool is not null);
                    if (_rented)
                        _pool.Return(array);
                    enumerator.Dispose();
                    _array = Array.Empty<T>();
                    _count = 0;
                    _rental = null;
                    _pool = null;
                }
            }
        }
    }
}

#endif // !NO_BUFFERS
