using System.Linq;
using System.Collections.Generic;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns every N-th element of a source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="step">Steps in which to partition source</param>
        /// <remarks>
        /// This operator uses deferred execution and streams the results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3, 4, 5 };
        /// IEnumerable&lt;int&gt; result = numbers.Every(2);
        /// </code>
        /// The <c>result</c> variable, when iterated over, will yield 1, 3 and 5, in turn.
        /// </example>
        
        public static IEnumerable<TSource> TakeEvery<TSource>(this IEnumerable<TSource> source, int step)
        {
            source.ThrowIfNull("source");
            step.ThrowIfNonPositive("step");
            return source.Where((e, i) => i % step == 0);
        }
    }
}
