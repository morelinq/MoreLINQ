#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
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
        /// Returns a sequence of <see cref="IList{T}"/> representing all of the subsets
        /// of any size that are part of the original sequence.
        /// </summary>
        /// <remarks>
        /// This operator produces all of the subsets of a given sequence. Subsets are returned
        /// in increasing cardinality, starting with the empty set and terminating with the
        /// entire original sequence.<br/>
        /// Subsets are produced in a deferred, streaming manner; however, each subset is returned 
        /// as a materialized list.<br/>
        /// There are 2^N subsets of a given sequence, where N => sequence.Count(). 
        /// </remarks>
        /// <param name="sequence">Sequence for which to produce subsets</param>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <returns>A sequence of lists that represent the all subsets of the original sequence</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <see langword="null"/></exception>
        
        public static IEnumerable<IList<T>> Subsets<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException("sequence");
            return SubsetsImpl(sequence);
        }

        /// <summary>
        /// Returns a sequence of <see cref="IList{T}"/> representing all subsets of the
        /// specified size that are part of the original sequence.
        /// </summary>
        /// <param name="sequence">Sequence for which to produce subsets</param>
        /// <param name="subsetSize">The size of the subsets to produce</param>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <returns>A sequence of lists that represents of K-sized subsets of the original sequence</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="sequence"/> is <see langword="null"/>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="subsetSize"/> is less than zero.
        /// </exception>
        
        public static IEnumerable<IList<T>> Subsets<T>(this IEnumerable<T> sequence, int subsetSize)
        {
            if (sequence == null)
                throw new ArgumentNullException("sequence");
            if (subsetSize < 0)
                throw new ArgumentOutOfRangeException("subsetSize", "Subset size must be >= 0");

            // NOTE: Theres an interesting trade-off that we have to make in this operator.
            // Ideally, we would throw an exception here if the {subsetSize} parameter is
            // greater than the sequence length. Unforunately, determining the length of a
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
        /// Underlying implementation for Subsets() overload.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="sequence">Sequence for which to produce subsets</param>
        /// <returns>Sequence of lists representing all subsets of a sequence</returns>
        
        private static IEnumerable<IList<T>> SubsetsImpl<T>(IEnumerable<T> sequence)
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

                yield return sequenceAsList; // the last subet is the original set itself
            }
        }

        /// <summary>
        /// This class is responsible for producing the lexographically ordered k-subsets
        /// </summary>
        
        private sealed class SubsetGenerator<T> : IEnumerable<IList<T>>
        {
            /// <summary>
            /// SubsetEnumerator uses a snapshot of the original sequence, and an
            /// iterative, reductive swap algorithm to produce all subsets of a
            /// predetermined size less than or equal to the original set size.
            /// </summary>

            private class SubsetEnumerator : IEnumerator<IList<T>>
            {
                private readonly IList<T> _set;   // the original set of elements
                private readonly T[] _subset;     // the current subset to return
                private readonly int[] _indices;  // indices into the original set

                // TODO: It would be desirable to give these index members clearer names
                private bool _continue;  // termination indicator, set when all subsets have been produced
                private int _m;            // previous swap index (upper index)
                private int _m2;           // current swap index (lower index)
                private int _k;            // size of the subset being produced
                private int _n;            // size of the original set (sequence)
                private int _z;            // count of items excluded from the subet

                public SubsetEnumerator(IList<T> set, int subsetSize)
                {
                    // precondition: subsetSize <= set.Count
                    if (subsetSize > set.Count)
                        throw new ArgumentOutOfRangeException("subsetSize", "Subset size must be <= sequence.Count()");
                    
                    // initialize set arrays...
                    _set = set;
                    _subset = new T[subsetSize];
                    _indices = new int[subsetSize];
                    // initialize index counters...
                    Reset();
                }

                public void Reset()
                {
                    _m = _subset.Length;
                    _m2 = -1;
                    _k = _subset.Length;
                    _n = _set.Count;
                    _z = _n - _k + 1;
                    _continue = _subset.Length > 0;
                }

                public IList<T> Current
                {
                    get { return (IList<T>)_subset.Clone(); }
                }

                object IEnumerator.Current
                {
                    get { return Current; }
                }

                public bool MoveNext()
                {
                    if (!_continue)
                        return false;

                    if (_m2 == -1)
                    {
                        _m2 = 0;
                        _m = _k;
                    }
                    else
                    {
                        if (_m2 < _n - _m)
                        {
                            _m = 0;
                        }
                        _m++;
                        _m2 = _indices[_k - _m];
                    }

                    for (var j = 1; j <= _m; j++)
                        _indices[_k + j - _m - 1] = _m2 + j;

                    ExtractSubset();

                    _continue = (_indices[0] != _z);
                    return true;
                }

                void IDisposable.Dispose() { }

                private void ExtractSubset()
                {
                    for (var i = 0; i < _k; i++)
                        _subset[i] = _set[_indices[i] - 1];
                }
            }

            private readonly IEnumerable<T> _sequence;
            private readonly int _subsetSize;

            public SubsetGenerator(IEnumerable<T> sequence, int subsetSize)
            {
                if (sequence == null)
                    throw new ArgumentNullException("sequence");
                if (subsetSize < 0)
                    throw new ArgumentOutOfRangeException("subsetSize", "{subsetSize} must be between 0 and set.Count()");
                _subsetSize = subsetSize;
                _sequence = sequence;
            }

            /// <summary>
            /// Returns an enumerator that produces all of the k-sized
            /// subsets of the initial value set. The enumerator returns
            /// and <see cref="IList{T}"/> for each subset.
            /// </summary>
            /// <returns>an <see cref="IEnumerator"/> that enumerates all k-sized subsets</returns>

            public IEnumerator<IList<T>> GetEnumerator()
            {
                return new SubsetEnumerator(_sequence.ToList(), _subsetSize);
            }

            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        }
    }
}