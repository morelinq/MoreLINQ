using System;
using System.Collections.Generic;

namespace MoreLinq
{
    /// <summary>
    /// Extension methods that provide conditional traversal and projection
    /// of a sequence based on an intrinsic or extrinsic condition.
    /// </summary>
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Consumes and projects items in a sequence as long as some condition is <c>true</c>
        /// </summary>
        /// <remarks>
        /// For the first element of the sequence, the <paramref name="whileCondition"/> is passed <c>default(T)</c>.<br/>
        /// The index is the zero-based offset of the current element being evaluated.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
        /// <param name="sequence">The sequence which to consume conditionally</param>
        /// <param name="whileCondition">A user-supplied test passed the previous and current elements, and an index, which returns <c>true</c> while items should be consumed</param>
        /// <param name="resultSelector">A user-supplied projection which transforms items for the result sequence</param>
        /// <returns>A sequence of projected elements selected while the condition was <c>true</c></returns>
        public static IEnumerable<TResult> While<TSource, TResult>(this IEnumerable<TSource> sequence, Func<TSource, TSource, int, bool> whileCondition, Func<TSource, TResult> resultSelector)
        {
            sequence.ThrowIfNull("sequence");
            whileCondition.ThrowIfNull("whileCondition");
            resultSelector.ThrowIfNull("resultSelector");

            return WhileImpl(sequence, whileCondition, resultSelector);
        }

        private static IEnumerable<TResult> WhileImpl<TSource, TResult>(this IEnumerable<TSource> sequence, Func<TSource, TSource, int, bool> whileCondition, Func<TSource, TResult> resultSelector)
        {
            using (var iter = sequence.GetEnumerator())
            {
                var index = -1;
                var prevItem = default(TSource);
                while (iter.MoveNext())
                {
                    var item = iter.Current;
                    if (!whileCondition(prevItem, item, ++index))
                        break;
                    prevItem = item;
                    yield return resultSelector(item);
                }
            }
        }
    }
}