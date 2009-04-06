using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MoreLinq
{
    public static partial class Enumerable
    {
        /// <summary>
        /// Traces the elements of a source sequence for diagnostics.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
        /// <param name="source">Source sequence whose elements to trace.</param>
        /// <returns>
        /// Return the source sequence unmodified.
        /// </returns>
        /// <remarks>
        /// This a pass-through operator that uses deferred execution and 
        /// streams the results.
        /// </remarks>

        public static IEnumerable<TSource> Trace<TSource>(this IEnumerable<TSource> source)
        {
            return Trace(source, (string) null);
        }

        /// <summary>
        /// Traces the elements of a source sequence for diagnostics using
        /// custom formatting.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
        /// <param name="source">Source sequence whose elements to trace.</param>
        /// <param name="format">
        /// String to use to format the trace message. If null then the
        /// element value becomes the traced message.
        /// </param>
        /// <returns>
        /// Return the source sequence unmodified.
        /// </returns>
        /// <remarks>
        /// This a pass-through operator that uses deferred execution and 
        /// streams the results.
        /// </remarks>

        public static IEnumerable<TSource> Trace<TSource>(this IEnumerable<TSource> source, string format)
        {
            source.ThrowIfNull("source");

            return TraceImpl(source, 
                string.IsNullOrEmpty(format)
                ? (Func<TSource, string>) (x => x == null ? string.Empty : x.ToString())
                : (x => string.Format(format, x)));
        }

        /// <summary>
        /// Traces the elements of a source sequence for diagnostics using
        /// a custom formatter.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
        /// <param name="source">Source sequence whose elements to trace.</param>
        /// <param name="formatter">Function used to format each source element into a string.</param>
        /// <returns>
        /// Return the source sequence unmodified.
        /// </returns>
        /// <remarks>
        /// This a pass-through operator that uses deferred execution and 
        /// streams the results.
        /// </remarks>

        public static IEnumerable<TSource> Trace<TSource>(this IEnumerable<TSource> source, Func<TSource, string> formatter)
        {
            source.ThrowIfNull("source");
            formatter.ThrowIfNull("formatter");
            return TraceImpl(source, formatter);
        }

        private static IEnumerable<TSource> TraceImpl<TSource>(IEnumerable<TSource> source, Func<TSource, string> formatter)
        {
            Debug.Assert(source != null);
            Debug.Assert(formatter != null);

            return source
#if !NO_TRACING
                .Pipe(x => System.Diagnostics.Trace.WriteLine(formatter(x)))
#endif
                ;
        }
    }
}
