using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreLinq
{
    static partial class MoreEnumerable
    {
        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,IEnumerable}" /> from an
        /// <see cref="IEnumerable{TSource}"/> according to a specified key selector
        /// Supports multiple values for one key and solves key collisions by
        /// appending element to the value IEnumerable.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{TSource}"/> to create the
        /// <see cref="Dictionary{TKey, IEnumerable}"/> MultiDictionary from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <returns>The MultiDictionary.</returns>
        public static Dictionary<TKey, IEnumerable<TSource>> ToMultiDictionary<TKey, TSource>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.ToMultiDictionary(keySelector, x => x, null);
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,IEnumerable}" /> from an
        /// <see cref="IEnumerable{TSource}"/> according to a specified key selector and
        /// value selector.
        /// Supports multiple values for one key and solves key collisions by
        /// appending element to the value IEnumerable.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TElement">Type of the element.</typeparam>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{TSource}"/> to create the
        /// <see cref="Dictionary{TKey, IEnumerable}"/> MultiDictionary from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="singleValueSelector">A function to extract a value from each element.</param>
        /// <returns>The MultiDictionary.</returns>
        public static Dictionary<TKey, IEnumerable<TElement>> ToMultiDictionary<TKey, TElement, TSource>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> singleValueSelector)
        {
            return source.ToMultiDictionary(keySelector, singleValueSelector, null);
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,IEnumerable}" /> from an
        /// <see cref="IEnumerable{TSource}"/> according to a specified key selector
        /// and comparer.
        /// Supports multiple values for one key and solves key collisions by
        /// appending element to the value IEnumerable.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{TSource}"/> to create the
        /// <see cref="Dictionary{TKey, IEnumerable}"/> MultiDictionary from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare keys.</param>
        /// <returns>The MultiDictionary.</returns>
        public static Dictionary<TKey, IEnumerable<TSource>> ToMultiDictionary<TKey, TSource>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return source.ToMultiDictionary(keySelector, x => x, comparer);
        }


        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,IEnumerable}" /> from an
        /// <see cref="IEnumerable{TSource}"/> according to a specified key selector,
        /// value selector and comparer.
        /// Supports multiple values for one key and solves key collisions by
        /// appending element to the value IEnumerable.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TElement">Type of the element.</typeparam>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{TSource}"/> to create the
        /// <see cref="Dictionary{TKey, IEnumerable}"/> MultiDictionary from</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="singleValueSelector">A function to extract a value from each element.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare keys.</param>
        /// <returns>The MultiDictionary.</returns>
        public static Dictionary<TKey, IEnumerable<TElement>> ToMultiDictionary<TKey, TElement, TSource>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> singleValueSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (singleValueSelector == null) throw new ArgumentNullException(nameof(singleValueSelector));
            
            return source
                .GroupBy(
                    keySelector,
                    (key, values) =>
                        new KeyValuePair<TKey, IEnumerable<TElement>>(key, values.Select(singleValueSelector)),
                    comparer
                )
                .ToDictionary(comparer);
        }
    }
}
