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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Interleaves the elements of two or more sequences into a single sequence, skipping sequences as they are consumed
        /// </summary>
        /// <remarks>
        /// Interleave combines sequences by visiting each in turn, and returning the first element of each, followed
        /// by the second, then the third, and so on. So, for example:<br/>
        /// <code>
        /// {1,1,1}.Interleave( {2,2,2}, {3,3,3} ) => { 1,2,3,1,2,3,1,2,3 }
        /// </code>
        /// This operator behaves in a deferred and streaming manner.<br/>
        /// When sequences are of unequal length, this method will skip those sequences that have been fully consumed
        /// and continue interleaving the remaining sequences.<br/>
        /// The sequences are interleaved in the order that they appear in the <paramref name="otherSequences"/>
        /// collection, with <paramref name="sequence"/> as the first sequence.
        /// </remarks>
        /// <typeparam name="T">The type of the elements of the source sequences</typeparam>
        /// <param name="sequence">The first sequence in the interleave group</param>
        /// <param name="otherSequences">The other sequences in the interleave group</param>
        /// <returns>A sequence of interleaved elements from all of the source sequences</returns>
        
        public static IEnumerable<T> Interleave<T>(this IEnumerable<T> sequence, params IEnumerable<T>[] otherSequences)
        {
            return Interleave(sequence, ImbalancedInterleaveStrategy.Skip, otherSequences);
        }

        /// <summary>
        /// Interleaves the elements of two or more sequences into a single sequence, applying the specified strategy when sequences are of unequal length
        /// </summary>
        /// <remarks>
        /// Interleave combines sequences by visiting each in turn, and returning the first element of each, followed
        /// by the second, then the third, and so on. So, for example:<br/>
        /// <code>
        /// {1,1,1}.Interleave( {2,2,2}, {3,3,3} ) => { 1,2,3,1,2,3,1,2,3 }
        /// </code>
        /// This operator behaves in a deferred and streaming manner.<br/>
        /// When sequences are of unequal length, this method will use the imbalance strategy specified to
        /// decide how to continue interleaving the remaining sequences. See <see cref="ImbalancedInterleaveStrategy"/>
        /// for more information.<br/>
        /// The sequences are interleaved in the order that they appear in the <paramref name="otherSequences"/>
        /// collection, with <paramref name="sequence"/> as the first sequence.
        /// </remarks>
        /// <typeparam name="T">The type of the elements of the source sequences</typeparam>
        /// <param name="sequence">The first sequence in the interleave group</param>
        /// <param name="imbalanceStrategy">Defines the behavior of the operator when sequences are of unequal length</param>
        /// <param name="otherSequences">The other sequences in the interleave group</param>
        /// <returns>A sequence of interleaved elements from all of the source sequences</returns>
        
        private static IEnumerable<T> Interleave<T>(this IEnumerable<T> sequence, ImbalancedInterleaveStrategy imbalanceStrategy, params IEnumerable<T>[] otherSequences)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (otherSequences == null) throw new ArgumentNullException("otherSequences");
            if (otherSequences.Any(s => s == null))
                throw new ArgumentNullException("otherSequences", "One or more sequences passed to Interleave was null.");

            return InterleaveImpl(new[] { sequence }.Concat(otherSequences), imbalanceStrategy);
        }

        private static IEnumerable<T> InterleaveImpl<T>(IEnumerable<IEnumerable<T>> sequences, ImbalancedInterleaveStrategy imbalanceStrategy)
        {
            // produce an iterator collection for all IEnumerable<T> instancess passed to us
            var iterators = sequences.Select(e => e.GetEnumerator()).Acquire();
            List<IEnumerator<T>> iteratorList = null;

            try
            {
                iteratorList = new List<IEnumerator<T>>(iterators);
                iterators = null;
                var shouldContinue = true;
                var consumedIterators = 0;
                var iterCount = iteratorList.Count;

                while (shouldContinue)
                {
                    // advance every iterator and verify a value exists to be yielded
                    for (var index = 0; index < iterCount; index++)
                    {
                        if (!iteratorList[index].MoveNext())
                        {
                            // check if all iterators have been consumed and we can terminate
                            // or if the imbalance strategy informs us that we MUST terminate
                            if (++consumedIterators == iterCount || imbalanceStrategy == ImbalancedInterleaveStrategy.Stop)
                            {
                                shouldContinue = false;
                                break;
                            }

                            iteratorList[index].Dispose(); // dispose the iterator sice we no longer need it

                            // otherwise, apply the imbalance strategy
                            switch (imbalanceStrategy)
                            {
                                case ImbalancedInterleaveStrategy.Pad:
                                    var newIter = iteratorList[index] = Generate(default(T), x => default(T)).GetEnumerator();
                                    newIter.MoveNext();
                                    break;

                                case ImbalancedInterleaveStrategy.Skip:
                                    iteratorList.RemoveAt(index); // no longer visit this particular iterator
                                    --iterCount; // reduce the expected number of iterators to visit
                                    --index; // decrement iterator index to compensate for index shifting
                                    --consumedIterators; // decrement consumer iterator count to stay in balance
                                    break;
                            }

                        }
                    }

                    if (shouldContinue) // only if all iterators could be advanced
                    {
                        // yield the values of each iterator's current position
                        for (var index = 0; index < iterCount; index++)
                        {
                            yield return iteratorList[index].Current;
                        }
                    }
                }
            }
            finally
            {
                Debug.Assert(iteratorList != null || iterators != null);
                foreach (var iter in (iteratorList ?? (IList<IEnumerator<T>>) iterators))
                    iter.Dispose();
            }
        }

        /// <summary>
        /// Defines the strategies available when Interleave is passed sequences of unequal length
        /// </summary>
        enum ImbalancedInterleaveStrategy
        {
            /// <summary>
            /// Extends a sequence by padding its tail with default(T)
            /// </summary>
            Pad,
            /// <summary>
            /// Removes the sequence from the interleave set, and continues interleaving remaining sequences.
            /// </summary>
            Skip,
            /// <summary>
            /// Stops the interleave operation.
            /// </summary>
            Stop,
        }
    }
}