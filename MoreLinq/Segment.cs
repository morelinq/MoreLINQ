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

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Divides a sequence into multiple sequences by using a segment detector based on the original sequence
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="source">The sequence to segment</param>
        /// <param name="newSegmentPredicate">A function, which returns <c>true</c> if the given element begins a new segment, and <c>false</c> otherwise</param>
        /// <returns>A sequence of segment, each of which is a portion of the original sequence</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either <paramref name="source"/> or <paramref name="newSegmentPredicate"/> are <see langword="null"/>.
        /// </exception>

        public static IEnumerable<IEnumerable<T>> Segment<T>(this IEnumerable<T> source, Func<T, bool> newSegmentPredicate)
        {
            if (newSegmentPredicate == null) throw new ArgumentNullException(nameof(newSegmentPredicate));

            return Segment(source, (curr, _, _) => newSegmentPredicate(curr));
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
            if (newSegmentPredicate == null) throw new ArgumentNullException(nameof(newSegmentPredicate));

            return Segment(source, (curr, _, index) => newSegmentPredicate(curr, index));
        }

        /// <summary>
        /// Divides a sequence into multiple sequences by using a segment detector based on the original sequence
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="source">The sequence to segment</param>
        /// <param name="newSegmentPredicate">A function, which returns <c>true</c> if the given current element, previous element or index indicate a new segment, and <c>false</c> otherwise</param>
        /// <returns>A sequence of segment, each of which is a portion of the original sequence</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either <paramref name="source"/> or <paramref name="newSegmentPredicate"/> are <see langword="null"/>.
        /// </exception>

        public static IEnumerable<IEnumerable<T>> Segment<T>(this IEnumerable<T> source, Func<T, T, int, bool> newSegmentPredicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (newSegmentPredicate == null) throw new ArgumentNullException(nameof(newSegmentPredicate));

            return _(); IEnumerable<IEnumerable<T>> _()
            {
                using var e = source.GetEnumerator();

                if (!e.MoveNext()) // break early (it's empty)
                    yield break;

                // Ensure that the first item is always part of the first
                // segment. This is an intentional behavior. Segmentation always
                // begins with the second element in the sequence.

                var previous = e.Current;
                var segment = new List<T> { previous };

                for (var index = 1; e.MoveNext(); index++)
                {
                    var current = e.Current;

                    if (newSegmentPredicate(current, previous, index))
                    {
                        yield return segment;              // yield the completed segment
                        segment = new List<T> { current }; // start a new segment
                    }
                    else // not a new segment, append and continue
                    {
                        segment.Add(current);
                    }

                    previous = current;
                }

                yield return segment;
            }
        }
    }
}
