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

#nullable enable // required for auto-generated sources (see below why)

// > Older code generation strategies may not be nullable aware. Setting the
// > project-level nullable context to "enable" could result in many
// > warnings that a user is unable to fix. To support this scenario any syntax
// > tree that is determined to be generated will have its nullable state
// > implicitly set to "disable", regardless of the overall project state.
//
// Source: https://github.com/dotnet/roslyn/blob/70e158ba6c2c99bd3c3fc0754af0dbf82a6d353d/docs/features/nullable-reference-types.md#generated-code

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
            return ToDelimitedStringImpl(source, delimiter, static (sb, e) => sb.Append(e));
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
            return ToDelimitedStringImpl(source, delimiter, static (sb, e) => sb.Append(e));
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
            return ToDelimitedStringImpl(source, delimiter, static (sb, e) => sb.Append(e));
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
            return ToDelimitedStringImpl(source, delimiter, static (sb, e) => sb.Append(e));
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
            return ToDelimitedStringImpl(source, delimiter, static (sb, e) => sb.Append(e));
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
            return ToDelimitedStringImpl(source, delimiter, static (sb, e) => sb.Append(e));
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
            return ToDelimitedStringImpl(source, delimiter, static (sb, e) => sb.Append(e));
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
            return ToDelimitedStringImpl(source, delimiter, static (sb, e) => sb.Append(e));
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
            return ToDelimitedStringImpl(source, delimiter, static (sb, e) => sb.Append(e));
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
            return ToDelimitedStringImpl(source, delimiter, static (sb, e) => sb.Append(e));
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
            return ToDelimitedStringImpl(source, delimiter, static (sb, e) => sb.Append(e));
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
            return ToDelimitedStringImpl(source, delimiter, static (sb, e) => sb.Append(e));
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
            return ToDelimitedStringImpl(source, delimiter, static (sb, e) => sb.Append(e));
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
            return ToDelimitedStringImpl(source, delimiter, static (sb, e) => sb.Append(e));
        }
    }
}
