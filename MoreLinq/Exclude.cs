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
        /// Excludes a contiguous number of elements from a sequence starting
        /// at a given index.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the sequence</typeparam>
        /// <param name="sequence">The sequence to exclude elements from</param>
        /// <param name="startIndex">The zero-based index at which to begin excluding elements</param>
        /// <param name="count">The number of elements to exclude</param>
        /// <returns>A sequence that excludes the specified portion of elements</returns>

        public static IEnumerable<T> Exclude<T>(this IEnumerable<T> sequence, int startIndex, int count)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));

            return count switch
            {
                < 0 => throw new ArgumentOutOfRangeException(nameof(count)),
                0 => sequence,
                _ => _()
            };

            IEnumerable<T> _()
            {
                var index = 0;
                var endIndex = startIndex + count;
                using var iter = sequence.GetEnumerator();

                // yield the first part of the sequence
                for (; index < startIndex; index++)
                {
                    if (!iter.MoveNext())
                        yield break;
                    yield return iter.Current;
                }

                // skip the next part (up to count items)
                for (; index < endIndex; index++)
                {
                    if (!iter.MoveNext())
                        yield break;
                }

                // yield the remainder of the sequence
                while (iter.MoveNext())
                    yield return iter.Current;
            }
        }
    }
}
