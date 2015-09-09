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
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;

    static partial class MoreEnumerable
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
            if (source == null) throw new ArgumentNullException("source");
            return ToDelimitedStringImpl(source, delimiter, (sb, e) => sb.Append(e));
        }

        static string ToDelimitedStringImpl<T>(IEnumerable<T> source, string delimiter, Func<StringBuilder, T, StringBuilder> append)
        {
            Debug.Assert(source != null);
            Debug.Assert(append != null);

            delimiter = delimiter ?? CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            var sb = new StringBuilder();
            var i = 0;

            foreach (var value in source)
            {
                if (i++ > 0) sb.Append(delimiter);
                append(sb, value);
            }

            return sb.ToString();
        }
    }
}
