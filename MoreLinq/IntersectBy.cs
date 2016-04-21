using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreLinq {

    partial class MoreEnumerable {

        /// <summary>
        /// Returns a collection containing a distinct set of elements from the first collection
        /// whose key matches a key of an element in the second collection.
        /// </summary>
        /// <typeparam name="T">The type of the source, keys, and result elements.</typeparam>
        /// <param name="first">The first collection.</param>
        /// <param name="second">The second collection.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>Collection containing a distinct set of elements from first collection
        /// whose key matches a key of an element in second collection.</returns>
        public static IEnumerable<T> IntersectBy<T>(this IEnumerable<T> first,
            IEnumerable<T> second,
            Func<T, T> keySelector) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectByImpl(first, second.Select(keySelector), keySelector, null);
        }

        /// <summary>
        /// Returns a collection containing a distinct set of elements from the first collection
        /// whose key matches a key of an element in the second collection.
        /// </summary>
        /// <typeparam name="T">The type of the source, keys, and result elements.</typeparam>
        /// <param name="first">The first collection.</param>
        /// <param name="second">The second collection.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="keyComparer">The key comparer.</param>
        /// <returns>Collection containing a distinct set of elements from first collection
        /// whose key matches a key of an element in second collection.</returns>
        public static IEnumerable<T> IntersectBy<T>(this IEnumerable<T> first,
            IEnumerable<T> second,
            Func<T, T> keySelector,
            IEqualityComparer<T> keyComparer) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectByImpl(first, second.Select(keySelector), keySelector, keyComparer);
        }

        /// <summary>
        /// Returns a collection containing a distinct set of elements from the first collection
        /// whose key matches a key of an element in the second collection.
        /// </summary>
        /// <typeparam name="TSource">The type of the source and result elements.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="first">The first collection.</param>
        /// <param name="second">The second collection.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>Collection containing a distinct set of elements from first collection
        /// whose key matches a key of an element in second collection.</returns>
        public static IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            Func<TSource, TKey> keySelector) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectByImpl(first, second.Select(keySelector), keySelector, null);
        }

        /// <summary>
        /// Returns a collection containing a distinct set of elements from the first collection
        /// whose key matches a key of an element in the second collection.
        /// </summary>
        /// <typeparam name="TSource">The type of the source and result elements.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="first">The first collection.</param>
        /// <param name="second">The second collection.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="keyComparer">The key comparer.</param>
        /// <returns>Collection containing a distinct set of elements from first collection
        /// whose key matches a key of an element in second collection.</returns>
        public static IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectByImpl(first, second.Select(keySelector), keySelector, keyComparer);
        }

        /// <summary>
        /// Returns a collection containing a distinct set of elements from the first collection
        /// whose key is not in the second collection.
        /// </summary>
        /// <typeparam name="TSource">The type of the source and result elements.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="first">The source collection.</param>
        /// <param name="second">The key collection.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>Collection containing a distinct set of elements from first collection
        /// whose key is not in second collection.</returns>
        public static IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first,
            IEnumerable<TKey> second,
            Func<TSource, TKey> keySelector) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectByImpl(first, second, keySelector, null);
        }

        /// <summary>
        /// Returns a collection containing a distinct set of elements from the first collection
        /// whose key is not in the second collection.
        /// </summary>
        /// <typeparam name="TSource">The type of the source and result elements.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="first">The source collection.</param>
        /// <param name="second">The key collection.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="keyComparer">The key comparer.</param>
        /// <returns>Collection containing a distinct set of elements from first collection
        /// whose key is not in second collection.</returns>
        public static IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first,
            IEnumerable<TKey> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectByImpl(first, second, keySelector, keyComparer);
        }

        private static IEnumerable<TSource> IntersectByImpl<TSource, TKey>(IEnumerable<TSource> first,
            IEnumerable<TKey> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer) {

            var keys = new HashSet<TKey>(second, keyComparer);
            foreach (var item in first) {
                var k = keySelector(item);
                if (keys.Contains(k)) {
                    yield return item;
                    keys.Remove(k);
                }
            }
        }
    }
}
