using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
