#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Phillip Palk. All rights reserved.
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
        /// Returns a sequence resulting from applying a function to each
        /// element in the source sequence and its
        /// predecessor, with the exception of the first element which is
        /// only returned as the predecessor of the second element.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="resultSelector">A transform function to apply to each pair of <paramref name="source"/>.</param>
        /// <returns>Returns the resulting sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult> Tuplewise<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e = source.GetEnumerator();

                if (!e.MoveNext())
                    yield break;
                var predecessor1 = e.Current;

                while (e.MoveNext())
                {
                    yield return resultSelector(predecessor1, e.Current);
                    (predecessor1) = (e.Current);
                }
            }
        }

        /// <summary>
        /// Returns a sequence resulting from applying a function to each
        /// element in the source sequence and its
        /// 2 predecessors, with the exception of the first and second elements which are
        /// only returned as the predecessors of the third element.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="resultSelector">A transform function to apply to each triplet of <paramref name="source"/>.</param>
        /// <returns>Returns the resulting sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult> Tuplewise<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e = source.GetEnumerator();

                if (!e.MoveNext())
                    yield break;
                var predecessor1 = e.Current;

                if (!e.MoveNext())
                    yield break;
                var predecessor2 = e.Current;

                while (e.MoveNext())
                {
                    yield return resultSelector(predecessor1, predecessor2, e.Current);
                    (predecessor1, predecessor2) = (predecessor2, e.Current);
                }
            }
        }

        /// <summary>
        /// Returns a sequence resulting from applying a function to each
        /// element in the source sequence and its
        /// 3 predecessors, with the exception of the first 3 elements which are
        /// only returned as the predecessors of the 4th element.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="resultSelector">A transform function to apply to each 4-tuple of <paramref name="source"/>.</param>
        /// <returns>Returns the resulting sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult> Tuplewise<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource, TSource, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e = source.GetEnumerator();

                if (!e.MoveNext())
                    yield break;
                var predecessor1 = e.Current;

                if (!e.MoveNext())
                    yield break;
                var predecessor2 = e.Current;

                if (!e.MoveNext())
                    yield break;
                var predecessor3 = e.Current;

                while (e.MoveNext())
                {
                    yield return resultSelector(predecessor1, predecessor2, predecessor3, e.Current);
                    (predecessor1, predecessor2, predecessor3) = (predecessor2, predecessor3, e.Current);
                }
            }
        }

    }
}
