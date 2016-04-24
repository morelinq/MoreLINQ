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

    partial class MoreEnumerable
    {

        /// <summary>
        /// Returns the minimal element of the given sequence, based on
        /// the given projection.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal projected value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector) {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            return MinByOrFallbackImpl(source, selector, null, false, default(TSource));
        }

        /// <summary>
        /// Returns the minimal element of the given sequence, based on
        /// the given projection and the specified comparer for projected values.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal projected value, the first
        /// one encountered will be returned. This operator uses immediate execution, but
        /// only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values.  
        /// If <c>null</c>, uses default comparer for TKey</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
        /// or <paramref name="comparer"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector,
            IComparer<TKey> comparer) {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return MinByOrFallbackImpl(source, selector, comparer, false, default(TSource));
        }

        /// <summary>
        /// Returns the minimal element of the given sequence, based on
        /// the given projection, or a fallback value if sequence is empty.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal projected value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="fallback">Value to return if source sequence is empty.</param>
        /// <returns>The minimal element, according to the projection, or fallback if empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
        public static TSource MinByOrFallback<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector,
            TSource fallback) {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return MinByOrFallbackImpl(source, selector, null, true, fallback);
        }

        /// <summary>
        /// Returns the minimal element of the given sequence, based on
        /// the given projection and the specified comparer for projected values,
        /// or the given fallback value if the sequence is empty. 
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal projected value, the first
        /// one encountered will be returned. This operator uses immediate execution, but
        /// only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values.  
        /// If <c>null</c>, uses default comparer for TKey</param>
        /// <param name="fallback">Value to return if source sequence is empty.</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
        /// or <paramref name="comparer"/> is null</exception>
        public static TSource MinByOrFallback<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector,
            IComparer<TKey> comparer,
            TSource fallback) {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return MinByOrFallbackImpl(source, selector, comparer, true, fallback);
        }

        /// <summary>
        /// Returns the minimal element of the given sequence, 
        /// or a fallback value if sequence is empty.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="fallback">Value to return if source sequence is empty.</param>
        /// <returns>The minimal element, or fallback if empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        public static TSource MinOrFallback<TSource>(this IEnumerable<TSource> source,
            TSource fallback) {

            if (source == null) throw new ArgumentNullException("source");
            return MinOrFallbackImpl(source, null, true, fallback);
        }

        /// <summary>
        /// Returns the minimal element of the given sequence, 
        /// based on the specified comparer,
        /// or a fallback value if sequence is empty.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="comparer">Comparer to use to compare projected values.  
        /// If <c>null</c>, uses default comparer for TKey</param>
        /// <param name="fallback">Value to return if source sequence is empty.</param>
        /// <returns>The minimal element, or fallback if empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        public static TSource MinOrFallback<TSource>(this IEnumerable<TSource> source,
            IComparer<TSource> comparer,
            TSource fallback) {

            if (source == null) throw new ArgumentNullException("source");
            return MinOrFallbackImpl(source, comparer, true, fallback);
        }

        private static TSource MinByOrFallbackImpl<TSource, TKey>(IEnumerable<TSource> source,
            Func<TSource, TKey> selector,
            IComparer<TKey> comparer,
            bool fallbackIfEmpty,
            TSource fallback) {
            
            comparer = comparer ?? Comparer<TKey>.Default;

            using (var sourceIterator = source.GetEnumerator()) {
                if (!sourceIterator.MoveNext()) {
                    if (fallbackIfEmpty) {
                        return fallback;
                    }
                    else {
                        throw new InvalidOperationException("Sequence contains no elements");
                    }
                }
                var min = sourceIterator.Current;
                var minKey = selector(min);
                while (sourceIterator.MoveNext()) {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0) {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }

        private static TSource MinOrFallbackImpl<TSource>(IEnumerable<TSource> source,
            IComparer<TSource> comparer,
            bool fallbackIfEmpty,
            TSource fallback) {

            comparer = comparer ?? Comparer<TSource>.Default;

            using (var sourceIterator = source.GetEnumerator()) {
                if (!sourceIterator.MoveNext()) {
                    if (fallbackIfEmpty) {
                        return fallback;
                    }
                    else {
                        throw new InvalidOperationException("Sequence contains no elements");
                    }
                }
                var min = sourceIterator.Current;
                while (sourceIterator.MoveNext()) {
                    var candidate = sourceIterator.Current;
                    if (comparer.Compare(candidate, min) < 0) {
                        min = candidate;
                    }
                }
                return min;
            }
        }
    }
}
