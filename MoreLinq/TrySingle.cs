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
        /// Similar to Single() or SingleOrDefault() but allows the caller to determine
        /// whether there were zero or many elements in the sequence in the event that there isn't just one.
        /// </summary>
        /// <param name="sources">The source sequence that will be tested for its cardinality.</param>
        /// <param name="zero">The value that should be provided to <paramref name="resultSelector" /> if the sequence has zero elements.</param>
        /// <param name="one">The value that should be provided to resultSelector if the sequence has one element.</param>
        /// <param name="many">The value that should be provided to resultSelector if the sequence has two or more elements.</param>
        /// <param name="resultSelector">A function that is provided with the cardinality, and if the sequence has just
        /// one element, the value of that element. Then transforms the result to an instance of TResult.</param>
        /// <typeparam name="T">The type of the elements of the sequence</typeparam>
        /// <typeparam name="TCardinality">The type that expresses cardinality.</typeparam>
        /// <typeparam name="TResult">The result type of the resultSelector.</typeparam>
        /// <returns>The value provided by the resultSelector.</returns>
        public static TResult TrySingle<T, TCardinality, TResult>(this IEnumerable<T> source,
            TCardinality zero, TCardinality one, TCardinality many,
            Func<TCardinality, T, TResult> resultSelector)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var result = values.Take(2).ToArray();
            switch (result.Length)
            {
                case 0:
                    return resultSelector(zero, default);
                case 1:
                    return resultSelector(one, result[0]);
                default:
                    return resultSelector(many, default);
            }
        }
    }
}