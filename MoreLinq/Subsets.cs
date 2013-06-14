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
        /// <typeparam name="T"></typeparam>
        private sealed class SubsetGenerator<T> : IEnumerable<IList<T>>
        {
            #region Nested Classes
            /// <summary>
            /// SubsetEnumerator uses a snapshot of the original sequence, and an
            /// iterative, reductive swap algorithm to produce all subsets of a
            /// predetermined size less than or equal to the original set size.
            /// </summary>
            private class SubsetEnumerator : IEnumerator<IList<T>>
            {
                #region Private Fields
                private readonly IList<T> m_Set;   // the original set of elements
                private readonly T[] m_Subset;     // the current subset to return
                private readonly int[] m_Indices;  // indices into the original set

                // TODO: It would be desirable to give these index members clearer names
                private bool m_Continue;  // termination indicator, set when all subsets have been produced
                private int m;            // previous swap index (upper index)
                private int m2;           // current swap index (lower index)
                private int k;            // size of the subset being produced
                private int n;            // size of the original set (sequence)
                private int z;            // count of items excluded from the subet
                #endregion

                #region Constructors
                public SubsetEnumerator(IList<T> set, int subsetSize)
                {
                    // precondition: subsetSize <= set.Count
                    if (subsetSize > set.Count)
                        throw new ArgumentOutOfRangeException("subsetSize", subsetSize, "Subset size must be <= sequence.Count()");
                    
                    // initialize set arrays...
                    m_Set = set;
                    m_Subset = new T[subsetSize];
                    m_Indices = new int[subsetSize];
                    // initialize index counters...
                    Reset();
                }
                #endregion

                #region IEnumerator Members
                public void Reset()
                {
                    m = m_Subset.Length;
                    m2 = -1;
                    k = m_Subset.Length;
                    n = m_Set.Count;
                    z = n - k + 1;
                    m_Continue = m_Subset.Length > 0;
                }

                public IList<T> Current
                {
                    get { return (IList<T>)m_Subset.Clone(); }
                }

                object IEnumerator.Current
                {
                    get { return Current; }
                }

                public bool MoveNext()
                {
                    if (!m_Continue)
                        return false;

                    if (m2 == -1)
                    {
                        m2 = 0;
                        m = k;
                    }
                    else
                    {
                        if (m2 < n - m)
                        {
                            m = 0;
                        }
                        m++;
                        m2 = m_Indices[k - m];
                    }

                    for (var j = 1; j <= m; j++)
                        m_Indices[k + j - m - 1] = m2 + j;

                    ExtractSubset();

                    m_Continue = (m_Indices[0] != z);
                    return true;
                }

                void IDisposable.Dispose() { }
                #endregion

                #region Private Methods
                private void ExtractSubset()
                {
                    for (var i = 0; i < k; i++)
                        m_Subset[i] = m_Set[m_Indices[i] - 1];
                }
                #endregion
            }
            #endregion

            #region Private Members

            private readonly IEnumerable<T> m_Sequence;
            private readonly int m_SubsetSize;
            #endregion

            #region Constructors
            public SubsetGenerator(IEnumerable<T> sequence, int subsetSize)
            {
                if (sequence == null)
                    throw new ArgumentNullException("sequence");
                if (subsetSize < 0)
                    throw new ArgumentOutOfRangeException("subsetSize", "{subsetSize} must be between 0 and set.Count()");
                m_SubsetSize = subsetSize;
                m_Sequence = sequence;
            }
            #endregion

            #region Public Methods
            /// <summary>
            /// Returns an enumerator that produces all of the k-sized
            /// subsets of the initial value set. The enumerator returns
            /// and <see cref="IList{T}"/> for each subset.
            /// </summary>
            /// <returns>an <see cref="IEnumerator"/> that enumerates all k-sized subsets</returns>
            public IEnumerator<IList<T>> GetEnumerator()
            {
                return new SubsetEnumerator(m_Sequence.ToList(), m_SubsetSize);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            #endregion
        }
    }
}