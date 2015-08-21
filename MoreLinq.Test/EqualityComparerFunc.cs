using System;
using System.Collections.Generic;

namespace MoreLinq.Test
{
    /// <summary>
    /// Utility class that allows Func{T,T,bool} to be used as an IEqualityComparer{T}
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class EqualityComparerFunc<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> CompareFunc;
        private readonly Func<T, int> HashFunc;

        #region Constructor
        /// <summary>
        /// Creates an equality comparer using a specified equality comparison function.
        /// </summary>
        /// <param name="compare"></param>
        public EqualityComparerFunc(Func<T, T, bool> compare)
            : this(compare, x => x == null ? 0 : x.GetHashCode())
        {
        }

        /// <summary>
        /// Creates an equality comparer using a specified equality comparison function
        /// and hashing functions.
        /// </summary>
        /// <param name="compare"></param>
        /// <param name="hash"></param>
        public EqualityComparerFunc(Func<T, T, bool> compare, Func<T, int> hash)
        {
            if (compare == null)
                throw new ArgumentException("comparer");
            if (hash == null)
                throw new ArgumentException("hash");

            CompareFunc = compare;
            HashFunc = hash;
        }
        #endregion

        #region IEqualityComparer<T> Members
        /// <summary>
        /// Implementation of the <c>IEqualityComparer{T}.Equals(T,T)</c> method - calls the cached equality function.
        /// </summary>
        /// <returns></returns>
        public bool Equals(T x, T y)
        {
            return CompareFunc(x, y);
        }

        /// <summary>
        /// Implementation of the <c>IEqualityComparer{T}.GetHasCode(T)</c> method - calls the cached hashing function.
        /// </summary>
        public int GetHashCode(T obj)
        {
            return HashFunc(obj);
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Static factory method that returns an equality comparer given a comparison function.
        /// </summary>
        public static EqualityComparerFunc<T> As(Func<T, T, bool> compare)
        {
            return new EqualityComparerFunc<T>(compare);
        }
        #endregion
    }
}