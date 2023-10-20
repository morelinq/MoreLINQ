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
        /// Extracts a contiguous count of elements from a sequence at a particular zero-based
        /// starting index.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="sequence">The sequence from which to extract elements.</param>
        /// <param name="startIndex">The zero-based index at which to begin slicing.</param>
        /// <param name="count">The number of items to slice out of the index.</param>
        /// <returns>
        /// A new sequence containing any elements sliced out from the source sequence.</returns>
        /// <remarks>
        /// <para>
        /// If the starting position or count specified result in slice extending past the end of
        /// the sequence, it will return all elements up to that point. There is no guarantee that
        /// the resulting sequence will contain the number of elements requested - it may have
        /// anywhere from 0 to <paramref name="count"/>.</para>
        /// <para>
        /// This method is implemented in an optimized manner for any sequence implementing <see
        /// cref="IList{T}"/>.</para>
        /// <para>
        /// The result of <see cref="Slice{T}"/> is identical to:
        /// <c>sequence.Skip(startIndex).Take(count)</c></para>
        /// </remarks>

        public static IEnumerable<T> Slice<T>(this IEnumerable<T> sequence, int startIndex, int count)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            return sequence switch
            {
                IList<T> list => SliceList(list.Count, i => list[i]),
                IReadOnlyList<T> list => SliceList(list.Count, i => list[i]),
                var seq => seq.Skip(startIndex).Take(count)
            };

            IEnumerable<T> SliceList(int listCount, Func<int, T> indexer)
            {
                var countdown = count;
                var index = startIndex;
                while (index < listCount && countdown-- > 0)
                    yield return indexer(index++);
            }
        }
    }
}
