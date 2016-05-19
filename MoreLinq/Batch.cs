#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2009 Atif Aziz. All rights reserved.
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

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Batches the source sequence into sized buckets.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of buckets.</param>
        /// <returns>A sequence of equally sized buckets containing elements of the source collection.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results (buckets and bucket content). 
        /// It is also identical to <see cref="Partition{TSource}(System.Collections.Generic.IEnumerable{TSource},int)"/>.
        /// </remarks>

        public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int size)
        {
            return Batch(source, size, x => x);
        }

        /// <summary>
        /// Batches the source sequence into sized buckets and applies a projection to each bucket.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of result returned by <paramref name="resultSelector"/>.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of buckets.</param>
        /// <param name="resultSelector">The projection to apply to each bucket.</param>
        /// <returns>A sequence of projections on equally sized buckets containing elements of the source collection.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results (buckets and bucket content).
        /// It is also identical to <see cref="Partition{TSource}(System.Collections.Generic.IEnumerable{TSource},int)"/>.
        /// </remarks>
        
        public static IEnumerable<TResult> Batch<TSource, TResult>(this IEnumerable<TSource> source, int size,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (size <= 0) throw new ArgumentOutOfRangeException("size");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");
            return BatchImpl(source, size, resultSelector);
        }

        /// <summary>
        /// Batches the sequence based on the first and last match predicates.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="firstMatchPredicate">Predicate to identify the first element of the result sequence</param>
        /// <param name="lastMatchPredicate">Predicate to identify the last element of the result sequence</param>
        /// <returns>A sequence where the first element satisfies the firstMatchPredicate and the last element matches the lastMatchPredicate. Elements in between should not satisfy the predicates.</returns>
        public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(this IEnumerable<TSource> source,
                                                                       Func<TSource, bool> firstMatchPredicate,
                                                                       Func<TSource, bool> lastMatchPredicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (firstMatchPredicate == null) throw new ArgumentNullException("firstMatchPredicate");
            if (lastMatchPredicate == null) throw new ArgumentNullException("lastMatchPredicate");

            return BatchByPredicatesImpl(source, firstMatchPredicate, lastMatchPredicate);
        }

        private static IEnumerable<TResult> BatchImpl<TSource, TResult>(this IEnumerable<TSource> source, int size,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            Debug.Assert(source != null);
            Debug.Assert(size > 0);
            Debug.Assert(resultSelector != null);

            TSource[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                {
                    bucket = new TSource[size];
                }

                bucket[count++] = item;

                // The bucket is fully buffered before it's yielded
                if (count != size)
                {
                    continue;
                }

                // Select is necessary so bucket contents are streamed too
                yield return resultSelector(bucket.Select(x => x));
               
                bucket = null;
                count = 0;
            }

            // Return the last bucket with all remaining elements
            if (bucket != null && count > 0)
            {
                yield return resultSelector(bucket.Take(count));
            }
        }

        private static IEnumerable<IEnumerable<TSource>> BatchByPredicatesImpl<TSource>(this IEnumerable<TSource> source,
                                                                       Func<TSource, bool> firstMatchPredicate,
                                                                       Func<TSource, bool> lastMatchPredicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(firstMatchPredicate != null);
            Debug.Assert(lastMatchPredicate != null);

            var currentBatch = new List<TSource>();            
            foreach (var item in source)
            {
                var firstMatched = firstMatchPredicate(item);
                if (firstMatched)
                {
                    currentBatch.Clear();
                    currentBatch.Add(item);
                    continue;
                }

                var lastMatched = lastMatchPredicate(item);
                if (lastMatched)
                {
                    currentBatch.Add(item);
                    yield return currentBatch;

                    currentBatch = new List<TSource>();
                    continue;
                }

                currentBatch.Add(item);
            }
        }
    }
}
