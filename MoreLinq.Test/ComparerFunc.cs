using System;
using System.Collections.Generic;

namespace MoreLinq.Test
{
    /// <summary>
    /// Utility class that allows Func{T,T,int} to be used as an IComparer{T}
    /// </summary>
    public sealed class ComparerFunc
    {
        public static ComparerFunc<T> As<T>(Func<T, T, int> compare)
        {
            return new ComparerFunc<T>(compare);
        }

        public static ComparerFunc<T> As<T>(T sample, Func<T, T, int> compare)
        {
            return new ComparerFunc<T>(compare);
        }
    }

    public sealed class ComparerFunc<T> : IComparer<T>
    {
        private readonly Func<T, T, int> CompareFunc;

        #region Constructor
        public ComparerFunc(Func<T, T, int> compare)
        {
            if (compare == null)
                throw new ArgumentException("comparer");
            CompareFunc = compare;
        }
        #endregion

        #region IComparer<T> Members
        public int Compare(T x, T y)
        {
            return CompareFunc(x, y);
        }
        #endregion

        #region Static Methods
        public static ComparerFunc<T> As(Func<T, T, int> compare)
        {
            return new ComparerFunc<T>(compare);
        }

        #endregion
    }
}