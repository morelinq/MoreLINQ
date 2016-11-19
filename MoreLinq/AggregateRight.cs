#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Leandro F. Vieira (leandromoh). All rights reserved.
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
        /// Applies an right-associative accumulator function over a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="func">An right-associative accumulator function to be invoked on each element.</param>
        /// <returns>The final accumulator value.</returns>
        /// <example>
        /// <code>
        /// var numbers = new int[] { 8, 12, 24, 4, 2 };
        /// int result = numbers.AggregateRight((a, b) => a / b);
        /// </code>
        /// The <c>result</c> variable will contain <c>8</c>.
        /// </example>
        public static TSource AggregateRight<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (func == null) throw new ArgumentNullException("func");
            if (!source.Any()) throw new InvalidOperationException("Sequence contains no elements");

            IList<TSource> e = (source as IList<TSource>) ?? source.ToArray();

            return AggregateRightImp(e, e.Last(), func, e.Count - 1);
        }

        /// <summary>
        /// Applies an right-associative accumulator function over a sequence.
        /// The specified seed value is used as the initial accumulator value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An right-associative accumulator function to be invoked on each element.</param>
        /// <returns>The final accumulator value.</returns>
        /// <example>
        /// <code>
        /// var numbers = Enumerable.Range(1, 5);
        /// string result = numbers.AggregateRight("6", (a, b) => string.Format("({0}/{1})", a, b));
        /// </code>
        /// The <c>result</c> variable will contain <c>"(1/(2/(3/(4/(5/6)))))"</c>.
        /// </example>
        public static TAccumulate AggregateRight<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, TAccumulate> func)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (func == null) throw new ArgumentNullException("func");

            IList<TSource> e = (source as IList<TSource>) ?? source.ToArray();

            return AggregateRightImp(e, seed, func, e.Count);
        }

        /// <summary>
        /// Applies an right-associative accumulator function over a sequence.
        /// The specified seed value is used as the initial accumulator value, 
        /// and the specified function is used to select the result value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An right-associative accumulator function to be invoked on each element.</param>
        /// <param name="resultSelector">A function to transform the final accumulator value into the result value.</param>
        /// <returns>The transformed final accumulator value.</returns>
        /// <example>
        /// <code>
        /// var numbers = Enumerable.Range(1, 5);
        /// int result = numbers.AggregateRight("6", (a, b) => string.Format("({0}/{1})", a, b), str => str.Length);
        /// </code>
        /// The <c>result</c> variable will contain <c>21</c>.
        /// </example>
        public static TResult AggregateRight<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (func == null) throw new ArgumentNullException("func");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            return resultSelector(source.AggregateRight(seed, func));
        }

        private static TResult AggregateRightImp<TSource, TResult>(IList<TSource> e, TResult current, Func<TSource, TResult, TResult> func, int i)
        {
            while (i-- > 0)
            {
                current = func(e[i], current);
            }

            return current;
        }
    }
}
