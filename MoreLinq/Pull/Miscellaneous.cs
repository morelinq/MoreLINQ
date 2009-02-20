using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreLinq.Pull
{
    /// <summary>
    /// Extension methods which don't fit anywhere else yet. Ideally this class should
    /// stay small and eventually be removed!
    /// </summary>
    public static class Miscellaneous
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
