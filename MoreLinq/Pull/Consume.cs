namespace MoreLinq.Pull
{
    using System.Collections.Generic;

    public static partial class Enumerable
    {
        /// <summary>
        /// Completely consumes the given sequence. This method uses immediate execution,
        /// and doesn't store any data during execution.
        /// </summary>
        /// <typeparam name="T">Element type of the sequence</typeparam>
        /// <param name="source">Source to consume</param>
        public static void Consume<T>(this IEnumerable<T> source)
        {
            source.ThrowIfNull("source");
            foreach (T element in source)
            {
            }
        }
    }
}
