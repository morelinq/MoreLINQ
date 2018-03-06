namespace MoreLinq.Test
{
    using System;
    using System.Collections.Generic;

    sealed class Comparer
    {
        /// <summary>
        /// Creates an <see cref="IComparer{T}"/> given a
        /// <see cref="Func{T,T,Int32}"/>.
        /// </summary>

        public static IComparer<T> Create<T>(Func<T, T, int> compare) =>
            new DelegatingComparer<T>(compare);

        sealed class DelegatingComparer<T> : IComparer<T>
        {
            readonly Func<T, T, int> _comparer;

            public DelegatingComparer(Func<T, T, int> comparer)
            {
                _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            }

            public int Compare(T x, T y) => _comparer(x, y);
        }
    }
}
