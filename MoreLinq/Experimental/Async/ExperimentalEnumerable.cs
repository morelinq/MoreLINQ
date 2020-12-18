#if !NO_ASYNC_STREAMS

namespace MoreLinq.Experimental.Async
{
    using System.Collections.Generic;

    /// <summary>
    /// <para>
    /// Provides a set of static methods for querying objects that
    /// implement <see cref="IAsyncEnumerable{T}" />.</para>
    /// <para>
    /// <strong>THE METHODS ARE EXPERIMENTAL. THEY MAY BE UNSTABLE AND
    /// UNTESTED. THEY MAY BE REMOVED FROM A FUTURE MAJOR OR MINOR RELEASE AND
    /// POSSIBLY WITHOUT NOTICE. USE THEM AT YOUR OWN RISK. THE METHODS ARE
    /// PUBLISHED FOR FIELD EXPERIMENTATION TO SOLICIT FEEDBACK ON THEIR
    /// UTILITY AND DESIGN/IMPLEMENTATION DEFECTS.</strong></para>
    /// </summary>

    public static partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Converts a query whose results evaluate asynchronously to use
        /// sequential instead of concurrent evaluation.
        /// </summary>
        /// <typeparam name="T">The type of the source elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>The converted sequence.</returns>

        public static IAsyncEnumerable<T> AsSequential<T>(this IAsyncQuery<T> source) =>
            source.MaxConcurrency(1);

        /// <summary>
        /// Returns a query whose results evaluate asynchronously to use a
        /// concurrency limit.
        /// </summary>
        /// <typeparam name="T">The type of the source elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="value"></param>
        /// <returns>
        /// A query whose results evaluate asynchronously using the given
        /// concurrency limit.</returns>

        public static IAsyncQuery<T> MaxConcurrency<T>(this IAsyncQuery<T> source, int value) =>
            source.WithOptions(source.Options.WithMaxConcurrency(value));
    }
}

#endif // !NO_ASYNC_STREAMS
