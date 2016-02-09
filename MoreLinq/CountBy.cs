namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Projects each element in a collection to a new element, and returns
        /// a dictionary wherein each key is a distinct projected element, and each
        /// value its number of occurrences.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the input sequence.</typeparam>
        /// <typeparam name="TResult">The type of new elements returned by <paramref name="projection"/>.</typeparam>
        /// <param name="source">The sequence of elements to undergo projection.</param>
        /// <param name="projection">The function that projects each element in
        /// <paramref name="source"/> to a new element.</param>
        /// <returns>A dictionary wherein each key is a distinct projected element, and each
        /// value its number of occurrences.</returns>
        /// 
        public static IDictionary<TResult, int> CountBy<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> projection)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (projection == null)
            {
                throw new ArgumentNullException("projection");
            }

            return source.Select(projection)
                         .GroupBy(projectedValue => projectedValue)
                         .ToDictionary(numItemsWithSameProjectedValue => numItemsWithSameProjectedValue.Key, 
                                       numItemsWithSameProjectedValue => numItemsWithSameProjectedValue.Count());
        }
    }
}
