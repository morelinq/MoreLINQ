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
            => Duplicates(source, EqualityComparer<T>.Default);

        /// <summary>
        ///   Returns all duplicated elements of the given source, using the specified element equality comparer.
        /// </summary>
        /// <param name="source">source sequence.</param>
        /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <typeparam name="T">The type of the elements in the source sequence</typeparam>
        /// <returns>all elements of the source sequence that are duplicated, based on the provided equality comparer</returns>
        public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> source, IEqualityComparer<T>? comparer)
            => Duplicates(source, IdFn, comparer);

        /// <summary>
        ///   Returns all duplicated elements of the given source, using the specified element equality comparer
        /// </summary>
        /// <param name="source">source sequence.</param>
        /// <param name="keySelector">Projection for determining duplication</param>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <returns>all elements of the source sequence that are duplicated, based on the provided equality comparer</returns>
        public static IEnumerable<TSource> Duplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            => Duplicates(source, keySelector, EqualityComparer<TKey>.Default);

        /// <summary>
        ///   Returns all duplicated elements of the given source, using the specified element equality comparer
        /// </summary>
        /// <param name="source">source sequence.</param>
        /// <param name="keySelector">Projection for determining duplication</param>
        /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <returns>all elements of the source sequence that are duplicated, based on the provided equality comparer</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is null.</exception>
        public static IEnumerable<TSource> Duplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return GetDuplicates();

            IEnumerable<TSource> GetDuplicates()
            {
                var enumeratedElements = new HashSet<TKey>(comparer);
                foreach (var element in source)
                {
                    if (enumeratedElements.Add(keySelector(element)) is false)
                        yield return element;
                }
            }
        }
    }
}
