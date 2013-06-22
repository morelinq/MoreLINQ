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
        /// Computes an incremental value between every adjacent element in a sequence: {N,N+1}, {N+1,N+2}, ...
        /// </summary>
        /// <remarks>
        /// The projection function is passed the previous and next element (in that order) and may use
        /// either or both in computing the result.<br/>
        /// If the sequence has less than two items, the result is always an empty sequence.<br/>
        /// The number of items in the resulting sequence is always one less than in the source sequence.<br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
        /// <param name="sequence">The sequence of elements to incrementally process</param>
        /// <param name="resultSelector">A projection applied to each pair of adjacent elements in the sequence</param>
        /// <returns>A sequence of elements resulting from projection every adjacent pair</returns>
        
        public static IEnumerable<TResult> Incremental<TSource, TResult>(this IEnumerable<TSource> sequence, Func<TSource, TSource, TResult> resultSelector)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            return IncrementalImpl(sequence, (prev, next, index) => resultSelector(prev, next));
        }

        /// <summary>
        /// Computes an incremental value between every adjacent element in a sequence: {N,N+1}, {N+1,N+2}, ...
        /// </summary>
        /// <remarks>
        /// The projection function is passed the previous element, next element, and the zero-based index of
        /// the next element (in that order) and may use any of these values in computing the result.<br/>
        /// If the sequence has less than two items, the result is always an empty sequence.<br/>
        /// The number of items in the resulting sequence is always one less than in the source sequence.<br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
        /// <param name="sequence">The sequence of elements to incrementally process</param>
        /// <param name="resultSelector">A projection applied to each pair of adjacent elements in the sequence</param>
        /// <returns>A sequence of elements resulting from projection every adjacent pair</returns>
        
        public static IEnumerable<TResult> Incremental<TSource, TResult>(this IEnumerable<TSource> sequence, Func<TSource, TSource, int, TResult> resultSelector)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            return IncrementalImpl(sequence, resultSelector);
        }

        private static IEnumerable<TResult> IncrementalImpl<TSource, TResult>(IEnumerable<TSource> sequence, Func<TSource, TSource, int, TResult> resultSelector)
        {
            using (var iter = sequence.GetEnumerator())
            {
                if (iter.MoveNext())
                {
                    var index = 0;
                    var prevItem = iter.Current;
                    while (iter.MoveNext())
                    {
                        var nextItem = iter.Current;
                        yield return resultSelector(prevItem, nextItem, ++index);
                        prevItem = nextItem;
                    }
                }
            }
        }
    }
}