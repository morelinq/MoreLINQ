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
        /// Run-length encodes a sequence by converting consecutive instances of the same element into
        /// a <c>KeyValuePair{T,int}</c> representing the item and its occurrence count.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="sequence">The sequence to run length encode</param>
        /// <returns>A sequence of <c>KeyValuePair{T,int}</c> where the key is the element and the value is the occurrence count</returns>

        public static IEnumerable<KeyValuePair<T, int>> RunLengthEncode<T>(this IEnumerable<T> sequence)
        {
            return RunLengthEncode(sequence, null);
        }

        /// <summary>
        /// Run-length encodes a sequence by converting consecutive instances of the same element into
        /// a <c>KeyValuePair{T,int}</c> representing the item and its occurrence count. This overload
        /// uses a custom equality comparer to identify equivalent items.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="sequence">The sequence to run length encode</param>
        /// <param name="comparer">The comparer used to identify equivalent items</param>
        /// <returns>A sequence of <c>KeyValuePair{T,int}</c> where they key is the element and the value is the occurrence count</returns>

        public static IEnumerable<KeyValuePair<T, int>> RunLengthEncode<T>(this IEnumerable<T> sequence, IEqualityComparer<T>? comparer)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));

            return _(comparer ?? EqualityComparer<T>.Default);

            IEnumerable<KeyValuePair<T, int>> _(IEqualityComparer<T> comparer)
            {
                // This implementation could also have been written using a foreach loop,
                // but it proved to be easier to deal with edge certain cases that occur
                // (such as empty sequences) using an explicit iterator and a while loop.

                using var iter = sequence.GetEnumerator();

                if (iter.MoveNext())
                {
                    var prevItem = iter.Current;
                    var runCount = 1;

                    while (iter.MoveNext())
                    {
                        if (comparer.Equals(prevItem, iter.Current))
                        {
                            ++runCount;
                        }
                        else
                        {
                            yield return new KeyValuePair<T, int>(prevItem, runCount);
                            prevItem = iter.Current;
                            runCount = 1;
                        }
                    }

                    yield return new KeyValuePair<T, int>(prevItem, runCount);
                }
            }
        }
    }
}
