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

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        /// <remarks>
        /// This operator uses immediate execution, but never consumes more
        /// than two elements from the sequence. When the source sequence is an
        /// <see cref="IList{T}"/> or <see cref="ICollection{T}"/> then the
        /// implementation optimizes by checking the number of elements in
        /// the underlying sequence.</remarks>

        public static TResult TrySingle<T, TCardinality, TResult>(this IEnumerable<T> source,
            TCardinality zero, TCardinality one, TCardinality many,
            Func<TCardinality, T, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            switch (source)
            {
                case IReadOnlyCollection<T> readOnlyCollection:
                    return TrySingleForReadOnlyCollection(readOnlyCollection);
                case ICollection<T> collection:
                    return TrySingleForCollection(collection);
                default:
                    return TrySingleForEnumerable(source);
            }

            TResult TrySingleForReadOnlyCollection(IReadOnlyCollection<T> theCollection)
            {
                switch (theCollection.Count)
                {
                    case 0:
                        return resultSelector(zero, default);
                    case 1:
                        return resultSelector(one,
                            theCollection is IReadOnlyList<T> theList ? theList[0]
                                                                      : theCollection.First());
                    default:
                        return resultSelector(many, default);
                }
            }

            TResult TrySingleForCollection(ICollection<T> theCollection)
            {
                switch (theCollection.Count)
                {
                    case 0:
                        return resultSelector(zero, default);
                    case 1:
                        return resultSelector(one,
                            theCollection is IList<T> theList ? theList[0]
                                                              : theCollection.First());
                    default:
                        return resultSelector(many, default);
                }
            }

            TResult TrySingleForEnumerable(IEnumerable<T> theEnumerable)
            {
                using (var e = theEnumerable.GetEnumerator())
                {
                    if (!e.MoveNext())
                        return resultSelector(zero, default);
                    var current = e.Current;
                    return !e.MoveNext() ? resultSelector(one, current)
                                         : resultSelector(many, default);
                }
            }
        }

        /// <summary>
        /// Attempts to retrieve and project the only element of a sequence,
        /// similar to <see cref="Enumerable.Single{T}(IEnumerable{T})"/>.
        /// Unlike that extension method if the sequence contains zero or many
        /// elements this operator doesn't throw an exception, rather it returns
        /// a tuple containing the observed cardinality of the sequence and
        /// either the single value (if only one element in the sequence) or
        /// the default value of <typeparamref name="T"/>. This enables the
        /// caller to differentiate between the case where a sequence contains
        /// zero elements and many elements.</summary>
        /// <param name="source">
        /// The source sequence that will be tested for its cardinality.</param>
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
        /// The type of the elements of the sequence.</typeparam>
        /// <typeparam name="TCardinality">
        /// The type that expresses cardinality.</typeparam>
        /// <returns>
        /// A tuple containing the identified <typeparamref name="TCardinality"/>
        /// and either the single value of <typeparamref name="T"/> in the sequence
        /// or its default value.</returns>
        /// <remarks>
        /// This operator uses immediate execution, but never consumes more
        /// than two elements from the sequence. When the source sequence is an
        /// <see cref="IList{T}"/> or <see cref="ICollection{T}"/> then the
        /// implementation optimizes by checking the number of elements in
        /// the underlying sequence.</remarks>

        public static (TCardinality Cardinality, T Value)
            TrySingle<T, TCardinality>(this IEnumerable<T> source,
                TCardinality zero,
                TCardinality one,
                TCardinality many) =>
            TrySingle(source, zero, one, many, ValueTuple.Create);
    }
}
