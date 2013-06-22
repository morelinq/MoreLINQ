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

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Produces a projection of a sequence by evaluating pairs of elements separated by a positive offset.
        /// </summary>
        /// <remarks>
        /// This operator evaluates in a deferred and streaming manner.<br/>
        /// For elements of the sequence that are less than <paramref name="offset"/> items from the end,
        /// default(T) is used as the lead value.<br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
        /// <param name="source">The sequence over which to evaluate Lead</param>
        /// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the sequence</param>
        /// <param name="resultSelector">A projection function which accepts the current and subsequent (lead) element (in that order) and produces a result</param>
        /// <returns>A sequence produced by projecting each element of the sequence with its lead pairing</returns>
        
        public static IEnumerable<TResult> Lead<TSource, TResult>(this IEnumerable<TSource> source, int offset, Func<TSource, TSource, TResult> resultSelector)
        {
            return Lead(source, offset, default(TSource), resultSelector);
        }

        /// <summary>
        /// Produces a projection of a sequence by evaluating pairs of elements separated by a positive offset.
        /// </summary>
        /// <remarks>
        /// This operator evaluates in a deferred and streaming manner.<br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
        /// <param name="source">The sequence over which to evaluate Lead</param>
        /// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the sequence</param>
        /// <param name="defaultLeadValue">A default value supplied for the leading element when none is available</param>
        /// <param name="resultSelector">A projection function which accepts the current and subsequent (lead) element (in that order) and produces a result</param>
        /// <returns>A sequence produced by projecting each element of the sequence with its lead pairing</returns>
        
        public static IEnumerable<TResult> Lead<TSource, TResult>(this IEnumerable<TSource> source, int offset, TSource defaultLeadValue, Func<TSource, TSource, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");
            if (offset <= 0) throw new ArgumentOutOfRangeException("offset");

            return LeadImpl(source, offset, defaultLeadValue, resultSelector);
        }

        private static IEnumerable<TResult> LeadImpl<TSource, TResult>(IEnumerable<TSource> source, int offset, TSource defaultLeadValue, Func<TSource, TSource, TResult> resultSelector)
        {
            var leadQueue = new Queue<TSource>();
            using (var iter = source.GetEnumerator())
            {
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
                {
                    yield return resultSelector(leadQueue.Dequeue(), defaultLeadValue);
                }
            }
        }
    }
}