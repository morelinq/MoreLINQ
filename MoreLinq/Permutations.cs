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
        #region Nested Classes
        /// <summary>
        /// The private implementation class that produces permutations of a sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class PermutationEnumerator<T> : IEnumerator<IList<T>>
        {
            // NOTE: The algorithm used to generate permutations uses the fact that any set
            //       can be put into 1-to-1 correspondence with the set of ordinals number (0..n).
            //       The implementation here is based on the algorithm described by Kenneth H. Rosen,
            //       in Discrete Mathematics and Its Applications, 2nd edition, pp. 282-284.
            //
            //       There are two significant changes from the original implementation.
            //       First, the algorithm uses lazy evaluation and streaming to fit well into the
            //       nature of most LINQ evaluations.
            //
            //       Second, the algorithm has been modified to use dynamically generated nested loop
            //       state machines, rather than an integral computation of the factorial function
            //       to determine when to terminate. The original algorithm required a priori knowledge
            //       of the number of iterations necessary to produce all permutations. This is a 
            //       necessary step to avoid overflowing the range of the permutation arrays used.
            //       The number of permutation iterations is defined as the factorial of the original 
            //       set size minus 1. 
            //
            //       However, there's a fly in the ointment. The factorial function grows VERY rapidly.
            //       13! overflows the range of a Int32; while 28! overflows the range of decimal.
            //       To overcome these limitations, the algorithm relies on the fact that the factorial
            //       of N is equivalent to the evaluation of N-1 nested loops. Unfortunatley, you can't
            //       just code up a variable number of nested loops ... this is where .NET generators
            //       with their elegant 'yield return' syntax come to the rescue.
            //       
            //       The methods of the Loop extension class (For and NestedLoops) provide the implementation
            //       of dynamic nested loops using generators and sequence composition. In a nutshell,
            //       the two Repeat() functions are the constructor of loops and nested loops, respectively.
            //       The NestedLoops() function produces a composition of loops where the loop counter
            //       for each nesting level is defined in a separate sequence passed in the call.
            //
            //       For example:        NestedLoops( () => DoSomething(), new[] { 6, 8 } )          
            //
            //       is equivalent to:   for( int i = 0; i < 6; i++ )
            //                               for( int j = 0; j < 8; j++ )
            //                                   DoSomething();

            #region Private Fields
            private readonly IList<T> m_ValueSet;
            private readonly int[] m_Permutation;
            private readonly IEnumerable<Action> m_Generator;

            private IEnumerator<Action> m_GeneratorIterator;
            private bool m_HasMoreResults;
            #endregion

            #region Constructors
            public PermutationEnumerator(IEnumerable<T> valueSet)
            {
                m_ValueSet = valueSet.ToArray();
                m_Permutation = new int[m_ValueSet.Count];
                // The nested loop construction below takes into account the fact that:
                // 1) for empty sets and sets of cardinality 1, there exists only a single permutation.
                // 2) for sets larger than 1 element, the number of nested loops needed is: set.Count-1
                m_Generator = NestedLoops(NextPermutation, Enumerable.Range(2, Math.Max(0, m_ValueSet.Count - 1)));
                Reset();
            }
            #endregion

            #region IEnumerator Members
            public void Reset()
            {
                if (m_GeneratorIterator != null)
                    m_GeneratorIterator.Dispose();
                // restore lexographic ordering of the permutation indexes
                for (var i = 0; i < m_Permutation.Length; i++)
                    m_Permutation[i] = i;
                // start a newiteration over the nested loop generator
                m_GeneratorIterator = m_Generator.GetEnumerator();
                // we must advance the nestedloop iterator to the initial element,
                // this ensures that we only ever produce N!-1 calls to NextPermutation()
                m_GeneratorIterator.MoveNext();
                m_HasMoreResults = true; // there's always at least one permutation: the original set itself
            }

            public IList<T> Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                Current = PermuteValueSet();
                // check if more permutation left to enumerate
                var prevResult = m_HasMoreResults;
                m_HasMoreResults = m_GeneratorIterator.MoveNext();
                if (m_HasMoreResults)
                    m_GeneratorIterator.Current(); // produce the next permutation ordering
                // we return prevResult rather than m_HasMoreResults because there is always
                // at least one permtuation: the original set. Also, this provides a simple way
                // to deal with the disparity between sets that have only one loop level (size 0-2)
                // and those that have two or more (size > 2).
                return prevResult;
            }

            void IDisposable.Dispose() { }
            #endregion

            #region Private Methods
            /// <summary>
            /// Transposes elements in the cached permutation array to produce the next permutation
            /// </summary>
            private void NextPermutation()
            {
                // find the largest index j with m_Permutation[j] < m_Permutation[j+1]
                var j = m_Permutation.Length - 2;
                while (m_Permutation[j] > m_Permutation[j + 1])
                    j--;

                // find index k such that m_Permutation[k] is the smallest integer
                // greater than m_Permutation[j] to the right of m_Permutation[j]
                var k = m_Permutation.Length - 1;
                while (m_Permutation[j] > m_Permutation[k])
                    k--;

                // interchange m_Permutation[j] and m_Permutation[k]
                var oldValue = m_Permutation[k];
                m_Permutation[k] = m_Permutation[j];
                m_Permutation[j] = oldValue;

                // move the tail of the permutation after the jth position in increasing order
                var x = m_Permutation.Length - 1;
                var y = j + 1;

                while (x > y)
                {
                    oldValue = m_Permutation[y];
                    m_Permutation[y] = m_Permutation[x];
                    m_Permutation[x] = oldValue;
                    x--;
                    y++;
                }
            }

            /// <summary>
            /// Creates a new list containing the values from the original
            /// set in their new permuted order.
            /// </summary>
            /// <remarks>
            /// The reason we return a new permuted value set, rather than reuse
            /// an existing collection, is that we have no control over what the
            /// consumer will do with the results produced. They could very easily
            /// generate and store a set of permutations and only then begin to
            /// process them. If we reused the same collection, the caller would
            /// be surprised to discover that all of the permutations looked the
            /// same.
            /// </remarks>
            /// <returns>List of permuted source sequence values</returns>
            private IList<T> PermuteValueSet()
            {
                var permutedSet = new T[m_Permutation.Length];
                for (var i = 0; i < m_Permutation.Length; i++)
                    permutedSet[i] = m_ValueSet[m_Permutation[i]];
                return permutedSet;
            }
            #endregion
        }
        #endregion

        /// <summary>
        /// Generates a sequence of lists that represent the permutations of the original sequence.
        /// </summary>
        /// <remarks>
        /// A permutation is a unique re-ordering of the elements of the sequence.<br/>
        /// This operator returns permutations in a deferred, streaming fashion; however, each
        /// permutation is materialized into a new list. There are N! permutations of a sequence,
        /// where N => sequence.Count().<br/>
        /// Be aware that the original sequence is considered one of the permutations and will be
        /// returned as one of the results.
        /// </remarks>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="sequence">The original sequence to permute</param>
        /// <returns>A sequence of lists representing permutations of the original sequence</returns>
        
        public static IEnumerable<IList<T>> Permutations<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");

            return PermutationsImpl(sequence);
        }

        private static IEnumerable<IList<T>> PermutationsImpl<T>(IEnumerable<T> sequence)
        {
            using (var iter = new PermutationEnumerator<T>(sequence))
            {
                while (iter.MoveNext())
                    yield return iter.Current;
            }
        }
    }
}