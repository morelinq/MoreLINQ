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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Pads a sequence with default values in the beginning if it is narrower (shorter
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
        /// <code><![CDATA[
        /// int[] numbers = { 123, 456, 789 };
        /// var result = numbers.PadStart(5);
        /// ]]></code>
        /// The <c>result</c> variable will contain <c>{ 0, 0, 123, 456, 789 }</c>.
        /// </example>

        public static IEnumerable<TSource?> PadStart<TSource>(this IEnumerable<TSource> source, int width)
        {
            return PadStart(source, width, default(TSource));
        }

        /// <summary>
        /// Pads a sequence with a given filler value in the beginning if it is narrower (shorter
        /// in length) than a given width.
        /// An additional parameter specifies the value to use for padding.
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
        /// <code><![CDATA[
        /// int[] numbers = { 123, 456, 789 };
        /// var result = numbers.PadStart(5, -1);
        /// ]]></code>
        /// The <c>result</c> variable will contain <c>{ -1, -1, 123, 456, 789 }</c>.
        /// </example>

        public static IEnumerable<TSource> PadStart<TSource>(this IEnumerable<TSource> source, int width, TSource padding)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (width < 0) throw new ArgumentException(null, nameof(width));
            return PadStartImpl(source, width, padding, null);
        }

        /// <summary>
        /// Pads a sequence with a dynamic filler value in the beginning if it is narrower (shorter
        /// in length) than a given width.
        /// An additional parameter specifies the function to calculate padding.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to pad.</param>
        /// <param name="width">The width/length below which to pad.</param>
        /// <param name="paddingSelector">
        /// Function to calculate padding given the index of the missing element.
        /// </param>
        /// <returns>
        /// Returns a sequence that is at least as wide/long as the width/length
        /// specified by the <paramref name="width"/> parameter.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// int[] numbers = { 123, 456, 789 };
        /// var result = numbers.PadStart(6, i => -i);
        /// ]]></code>
        /// The <c>result</c> variable will contain <c>{ 0, -1, -2, 123, 456, 789 }</c>.
        /// </example>

        public static IEnumerable<TSource> PadStart<TSource>(this IEnumerable<TSource> source, int width, Func<int, TSource> paddingSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (paddingSelector == null) throw new ArgumentNullException(nameof(paddingSelector));
            if (width < 0) throw new ArgumentException(null, nameof(width));
            return PadStartImpl(source, width, default, paddingSelector);
        }

        static IEnumerable<T> PadStartImpl<T>(IEnumerable<T> source,
            int width, T? padding, Func<int, T>? paddingSelector)
        {
            return _(); IEnumerable<T> _()
            {
                if (source.TryAsCollectionLike() is { Count: var collectionCount } && collectionCount < width)
                {
                    var paddingCount = width - collectionCount;
                    for (var i = 0; i < paddingCount; i++)
                        yield return paddingSelector is { } selector ? selector(i) : padding!;

                    foreach (var item in source)
                        yield return item;
                }
                else
                {
                    var array = new T[width];
                    var count = 0;

                    using (var e = source.GetEnumerator())
                    {
                        for (; count < width && e.MoveNext(); count++)
                            array[count] = e.Current;

                        if (count == width)
                        {
                            for (var i = 0; i < count; i++)
                                yield return array[i];

                            while (e.MoveNext())
                                yield return e.Current;

                            yield break;
                        }
                    }

                    var len = width - count;

                    for (var i = 0; i < len; i++)
                        yield return paddingSelector != null ? paddingSelector(i) : padding!;

                    for (var i = 0; i < count; i++)
                        yield return array[i];
                }
            }
        }
    }
}
