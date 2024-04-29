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
        /// Returns a sequence of <see cref="IList{T}"/> representing all of the subsets of any size
        /// that are part of the original sequence. In mathematics, it is equivalent to the
        /// <em>power set</em> of a set.
        /// </summary>
        /// <param name="sequence">Sequence for which to produce subsets.</param>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <returns>
        /// A sequence of lists that represent the all subsets of the original sequence.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <see
        /// langword="null"/>.</exception>
        /// <remarks>
        /// <para>
        /// This operator produces all of the subsets of a given sequence. Subsets are returned in
        /// increasing cardinality, starting with the empty set and terminating with the entire
        /// original sequence.</para>
        /// <para>
        /// Subsets are produced in a deferred, streaming manner; however, each subset is returned
        /// as a materialized list.</para>
        /// <para>
        /// There are 2<sup>N</sup> subsets of a given sequence, where N &#8658;
        /// <c>sequence.Count()</c>.</para>
        /// </remarks>

        public static IEnumerable<IList<T>> Subsets<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));

            return _(sequence);

            static IEnumerable<IList<T>> _(IEnumerable<T> sequence)
            {
                var sequenceAsList = sequence.ToList();
                var sequenceLength = sequenceAsList.Count;

                yield return []; // the first subset is the empty set

                // next check also resolves the case of permuting empty sets
                if (sequenceLength is 0)
                    yield break;

                // all other subsets are computed using the subset generator...

                for (var k = 1; k < sequenceLength; k++)
                {
                    // each intermediate subset is a lexographically ordered K-subset
                    foreach (var subset in Subsets(sequenceAsList, k))
                        yield return subset;
                }

                yield return sequenceAsList; // the last subset is the original set itself
            }
        }

        /// <summary>
        /// Returns a sequence of <see cref="IList{T}"/> representing all subsets of a given size
        /// that are part of the original sequence. In mathematics, it is equivalent to the
        /// <em>combinations</em> or <em>k-subsets</em> of a set.
        /// </summary>
        /// <param name="sequence">Sequence for which to produce subsets.</param>
        /// <param name="subsetSize">The size of the subsets to produce.</param>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <returns>
        /// A sequence of lists that represents of K-sized subsets of the original
        /// sequence.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="sequence"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="subsetSize"/> is less than zero.
        /// </exception>

        public static IEnumerable<IList<T>> Subsets<T>(this IEnumerable<T> sequence, int subsetSize)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (subsetSize < 0)
                throw new ArgumentOutOfRangeException(nameof(subsetSize), "Subset size must be >= 0");

            // NOTE: There's an interesting trade-off that we have to make in this operator.
            // Ideally, we would throw an exception here if the {subsetSize} parameter is
            // greater than the sequence length. Unfortunately, determining the length of a
            // sequence is not always possible without enumerating it. Herein lies the rub.
            // We want Subsets() to be a deferred operation that only iterates the sequence
            // when the caller is ready to consume the results. However, this forces us to
            // defer the precondition check on the {subsetSize} upper bound. This can result
            // in an exception that is far removed from the point of failure - an unfortunate
            // and undesirable outcome.
            // At the moment, this operator prioritizes deferred execution over fail-fast
            // preconditions. This however, needs to be carefully considered - and perhaps
            // may change after further thought and review.

            return _(sequence, subsetSize);

            static IEnumerable<IList<T>> _(IEnumerable<T> sequence, int subsetSize)
            {
                foreach (var subset in Subsets(sequence.ToList(), subsetSize))
                    yield return subset;
            }
        }
        /// <summary>
        /// Produces lexographically ordered k-subsets.
        /// </summary>
        /// <remarks>
        /// It uses a snapshot of the original sequence, and an iterative,
        /// reductive swap algorithm to produce all subsets of a predetermined
        /// size less than or equal to the original set size.
        /// </remarks>

        static IEnumerable<IList<T>> Subsets<T>(List<T> set, int subsetSize)
        {
            // precondition: subsetSize <= set.Count
            if (subsetSize > set.Count)
                throw new ArgumentOutOfRangeException(nameof(subsetSize), "Subset size must be <= sequence.Count()");

            var indices = new int[subsetSize];  // indices into the original set
            var prevSwapIndex = subsetSize;     // previous swap index (upper index)
            var currSwapIndex = -1;             // current swap index (lower index)
            var setSize = set.Count;

            do
            {
                if (currSwapIndex == -1)
                {
                    currSwapIndex = 0;
                    prevSwapIndex = subsetSize;
                }
                else
                {
                    if (currSwapIndex < setSize - prevSwapIndex)
                        prevSwapIndex = 0;

                    prevSwapIndex++;
                    currSwapIndex = indices[subsetSize - prevSwapIndex];
                }

                for (var i = 1; i <= prevSwapIndex; i++)
                    indices[subsetSize + i - prevSwapIndex - 1] = currSwapIndex + i;

                var subset = new T[subsetSize];     // the current subset to return
                for (var i = 0; i < subsetSize; i++)
                    subset[i] = set[indices[i] - 1];

                yield return subset;
            }
            while (indices is [var fi, ..]
                   // .... count of items excluded from the subset
                   && fi != setSize - subsetSize + 1);
        }
    }
}
