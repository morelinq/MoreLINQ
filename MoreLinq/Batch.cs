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
        /// </remarks>

        public static IEnumerable<TResult> Batch<TSource, TResult>(this IEnumerable<TSource> source, int size,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _().TakeWhile(x => x.Any()).Select(resultSelector); 
            
            IEnumerable<IEnumerable<TSource>> _()
            {
                var values = new List<TSource>();
                var group = 1;
                var disposed = false;
                var e = source.GetEnumerator();

                try
                {
                    while (!disposed)
                    {
                        yield return GetBatch();
                        group++;
                    }
                }
                finally
                {
                    if (!disposed)
                        e.Dispose();
                }

                IEnumerable<TSource> GetBatch()
                {
                    var min = (group - 1) * size + 1;
                    var max = group * size;
                    var hasValue = false;

                    while (values.Count < min && e.MoveNext())
                    {
                        values.Add(e.Current);
                    }

                    for (var i = min; i <= max; i++)
                    {
                        if (i <= values.Count)
                        {
                            hasValue = true;
                        }
                        else if (hasValue = (!disposed && e.MoveNext()))
                        {
                            values.Add(e.Current);
                        }
                        else
                        {
                            if (!disposed)
                            {
                                e.Dispose(); 
                                disposed = true;
                            }
                        }

                        if (hasValue)
                            yield return values[i - 1];
                        else
                            yield break;
                    }
                }
            }
        }
    }
}
