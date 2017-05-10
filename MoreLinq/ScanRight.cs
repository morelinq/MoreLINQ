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
    using System.Linq;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Peforms a right-associative scan (inclusive prefix) on a sequence of elements.
        /// This operator is the right-associative version of the 
        /// <see cref="MoreEnumerable.Scan{TSource}(IEnumerable{TSource}, Func{TSource, TSource, TSource})"/> LINQ operator.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="func">
        /// A right-associative accumulator function to be invoked on each element.
        /// Its first argument is the current value in the sequence; second argument is the previous accumulator value.
        /// </param>
        /// <returns>The scanned sequence.</returns>
        /// <example>
        /// <code>
        /// var result = Enumerable.Range(1, 5).Select(i => i.ToString()).ScanRight((a, b) => string.Format("({0}/{1})", a, b));
        /// </code>
        /// The <c>result</c> variable will contain <c>[ "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" ]</c>.
        /// </example>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TSource> ScanRight<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return ScanRightImpl(source, func);
        }

        private static IEnumerable<TSource> ScanRightImpl<TSource>(IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
        {
            var list = (source as IList<TSource>) ?? source.ToList();

            if (list.Count == 0)
                yield break;

            foreach (var item in ScanRightImpl(list, list.Last(), func, list.Count - 1))
                yield return item;
        }

        /// <summary>
        /// Peforms a right-associative scan (inclusive prefix) on a sequence of elements.
        /// The specified seed value is used as the initial accumulator value.
        /// This operator is the right-associative version of the 
        /// <see cref="MoreEnumerable.Scan{TSource, TState}(IEnumerable{TSource}, TState, Func{TState, TSource, TState})"/> LINQ operator.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
        /// <returns>The scanned sequence.</returns>
        /// <example>
        /// <code>
        /// var result = Enumerable.Range(1, 4).ScanRight("5", (a, b) => string.Format("({0}/{1})", a, b));
        /// </code>
        /// The <c>result</c> variable will contain <c>[ "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" ]</c>.
        /// </example>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TAccumulate> ScanRight<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, TAccumulate> func)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return ScanRightImpl(source, seed, func);
        }

        private static IEnumerable<TAccumulate> ScanRightImpl<TSource, TAccumulate>(IEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, TAccumulate> func)
        {
            var list = (source as IList<TSource>) ?? source.ToList();

            foreach (var item in ScanRightImpl(list, seed, func, list.Count))
                yield return item;
        }

        private static IEnumerable<TResult> ScanRightImpl<TSource, TResult>(IList<TSource> list, TResult accumulator, Func<TSource, TResult, TResult> func, int i)
        {
            var stack = new Stack<TResult>(i + 1);
            stack.Push(accumulator);

            while (i-- > 0)
            {
                accumulator = func(list[i], accumulator);
                stack.Push(accumulator);
            }

            return stack;
        }
    }
}
