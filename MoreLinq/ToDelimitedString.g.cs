#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2013 Atif Aziz. All rights reserved.
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

#if !MORELINQ
//
// For projects that may include/embed this source file directly, suppress the
// following warnings since the hosting project may not require CLS compliance
// and MoreEnumerable will most probably be internal.
//
#pragma warning disable 3019 // CLS compliance checking will not be performed on 'type' because it is not visible from outside this assembly
#pragma warning disable 3021 // 'type' does not need a CLSCompliant attribute because the assembly does not have a CLSCompliant attribute
#endif

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    partial class MoreEnumerable
    {
        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>

        public static string ToDelimitedString(this IEnumerable<bool> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Boolean);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Func<StringBuilder, bool, StringBuilder> Boolean = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>

        public static string ToDelimitedString(this IEnumerable<byte> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Byte);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Func<StringBuilder, byte, StringBuilder> Byte = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>

        public static string ToDelimitedString(this IEnumerable<char> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Char);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Func<StringBuilder, char, StringBuilder> Char = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>

        public static string ToDelimitedString(this IEnumerable<decimal> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Decimal);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Func<StringBuilder, decimal, StringBuilder> Decimal = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>

        public static string ToDelimitedString(this IEnumerable<double> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Double);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Func<StringBuilder, double, StringBuilder> Double = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>

        public static string ToDelimitedString(this IEnumerable<float> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Single);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Func<StringBuilder, float, StringBuilder> Single = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>

        public static string ToDelimitedString(this IEnumerable<int> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Int32);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Func<StringBuilder, int, StringBuilder> Int32 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>

        public static string ToDelimitedString(this IEnumerable<long> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Int64);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Func<StringBuilder, long, StringBuilder> Int64 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        [CLSCompliant(false)]
        public static string ToDelimitedString(this IEnumerable<sbyte> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.SByte);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Func<StringBuilder, sbyte, StringBuilder> SByte = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>

        public static string ToDelimitedString(this IEnumerable<short> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Int16);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Func<StringBuilder, short, StringBuilder> Int16 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>

        public static string ToDelimitedString(this IEnumerable<string> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.String);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Func<StringBuilder, string, StringBuilder> String = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        [CLSCompliant(false)]
        public static string ToDelimitedString(this IEnumerable<uint> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.UInt32);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Func<StringBuilder, uint, StringBuilder> UInt32 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        [CLSCompliant(false)]
        public static string ToDelimitedString(this IEnumerable<ulong> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.UInt64);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Func<StringBuilder, ulong, StringBuilder> UInt64 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements.</param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="source"/>
        /// delimited by <paramref name="delimiter"/>. If the source sequence
        /// is empty, the method returns an empty string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        [CLSCompliant(false)]
        public static string ToDelimitedString(this IEnumerable<ushort> source, string delimiter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.UInt16);
        }

        static partial class StringBuilderAppenders
        {
            public static readonly Func<StringBuilder, ushort, StringBuilder> UInt16 = (sb, e) => sb.Append(e);
        }


        static partial class StringBuilderAppenders {}
    }
}
