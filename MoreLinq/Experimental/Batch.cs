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

#if !NO_MEMORY

namespace MoreLinq.Experimental
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;

    static partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Batches the source sequence into sized buckets using a memory pool
        /// to rent memory to back each bucket.
        /// </summary>
        /// <typeparam name="T">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of buckets.</param>
        /// <param name="pool">The memory pool used to rent memory for each bucket.</param>
        /// <returns>
        /// A <see cref="ICurrentList{T}"/> that can be used to enumerate
        /// equally sized buckets containing elements of the source collection.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results
        /// (buckets are streamed but their content buffered).</para>
        /// <para>
        /// <para>
        /// Each bucket is backed by rented memory that may be at least
        /// <paramref name="size"/> in length.
        /// </para>
        /// <para>
        /// When more than one bucket is streamed, all buckets except the last
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

        public static ICurrentList<T>
            Batch<T>(this IEnumerable<T> source, int size, MemoryPool<T> pool)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (pool == null) throw new ArgumentNullException(nameof(pool));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            ICurrentList<T> Cursor(IEnumerator<(IMemoryOwner<T>, int)> source) =>
                new CurrentBucketMemory<T>(source);

            IEnumerator<(IMemoryOwner<T>, int)> Empty() { yield break; }

            switch (source)
            {
                case ICollection<T> { Count: 0 }:
                {
                    return Cursor(Empty());
                }
                case IList<T> list when list.Count <= size:
                {
                    return Cursor(_()); IEnumerator<(IMemoryOwner<T>, int)> _()
                    {
                        var bucket = pool.Rent(list.Count);
                        for (var i = 0; i < list.Count; i++)
                            bucket.Memory.Span[i] = list[i];
                        yield return (bucket, list.Count);
                    }
                }
                case IReadOnlyCollection<T> { Count: 0 }:
                {
                    return Cursor(Empty());
                }
                case IReadOnlyList<T> list when list.Count <= size:
                {
                    return Cursor(_()); IEnumerator<(IMemoryOwner<T>, int)> _()
                    {
                        var bucket = pool.Rent(list.Count);
                        for (var i = 0; i < list.Count; i++)
                            bucket.Memory.Span[i] = list[i];
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

                IEnumerator<(IMemoryOwner<T>, int)> Batch(int size)
                {
                    IMemoryOwner<T>? bucket = null;
                    var count = 0;

                    foreach (var item in source)
                    {
                        bucket ??= pool.Rent(size);
                        bucket.Memory.Span[count++] = item;

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
        }

        sealed class CurrentBucketMemory<T> : CurrentList<T>
        {
            bool _started;
            IEnumerator<(IMemoryOwner<T> Bucket, int Length)>? _enumerator;

            public CurrentBucketMemory(IEnumerator<(IMemoryOwner<T>, int)> enumerator) =>
                _enumerator = enumerator;

            public override Span<T> CurrentItemsSpan => Memory.Span;

            public override bool UpdateWithNext()
            {
                if (_enumerator is { } enumerator)
                {
                    if (_started)
                        enumerator.Current.Bucket.Dispose();
                    else
                        _started = true;

                    if (!enumerator.MoveNext())
                    {
                        enumerator.Dispose();
                        _enumerator = null;
                        return false;
                    }

                    return true;
                }

                return false;
            }

            Memory<T> Memory => _started && _enumerator?.Current.Bucket is { Memory: var v } ? v : throw new InvalidOperationException();

            public override int Count => _started && _enumerator?.Current.Length is { } v ? v : throw new InvalidOperationException();

            public override T this[int index]
            {
                get => index >= 0 && index < Count ? Memory.Span[index] : throw new IndexOutOfRangeException();
                set => throw new NotSupportedException();
            }

            public override void Dispose()
            {
                if (_enumerator is { } enumerator)
                {
                    _enumerator = null;
                    if (_started)
                        enumerator.Current.Bucket.Dispose();
                    enumerator.Dispose();
                }
            }

            public override IEnumerator<T> GetEnumerator()
            {
                for (var i = 0; i < Count; i++)
                    yield return this[i];
            }
        }

        /// <summary>
        /// Batches the source sequence into sized buckets using an array pool
        /// to rent arrays to back each bucket and returns a sequence of
        /// elements projected from each bucket.
        /// </summary>
        /// <typeparam name="TSource">
        /// Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TQuery">
        /// Type of elements in the sequence returned by <paramref name="querySelector"/>.</typeparam>
        /// <typeparam name="TResult">
        /// Type of elements of the resulting sequence.
        /// </typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of buckets.</param>
        /// <param name="pool">The pool used to rent the array for each bucket.</param>
        /// <param name="querySelector">A function that projects a query over
        /// all buckets.</param>
        /// <param name="resultSelector">A function that projects a result from
        /// the input sequence produced over a bucket.</param>
        /// <returns>
        /// A sequence whose elements are projected from each bucket (returned by
        /// <paramref name="resultSelector"/>).
        /// </returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results
        /// (buckets are streamed but their content buffered).</para>
        /// <para>
        /// <para>
        /// Each bucket is backed by a rented array that may be at least
        /// <paramref name="size"/> in length.
        /// </para>
        /// <para>
        /// When more than one bucket is streamed, all buckets except the last
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
            Batch<TSource, TQuery, TResult>(
                this IEnumerable<TSource> source, int size, ArrayPool<TSource> pool,
                Func<ICurrentList<TSource>, IEnumerable<TQuery>> querySelector,
                Func<IEnumerable<TQuery>, TResult> resultSelector)
        {
            using var current = source.Batch(size, pool);
            if (current.UpdateWithNext())
            {
                var query = querySelector(current);
                do
                {
                    var result = resultSelector(query);
                    if (result is IEnumerable<TSource> && ReferenceEquals(result, query))
                        throw new InvalidOperationException();
                    yield return result;
                }
                while (current.UpdateWithNext());
            }
        }

        /// <summary>
        /// Batches the source sequence into sized buckets using a array pool
        /// to rent an array to back each bucket.
        /// </summary>
        /// <typeparam name="T">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of buckets.</param>
        /// <param name="pool">The pool used to rent the array for each bucket.</param>
        /// <returns>
        /// A <see cref="ICurrentList{T}"/> that can be used to enumerate
        /// equally sized buckets containing elements of the source collection.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results
        /// (buckets are streamed but their content buffered).</para>
        /// <para>
        /// <para>
        /// Each bucket is backed by a rented array that may be at least
        /// <paramref name="size"/> in length.
        /// </para>
        /// <para>
        /// When more than one bucket is streamed, all buckets except the last
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

        public static ICurrentList<T>
            Batch<T>(this IEnumerable<T> source, int size, ArrayPool<T> pool)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (pool == null) throw new ArgumentNullException(nameof(pool));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            ICurrentList<T> Cursor(IEnumerator<(T[], int)> source) =>
                new CurrentBucketArray<T>(source, pool);

            IEnumerator<(T[], int)> Empty() { yield break; }

            switch (source)
            {
                case ICollection<T> { Count: 0 }:
                {
                    return Cursor(Empty());
                }
                case ICollection<T> collection when collection.Count <= size:
                {
                    var bucket = pool.Rent(collection.Count);
                    collection.CopyTo(bucket, 0);
                    return Cursor(MoreEnumerable.Return((bucket, collection.Count)).GetEnumerator());
                }
                case IReadOnlyCollection<T> { Count: 0 }:
                {
                    return Cursor(Empty());
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

        sealed class CurrentBucketArray<T> : CurrentList<T>
        {
            bool _started;
            IEnumerator<(T[] Bucket, int Length)>? _enumerator;
            ArrayPool<T>? _pool;

            public CurrentBucketArray(IEnumerator<(T[], int)> enumerator, ArrayPool<T> pool) =>
                (_enumerator, _pool) = (enumerator, pool);

            public override Span<T> CurrentItemsSpan => Array.AsSpan();

            public override bool UpdateWithNext()
            {
                if (_enumerator is { } enumerator)
                {
                    Debug.Assert(_pool is not null);
                    if (_started)
                        _pool.Return(enumerator.Current.Bucket);
                    else
                        _started = true;

                    if (!enumerator.MoveNext())
                    {
                        enumerator.Dispose();
                        _enumerator = null;
                        _pool = null;
                        return false;
                    }

                    return true;
                }

                return false;
            }

            T[] Array => _started && _enumerator?.Current.Bucket is { } v ? v : throw new InvalidOperationException();

            public override int Count => _started && _enumerator?.Current.Length is { } v ? v : throw new InvalidOperationException();

            public override T this[int index]
            {
                get => index >= 0 && index < Count ? Array[index] : throw new IndexOutOfRangeException();
                set => throw new NotSupportedException();

            }

            public override void Dispose()
            {
                if (_enumerator is { } enumerator)
                {
                    Debug.Assert(_pool is not null);
                    _pool.Return(enumerator.Current.Bucket);
                    enumerator.Dispose();
                    _enumerator = null;
                    _pool = null;
                }
            }
        }
    }
}

#endif // !NO_MEMORY
