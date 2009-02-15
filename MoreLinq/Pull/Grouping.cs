using System;
using System.Collections.Generic;

namespace MoreLinq.Pull
{
    /// <summary>
    /// Grouping operators.
    /// </summary>
    public static class Grouping
    {
        /// <summary>
        /// Returns tuples, where each tuple contains the N-th element from 
        /// each of the argument sequences. The returned sequence is truncated
        /// in length to the length of the shortest argument sequence.
        /// </summary>
        /// <example>
        /// zip((1, 2, 3), (4, 5, 6)) -> ((1, 4), (2, 5), (3, 6))
        /// </example>        
        /// <remarks>
        /// Functions similar to the built-in zip function of Python. See:
        /// http://www.python.org/doc/2.5.2/lib/built-in-funcs.html#l2h-81
        ///
        /// For an interesting discussion regarding rare cases, see:
        /// [Python-3000] have zip() raise exception for sequences of different lengths
        /// http://mail.python.org/pipermail/python-3000/2006-August/003338.html
        /// </remarks>
        /// <typeparam name="TFirst">Type of elements in first sequence</typeparam>
        /// <typeparam name="TSecond">Type of elements in second sequence</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence</typeparam>
        /// <param name="first">First sequence</param>
        /// <param name="second">Second sequence</param>
        /// <param name="resultSelector">Function to apply to each pair of elements</param>
        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, 
            IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            first.ThrowIfNull("first");
            second.ThrowIfNull("second");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipImpl(first, second, resultSelector);
        }

        private static IEnumerable<TResult> ZipImpl<TFirst, TSecond, TResult>(
            IEnumerable<TFirst> first, 
            IEnumerable<TSecond> second, 
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            using (var e1 = first.GetEnumerator())
            {
                using (var e2 = second.GetEnumerator())
                {
                    if (!e1.MoveNext())
                        yield break;
                    do
                    {
                        if (!e2.MoveNext())
                            yield break;
                        yield return resultSelector(e1.Current, e2.Current);
                    }
                    while (e1.MoveNext());
                }
            }
        }
    }
}
