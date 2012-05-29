using System;
using System.Collections.Generic;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Repeats the specific sequences <paramref name="repeatCount"/> times.
        /// </summary>
        /// <param name="sequence">The sequence to repeat</param>
        /// <param name="repeatCount">Number of times to repeat the sequence</param>
        /// <returns>A sequence produced from the repetition of the original source sequence</returns>
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> sequence, int repeatCount)
        {
            if (repeatCount < 0)
                throw new ArgumentOutOfRangeException("repeatCount", "Repeat count must be >= 0");
            return RepeatImpl(sequence, repeatCount);
        }

        private static IEnumerable<T> RepeatImpl<T>(this IEnumerable<T> sequence, int repeatCount)
        {
            while (repeatCount-- > 0)
            {
                foreach (var item in sequence)
                    yield return item;
            }
        }
    }
}