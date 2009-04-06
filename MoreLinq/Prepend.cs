using System.Collections.Generic;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq
{
    public static partial class Enumerable
    {
        /// <summary>
        /// Prepends a single value to a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to prepend to.</param>
        /// <param name="value">The value to prepend.</param>
        /// <returns>
        /// Returns a sequence where a value is prepended to it.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// IEnumerable&lt;int&gt; result = numbers.Prepend(0);
        /// </code>
        /// The <c>result</c> variable, when iterated over, will yield 
        /// 0, 1, 2 and 3, in turn.

        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            source.ThrowIfNull("source");
            return LinqEnumerable.Concat(LinqEnumerable.Repeat(value, 1), source);
        }
    }
}