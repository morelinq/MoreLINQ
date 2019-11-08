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

            return Window(source, size, false, false);
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

        private static IEnumerable<IList<TSource>> Window<TSource>(IEnumerable<TSource> source, int size,
            bool hasPartialBegin, bool hasPartialEnd)
        {
            using var iter = source.GetEnumerator();

            var hasNext = iter.MoveNext();

            // early break
            if (!hasNext)
                yield break;

            // Store the window to be yield.
            // In any cases we build the next window (if any) before yielding
            // Loops do not have to yield the last window they created.
            TSource[] window;

            // Warm-up
            if (hasPartialBegin)
            {
                // build first partial window;
                window = new[] {iter.Current};
                hasNext = iter.MoveNext();

                // build other partial windows
                while (window.Length < size && hasNext)
                {
                    // Prepare next window, bigger than the previous one
                    var nextWindow = new TSource[window.Length + 1];
                    Array.Copy(window, nextWindow, window.Length);

                    // window ready to ship, we forget it immediately
                    yield return window;

                    nextWindow[nextWindow.Length - 1] = iter.Current;
                    hasNext = iter.MoveNext();
                    window = nextWindow;
                }
            }
            else
            {
                // build first window
                window = new TSource[size];
                int i;
                for (i = 0; i < size && hasNext; i++)
                {
                    window[i] = iter.Current;
                    hasNext = iter.MoveNext();
                }

                // Ensure correct size on partial window cases
                if (i != size)
                {
                    if (hasPartialEnd)
                        Array.Resize(ref window, i);
                    else
                        yield break;
                }
                    Array.Resize(ref window, i);
            }

            // Main loop
            if (window.Length == size)
            {
                // build windows of given size
                while (hasNext)
                {
                    // Prepare next window, same size as the previous one
                    var nextWindow = new TSource[size];
                    Array.Copy(window, 1, nextWindow, 0, size - 1);

                    // window ready to ship, we forget it immediately
                    yield return window;

                    nextWindow[size - 1] = iter.Current;
                    hasNext = iter.MoveNext();
                    window = nextWindow;
                }
            }

            // Cool down
            if (hasPartialEnd)
            {
                // build final partial windows
                while (window.Length > 1)
                {
                    // Prepare next window, smaller than the previous one
                    var nextWindow = new TSource[window.Length - 1];
                    Array.Copy(window, 1, nextWindow, 0, nextWindow.Length);

                    // window ready to ship, we forget it immediately
                    yield return window;
                    window = nextWindow;
                }
            }

            // No more windows to build, we can finally yield this one
            if (hasPartialBegin || hasPartialEnd || window.Length == size)
            {
                yield return window;
            }
        }
    }
}
