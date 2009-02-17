using System;
using System.Collections.Generic;
using System.Diagnostics;
using SysTrace = System.Diagnostics.Trace;

namespace MoreLinq.Pull
{
    /// <summary>
    /// Extension methods which are likely to induce side effects.
    /// </summary>
    public static class SideEffects
    {
        /// <summary>
        /// Immediately executes the given action on each element in the source sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="source">The sequence of elements</param>
        /// <param name="action">The action to execute on each element</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            source.ThrowIfNull("source");
            action.ThrowIfNull("action");
            foreach (T element in source)
            {
                action(element);
            }
        }

        /// <summary>
        /// Executes the given action on each element in the source sequence
        /// and yields it.
        /// </summary>
        /// <remarks>
        /// The returned sequence is essentially a duplicate of
        /// the original, but with the extra action being executed while the
        /// sequence is evaluated. The action is always taken before the element
        /// is yielded, so any changes made by the action will be visible in the
        /// returned sequence. This operator executes lazily, and with deferred
        /// execution.
        /// </remarks>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="source">The sequence of elements</param>
        /// <param name="action">The action to execute on each element</param>
        public static IEnumerable<T> Pipe<T>(this IEnumerable<T> source, Action<T> action)
        {
            source.ThrowIfNull("source");
            action.ThrowIfNull("action");
            return PipeImpl(source, action);
        }

        private static IEnumerable<T> PipeImpl<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
                yield return element;
            }
        }

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
            return Trace(source, null);
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
            return TraceImpl(source, format);
        }

        private static IEnumerable<TSource> TraceImpl<TSource>(IEnumerable<TSource> source, string format)
        {
            Debug.Assert(source != null);

            var formatter = string.IsNullOrEmpty(format)
                ? (Func<TSource, string>) (x => x == null ? string.Empty : x.ToString())
                : (x => string.Format(format, x));

            return source.Pipe(x => SysTrace.WriteLine(formatter(x)));
        }
    }
}
