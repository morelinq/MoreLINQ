namespace MoreLinq.Pull
{
    using System.Collections.Generic;
    using System.Text;
    using System.Globalization;
    using System.Diagnostics;

    public static partial class Enumerable
    {
        /// <summary>
        /// Creates a delimited string from a sequence of values. The 
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>

        public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source)
        {
            return ToDelimitedString(source, null);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>

        public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source, string delimiter)
        {
            source.ThrowIfNull("source");
            return ToDelimitedStringImpl(source, delimiter ?? CultureInfo.CurrentCulture.TextInfo.ListSeparator);
        }

        private static string ToDelimitedStringImpl<TSource>(IEnumerable<TSource> source, string delimiter)
        {
            Debug.Assert(source != null);
            Debug.Assert(delimiter != null);

            var sb = new StringBuilder();

            foreach (var value in source)
            {
                if (sb.Length > 0) sb.Append(delimiter);
                sb.Append(value);
            }

            return sb.ToString();
        }
    }
}
