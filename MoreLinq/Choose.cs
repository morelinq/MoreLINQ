#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Atif Aziz. All rights reserved.
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
        /// Applies a function to each element of the source sequence and
        /// returns a new sequence of result elements for source elements
        /// where the function returns a couple (2-tuple) having a <c>true</c>
        /// as its first element and result as the second.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements in the returned sequence.</typeparam>
        /// <param name="source"> The source sequence.</param>
        /// <param name="chooser">The function that is applied to each source
        /// element.</param>
        /// <returns>A sequence <typeparamref name="TResult"/> elements.</returns>
        /// <remarks>
        /// This method uses deferred execution semantics and streams its
        /// results.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var str = "O,l,2,3,4,S,6,7,B,9";
        /// var xs = str.Split(',').Choose(s => (int.TryParse(s, out var n), n));
        /// ]]></code>
        /// The <c>xs</c> variable will be a sequence of the integers 2, 3, 4,
        /// 6, 7 and 9.
        /// </example>

        public static IEnumerable<TResult> Choose<T, TResult>(this IEnumerable<T> source,
            Func<T, (bool, TResult)> chooser)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (chooser == null) throw new ArgumentNullException(nameof(chooser));

            return _(); IEnumerable<TResult> _()
            {
                foreach (var item in source)
                {
                    var (some, value) = chooser(item);
                    if (some)
                        yield return value;
                }
            }
        }
    }
}
