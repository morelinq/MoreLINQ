#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2012 Atif Aziz. All rights reserved.
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
    using System.Linq;

    static partial class MoreEnumerable
    {

        /// <summary>
        /// Splits the source sequence by a separator.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="separator">Separator element.</param>
        /// <returns>A sequence of splits of elements.</returns>

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source,
            TSource separator)
        {
            return Split(source, separator, int.MaxValue);
        }

        /// <summary>
        /// Splits the source sequence by a separator given a maximum count of splits.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="separator">Separator element.</param>
        /// <param name="count">Maximum number of splits.</param>
        /// <returns>A sequence of splits of elements.</returns>

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source,
            TSource separator, int count)
        {
            return Split(source, separator, count, s => s);
        }

        /// <summary>
        /// Splits the source sequence by a separator and then transforms
        /// the splits into results.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result sequence elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="separator">Separator element.</param>
        /// <param name="resultSelector">Function used to project splits
        /// of source elements into elements of the resulting sequence.</param>
        /// <returns>
        /// A sequence of values typed as <typeparamref name="TResult"/>.
        /// </returns>

        public static IEnumerable<TResult> Split<TSource, TResult>(this IEnumerable<TSource> source,
            TSource separator,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            return Split(source, separator, int.MaxValue, resultSelector);
        }

        /// <summary>
        /// Splits the source sequence by a separator, given a maximum count
        /// of splits, and then transforms the splits into results.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result sequence elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="separator">Separator element.</param>
        /// <param name="count">Maximum number of splits.</param>
        /// <param name="resultSelector">Function used to project splits
        /// of source elements into elements of the resulting sequence.</param>
        /// <returns>
        /// A sequence of values typed as <typeparamref name="TResult"/>.
        /// </returns>

        public static IEnumerable<TResult> Split<TSource, TResult>(this IEnumerable<TSource> source,
            TSource separator, int count,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            return Split(source, separator, null, count, resultSelector);
        }

        /// <summary>
        /// Splits the source sequence by a separator and then transforms the
        /// splits into results.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="separator">Separator element.</param>
        /// <param name="comparer">Comparer used to determine separator
        /// element equality.</param>
        /// <returns>A sequence of splits of elements.</returns>

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source,
            TSource separator, IEqualityComparer<TSource> comparer)
        {
            return Split(source, separator, comparer, int.MaxValue);
        }

        /// <summary>
        /// Splits the source sequence by a separator, given a maximum count
        /// of splits. A parameter specifies how the separator is compared
        /// for equality.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="separator">Separator element.</param>
        /// <param name="comparer">Comparer used to determine separator
        /// element equality.</param>
        /// <param name="count">Maximum number of splits.</param>
        /// <returns>A sequence of splits of elements.</returns>

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source,
            TSource separator, IEqualityComparer<TSource> comparer, int count)
        {
            return Split(source, separator, comparer, count, s => s);
        }

        /// <summary>
        /// Splits the source sequence by a separator and then transforms the
        /// splits into results. A parameter specifies how the separator is
        /// compared for equality.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result sequence elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="separator">Separator element.</param>
        /// <param name="comparer">Comparer used to determine separator
        /// element equality.</param>
        /// <param name="resultSelector">Function used to project splits
        /// of source elements into elements of the resulting sequence.</param>
        /// <returns>
        /// A sequence of values typed as <typeparamref name="TResult"/>.
        /// </returns>

        public static IEnumerable<TResult> Split<TSource, TResult>(this IEnumerable<TSource> source,
            TSource separator, IEqualityComparer<TSource> comparer,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            return Split(source, separator, comparer, int.MaxValue, resultSelector);
        }

        /// <summary>
        /// Splits the source sequence by a separator, given a maximum count
        /// of splits, and then transforms the splits into results. A
        /// parameter specifies how the separator is compared for equality.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result sequence elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="separator">Separator element.</param>
        /// <param name="comparer">Comparer used to determine separator
        /// element equality.</param>
        /// <param name="count">Maximum number of splits.</param>
        /// <param name="resultSelector">Function used to project splits
        /// of source elements into elements of the resulting sequence.</param>
        /// <returns>
        /// A sequence of values typed as <typeparamref name="TResult"/>.
        /// </returns>

        public static IEnumerable<TResult> Split<TSource, TResult>(this IEnumerable<TSource> source,
            TSource separator, IEqualityComparer<TSource> comparer, int count,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (count <= 0) throw new ArgumentOutOfRangeException("count");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");
            return SplitImpl(source, separator, comparer ?? EqualityComparer<TSource>.Default, count, resultSelector);
        }

        private static IEnumerable<TResult> SplitImpl<TSource, TResult>(IEnumerable<TSource> source,
            TSource separator, IEqualityComparer<TSource> comparer, int count,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            Debug.Assert(source != null);
            Debug.Assert(comparer != null);
            Debug.Assert(count >= 0);
            Debug.Assert(resultSelector != null);

            return Split(source, item => comparer.Equals(item, separator), count, resultSelector);
        }

        /// <summary>
        /// Splits the source sequence by separator elements identified by a
        /// function.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="separatorFunc">Predicate function used to determine
        /// the splitter elements in the source sequence.</param>
        /// <returns>A sequence of splits of elements.</returns>

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> separatorFunc)
        {
            return Split(source, separatorFunc, int.MaxValue);
        }

        /// <summary>
        /// Splits the source sequence by separator elements identified by a
        /// function, given a maximum count of splits.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="separatorFunc">Predicate function used to determine
        /// the splitter elements in the source sequence.</param>
        /// <param name="count">Maximum number of splits.</param>
        /// <returns>A sequence of splits of elements.</returns>

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> separatorFunc, int count)
        {
            return Split(source, separatorFunc, count, s => s);
        }

        /// <summary>
        /// Splits the source sequence by separator elements identified by
        /// a function and then transforms the splits into results.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result sequence elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="separatorFunc">Predicate function used to determine
        /// the splitter elements in the source sequence.</param>
        /// <param name="resultSelector">Function used to project splits
        /// of source elements into elements of the resulting sequence.</param>
        /// <returns>
        /// A sequence of values typed as <typeparamref name="TResult"/>.
        /// </returns>

        public static IEnumerable<TResult> Split<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, bool> separatorFunc,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            return Split(source, separatorFunc, int.MaxValue, resultSelector);
        }

        /// <summary>
        /// Splits the source sequence by separator elements identified by
        /// a function, given a maximum count of splits, and then transforms
        /// the splits into results.
        /// </summary>
        /// <typeparam name="TSource">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result sequence elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="separatorFunc">Predicate function used to determine
        /// the splitter elements in the source sequence.</param>
        /// <param name="count">Maximum number of splits.</param>
        /// <param name="resultSelector">Function used to project a split
        /// group of source elements into an element of the resulting sequence.</param>
        /// <returns>
        /// A sequence of values typed as <typeparamref name="TResult"/>.
        /// </returns>

        public static IEnumerable<TResult> Split<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, bool> separatorFunc, int count,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (separatorFunc == null) throw new ArgumentNullException("separatorFunc");
            if (count <= 0) throw new ArgumentOutOfRangeException("count");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");
            return SplitImpl(source, separatorFunc, count, resultSelector);
        }

        private static IEnumerable<TResult> SplitImpl<TSource, TResult>(IEnumerable<TSource> source,
            Func<TSource, bool> separatorFunc, int count,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            Debug.Assert(source != null);
            Debug.Assert(separatorFunc != null);
            Debug.Assert(count >= 0);
            Debug.Assert(resultSelector != null);

            if (count == 0) // No splits?
            {
                yield return resultSelector(source);
            }
            else
            {
                List<TSource> items = null;

                foreach (var item in source)
                {
                    if (count > 0 && separatorFunc(item))
                    {
                        yield return resultSelector(items ?? Enumerable.Empty<TSource>());
                        count--;
                        items = null;
                    }
                    else
                    {
                        if (items == null)
                            items = new List<TSource>();

                        items.Add(item);
                    }
                }

                if (items != null && items.Count > 0)
                    yield return resultSelector(items);
            }
        }
    }
}
