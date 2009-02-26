namespace MoreLinq.Pull
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public static partial class Enumerable
    {
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
        /// <param name="padding">The value to use for padding.</param>
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

        public static IEnumerable<TSource> Pad<TSource>(this IEnumerable<TSource> source, int width, TSource padding)
        {
            source.ThrowIfNull("source");
            if (width < 0) throw new ArgumentException(null, "width");
            return PadImpl(source, width, padding, null);
        }

        /// <summary>
        /// Pads a sequence with a dynamic filler value if it is narrower (shorter 
        /// in length) than a given width.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to pad.</param>
        /// <param name="width">The width/length below which to pad.</param>
        /// <param name="paddingSelector">Function to calculate padding.</param>
        /// <returns>
        /// Returns a sequence that is at least as wide/long as the width/length
        /// specified by the <paramref name="width"/> parameter.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 0, 1, 2 };
        /// IEnumerable&lt;int&gt; result = numbers.Pad(5, i => -i);
        /// </code>
        /// The <c>result</c> variable, when iterated over, will yield 
        /// 0, 1, 2, -3 and -4, in turn.
        /// </example>

        public static IEnumerable<TSource> Pad<TSource>(this IEnumerable<TSource> source, int width, Func<int, TSource> paddingSelector)
        {
            source.ThrowIfNull("source");
            source.ThrowIfNull("paddingSelector");
            if (width < 0) throw new ArgumentException(null, "width");
            return PadImpl(source, width, default(TSource), paddingSelector);
        }

        private static IEnumerable<T> PadImpl<T>(IEnumerable<T> source,
            int width, T padding, Func<int, T> paddingSelector)
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
                yield return paddingSelector != null ? paddingSelector(count) : padding;
                count++;
            }
        }
    }
}