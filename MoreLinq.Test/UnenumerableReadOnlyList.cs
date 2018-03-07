namespace MoreLinq.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// This class implement <see cref="IReadOnlyList{T}"/> but specifically prohibits enumeration using GetEnumerator().
    /// It is provided to assist in testing extension methods that MUST NOT call the GetEnumerator()
    /// method of <see cref="IEnumerable"/> - either because they should be using the indexer or because they are
    /// expected to be lazily evaluated.
    /// </summary>

    sealed class UnenumerableReadOnlyList<T> : IReadOnlyList<T>
    {
        readonly IReadOnlyList<T> _list;

        public UnenumerableReadOnlyList(IEnumerable<T> enumerable)
        {
            _list = enumerable.ToList();
        }

        // intentionally implemented to throw exception - ensures iteration is not used in Slice
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();
        // all other IReadOnlyList<T> members are forwarded back to the underlying private list
        public int Count => _list.Count;

        public T this[int index] => _list[index];
    }
}
