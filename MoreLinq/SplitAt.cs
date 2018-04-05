#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2009 Atif Aziz. All rights reserved.
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
        /// Splits the sequence into sub-sequences at given offsets into the
        /// sequence.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="offsets">
        /// The zero-based offsets into the source sequence at which at which
        /// to split the source sequence.</param>
        /// <typeparam name="T">
        /// The type of the element in the source sequence.</typeparam>
        /// <returns>
        /// A sequence of splits.</returns>
        /// <remarks>
        /// This method uses deferred execution semantics and streams the
        /// sub-sequences, where each sub-sequence is buffered.
        /// </remarks>

        public static IEnumerable<IEnumerable<T>> SplitAt<T>(
            this IEnumerable<T> source, params int[] offsets)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (offsets == null) throw new ArgumentNullException(nameof(offsets));

            return _(); IEnumerable<IEnumerable<T>> _()
            {
                using (var oe = offsets.Concat(int.MaxValue).GetEnumerator())
                {
                    oe.MoveNext();
                    var offset = oe.Current;

                    List<T> list = null;
                    foreach (var e in source.Index())
                    {
                        retry:
                        if (e.Key < offset)
                        {
                            if (list == null)
                                list = new List<T>();
                            list.Add(e.Value);
                        }
                        else
                        {
                            yield return list ?? Enumerable.Empty<T>();
                            offset = oe.MoveNext() ? oe.Current : -1;
                            list = null;
                            goto retry;
                        }
                    }

                    if (list != null)
                        yield return list;

                    while (oe.MoveNext())
                        yield return Enumerable.Empty<T>();
                }
            }
        }
    }
}
