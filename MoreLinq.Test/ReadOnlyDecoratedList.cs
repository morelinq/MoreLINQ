namespace MoreLinq.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A decorator for a <see cref="IReadOnlyList{T}"/> that ensures it does not implement another interface.
    /// </summary>
    sealed class ReadOnlyDecoratedList<T> : IReadOnlyList<T>
    {
        private readonly IReadOnlyList<T> _list;

        public ReadOnlyDecoratedList(IReadOnlyList<T> readOnlyList)
        {
            _list = readOnlyList;
        }

        public T this[int index] => _list[index];
        public int Count => _list.Count;
        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
