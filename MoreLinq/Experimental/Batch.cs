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
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Represents a bucket returned by one of the <c>Batch</c> overloads of
    /// <see cref="ExperimentalEnumerable"/>.
    /// </summary>
    /// <typeparam name="T">Type of elements in the bucket</typeparam>

    public interface IBatchBucket<T> : IDisposable, IList<T>
    {
        /// <summary>
        /// Returns a new span over the bucket elements.
        /// </summary>

        Span<T> AsSpan();

        /// <summary>
        /// Update this instance with the next set of elements from the source.
        /// </summary>
        /// <returns>
        /// A Boolean that is <c>true</c> if this instance was updated with
        /// new elements; otherwise <c>false</c> to indicate that the end of
        /// the bucket source has been reached.
        /// </returns>

        bool MoveNext();

        int IList<T>.IndexOf(T item)
        {
            var comparer = EqualityComparer<T>.Default;

            for (var i = 0; i < Count; i++)
            {
                if (comparer.Equals(this[i], item))
                    return i;
            }

            return -1;
        }

        bool ICollection<T>.Contains(T item) => IndexOf(item) >= 0;

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex), arrayIndex, null);
            if (arrayIndex + Count > array.Length) throw new ArgumentException(null, nameof(arrayIndex));

            for (int i = 0, j = arrayIndex; i < Count; i++, j++)
                array[j] = this[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
        void IList<T>.RemoveAt(int index) => throw new NotSupportedException();
        void ICollection<T>.Add(T item) => throw new NotSupportedException();
        void ICollection<T>.Clear() => throw new NotSupportedException();
        bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
        bool ICollection<T>.IsReadOnly => true;
    }

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
        /// A <see cref="IBatchBucket{T}"/> that can be used to enumerate
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

        public static IBatchBucket<T>
            Batch<T>(this IEnumerable<T> source, int size, MemoryPool<T> pool)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (pool == null) throw new ArgumentNullException(nameof(pool));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            IBatchBucket<T> Cursor(IEnumerator<(IMemoryOwner<T>, int)> source) =>
                new RentedMemoryBatchBucket<T>(source);

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

        sealed class RentedMemoryBatchBucket<T> : IBatchBucket<T>
        {
            bool _started;
            IEnumerator<(IMemoryOwner<T> Bucket, int Length)>? _enumerator;

            public RentedMemoryBatchBucket(IEnumerator<(IMemoryOwner<T>, int)> enumerator) =>
                _enumerator = enumerator;

            public Span<T> AsSpan() => Memory.Span;

            public bool MoveNext()
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

            public int Count => _started && _enumerator?.Current.Length is { } v ? v : throw new InvalidOperationException();

            public T this[int index]
            {
                get => index >= 0 && index < Count ? Memory.Span[index] : throw new IndexOutOfRangeException();
                set => throw new NotSupportedException();
            }

            public void Dispose()
            {
                _enumerator?.Dispose();
                _enumerator = null;
            }

            public IEnumerator<T> GetEnumerator()
            {
                for (var i = 0; i < Count; i++)
                    yield return this[i];
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
        /// A <see cref="IBatchBucket{T}"/> that can be used to enumerate
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

        public static IBatchBucket<T>
            Batch<T>(this IEnumerable<T> source, int size, ArrayPool<T> pool)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (pool == null) throw new ArgumentNullException(nameof(pool));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            IBatchBucket<T> Cursor(IEnumerator<(T[], int)> source) =>
                new RentedArrayBatchBucket<T>(source, pool);

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

        sealed class RentedArrayBatchBucket<T> : IBatchBucket<T>
        {
            bool _started;
            IEnumerator<(T[] Bucket, int Length)>? _enumerator;
            ArrayPool<T>? _pool;

            public RentedArrayBatchBucket(IEnumerator<(T[], int)> enumerator, ArrayPool<T> pool) =>
                (_enumerator, _pool) = (enumerator, pool);

            public Span<T> AsSpan() => Array.AsSpan();

            public bool MoveNext()
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

            public int Count => _started && _enumerator?.Current.Length is { } v ? v : throw new InvalidOperationException();

            public T this[int index]
            {
                get => index >= 0 && index < Count ? Array[index] : throw new IndexOutOfRangeException();
                set => throw new NotSupportedException();

            }

            public void Dispose()
            {
                _enumerator?.Dispose();
                _enumerator = null;
                _pool = null;
            }

            public IEnumerator<T> GetEnumerator() => Array.Take(Count).GetEnumerator();
        }
    }
}

#endif // !NO_MEMORY
