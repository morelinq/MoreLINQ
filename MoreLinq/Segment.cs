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

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Divides a sequence into multiple sequences by using a segment detector based on the original sequence
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="sequence">The sequence to segment</param>
        /// <param name="newSegmentPredicate">A function, which returns <c>true</c> if the given element begins a new segment, and <c>false</c> otherwise</param>
        /// <returns>A sequence of segment, each of which is a portion of the original sequence</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either <paramref name="sequence"/> or <paramref name="newSegmentPredicate"/> are <see langword="null"/>.
        /// </exception>
        
        public static IEnumerable<IEnumerable<T>> Segment<T>(this IEnumerable<T> sequence, Func<T, bool> newSegmentPredicate)
        {
            if (newSegmentPredicate == null) throw new ArgumentNullException("newSegmentPredicate");

            return Segment(sequence, (curr, prev, index) => newSegmentPredicate(curr));
        }

        /// <summary>
        /// Divides a sequence into multiple sequences by using a segment detector based on the original sequence
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="source">The sequence to segment</param>
        /// <param name="newSegmentPredicate">A function, which returns <c>true</c> if the given element or index indicate a new segment, and <c>false</c> otherwise</param>
        /// <returns>A sequence of segment, each of which is a portion of the original sequence</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either <paramref name="source"/> or <paramref name="newSegmentPredicate"/> are <see langword="null"/>.
        /// </exception>
        
        public static IEnumerable<IEnumerable<T>> Segment<T>(this IEnumerable<T> source, Func<T, int, bool> newSegmentPredicate)
        {
            if (newSegmentPredicate == null) throw new ArgumentNullException("newSegmentPredicate");

            return Segment(source, (curr, prev, index) => newSegmentPredicate(curr, index));
        }

        /// <summary>
        /// Divides a sequence into multiple sequences by using a segment detector based on the original sequence
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="sequence">The sequence to segment</param>
        /// <param name="newSegmentPredicate">A function, which returns <c>true</c> if the given current element, previous element or index indicate a new segment, and <c>false</c> otherwise</param>
        /// <returns>A sequence of segment, each of which is a portion of the original sequence</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either <paramref name="sequence"/> or <paramref name="newSegmentPredicate"/> are <see langword="null"/>.
        /// </exception>
        
        public static IEnumerable<IEnumerable<T>> Segment<T>(this IEnumerable<T> sequence, Func<T, T, int, bool> newSegmentPredicate)
        {
            if (sequence == null) throw new ArgumentNullException("source");
            if (newSegmentPredicate == null) throw new ArgumentNullException("newSegmentPredicate");

            return SegmentImpl(sequence, newSegmentPredicate);
        }
                
        private static IEnumerable<IEnumerable<T>> SegmentImpl<T>(IEnumerable<T> source, Func<T, T, int, bool> newSegmentPredicate)
        {
            var index = -1;
            using (var iter = source.GetEnumerator())
            {
                var segment = new List<T>();
                var prevItem = default(T);

                // ensure that the first item is always part
                // of the first segment. This is an intentional
                // behavior. Segmentation always begins with
                // the second element in the sequence.
                if (iter.MoveNext())
                {
                    ++index;
                    segment.Add(iter.Current);
                    prevItem = iter.Current;
                }

                while (iter.MoveNext())
                {
                    ++index;
                    // check if the item represents the start of a new segment
                    var isNewSegment = newSegmentPredicate(iter.Current, prevItem, index);
                    prevItem = iter.Current;

                    if (!isNewSegment)
                    {
                        // if not a new segment, append and continue
                        segment.Add(iter.Current);
                        continue;
                    }
                    yield return segment; // yield the completed segment

                    // start a new segment...
                    segment = new List<T> { iter.Current };
                }
                // handle the case of the sequence ending before new segment is detected
                if (segment.Count > 0)
                    yield return segment;
            }
        }
    }
}