#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2010 Leopold Bushkin. All rights reserved.
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
    using Experimental;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns the Cartesian product of two sequences by combining each
        /// element of the first set with each in the second and applying a
        /// user-defined projection to the pair.
        /// </summary>
        /// <typeparam name="TFirst">
        /// The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="TSecond">
        /// The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <param name="resultSelector">A projection function that combines
        /// elements from both sequences.</param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// <para>
        /// Elements of <paramref name="second"/> are cached when being paired
        /// with the first element of the <paramref name="first"/>. The cache is
        /// then re-used for pairing with all subsequent element of
        /// <paramref name="first"/>.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>

        public static IEnumerable<TResult> Cartesian<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                var secondMemo = second.Memoize();
                using (secondMemo as IDisposable)
                {
                    foreach (var item1 in first)
                    foreach (var item2 in secondMemo)
                        yield return resultSelector(item1, item2);
                }
            }
        }
    }
}
