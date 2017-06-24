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
        /// Return a sequence with the columns in the source swapped into rows.
        /// </summary>
        /// <typeparam name="T">Type of the source sequence</typeparam>
        /// <param name="source">Source sequence</param>
        /// <returns>
        /// The transpose version of source.
        /// </returns>
        /// <remarks>
        /// If some of the rows are shorter than the following rows, their elements are skipped.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// var result = [[1,2,3],[4,5,6]].Transpose();
        /// </code>
        /// The <c>result</c> variable will contain [[1,4],[2,5],[3,6]].
        /// </example>
        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            
            return _().TakeWhile(x => x.Any());

            IEnumerable<IEnumerable<T>> _()
            {
                var list = new List<IEnumerator<T>>();
                var values = new List<List<T>>();
                var empty = new T[0];
                var count = 0;

                var buffer = new List<IEnumerable<T>>();
                var cached = buffer.Concat(EnumerateOnce(source).Pipe(x => buffer.Add(x)));

                var enumerators = cached.Select((p, i) =>
                {
                    if (i < list.Count)
                        return list[i];

                    if (p == null)
                        throw new InvalidOperationException($"The sequence at index {i} inside source is null.");

                    list.Add(p.GetEnumerator());

                    return list[i];
                });

                while (true)
                {
                    var countClosure = count;

                    yield return enumerators.SelectMany((p, j) =>
                    {
                        if (j >= values.Count)
                            values.Add(new List<T>());

                        var row = values[j];

                        while (countClosure >= row.Count && p != null && p.MoveNext())
                            row.Add(p.Current);

                        if (countClosure >= row.Count && p != null)
                        {
                            list[j].Dispose();
                            list[j] = null;
                        }

                        return countClosure < row.Count ? new[] { row[countClosure] } : empty;
                    });

                    count++;
                }
            }

            IEnumerable<U> EnumerateOnce<U>(IEnumerable<U> sequence)
            {
                IEnumerator<U> ector = null;

                return f(); IEnumerable<U> f()
                {
                    ector = ector ?? sequence.GetEnumerator();

                    while (ector.MoveNext())
                        yield return ector.Current;

                    ector.Dispose();
                }
            }
        }
    }
}
