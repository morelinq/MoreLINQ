using System.Linq;
using System.Collections.Generic;

namespace MoreLinq.Pull
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Concatenation operators.
    /// </summary>
    public static class Concatenation
    {
        /// <summary>
        /// Returns a sequence consisting of the head element and the given tail elements.
        /// This operator uses deferred execution and streams its results.
        /// </summary>
        /// <typeparam name="T">Type of sequence</typeparam>
        /// <param name="head">Head element of the new sequence.</param>
        /// <param name="tail">All elements of the tail. Must not be null.</param>
        /// <returns>A sequence consisting of the head elements and the given tail elements.</returns>
        public static IEnumerable<T> Concat<T>(this T head, IEnumerable<T> tail)
        {
            tail.ThrowIfNull("tail");
            return tail.Prepend(head);
        }

        /// <summary>
        /// Returns a sequence consisting of the head elements and the given tail element.
        /// This operator uses deferred execution and streams its results.
        /// </summary>
        /// <typeparam name="T">Type of sequence</typeparam>
        /// <param name="head">All elements of the head. Must not be null.</param>
        /// <param name="tail">Tail element of the new sequence.</param>
        /// <returns>A sequence consisting of the head elements and the given tail element.</returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> head, T tail)
        {
            head.ThrowIfNull("head");
            return head.Concat(Enumerable.Repeat(tail, 1));
        }

        /// <summary>
        /// Prepends a single value to a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to prepend to.</param>
        /// <param name="value">The value to prepend.</param>
        /// <returns>
        /// Returns a sequence where a value is prepended to it.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// IEnumerable&lt;int&gt; result = numbers.Prepend(0);
        /// </code>
        /// The <c>result</c> variable, when iterated over, will yield 
        /// 0, 1, 2 and 3, in turn.

        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            source.ThrowIfNull("source");
            return Enumerable.Repeat(value, 1).Concat(source);
        }

        /// <summary>
        /// Pads a sequence with default values if it is narrower (shorter 
        /// in length) than a given width.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to pad.</param>
        /// <param name="width">The width/length below which to pad.</param>
        /// <returns>
        /// Returns a sequence that is at least as wide/long as the width/length
        /// specified by the <paramref name="width"/> parameter.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 123, 456, 789 };
        /// IEnumerable&lt;int&gt; result = numbers.Pad(5);
        /// </code>
        /// The <c>result</c> variable, when iterated over, will yield 
        /// 123, 456, 789 and two zeroes, in turn.
        /// </example>

        public static IEnumerable<TSource> Pad<TSource>(this IEnumerable<TSource> source, int width)
        {
            return Pad(source, width, default(TSource));
        }

        /// <summary>
        /// Pads a sequence with a given filler value if it is narrower (shorter 
        /// in length) than a given width.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to pad.</param>
        /// <param name="width">The width/length below which to pad.</param>
        /// <param name="filler">The value to use for padding.</param>
        /// <returns>
        /// Returns a sequence that is at least as wide/long as the width/length
        /// specified by the <paramref name="width"/> parameter.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 123, 456, 789 };
        /// IEnumerable&lt;int&gt; result = numbers.Pad(5, -1);
        /// </code>
        /// The <c>result</c> variable, when iterated over, will yield 
        /// 123, 456, and 789 followed by two occurrences of -1, in turn.
        /// </example>

        public static IEnumerable<TSource> Pad<TSource>(this IEnumerable<TSource> source, int width, TSource filler)
        {
            source.ThrowIfNull("source");
            if (width < 0) throw new ArgumentException(null, "width");
            return PadImpl(source, width, filler);
        }

        private static IEnumerable<T> PadImpl<T>(this IEnumerable<T> source, int width, T filler)
        {
            Debug.Assert(source != null);
            Debug.Assert(width >= 0);

            var count = 0;
            foreach (var item in source)
            {
                yield return item;
                count++;
            }
            while (count < width)
            {
                yield return filler;
                count++;
            }
        }
    }
}
