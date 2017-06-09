#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Leandro F. Vieira (leandromoh). All rights reserved.
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

    static partial class MoreEnumerable
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
        /// IEnumerable&lt;int&gt; result = numbers.PadLeft(5);
        /// </code>
        /// The <c>result</c> variable, when iterated over, will yield 
        /// 123, 456, 789 and two zeroes, in turn.
        /// </example>

        public static IEnumerable<TSource> PadLeft<TSource>(this IEnumerable<TSource> source, int width)
        {
            return PadLeft(source, width, default(TSource));
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
        /// IEnumerable&lt;int&gt; result = numbers.PadLeft(5, -1);
        /// </code>
        /// The <c>result</c> variable, when iterated over, will yield 
        /// 123, 456, and 789 followed by two occurrences of -1, in turn.
        /// </example>

        public static IEnumerable<TSource> PadLeft<TSource>(this IEnumerable<TSource> source, int width, TSource padding)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (width < 0) throw new ArgumentException(null, nameof(width));
            return PadLeftImpl(source, width, padding, null);
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
        /// IEnumerable&lt;int&gt; result = numbers.PadLeft(5, i => -i);
        /// </code>
        /// The <c>result</c> variable, when iterated over, will yield 
        /// 0, 1, 2, -3 and -4, in turn.
        /// </example>

        public static IEnumerable<TSource> PadLeft<TSource>(this IEnumerable<TSource> source, int width, Func<int, TSource> paddingSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (paddingSelector == null) throw new ArgumentNullException(nameof(paddingSelector));
            if (width < 0) throw new ArgumentException(null, nameof(width));
            return PadLeftImpl(source, width, default(TSource), paddingSelector);
        }

        private static IEnumerable<T> PadLeftImpl<T>(IEnumerable<T> source,
            int width, T padding, Func<int, T> paddingSelector)
        {
            using (var e = source.GetEnumerator())
            {
                var list = new List<T>(width);

                for (int i = 0; i < width && e.MoveNext(); i++)
                {
                    list.Add(e.Current);
                }

                if (list.Count < width)
                {
                    var len = width - list.Count;

                    for (int i = 0; i < len; i++)
                        yield return paddingSelector != null ? paddingSelector(i) : padding;

                    foreach (var item in list)
                        yield return item;
                }
                else
                {
                    foreach (var item in list)
                        yield return item;

                    while (e.MoveNext())
                        yield return e.Current;
                }
            }

        }
    }
}