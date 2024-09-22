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
    using System.Diagnostics.CodeAnalysis;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns the maximal elements of the given sequence, based on
        /// the given projection.
        /// </summary>
        /// <remarks>
        /// This overload uses the default comparer  for the projected type.
        /// This operator uses deferred execution. The results are evaluated
        /// and cached on first use to returned sequence.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The sequence of maximal elements, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>

        [Obsolete($"Use {nameof(ExtremaMembers.Maxima)} instead.")]
        [ExcludeFromCodeCoverage]
#if !NET6_0_OR_GREATER
        public static IExtremaEnumerable<TSource> MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
#else
        public static IExtremaEnumerable<TSource> MaxBy<TSource, TKey>(IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
#endif
        {
            return MaxBy(source, selector, null);
        }

        /// <summary>
        /// Returns the maximal elements of the given sequence, based on
        /// the given projection and the specified comparer for projected values.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution. The results are evaluated
        /// and cached on first use to returned sequence.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The sequence of maximal elements, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/>
        /// or <paramref name="comparer"/> is null</exception>

        [Obsolete($"Use {nameof(ExtremaMembers.Maxima)} instead.")]
        [ExcludeFromCodeCoverage]
#if !NET6_0_OR_GREATER
        public static IExtremaEnumerable<TSource> MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey>? comparer)
#else
        public static IExtremaEnumerable<TSource> MaxBy<TSource, TKey>(IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey>? comparer)
#endif
        {
            return source.Maxima(selector, comparer);
        }
    }
}
