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

    partial class MoreEnumerable
    {
        /// <summary>
        /// Determines the cardinality of the source sequence, provides a selector method with this cardinality and
        /// the single element of the sequence if it contains one element only. Enables the caller to distinguish between
        /// sequences that have zero elements and those that have multiple elements, when the caller expects the sequence
        /// to contain a single element.
        /// </summary>
        /// <param name="source">The source sequence that will be tested for its cardinality.</param>
        /// <param name="zero">The value that should be provided to <paramref name="resultSelector" /> if the sequence has zero elements.</param>
        /// <param name="one">The value that should be provided to <paramref name="resultSelector" /> if the sequence has one element.</param>
        /// <param name="many">The value that should be provided to <paramref name="resultSelector"/> if the sequence has two or more elements.</param>
        /// <param name="resultSelector">A function that is provided with the cardinality, and if the sequence has just
        /// one element, the value of that element. Then transforms the result to an instance of TResult.</param>
        /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="TCardinality">The type that expresses cardinality.</typeparam>
        /// <typeparam name="TResult">The result type of the <paramref name="resultSelector"/> function.</typeparam>
        /// <returns>The value returned by the <paramref name="resultSelector"/>.</returns>

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
                return !e.MoveNext() ? resultSelector(one, current) : resultSelector(many, default);
            }
        }
    }
}
