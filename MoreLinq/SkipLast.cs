#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Leandro F. Vieira (leandromoh). All rights reserved.
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
        /// Bypasses a specified number of elements at the end of the sequence.
        /// </summary>
        /// <typeparam name="T">Type of the source sequence</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="count">The number of elements to bypass at the end of the source sequence.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> containing the source sequence elements except for the bypassed ones at the end.
        /// </returns>

#if NETSTANDARD2_1 || NETCOREAPP2_0_OR_GREATER
        public static IEnumerable<T> SkipLast<T>(IEnumerable<T> source, int count)
#else
        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int count)
#endif
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return count < 1 ? source
                 : source.CountDown(count, (e, cd) => (Element: e, Countdown: cd))
                         .TakeWhile(e => e.Countdown == null)
                         .Select(e => e.Element);
        }
    }
}
