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

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns items from the input sequence until the given predicate returns true
        /// when applied to the current source item; that item will be the last returned.
        /// </summary>
        /// <remarks>
        /// <para>
        /// TakeUntil differs from Enumerable.TakeWhile in two respects. Firstly, the sense
        /// of the predicate is reversed: it is expected that the predicate will return false
        /// to start with, and then return true - for example, when trying to find a matching
        /// item in a sequence.
        /// </para>
        /// <para>
        /// Secondly, TakeUntil yields the element which causes the predicate to return true. For
        /// example, in a sequence <code><![CDATA[{ 1, 2, 3, 4, 5 }]]></code> and with a predicate of
        /// <code><![CDATA[x => x == 3]]></code>, the result would be <code><![CDATA[{ 1, 2, 3 }]]></code>.
        /// </para>
        /// <para>
        /// TakeUntil is as lazy as possible: it will not iterate over the source sequence
        /// until it has to, it won't iterate further than it has to, and it won't evaluate
        /// the predicate until it has to. (This means that an item may be returned which would
        /// actually cause the predicate to throw an exception if it were evaluated, so long as
        /// no more items of data are requested.)
        /// </para>
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="predicate">Predicate used to determine when to stop yielding results from the source.</param>
        /// <returns>Items from the source sequence, until the predicate returns true when applied to the item.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null</exception>

        public static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return _(); IEnumerable<TSource> _()
            {
                foreach (var item in source)
                {
                    yield return item;
                    if (predicate(item))
                        yield break;
                }
            }
        }
    }
}
