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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// The private implementation class that produces permutations of a sequence.
        /// </summary>

        sealed class PermutationEnumerator<T> : IEnumerator<IList<T>>
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
            //       of N is equivalent to the evaluation of N-1 nested loops. Unfortunately, you can't
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

            readonly IList<T> valueSet;
            readonly int[] permutation;
            readonly IEnumerable<Action> generator;

            IEnumerator<Action> generatorIterator;
            bool hasMoreResults;

            IList<T>? current;

            public PermutationEnumerator(IEnumerable<T> valueSet)
            {
                this.valueSet = valueSet.ToArray();
                this.permutation = new int[this.valueSet.Count];
                // The nested loop construction below takes into account the fact that:
                // 1) for empty sets and sets of cardinality 1, there exists only a single permutation.
                // 2) for sets larger than 1 element, the number of nested loops needed is: set.Count-1
                this.generator = NestedLoops(NextPermutation, Generate(2UL, n => n + 1).Take(Math.Max(0, this.valueSet.Count - 1)));
                Reset();
            }

            [MemberNotNull(nameof(generatorIterator))]
            public void Reset()
            {
                this.current = null;
                this.generatorIterator?.Dispose();
                // restore lexographic ordering of the permutation indexes
                for (var i = 0; i < this.permutation.Length; i++)
                    this.permutation[i] = i;
                // start a new iteration over the nested loop generator
                this.generatorIterator = this.generator.GetEnumerator();
                // we must advance the nested loop iterator to the initial element,
                // this ensures that we only ever produce N!-1 calls to NextPermutation()
                _ = this.generatorIterator.MoveNext();
                this.hasMoreResults = true; // there's always at least one permutation: the original set itself
            }

            public IList<T> Current
            {
                get
                {
                    Debug.Assert(this.current is not null);
                    return this.current;
                }
            }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                this.current = PermuteValueSet();
                // check if more permutation left to enumerate
                var prevResult = this.hasMoreResults;
                this.hasMoreResults = this.generatorIterator.MoveNext();
                if (this.hasMoreResults)
                    this.generatorIterator.Current(); // produce the next permutation ordering
                // we return prevResult rather than m_HasMoreResults because there is always
                // at least one permutation: the original set. Also, this provides a simple way
                // to deal with the disparity between sets that have only one loop level (size 0-2)
                // and those that have two or more (size > 2).
                return prevResult;
            }

            void IDisposable.Dispose() => this.generatorIterator.Dispose();

            /// <summary>
            /// Transposes elements in the cached permutation array to produce the next permutation
            /// </summary>
            void NextPermutation()
            {
                // find the largest index j with m_Permutation[j] < m_Permutation[j+1]
                var j = this.permutation.Length - 2;
                while (this.permutation[j] > this.permutation[j + 1])
                    j--;

                // find index k such that m_Permutation[k] is the smallest integer
                // greater than m_Permutation[j] to the right of m_Permutation[j]
                var k = this.permutation.Length - 1;
                while (this.permutation[j] > this.permutation[k])
                    k--;

                (this.permutation[j], this.permutation[k]) = (this.permutation[k], this.permutation[j]);

                // move the tail of the permutation after the jth position in increasing order
                for (int x = this.permutation.Length - 1, y = j + 1; x > y; x--, y++)
                    (this.permutation[x], this.permutation[y]) = (this.permutation[y], this.permutation[x]);
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
            /// <returns>Array of permuted source sequence values</returns>
            T[] PermuteValueSet()
            {
                var permutedSet = new T[this.permutation.Length];
                for (var i = 0; i < this.permutation.Length; i++)
                    permutedSet[i] = this.valueSet[this.permutation[i]];
                return permutedSet;
            }
        }

        /// <summary>
        /// Generates a sequence of lists that represent the permutations of the original sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="sequence">The original sequence to permute.</param>
        /// <returns>
        /// A sequence of lists representing permutations of the original sequence.</returns>
        /// <exception cref="OverflowException">
        /// Too many permutations (limited by <see cref="ulong.MaxValue"/>); thrown during iteration
        /// of the resulting sequence.</exception>
        /// <remarks>
        /// <para>
        /// A permutation is a unique re-ordering of the elements of the sequence.</para>
        /// <para>
        /// This operator returns permutations in a deferred, streaming fashion; however, each
        /// permutation is materialized into a new list. There are N! permutations of a sequence,
        /// where N &#8658; <c>sequence.Count()</c>.</para>
        /// <para>
        /// Be aware that the original sequence is considered one of the permutations and will be
        /// returned as one of the results.</para>
        /// </remarks>

        public static IEnumerable<IList<T>> Permutations<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));

            return _(); IEnumerable<IList<T>> _()
            {
                using var iter = new PermutationEnumerator<T>(sequence);

                while (iter.MoveNext())
                    yield return iter.Current;
            }
        }
    }
}
