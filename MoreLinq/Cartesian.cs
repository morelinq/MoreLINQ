using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns the Cartesian product of two sequences by combining each element of the first set with each in the second
        /// and applying the user=define projection to the pair.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of <paramref name="first"/></typeparam>
        /// <typeparam name="TSecond">The type of the elements of <paramref name="second"/></typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence</typeparam>
        /// <param name="first">The first sequence of elements</param>
        /// <param name="second">The second sequence of elements</param>
        /// <param name="resultSelector">A projection function that combines elements from both sequences</param>
        /// <returns>A sequence representing the Cartesian product of the two source sequences</returns>
        public static IEnumerable<TResult> Cartesian<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            return from item1 in first 
                   from item2 in second // TODO buffer to avoid multiple enumerations
                   select resultSelector(item1, item2);
        }
    }
}