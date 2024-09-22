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

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Returns a tuple with the cardinality of the sequence and the
        /// single element in the sequence if it contains exactly one element.
        /// similar to <see cref="Enumerable.Single{T}(IEnumerable{T})"/>.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="zero">
        /// The value that is returned in the tuple if the sequence has zero
        /// elements.</param>
        /// <param name="one">
        /// The value that is returned in the tuple if the sequence has a
        /// single element only.</param>
        /// <param name="many">
        /// The value that is returned in the tuple if the sequence has two or
        /// more elements.</param>
        /// <typeparam name="T">
        /// The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TCardinality">
        /// The type that expresses cardinality.</typeparam>
        /// <returns>
        /// A tuple containing the identified <typeparamref name="TCardinality"/>
        /// and either the single value of <typeparamref name="T"/> in the sequence
        /// or its default value.</returns>
        /// <remarks>
        /// This operator uses immediate execution, but never consumes more
        /// than two elements from the sequence.
        /// </remarks>

        public static (TCardinality Cardinality, T? Value)
            TrySingle<T, TCardinality>(this IEnumerable<T> source,
                TCardinality zero, TCardinality one, TCardinality many) =>
            TrySingle(source, zero, one, many, ValueTuple.Create);

        /// <summary>
        /// Returns a result projected from the the cardinality of the sequence
        /// and the single element in the sequence if it contains exactly one
        /// element.
        /// </summary>
        /// <param name="source">The source sequence.</param>
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
        /// A function that receives the cardinality and, if the
        /// sequence has just one element, the value of that element as
        /// argument and projects a resulting value of type
        /// <typeparamref name="TResult"/>.</param>
        /// <typeparam name="T">
        /// The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TCardinality">
        /// The type that expresses cardinality.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the result value returned by the
        /// <paramref name="resultSelector"/> function. </typeparam>
        /// <returns>
        /// The value returned by <paramref name="resultSelector"/>.
        /// </returns>
        /// <remarks>
        /// This operator uses immediate execution, but never consumes more
        /// than two elements from the sequence.
        /// </remarks>

        public static TResult TrySingle<T, TCardinality, TResult>(this IEnumerable<T> source,
            TCardinality zero, TCardinality one, TCardinality many,
            Func<TCardinality, T?, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            switch (source.TryAsCollectionLike())
            {
                case { Count: 0 }:
                    return resultSelector(zero, default);
                case { Count: 1 }:
                {
                    var item = source switch
                    {
                        IReadOnlyList<T> list => list[0],
                        IList<T> list => list[0],
                        _ => source.First()
                    };
                    return resultSelector(one, item);
                }
                case not null:
                    return resultSelector(many, default);
                default:
                {
                    using var e = source.GetEnumerator();
                    if (!e.MoveNext())
                        return resultSelector(zero, default);
                    var current = e.Current;
                    return !e.MoveNext() ? resultSelector(one, current)
                                         : resultSelector(many, default);
                }
            }
        }
    }
}
