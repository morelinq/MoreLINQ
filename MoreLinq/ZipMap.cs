#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2020 Daniel Jonsson. All rights reserved.
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
        /// Applies a function on each element and returns a sequence of
        /// tuples with the source element and the result from the function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of value returned by <paramref name="func"/>.</typeparam>
        /// <param name="source">The sequence to iterate over.</param>
        /// <param name="func">The function to apply to each element.</param>
        /// <returns>
        /// Returns a sequence of tuples with the source element and the
        /// result from <paramref name="func"/> parameter.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// string[] strings = { "foo", "bar", "baz" };
        /// var result = strings.ZipMap(s => Regex.IsMatch(s, @"^b"));
        /// ]]></code>
        /// <para>
        /// The <c>result</c> variable, when iterated over, will yield
        /// <c>("foo", false)</c>, <c>("bar", true)</c> and
        /// <c>("baz", true)</c>, in turn.</para>
        /// </example>

        public static IEnumerable<(TSource Item, TResult Result)> ZipMap<TSource, TResult>(
            this IEnumerable<TSource> source, Func<TSource, TResult> func)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));

            foreach (var item in source)
            {
                yield return (item, func(item));
            }
        }
    }
}
