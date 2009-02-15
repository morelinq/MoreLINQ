using System;

namespace MoreLinq.Test.Pull
{
    /// <summary>
    /// Functions which throw NotImplementedException if they're ever called.
    /// </summary>
    static class BreakingFunc
    {
        internal static Func<TResult> Of<TResult>()
        {
            return () => { throw new NotImplementedException(); };
        }

        internal static Func<T, TResult> Of<T, TResult>()
        {
            return t => { throw new NotImplementedException(); };
        }

        internal static Func<T1, T2, TResult> Of<T1, T2, TResult>()
        {
            return (t1, t2) => { throw new NotImplementedException(); };
        }

        internal static Func<T1, T2, T3, TResult> Of<T1, T2, T3, TResult>()
        {
            return (t1, t2, t3) => { throw new NotImplementedException(); };
        }

        internal static Func<T1, T2, T3, T4, TResult> Of<T1, T2, T3, T4, TResult>()
        {
            return (t1, t2, t3, t4) => { throw new NotImplementedException(); };
        }
    }
}
