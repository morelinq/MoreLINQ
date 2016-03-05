#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System.Linq;

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Creates a delimited string from a sequence of values. The
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <see cref="TextInfo.ListSeparator"/>. If the source
        /// sequence is empty, the method returns an empty string.
        /// </returns>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>

        public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source)
        {
            return ToDelimitedString(source, null);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>

        public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, (sb, e) => sb.Append(e), 256);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>

        public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source, char delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, (sb, e) => sb.Append(e), 256);
        }

        /// <summary>
        /// Creates a delimited string from an array of string values and a given delimiter.
        /// </summary>
        /// <param name="source">The array of string items to join. Each is converted to a string using the simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// This is optimized version of more generic method that accepts IEnumarable{T}.
        /// It calculates entire length of the result string and initializes StringBuilder's capacity with it.
        /// </remarks>

        public static string ToDelimitedString(this IList<string> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");

            int capacity = source.Sum(v => v.Length);
            if (delimiter != null)
            {
                capacity += (source.Count - 1) * delimiter.Length;
            }

            return ToDelimitedStringImpl(source, delimiter, capacity);
        }

        /// <summary>
        /// Creates a delimited string from an array of string values and a given delimiter.
        /// </summary>
        /// <param name="source">The array of string items to join. Each is converted to a string using the simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// This is optimized version of more generic method that accepts IEnumarable{T}.
        /// It calculates entire length of the result string and initializes StringBuilder's capacity with it.
        /// </remarks>

        public static string ToDelimitedString(this IList<string> source, char delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");

            int capacity = source.Sum(v => v.Length) + (source.Count - 1) * 1;

            return ToDelimitedStringImpl(source, delimiter, capacity);
        }


        private static string ToDelimitedStringImpl<TSource>(IEnumerable<TSource> source, string delimiter, Action<StringBuilder, TSource> append, int capacity)
        {
            Debug.Assert(source != null);
            Debug.Assert(append != null);

            delimiter = delimiter ?? CultureInfo.CurrentCulture.TextInfo.ListSeparator;

            var sb = new StringBuilder(capacity);
            var i = 0;

            foreach (var value in source)
            {
                if (i++ > 0) sb.Append(delimiter);
                append(sb, value);
            }

            return sb.ToString();
        }

        private static string ToDelimitedStringImpl<TSource>(IEnumerable<TSource> source, char delimiter, Action<StringBuilder, TSource> append, int capacity)
        {
            Debug.Assert(source != null);
            Debug.Assert(append != null);

            var sb = new StringBuilder(capacity);
            var i = 0;

            foreach (var value in source)
            {
                if (i++ > 0) sb.Append(delimiter);
                append(sb, value);
            }

            return sb.ToString();
        }

        private static string ToDelimitedStringImpl(IList<string> source, string delimiter, int capacity)
        {
            Debug.Assert(source != null);

            StringBuilder sb = new StringBuilder(capacity);
            int i = 0;

            foreach (string value in source)
            {
                if (i++ > 0) sb.Append(delimiter);
                sb.Append(value);
            }

            return sb.ToString();
        }

        private static string ToDelimitedStringImpl(IList<string> source, char delimiter, int capacity)
        {
            Debug.Assert(source != null);

            StringBuilder sb = new StringBuilder(capacity);
            int i = 0;

            foreach (string value in source)
            {
                if (i++ > 0) sb.Append(delimiter);
                sb.Append(value);
            }

            return sb.ToString();
        }
    }
}
