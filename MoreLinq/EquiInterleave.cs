#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Pierre Lando. All rights reserved.
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

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Interleaves the elements of two or more sequences into a single sequence.
        /// If the input sequences are of different lengths, an exception is thrown.
        /// </summary>
        /// <remarks>
        /// Interleave combines sequences by visiting each in turn, and returning the first element of each, followed
        /// by the second, then the third, and so on. So, for example:<br/>
        /// <code><![CDATA[
        /// {1,1,1}.Interleave( {2,2,2}, {3,3,3} ) => { 1,2,3,1,2,3,1,2,3 }
        /// ]]></code>
        /// This operator behaves in a deferred and streaming manner.<br/>
        /// As soon as a sequence shorter than the other is detected, an exception is thrown.<br/>
        /// The sequences are interleaved in the order that they appear in the <paramref name="otherSequences"/>
        /// collection, with <paramref name="sequence"/> as the first sequence.
        /// </remarks>
        /// <typeparam name="T">The type of the elements of the source sequences</typeparam>
        /// <param name="sequence">The first sequence in the interleave group</param>
        /// <param name="otherSequences">The other sequences in the interleave group</param>
        /// <returns>
        /// A sequence of interleaved elements from all of the source sequences</returns>
        /// <exception cref="InvalidOperationException">
        /// The source sequences are of different lengths.</exception>

        public static IEnumerable<T> EquiInterleave<T>(this IEnumerable<T> sequence, params IEnumerable<T>[] otherSequences)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (otherSequences == null) throw new ArgumentNullException(nameof(otherSequences));

            return EquiInterleave(otherSequences.Prepend(sequence));
        }

        private static IEnumerable<T> EquiInterleave<T>(IEnumerable<IEnumerable<T>> sequences)
        {
            var enumerators = new List<IEnumerator<T>>();

            try
            {
                foreach (var sequence in sequences)
                {
                    if (sequence == null)
                        throw new ArgumentException("An item is null.", nameof(sequences));
                    enumerators.Add(sequence.GetEnumerator());
                }

                if (enumerators.Count == 0)
                    yield break;

                bool MoveNext()
                {
                    var hasNext = enumerators[0].MoveNext();
                    for (var i = 1; i < enumerators.Count; i++)
                    {
                        if (enumerators[i].MoveNext() != hasNext)
                            throw new InvalidOperationException("Input sequences are of different lengths.");
                    }

                    return hasNext;
                }

                while (MoveNext())
                {
                    foreach (var enumerator in enumerators)
                        yield return enumerator.Current;
                }
            }
            finally
            {
                foreach (var enumerator in enumerators)
                    enumerator.Dispose();
            }
        }
    }
}
