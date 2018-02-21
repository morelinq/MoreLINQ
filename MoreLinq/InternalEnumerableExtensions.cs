using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreLinq
{
    static class InternalEnumerableExtensions
    {
        /// <summary>
        /// Gets the count of an enumerable if it is precalculated in <see cref="ICollection{T}"/>
        /// or <see cref="IReadOnlyCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to enumerate.</typeparam>
        /// <param name="source">The enumerable to get the count for.</param>
        /// <returns>The count if it is available without enumeration, or null if it is not.</returns>
        public static int? TryGetCollectionCount<T>(this IEnumerable<T> source)
        {
            return source is ICollection<T> collection ? collection.Count
                : source is IReadOnlyCollection<T> readOnlyCollection ? readOnlyCollection.Count
                : (int?)null;
        }
    }
}
