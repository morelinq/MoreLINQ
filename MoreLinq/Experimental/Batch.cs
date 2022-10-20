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
    using System.Linq;

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
        /// <returns>A sequence of equally sized buckets containing elements of the source collection.</returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results
        /// (buckets are streamed but their content buffered).</para>
        /// <para>
        /// <para>
        /// Each bucket is backed by rented memory that may be at least
        /// <paramref name="size"/> in length. The second element paired with
        /// each bucket is the actual length of the bucket that is valid to use.
        /// The rented memory should be disposed to return it to the pool given
        /// in the <paramref name="pool"/> argument. If it is returned as each
        /// bucket is retrieved during iteration then there is a good chance that
        /// the same memory will be reused for subsequent buckets. This can save
        /// allocations for very large buckets.
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

        public static IEnumerable<(IMemoryOwner<T> Bucket, int Length)>
            Batch<T>(this IEnumerable<T> source, int size, MemoryPool<T> pool)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (pool == null) throw new ArgumentNullException(nameof(pool));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            switch (source)
            {
                case ICollection<T> { Count: 0 }:
                {
                    return Enumerable.Empty<(IMemoryOwner<T>, int)>();
                }
                case ICollection<T> collection when collection.Count <= size:
                {
                    return Batch(collection.Count);
                }
                case IReadOnlyCollection<T> { Count: 0 }:
                {
                    return Enumerable.Empty<(IMemoryOwner<T>, int)>();
                }
                case IReadOnlyList<T> list when list.Count <= size:
                {
                    return _(); IEnumerable<(IMemoryOwner<T>, int)> _()
                    {
                        var bucket = pool.Rent(list.Count);
                        for (var i = 0; i < list.Count; i++)
                            bucket.Memory.Span[i] = list[i];
                        yield return (bucket, list.Count);
                    }
                }
                case IReadOnlyCollection<T> collection when collection.Count <= size:
                {
                    return Batch(collection.Count);
                }
                default:
                {
                    return Batch(size);
                }

                IEnumerable<(IMemoryOwner<T>, int)> Batch(int size)
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

        /// <summary>
        /// Batches the source sequence into sized buckets using a array pool
        /// to rent an array to back each bucket.
        /// </summary>
        /// <typeparam name="T">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of buckets.</param>
        /// <param name="pool">The pool used to rent the array for each bucket.</param>
        /// <returns>A sequence of equally sized buckets containing elements of the source collection.</returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results
        /// (buckets are streamed but their content buffered).</para>
        /// <para>
        /// <para>
        /// Each bucket is backed by a rented array that may be at least
        /// <paramref name="size"/> in length. The second element paired with
        /// each bucket is the actual length of the bucket that is valid to use.
        /// The rented array should be returned to the pool sent as the
        /// <paramref name="pool"/> argument. If it is returned as each bucket
        /// is retrieved during iteration then there is a good chance that the
        /// same array allocation will be reused for subsequent buckets. This
        /// can save allocations for very large buckets.
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

        public static IEnumerable<(T[] Bucket, int Length)>
            Batch<T>(this IEnumerable<T> source, int size, ArrayPool<T> pool)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (pool == null) throw new ArgumentNullException(nameof(pool));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            switch (source)
            {
                case ICollection<T> { Count: 0 }:
                {
                    return Enumerable.Empty<(T[], int)>();
                }
                case ICollection<T> collection when collection.Count <= size:
                {
                    var bucket = pool.Rent(collection.Count);
                    collection.CopyTo(bucket, 0);
                    return MoreEnumerable.Return((bucket, collection.Count));
                }
                case IReadOnlyCollection<T> { Count: 0 }:
                {
                    return Enumerable.Empty<(T[], int)>();
                }
                case IReadOnlyList<T> list when list.Count <= size:
                {
                    return _(); IEnumerable<(T[], int)> _()
                    {
                        var bucket = pool.Rent(list.Count);
                        for (var i = 0; i < list.Count; i++)
                            bucket[i] = list[i];
                        yield return (bucket, list.Count);
                    }
                }
                case IReadOnlyCollection<T> collection when collection.Count <= size:
                {
                    return Batch(collection.Count);
                }
                default:
                {
                    return Batch(size);
                }
            }

            IEnumerable<(T[], int)> Batch(int size)
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
    }
}

#endif // !NO_MEMORY
