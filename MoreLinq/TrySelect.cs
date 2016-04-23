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
        /// Projects each element of a sequence into a new form.
        /// Any exceptions thrown by the selector delegate are handled,
        /// and a fallback value is selected for such elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="sequence">The source sequence.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="fallback">The fallback value.</param>
        /// <returns>Sequence of values projected from source sequence.</returns>
        /// <remarks>This operator uses lazy evaluation.</remarks>
        public static IEnumerable<TResult> TrySelect<TSource, TResult>(this IEnumerable<TSource> sequence,
            Func<TSource, TResult> selector, 
            TResult fallback){
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (selector == null) throw new ArgumentNullException("selector");
            return TrySelectImpl<TSource, TResult, Exception>(sequence, selector, fallback);
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// Any exceptions of the given type thrown by the selector delegate are handled,
        /// and a fallback value is selected for such elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TException">The type of exception to handle.</typeparam>
        /// <param name="sequence">The source sequence.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="fallback">The fallback value.</param>
        /// <returns>Sequence of values projected from source sequence.</returns>
        /// <remarks>Exception types other than <typeparamref name="TException"/> will not be handled.
        /// This operator uses lazy evaluation.</remarks>
        public static IEnumerable<TResult> TrySelect<TSource, TResult, TException>(this IEnumerable<TSource> sequence,
            Func<TSource, TResult> selector,
            TResult fallback)   
            where TException : Exception {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (selector == null) throw new ArgumentNullException("selector");
            return TrySelectImpl<TSource, TResult, TException>(sequence, selector, fallback);
        }

        private static IEnumerable<TResult> TrySelectImpl<TSource, TResult, TException>(
            IEnumerable<TSource> sequence,
            Func<TSource, TResult> selector,
            TResult fallback)
            where TException : Exception {
                        
            foreach (var element in sequence) {

                TResult result;
                try {
                    result = selector(element);
                }
                catch (TException) {
                    result = fallback;
                }
                yield return result;
            }
        }
    }
}
