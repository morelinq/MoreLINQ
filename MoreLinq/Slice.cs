using System.Collections.Generic;
using System.Linq;

namespace MoreLinq
{
    using System;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Extracts <paramref name="count"/> elements from a sequence at a particular zero-based starting index
        /// </summary>
        /// <remarks>
        /// If the starting position or count specified result in slice extending past the end of the sequence,
        /// it will return all elements up to that point. There is no guarantee that the resulting sequence will
        /// contain the number of elements requested - it may have anywhere from 0 to <paramref name="count"/>.<br/>
        /// This method is implemented in an optimized manner for any sequence implementing <c>IList{T}</c>.<br/>
        /// The result of Slice() is identical to: <c>sequence.Skip(startIndex).Take(count)</c>
        /// </remarks>
        /// <typeparam name="T">The type of the elements in the source sequence</typeparam>
        /// <param name="sequence">The sequence from which to extract elements</param>
        /// <param name="startIndex">The zero-based index at which to begin slicing</param>
        /// <param name="count">The number of items to slice out of the index</param>
        /// <returns>A new sequence containing any elements sliced out from the source sequence</returns>
        public static IEnumerable<T> Slice<T>(this IEnumerable<T> sequence, int startIndex, int count)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (startIndex < 0) throw new ArgumentOutOfRangeException("startIndex");
            if (count < 0) throw new ArgumentOutOfRangeException("count");

            // optimization for anything implementing IList<T>
            var asList = sequence as IList<T>;
            return asList != null
                ? SliceImpl(asList, startIndex, count)
                : SliceImpl(sequence, startIndex, count);
        }

        private static IEnumerable<T> SliceImpl<T>(IEnumerable<T> sequence, int startIndex, int count)
        {
            return sequence.Skip(startIndex).Take(count);
        }

        private static IEnumerable<T> SliceImpl<T>(IList<T> list, int startIndex, int count)
        {
            var listCount = list.Count;
            var index = startIndex;
            while (index < listCount && count-- > 0)
                yield return list[index++];
        }
    }
}