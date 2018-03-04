#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Leandro F. Vieira (leandromoh). All rights reserved.
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
        /// Transpose the rows of a sequence into columns.
        /// </summary>
        /// <typeparam name="T">Type of the source sequence</typeparam>
        /// <param name="source">Source sequence</param>
        /// <returns>
        /// Return a sequence with the columns in the source swapped into rows.
        /// </returns>
        /// <remarks>
        /// If some of the rows are shorter than the following rows, their elements are skipped.
        /// This operator uses deferred execution and streams its results.
        /// Source sequence is consumed greedily when an iteration of the resulting sequence begins.
        /// The inner sequences are consumed lazily, according as the resulting sequences are streaming.
        /// </remarks>
        /// <example>
        /// <code>
        /// var matrix = new[]
        /// {
        ///     new int[] { 10, 11 },
        ///     new int[] { 20 },
        ///     new int[] { 30, 31, 32 }
        /// };
        /// var result = matrix.Transpose();
        /// </code>
        /// The <c>result</c> variable will contain [[10, 20, 30], [11, 31], [32]].
        /// </example>
        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return _(); IEnumerable<IEnumerable<T>> _()
            {
                var enumerators = source.Select(e => e.GetEnumerator()).Acquire();

                try
                {
                    while (true)
                    {
                        var row = new List<T>();
                        for (var i = 0; i < enumerators.Count; i++)
                        {
                            if (enumerators[i].MoveNext())
                            {
                                row.Add(enumerators[i].Current);
                            }
                            else
                            {
                                enumerators[i].Dispose();
                                enumerators.RemoveAt(i);
                                i--;
                            }
                        }

                        if (row.Any())
                            yield return row;
                        else
                            yield break;
                    }
                }
                finally
                {
                    foreach (var e in enumerators)
                        e.Dispose();
                }
            }
        }
    }
}
