namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    static partial class MoreEnumerable
    {
        /// <summary>
        ///   Returns all duplicated elements of the given source.
        /// </summary>
        /// <param name="source">source sequence.</param>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <returns>all elements that are duplicated.</returns>
        public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> source)
            => Duplicates(source, null);

        /// <summary>
        ///   Returns all duplicated elements of the given source, using the specified element equality comparer
        /// </summary>
        /// <param name="source">source sequence.</param>
        /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <returns>all elements of the source sequence that are duplicated, based on the provided equality comparer</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IEnumerable<TSource> Duplicates<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return GetDuplicates();

            IEnumerable<TSource> GetDuplicates()
            {
                var enumeratedElements = new HashSet<TSource>(comparer);
                foreach (var element in source)
                {
                    if (enumeratedElements.Add(element) is false)
                        yield return element;
                }
            }
        }
    }
}
