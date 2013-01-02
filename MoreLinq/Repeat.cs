namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Repeats the specific sequences <paramref name="count"/> times.
        /// </summary>
        /// <param name="sequence">The sequence to repeat</param>
        /// <param name="count">Number of times to repeat the sequence</param>
        /// <returns>A sequence produced from the repetition of the original source sequence</returns>
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> sequence, int count)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (count < 0) throw new ArgumentOutOfRangeException("count", count, "Repeat count must be greater than or equal to zero.");
            return RepeatImpl(sequence, count);
        }

        private static IEnumerable<T> RepeatImpl<T>(this IEnumerable<T> sequence, int count)
        {
            while (count-- > 0)
            {
                // TODO buffer to avoid multiple enumerations
                foreach (var item in sequence)
                    yield return item;
            }
        }
    }
}