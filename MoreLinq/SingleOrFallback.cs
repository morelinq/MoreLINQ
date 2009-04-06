using System;
using System.Collections.Generic;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns the single element in the given sequence, or the result
        /// of executing a fallback delegate if the sequence is empty.
        /// </summary>
        /// <remarks>
        /// The fallback delegate is not executed if the sequence is non-empty.
        /// </remarks>
        /// <typeparam name="T">Element type of sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="fallback">The fallback delegate to execute if the sequence is empty</param>
        /// <exception cref="ArgumentNullException">source or fallback is null</exception>
        /// <exception cref="InvalidOperationException">The sequence has more than one element</exception>
        /// <returns>The single element in the sequence, or the result of calling the
        /// fallback delegate if the sequence is empty.</returns>

        public static T SingleOrFallback<T>(this IEnumerable<T> source, Func<T> fallback)
        {
            source.ThrowIfNull("source");
            fallback.ThrowIfNull("fallback");
            using (IEnumerator<T> iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    return fallback();
                }
                T first = iterator.Current;
                if (iterator.MoveNext())
                {
                    throw new InvalidOperationException();
                }
                return first;
            }
        }
    }
}
