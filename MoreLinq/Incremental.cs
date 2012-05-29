using System;
using System.Collections.Generic;

namespace MoreLinq
{
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
            sequence.ThrowIfNull("sequence");
            resultSelector.ThrowIfNull("resultSelector");

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
            sequence.ThrowIfNull("sequence");
            resultSelector.ThrowIfNull("resultSelector");

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