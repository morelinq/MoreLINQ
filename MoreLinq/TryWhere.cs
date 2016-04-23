#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
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

namespace MoreLinq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    partial class MoreEnumerable {

        /// <summary>
        /// Filters a sequence of values based on a predicate. 
        /// Any exceptions thrown by evaluating the predicate are handled, 
        /// and results can be filtered by whether or not an exception was thrown. 
        /// </summary>
        /// <typeparam name="TSource">The type of the source sequence.</typeparam>
        /// <param name="sequence">The source sequence.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="yieldOnThrow">If set to <c>true</c>, elements for which the 
        /// predicate throws an exception will be returned; otherwise, such elements are not returned.</param>
        /// <returns>Filtered sequence of values from source collection.</returns>
        /// <remarks>This operator uses lazy evaluation.</remarks>
        public static IEnumerable<TSource> TryWhere<TSource>(this IEnumerable<TSource> sequence,
            Func<TSource, bool> predicate, 
            bool yieldOnThrow) {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (predicate == null) throw new ArgumentNullException("condition");
            return TryWhereImpl<TSource, Exception>(sequence, predicate, yieldOnThrow);
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate. 
        /// Any exceptions of the given type thrown by evaluating the predicate are handled, 
        /// and results can be filtered by whether or not that kind of exception was thrown. 
        /// </summary>
        /// <typeparam name="TSource">The type of the source sequence.</typeparam>
        /// <typeparam name="TException">The type of exception to handle.</typeparam>
        /// <param name="sequence">The source sequence.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="yieldOnThrow">If set to <c>true</c>, elements for which the 
        /// predicate throws a <typeparamref name="TException"/> will be returned; otherwise, such elements are not returned.</param>
        /// <returns>Filtered sequence of values from source collection.</returns>
        /// <remarks>Exception types other than <typeparamref name="TException"/> will not be handled.
        /// This operator uses lazy evaluation.</remarks>
        public static IEnumerable<TSource> TryWhere<TSource, TException>(this IEnumerable<TSource> sequence,
            Func<TSource, bool> predicate, 
            bool yieldOnThrow)
            where TException : Exception {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (predicate == null) throw new ArgumentNullException("condition");
            return TryWhereImpl<TSource, TException>(sequence, predicate, yieldOnThrow);
        }

        private static IEnumerable<TSource> TryWhereImpl<TSource, TException>(
            IEnumerable<TSource> sequence,
            Func<TSource, Boolean> predicate,
            bool yieldOnThrow)
            where TException : Exception {
            
            foreach (var element in sequence) {

                try {
                    if (!predicate(element))
                        continue;
                }
                catch (TException) {
                    if (!yieldOnThrow)
                        continue;
                }

                yield return element;
            }
        }
    }
}
