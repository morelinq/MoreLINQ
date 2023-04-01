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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Traces the elements of a source sequence for diagnostics.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
        /// <param name="source">Source sequence whose elements to trace.</param>
        /// <returns>
        /// Return the source sequence unmodified.
        /// </returns>
        /// <remarks>
        /// This a pass-through operator that uses deferred execution and
        /// streams the results.
        /// </remarks>

        public static IEnumerable<TSource> Trace<TSource>(this IEnumerable<TSource> source)
        {
            return Trace(source, (string?)null);
        }

        /// <summary>
        /// Traces the elements of a source sequence for diagnostics using
        /// custom formatting.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
        /// <param name="source">Source sequence whose elements to trace.</param>
        /// <param name="format">
        /// String to use to format the trace message. If null then the
        /// element value becomes the traced message.
        /// </param>
        /// <returns>
        /// Return the source sequence unmodified.
        /// </returns>
        /// <remarks>
        /// This a pass-through operator that uses deferred execution and
        /// streams the results.
        /// </remarks>

        public static IEnumerable<TSource> Trace<TSource>(this IEnumerable<TSource> source, string? format)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return TraceImpl(source, string.IsNullOrEmpty(format)
                                     ? x => x?.ToString() ?? string.Empty
                                     : x => string.Format(null, format, x));
        }

        /// <summary>
        /// Traces the elements of a source sequence for diagnostics using
        /// a custom formatter.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
        /// <param name="source">Source sequence whose elements to trace.</param>
        /// <param name="formatter">Function used to format each source element into a string.</param>
        /// <returns>
        /// Return the source sequence unmodified.
        /// </returns>
        /// <remarks>
        /// This a pass-through operator that uses deferred execution and
        /// streams the results.
        /// </remarks>

        public static IEnumerable<TSource> Trace<TSource>(this IEnumerable<TSource> source, Func<TSource, string> formatter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            return TraceImpl(source, formatter);
        }

        static IEnumerable<TSource> TraceImpl<TSource>(IEnumerable<TSource> source, Func<TSource, string> formatter)
        {
            return source.Pipe(x => System.Diagnostics.Trace.WriteLine(formatter(x)));
        }
    }
}
