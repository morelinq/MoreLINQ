using System;
using System.Collections.Generic;

namespace MoreLinq
{
    /// <summary>
    /// Strategy determining the handling of the case where the inputs are of
    /// unequal lengths in <see cref="Enumerable.Zip{T1,T2,TResult}(IEnumerable{T1},IEnumerable{T2},Func{T1,T2,TResult},ImbalancedZipStrategy)"/>.
    /// </summary>
    public enum ImbalancedZipStrategy : byte
    {
        /// <summary>
        /// The result sequence ends when either input sequence is exhausted.
        /// </summary>
        Truncate = 0,
        /// <summary>
        /// The result sequence ends when both sequences are exhausted. The 
        /// shorter sequence is effectively "padded" at the end with the default
        /// value for its element type.
        /// </summary>
        Pad = 1,
        /// <summary>
        /// <see cref="InvalidOperationException" /> is thrown if one sequence
        /// is exhausted but not the other.
        /// </summary>
        Fail = 2
    }
}
