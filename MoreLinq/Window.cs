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
        /// Processes a sequence into a series of subsequences representing a windowed subset of the original
        /// </summary>
        /// <remarks>
        /// The number of sequences returned is: <c>Max(0, sequence.Count() - windowSize) + 1</c><br/>
        /// Returned subsequences are buffered, but the overall operation is streamed.<br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
        /// <param name="source">The sequence to evaluate a sliding window over</param>
        /// <param name="size">The size (number of elements) in each window</param>
        /// <returns>A series of sequences representing each sliding window subsequence</returns>

        public static IEnumerable<IList<TSource>> Window<TSource>(this IEnumerable<TSource> source, int size)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            return _(); IEnumerable<IList<TSource>> _()
            {
                using (var iter = source.GetEnumerator())
                {
                    // generate the first window of items
                    var window = new TSource[size];
                    int i;
                    for (i = 0; i < size && iter.MoveNext(); i++)
                        window[i] = iter.Current;

                    if (i < size)
                        yield break;

                    // return the first window (whatever size it may be)
                    yield return window;

                    // generate the next window by shifting forward by one item
                    while (iter.MoveNext())
                    {
                        // NOTE: If we used a circular queue rather than a list,
                        //       we could make this quite a bit more efficient.
                        //       Sadly the BCL does not offer such a collection.
                        var newWindow = new TSource[size];
                        Array.Copy(window, 1, newWindow, 0, size - 1);
                        newWindow[size - 1] = iter.Current;
                        yield return newWindow;
                        window = newWindow;
                    }
                }
            }
        }

        /// <summary>
        /// Processes a sequence into a series of subsequences representing a windowed subset of the original
        /// </summary>
        /// <remarks>
        /// The number of sequences returned is: <c>Max(0, sequence.Count() - windowSize) + 1</c><br/>
        /// Returned subsequences are buffered, but the overall operation is streamed.<br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
        /// <param name="source">The sequence to evaluate a sliding window over</param>
        /// <param name="size">The size (number of elements) in each window</param>
        /// <returns>A series of sequences representing each sliding window subsequence</returns>

        [Obsolete("Use " + nameof(Window) + " instead.")]
        public static IEnumerable<IEnumerable<TSource>> Windowed<TSource>(this IEnumerable<TSource> source, int size) =>
            source.Window(size);
    }
}
