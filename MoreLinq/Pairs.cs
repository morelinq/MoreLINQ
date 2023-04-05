#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2012 Atif Aziz. All rights reserved.
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
        /// Enumerates all positionally distinct pairs of items in an
        /// enumeration. See examples for clarifications. Two of the
        /// same values at different positions would appear in a pair.
        /// The enumerated pairs are returned as <see cref="Tuple{T,T}"/>s.
        /// The <see cref="Tuple{T,T}.Item1"/> will be the item appearing
        /// positionally before the other in <paramref name="source"/>.
        /// The pairs should not be assumed to be enumerated in any
        /// particular order.
        /// </summary>
        /// <example>
        /// [A, B, C, D] -> [(A, B), (A, C), (A, D), (B, C), (B, D), (C, D)]
        /// </example>
        /// <example>
        /// [A, B, A, D] -> [(A, B), (A, A), (A, D), (B, A), (B, D), (A, D)]
        /// </example>
        /// <param name="source">The enumeration</param>
        /// <typeparam name="T">The type of items in the enumeration</typeparam>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable<(T, T)> Pairs<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            /*
                We will return in order like

                [A, B, C, D] -> [(A, B), (A, C), (B, C), (A, D), (B, D), (C, D)]

                because it is ideal for minimizing both enumerations and space.

                If it were needed to return in a more natural order like

                [(A, B), (A, C), (A, D), (B, C), (B, D), (C, D)]

                we would need Reverse or SkipWhile, each adding computations.
            */
            return source.SelectMany((t2, i2) => source.Take(i2).Select(t1 => (t1, t2)));
        }
    }
}