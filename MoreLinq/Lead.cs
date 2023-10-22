#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2010 Leopold Bushkin. All rights reserved.
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

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Produces a projection of a sequence by evaluating pairs of elements separated by a
        /// positive offset.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">The sequence over which to evaluate lead.</param>
        /// <param name="offset">The offset (expressed as a positive number) by which to lead each
        /// element of the sequence.</param>
        /// <param name="resultSelector">A projection function which accepts the current and
        /// subsequent (lead) element (in that order) and produces a result.</param>
        /// <returns>
        /// A sequence produced by projecting each element of the sequence with its lead
        /// pairing.</returns>
        /// <remarks>
        /// <para>
        /// This operator evaluates in a deferred and streaming manner.</para>
        /// <para>
        /// For elements of the sequence that are less than <paramref name="offset"/> items from the
        /// end, <c>default(T)</c> is used as the lead value.</para>
        /// </remarks>

        public static IEnumerable<TResult> Lead<TSource, TResult>(this IEnumerable<TSource> source, int offset, Func<TSource, TSource?, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.Select(Some)
                         .Lead(offset, default, (curr, lead) => resultSelector(curr.Value, lead is (true, var some) ? some : default));
        }

        /// <summary>
        /// Produces a projection of a sequence by evaluating pairs of elements separated by a
        /// positive offset.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">The sequence over which to evaluate Lead.</param>
        /// <param name="offset">The offset (expressed as a positive number) by which to lead each
        /// element of the sequence.</param>
        /// <param name="defaultLeadValue">A default value supplied for the leading element when
        /// none is available.</param>
        /// <param name="resultSelector">A projection function which accepts the current and
        /// subsequent (lead) element (in that order) and produces a result.</param>
        /// <returns>
        /// A sequence produced by projecting each element of the sequence with its lead
        /// pairing.</returns>
        /// <remarks>
        /// This operator evaluates in a deferred and streaming manner.
        /// </remarks>

        public static IEnumerable<TResult> Lead<TSource, TResult>(this IEnumerable<TSource> source, int offset, TSource defaultLeadValue, Func<TSource, TSource, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));

            return _(); IEnumerable<TResult> _()
            {
                var leadQueue = new Queue<TSource>(offset);
                using var iter = source.GetEnumerator();

                bool hasMore;
                // first, prefetch and populate the lead queue with the next step of
                // items to be streamed out to the consumer of the sequence
                while ((hasMore = iter.MoveNext()) && leadQueue.Count < offset)
                    leadQueue.Enqueue(iter.Current);
                // next, while the source sequence has items, yield the result of
                // the projection function applied to the top of queue and current item
                while (hasMore)
                {
                    yield return resultSelector(leadQueue.Dequeue(), iter.Current);
                    leadQueue.Enqueue(iter.Current);
                    hasMore = iter.MoveNext();
                }
                // yield the remaining values in the lead queue with the default lead value
                while (leadQueue.Count > 0)
                    yield return resultSelector(leadQueue.Dequeue(), defaultLeadValue);
            }
        }

        /// <summary>
        /// Produces a sequence of tuple containing a pair of elements separated by a positive offset.
        /// </summary>
        /// <remarks>
        /// This operator evaluates in a deferred and streaming manner.<br/>
        /// For elements of the sequence that are less than <paramref name="offset"/> items from the end,
        /// default(T) is used as the lead value.<br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
        /// <param name="source">The sequence over which to evaluate Lead</param>
        /// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the sequence</param>
        /// <returns>The produced sequence of tuple</returns>

        public static IEnumerable<(TSource Item, TSource? OffsetItem)> Lead<TSource>(this IEnumerable<TSource> source, int offset)
        {
            return Lead(source, offset, ValueTuple.Create);
        }

        /// <summary>
        /// Produces a sequence of tuple containing a pair of elements separated by a positive offset.
        /// </summary>
        /// <remarks>
        /// This operator evaluates in a deferred and streaming manner.<br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
        /// <param name="source">The sequence over which to evaluate Lead</param>
        /// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the sequence</param>
        /// <param name="defaultLeadValue">A default value supplied for the leading element when none is available</param>
        /// <returns>The produced sequence of tuple</returns>

        public static IEnumerable<(TSource Item, TSource OffsetItem)> Lead<TSource>(this IEnumerable<TSource> source, int offset, TSource defaultLeadValue)
        {
            return Lead(source, offset, defaultLeadValue, ValueTuple.Create);
        }
    }
}
