using System.Collections.Generic;
using System.Linq;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Processes a sequence into a series of subsequences representing a windowed subset of the original
        /// </summary>
        /// <remarks>
        /// This operator is guaranteed to return at least one result, even if the source sequence is smaller
        /// than the window size.<br/>
        /// The number of sequences returned is: <c>Max(0, sequence.Count() - windowSize) + 1</c><br/>
        /// Returned subsequences are buffered, but the overall operation is streamed.<br/>
        /// </remarks>
        /// <typeparam name="T">The type of the elements of the source sequence</typeparam>
        /// <param name="sequence">The sequence to evaluate a sliding window over</param>
        /// <param name="windowSize">The size (number of elements) in each window</param>
        /// <returns>A series of sequences representing each sliding window subsequence</returns>
        public static IEnumerable<IEnumerable<T>> SlidingWindow<T>(this IEnumerable<T> sequence, int windowSize)
        {
            sequence.ThrowIfNull("sequence");
            windowSize.ThrowIfNonPositive("windowSize");

            return SlidingWindowImpl(sequence, windowSize);
        }

        private static IEnumerable<IEnumerable<T>> SlidingWindowImpl<T>(this IEnumerable<T> sequence, int windowSize)
        {
            using (var iter = sequence.GetEnumerator())
            {
                // generate the first window of items
                var countLeft = windowSize;
                var window = new List<T>();
                // NOTE: The order of evaluation in the if() below is important
                //       because it relies on short-circuit behavior to ensure
                //       we don't move the iterator once the window is complete
                while (countLeft-- > 0 && iter.MoveNext())
                {
                    window.Add(iter.Current);
                }

                // return the first window (whatever size it may be)
                yield return window;

                // generate the next window by shifting forward by one item
                while (iter.MoveNext())
                {
                    // NOTE: If we used a circular queue rather than a list, 
                    //       we could make this quite a bit more efficient.
                    //       Sadly the BCL does not offer such a collection.
                    window = new List<T>(window.Skip(1)) { iter.Current };
                    yield return window;
                }
            }
        }
    }
}