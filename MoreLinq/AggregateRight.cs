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
        /// Applies a right-associative accumulator function over a sequence.
        /// This operator is the right-associative version of the
        /// <see cref="Enumerable.Aggregate{TSource}(IEnumerable{TSource}, Func{TSource, TSource, TSource})"/> LINQ operator.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
        /// <returns>The final accumulator value.</returns>
        /// <example>
        /// <code><![CDATA[
        /// string result = Enumerable.Range(1, 5).Select(i => i.ToString()).AggregateRight((a, b) => $"({a}/{b})");
        /// ]]></code>
        /// The <c>result</c> variable will contain <c>"(1/(2/(3/(4/5))))"</c>.
        /// </example>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TSource AggregateRight<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return source.ToListLike() switch
            {
                { Count: 0 } => throw new InvalidOperationException("Sequence contains no elements."),
                var list => AggregateRightImpl(list, list[^1], func, list.Count - 1)
            };
        }

        /// <summary>
        /// Applies a right-associative accumulator function over a sequence.
        /// The specified seed value is used as the initial accumulator value.
        /// This operator is the right-associative version of the
        /// <see cref="Enumerable.Aggregate{TSource, TAccumulate}(IEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate})"/> LINQ operator.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
        /// <returns>The final accumulator value.</returns>
        /// <example>
        /// <code><![CDATA[
        /// var numbers = Enumerable.Range(1, 5);
        /// string result = numbers.AggregateRight("6", (a, b) => $"({a}/{b})");
        /// ]]></code>
        /// The <c>result</c> variable will contain <c>"(1/(2/(3/(4/(5/6)))))"</c>.
        /// </example>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TAccumulate AggregateRight<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, TAccumulate> func)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));

            var list = source.ToListLike();

            return AggregateRightImpl(list, seed, func, list.Count);
        }

        /// <summary>
        /// Applies a right-associative accumulator function over a sequence.
        /// The specified seed value is used as the initial accumulator value,
        /// and the specified function is used to select the result value.
        /// This operator is the right-associative version of the
        /// <see cref="Enumerable.Aggregate{TSource, TAccumulate, TResult}(IEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate}, Func{TAccumulate, TResult})"/> LINQ operator.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
        /// <param name="resultSelector">A function to transform the final accumulator value into the result value.</param>
        /// <returns>The transformed final accumulator value.</returns>
        /// <example>
        /// <code><![CDATA[
        /// var numbers = Enumerable.Range(1, 5);
        /// int result = numbers.AggregateRight("6", (a, b) => $"({a}/{b})", str => str.Length);
        /// ]]></code>
        /// The <c>result</c> variable will contain <c>21</c>.
        /// </example>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TResult AggregateRight<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return resultSelector(source.AggregateRight(seed, func));
        }

        static TResult AggregateRightImpl<TSource, TResult>(ListLike<TSource> list, TResult accumulator, Func<TSource, TResult, TResult> func, int i)
        {
            while (i-- > 0)
            {
                accumulator = func(list[i], accumulator);
            }

            return accumulator;
        }
    }
}
