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
        /// Transposes a sequence of rows into a sequence of columns.
        /// </summary>
        /// <typeparam name="T">Type of source sequence elements.</typeparam>
        /// <param name="source">Source sequence to transpose.</param>
        /// <returns>
        /// Returns a sequence of columns in the source swapped into rows.
        /// </returns>
        /// <remarks>
        /// If a rows is shorter than a follow it then the shorter row's
        /// elements are skipped in the corresponding column sequences.
        /// This operator uses deferred execution and streams its results.
        /// Source sequence is consumed greedily when an iteration begins.
        /// The inner sequences representing rows are consumed lazily and
        /// resulting sequences of columns are streamed.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var matrix = new[]
        /// {
        ///     new[] { 10, 11 },
        ///     new[] { 20 },
        ///     new[] { 30, 31, 32 }
        /// };
        /// var result = matrix.Transpose();
        /// ]]></code>
        /// The <c>result</c> variable will contain [[10, 20, 30], [11, 31], [32]].
        /// </example>

        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return _(); IEnumerable<IEnumerable<T>> _()
            {
#pragma warning disable IDE0007 // Use implicit type (false positive)
                IEnumerator<T>?[] enumerators = source.Select(e => e.GetEnumerator()).Acquire();
#pragma warning restore IDE0007 // Use implicit type

                try
                {
                    while (true)
                    {
                        var column = new T[enumerators.Length];
                        var count = 0;
                        for (var i = 0; i < enumerators.Length; i++)
                        {
                            var enumerator = enumerators[i];
                            if (enumerator == null)
                                continue;

                            if (enumerator.MoveNext())
                            {
                                column[count++] = enumerator.Current;
                            }
                            else
                            {
                                enumerator.Dispose();
                                enumerators[i] = null;
                            }
                        }

                        if (count == 0)
                            yield break;

                        Array.Resize(ref column, count);
                        yield return column;
                    }
                }
                finally
                {
                    foreach (var e in enumerators)
                        e?.Dispose();
                }
            }
        }
    }
}
