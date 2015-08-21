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
    using System.Text;

    partial class MoreEnumerable
    {
        /// <summary>
        /// Creates a delimited string from a sequence of values. The 
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>

        public static string ToDelimitedString(this IEnumerable<string> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders 
        {
            public static readonly Func<StringBuilder, string, StringBuilder> String = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>

        public static string ToDelimitedString(this IEnumerable<string> source, string delimiter)
        {
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.String);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The 
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>

        public static string ToDelimitedString(this IEnumerable<bool> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders 
        {
            public static readonly Func<StringBuilder, bool, StringBuilder> Boolean = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>

        public static string ToDelimitedString(this IEnumerable<bool> source, string delimiter)
        {
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Boolean);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The 
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>

        public static string ToDelimitedString(this IEnumerable<sbyte> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders 
        {
            public static readonly Func<StringBuilder, sbyte, StringBuilder> SByte = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>

        public static string ToDelimitedString(this IEnumerable<sbyte> source, string delimiter)
        {
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.SByte);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The 
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>

        public static string ToDelimitedString(this IEnumerable<byte> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders 
        {
            public static readonly Func<StringBuilder, byte, StringBuilder> Byte = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>

        public static string ToDelimitedString(this IEnumerable<byte> source, string delimiter)
        {
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Byte);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The 
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>

        public static string ToDelimitedString(this IEnumerable<char> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders 
        {
            public static readonly Func<StringBuilder, char, StringBuilder> Char = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>

        public static string ToDelimitedString(this IEnumerable<char> source, string delimiter)
        {
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Char);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The 
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>

        public static string ToDelimitedString(this IEnumerable<short> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders 
        {
            public static readonly Func<StringBuilder, short, StringBuilder> Int16 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>

        public static string ToDelimitedString(this IEnumerable<short> source, string delimiter)
        {
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Int16);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The 
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>

        public static string ToDelimitedString(this IEnumerable<int> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders 
        {
            public static readonly Func<StringBuilder, int, StringBuilder> Int32 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>

        public static string ToDelimitedString(this IEnumerable<int> source, string delimiter)
        {
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Int32);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The 
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>

        public static string ToDelimitedString(this IEnumerable<long> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders 
        {
            public static readonly Func<StringBuilder, long, StringBuilder> Int64 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>

        public static string ToDelimitedString(this IEnumerable<long> source, string delimiter)
        {
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Int64);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The 
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>

        public static string ToDelimitedString(this IEnumerable<float> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders 
        {
            public static readonly Func<StringBuilder, float, StringBuilder> Single = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>

        public static string ToDelimitedString(this IEnumerable<float> source, string delimiter)
        {
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Single);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The 
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>

        public static string ToDelimitedString(this IEnumerable<double> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders 
        {
            public static readonly Func<StringBuilder, double, StringBuilder> Double = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>

        public static string ToDelimitedString(this IEnumerable<double> source, string delimiter)
        {
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Double);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The 
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>

        public static string ToDelimitedString(this IEnumerable<decimal> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders 
        {
            public static readonly Func<StringBuilder, decimal, StringBuilder> Decimal = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>

        public static string ToDelimitedString(this IEnumerable<decimal> source, string delimiter)
        {
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Decimal);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The 
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>

        public static string ToDelimitedString(this IEnumerable<ushort> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders 
        {
            public static readonly Func<StringBuilder, ushort, StringBuilder> UInt16 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>

        public static string ToDelimitedString(this IEnumerable<ushort> source, string delimiter)
        {
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.UInt16);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The 
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>

        public static string ToDelimitedString(this IEnumerable<uint> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders 
        {
            public static readonly Func<StringBuilder, uint, StringBuilder> UInt32 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>

        public static string ToDelimitedString(this IEnumerable<uint> source, string delimiter)
        {
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.UInt32);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values. The 
        /// delimiter used depends on the current culture of the executing thread.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>

        public static string ToDelimitedString(this IEnumerable<ulong> source)
        {
            return ToDelimitedString(source, null);
        }

        static partial class StringBuilderAppenders 
        {
            public static readonly Func<StringBuilder, ulong, StringBuilder> UInt64 = (sb, e) => sb.Append(e);
        }

        /// <summary>
        /// Creates a delimited string from a sequence of values and
        /// a given delimiter.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers the sequence.
        /// </remarks>
        /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
        /// simple ToString() conversion.</param>
        /// <param name="delimiter">The delimiter to inject between elements. May be null, in which case
        /// the executing thread's current culture's list separator is used.</param>

        public static string ToDelimitedString(this IEnumerable<ulong> source, string delimiter)
        {
            return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.UInt64);
        }


        static partial class StringBuilderAppenders {}
    }
}
