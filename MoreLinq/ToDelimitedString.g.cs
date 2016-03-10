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

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    partial class MoreEnumerable
    {
        /// <summary>
        /// Creates a delimited string from a sequence of values. The
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<bool> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Action<StringBuilder, bool> Boolean = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<bool> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Boolean, 256);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<byte> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Action<StringBuilder, byte> Byte = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<byte> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Byte, 256);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<char> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Action<StringBuilder, char> Char = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<char> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Char, 256);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<decimal> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Action<StringBuilder, decimal> Decimal = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<decimal> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Decimal, 256);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<double> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Action<StringBuilder, double> Double = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<double> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Double, 256);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<float> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Action<StringBuilder, float> Single = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<float> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Single, 256);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<int> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Action<StringBuilder, int> Int32 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<int> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Int32, 256);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<long> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Action<StringBuilder, long> Int64 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<long> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Int64, 256);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
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
        [CLSCompliant(false)]
        public static string ToDelimitedString(this IEnumerable<sbyte> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Action<StringBuilder, sbyte> SByte = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
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
        [CLSCompliant(false)]
        public static string ToDelimitedString(this IEnumerable<sbyte> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.SByte, 256);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<short> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Action<StringBuilder, short> Int16 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<short> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Int16, 256);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<string> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Action<StringBuilder, string> String = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
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

        public static string ToDelimitedString(this IEnumerable<string> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.String, 256);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
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
        [CLSCompliant(false)]
        public static string ToDelimitedString(this IEnumerable<uint> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Action<StringBuilder, uint> UInt32 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
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
        [CLSCompliant(false)]
        public static string ToDelimitedString(this IEnumerable<uint> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.UInt32, 256);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
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
        [CLSCompliant(false)]
        public static string ToDelimitedString(this IEnumerable<ulong> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Action<StringBuilder, ulong> UInt64 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
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
        [CLSCompliant(false)]
        public static string ToDelimitedString(this IEnumerable<ulong> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.UInt64, 256);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
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
        [CLSCompliant(false)]
        public static string ToDelimitedString(this IEnumerable<ushort> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Action<StringBuilder, ushort> UInt16 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
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
        [CLSCompliant(false)]
        public static string ToDelimitedString(this IEnumerable<ushort> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.UInt16, 256);
        }


        static partial class StringBuilderAppenders {}
    }
}
