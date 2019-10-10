#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 James Webster. All rights reserved.
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

using System.Linq;

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    partial class MoreEnumerable
    {
        /// <summary>
        /// Attempts to retrieve and project the only element of a sequence,
        /// similar to <see cref="Enumerable.Single{T}(IEnumerable{T})"/>.
        /// Unlike that extension method if the sequence contains zero or many
        /// elements this operator doesn't throw an exception, rather it provides
        /// the <paramref name="resultSelector"/> function with the observed
        /// cardinality. This enables the caller to differentiate between the
        /// case where a sequence contains zero elements and many elements.
        /// </summary>
        /// <param name="source">
        /// The source sequence that will be tested for its cardinality.</param>
        /// <param name="zero">
        /// The value that is passed as the first argument to
        /// <paramref name="resultSelector" /> if the sequence has zero
        /// elements.</param>
        /// <param name="one">
        /// The value that is passed as the first argument to
        /// <paramref name="resultSelector" /> if the sequence has a
        /// single element only.</param>
        /// <param name="many">
        /// The value that is passed as the first argument to
        /// <paramref name="resultSelector" /> if the sequence has two or
        /// more elements.</param>
        /// <param name="resultSelector">
        /// A function that is provided with the cardinality, and if the
        /// sequence has just one element, the value of that element. Then
        /// transforms the result to an instance of TResult.</param>
        /// <typeparam name="T">
        /// The type of the elements of the sequence.</typeparam>
        /// <typeparam name="TCardinality">
        /// The type that expresses cardinality.</typeparam>
        /// <typeparam name="TResult">
        /// The result type of the <paramref name="resultSelector"/> function.
        /// </typeparam>
        /// <returns>
        /// The value returned by the <paramref name="resultSelector"/>.
        /// </returns>

        public static TResult TrySingle<T, TCardinality, TResult>(this IEnumerable<T> source,
            TCardinality zero, TCardinality one, TCardinality many,
            Func<TCardinality, T, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                    return resultSelector(zero, default);
                var current = e.Current;
                return !e.MoveNext() ? resultSelector(one, current)
                                     : resultSelector(many, default);
            }
        }

        /// <summary>
        /// An overload of <see cref="TrySingle{T, TCardinality, TResult}(IEnumerable{T},TCardinality, TCardinality, TCardinality, Func{TCardinality, T, TResult})"/>
        /// that returns the observed cardinality and single element,
        /// if available, in a tuple.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="zero"></param>
        /// <param name="one"></param>
        /// <param name="many"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TCardinality"></typeparam>
        /// <returns></returns>
        public static (TCardinality Cardinality, T Value)
            TrySingle<T, TCardinality>(this IEnumerable<T> source,
                TCardinality zero,
                TCardinality one,
                TCardinality many) =>
            TrySingle(source, zero, one, many, ValueTuple.Create);
    }
}
