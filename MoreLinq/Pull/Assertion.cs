using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MoreLinq.Pull
{
    /// <summary>
    /// Operators which apply assertions to sequences "in flight" - they can be
    /// used as normal, but throw exceptions (lazily) if the operator detects
    /// an assertion failure as the data is being read.
    /// </summary>
    public static class Assertion
    {
        private static readonly Func<int, int, Exception> defaultErrorSelector = OnAssertCountFailure;

        /// <summary>
        /// Asserts that a source sequence contains a given count of elements.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Count to assert.</param>
        /// <returns>
        /// Returns the original sequence as long it is contains the
        /// number of elements specified by <paramref name="count"/>.
        /// Otherwise it throws <see cref="Exception" />.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<TSource> AssertCount<TSource>(this IEnumerable<TSource> source, 
            int count)
        {
            source.ThrowIfNull("source");
            count.ThrowIfNegative("count");

            return AssertCountImpl(source, count, defaultErrorSelector);
        }

        /// <summary>
        /// Asserts that a source sequence contains a given count of elements.
        /// A parameter specifies the exception to be thrown.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Count to assert.</param>
        /// <param name="errorSelector">Function that returns the <see cref="Exception"/> object to throw.</param>
        /// <returns>
        /// Returns the original sequence as long it is contains the
        /// number of elements specified by <paramref name="count"/>.
        /// Otherwise it throws the <see cref="Exception" /> object
        /// returned by calling <paramref name="errorSelector"/>.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<TSource> AssertCount<TSource>(this IEnumerable<TSource> source, 
            int count, Func<int, int, Exception> errorSelector)
        {
            source.ThrowIfNull("source");
            if (count < 0) throw new ArgumentException(null, "count");
            errorSelector.ThrowIfNull("errorSelector");

            return AssertCountImpl(source, count, errorSelector);
        }

        private static Exception OnAssertCountFailure(int cmp, int count)
        {
            var message = cmp < 0 
                        ? "Sequence contains too few elements when exactly {0} were expected."
                        : "Sequence contains too many elements when exactly {0} were expected.";
            return new SequenceException(string.Format(message, count.ToString("N0")));
        }

        private static IEnumerable<TSource> AssertCountImpl<TSource>(IEnumerable<TSource> source, 
            int count, Func<int, int, Exception> errorSelector)
        {
            var collection = source as ICollection<TSource>; // Optimization for collections
            if (collection != null)
            {
                if (collection.Count != count)
                    throw errorSelector(collection.Count.CompareTo(count), count);
                return source;
            }
            
            return ExpectingCountYieldingImpl(source, count, errorSelector);
        }

        private static IEnumerable<TSource> ExpectingCountYieldingImpl<TSource>(IEnumerable<TSource> source, 
            int count, Func<int, int, Exception> errorSelector)
        {
            var iterations = 0;
            foreach (TSource element in source)
            {
                iterations++;
                if (iterations > count)
                {
                    throw errorSelector(1, count);
                }
                yield return element;
            }
            if (iterations != count)
            {
                throw errorSelector(-1, count);
            }
        }
    }
}