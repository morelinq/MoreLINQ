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
    using System.Collections;
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

            return _(); IEnumerable<IList<T>> _()
            {
                var sequenceAsList = sequence.ToList();
                var sequenceLength = sequenceAsList.Count;

                // the first subset is the empty set
                yield return new List<T>();

                // all other subsets are computed using the subset generator
                // this check also resolves the case of permuting empty sets
                if (sequenceLength > 0)
                {
                    for (var i = 1; i < sequenceLength; i++)
                    {
                        // each intermediate subset is a lexographically ordered K-subset
                        var subsetGenerator = new SubsetGenerator<T>(sequenceAsList, i);
                        foreach (var subset in subsetGenerator)
                            yield return subset;
                    }

                    yield return sequenceAsList; // the last subset is the original set itself
                }
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

            return new SubsetGenerator<T>(sequence, subsetSize);
        }

        /// <summary>
        /// This class is responsible for producing the lexographically ordered k-subsets
        /// </summary>

        sealed class SubsetGenerator<T> : IEnumerable<IList<T>>
        {
            /// <summary>
            /// SubsetEnumerator uses a snapshot of the original sequence, and an
            /// iterative, reductive swap algorithm to produce all subsets of a
            /// predetermined size less than or equal to the original set size.
            /// </summary>

            sealed class SubsetEnumerator : IEnumerator<IList<T>>
            {
                readonly IList<T> set;    // the original set of elements
                readonly T[] subset;      // the current subset to return
                readonly int[] indices;   // indices into the original set

                bool @continue;  // termination indicator, set when all subsets have been produced

                int prevSwapIndex;        // previous swap index (upper index)
                int currSwapIndex;        // current swap index (lower index)
                int subsetSize;           // size of the subset being produced
                int setSize;              // size of the original set (sequence)

                public SubsetEnumerator(IList<T> set, int subsetSize)
                {
                    // precondition: subsetSize <= set.Count
                    if (subsetSize > set.Count)
                        throw new ArgumentOutOfRangeException(nameof(subsetSize), "Subset size must be <= sequence.Count()");

                    // initialize set arrays...
                    this.set = set;
                    this.subset = new T[subsetSize];
                    this.indices = new int[subsetSize];
                    // initialize index counters...
                    Reset();
                }

                public void Reset()
                {
                    this.prevSwapIndex = this.subset.Length;
                    this.currSwapIndex = -1;
                    this.subsetSize = this.subset.Length;
                    this.setSize = this.set.Count;
                    this.@continue = true;
                }

                public IList<T> Current => (IList<T>)this.subset.Clone();

                object IEnumerator.Current => Current;

                public bool MoveNext()
                {
                    if (!this.@continue)
                        return false;

                    if (this.currSwapIndex == -1)
                    {
                        this.currSwapIndex = 0;
                        this.prevSwapIndex = this.subsetSize;
                    }
                    else
                    {
                        if (this.currSwapIndex < this.setSize - this.prevSwapIndex)
                        {
                            this.prevSwapIndex = 0;
                        }
                        this.prevSwapIndex++;
                        this.currSwapIndex = this.indices[this.subsetSize - this.prevSwapIndex];
                    }

                    for (var j = 1; j <= this.prevSwapIndex; j++)
                        this.indices[this.subsetSize + j - this.prevSwapIndex - 1] = this.currSwapIndex + j;

                    ExtractSubset();

                    this.@continue = this.indices is [var i, ..]
                                     // .... count of items excluded from the subset
                                     && i != this.setSize - this.subsetSize + 1;

                    return true;
                }

                void IDisposable.Dispose() { }

                void ExtractSubset()
                {
                    for (var i = 0; i < this.subsetSize; i++)
                        this.subset[i] = this.set[this.indices[i] - 1];
                }
            }

            readonly IEnumerable<T> sequence;
            readonly int subsetSize;

            public SubsetGenerator(IEnumerable<T> sequence, int subsetSize)
            {
                if (sequence is null)
                    throw new ArgumentNullException(nameof(sequence));
                if (subsetSize < 0)
                    throw new ArgumentOutOfRangeException(nameof(subsetSize), "{subsetSize} must be between 0 and set.Count()");
                this.subsetSize = subsetSize;
                this.sequence = sequence;
            }

            /// <summary>
            /// Returns an enumerator that produces all of the k-sized
            /// subsets of the initial value set. The enumerator returns
            /// and <see cref="IList{T}"/> for each subset.
            /// </summary>
            /// <returns>an <see cref="IEnumerator"/> that enumerates all k-sized subsets</returns>

            public IEnumerator<IList<T>> GetEnumerator()
            {
                return new SubsetEnumerator(this.sequence.ToList(), this.subsetSize);
            }

            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        }
    }
}
