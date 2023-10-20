#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2010 Leopold Bushkin. All rights reserved.
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
        /// Interleaves the elements of two or more sequences into a single sequence, skipping
        /// sequences as they are consumed.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the source sequences.</typeparam>
        /// <param name="sequence">The first sequence in the interleave group.</param>
        /// <param name="otherSequences">The other sequences in the interleave group.</param>
        /// <returns>A sequence of interleaved elements from all of the source sequences.</returns>
        /// <remarks>
        /// <para>
        /// Interleave combines sequences by visiting each in turn, and returning the first element
        /// of each, followed by the second, then the third, and so on. So, for example:</para>
        /// <code><![CDATA[
        /// var xs = new[] { 1, 1, 1 }.Interleave(new[] { 2, 2, 2 }, new[] { 3, 3, 3 });
        /// // xs = { 1, 2, 3, 1, 2, 3, 1, 2, 3 }
        /// ]]></code>
        /// <para>
        /// This operator behaves in a deferred and streaming manner.</para>
        /// <para>
        /// When sequences are of unequal length, this method will skip those sequences that have
        /// been fully consumed and continue interleaving the remaining sequences.</para>
        /// <para>
        /// The sequences are interleaved in the order that they appear in the <paramref
        /// name="otherSequences"/> collection, with <paramref name="sequence"/> as the first
        /// sequence.</para>
        /// </remarks>

        public static IEnumerable<T> Interleave<T>(this IEnumerable<T> sequence, params IEnumerable<T>[] otherSequences)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (otherSequences == null) throw new ArgumentNullException(nameof(otherSequences));
            if (otherSequences.Any(s => s == null))
                throw new ArgumentNullException(nameof(otherSequences), "One or more sequences passed to Interleave was null.");

            return Impl(otherSequences.Prepend(sequence));

            static IEnumerable<T> Impl(IEnumerable<IEnumerable<T>> sequences)
            {
                var enumerators = new LinkedList<IEnumerator<T>>();

                try
                {
                    // First, yield first element of each sequence.

                    foreach (var sequence in sequences)
                    {
                        var enumerator = sequence.GetEnumerator();

                        _ = enumerators.AddLast(enumerator);
                        if (enumerator.MoveNext())
                        {
                            yield return enumerator.Current;
                        }
                        else // Dispose and remove empty sequence
                        {
                            enumerator.Dispose();
                            _ = enumerators.Remove(enumerator);
                        }
                    }

                    // Then, yield remaining elements from each sequence.

                    var node = enumerators.First;
                    while (node is { Value: var enumerator, Next: var nextNode })
                    {
                        if (enumerator.MoveNext())
                        {
                            yield return enumerator.Current;
                        }
                        else
                        {
                            enumerator.Dispose();
                            enumerators.Remove(node);
                        }

                        // Work on next node or restart from first one.
                        node = nextNode ?? enumerators.First;
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
}
