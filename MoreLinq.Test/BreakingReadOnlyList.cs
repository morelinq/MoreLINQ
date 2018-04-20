namespace MoreLinq.Test
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// This class implement <see cref="IReadOnlyList{T}"/> but specifically prohibits enumeration using GetEnumerator().
    /// It is provided to assist in testing extension methods that MUST NOT call the GetEnumerator()
    /// method of <see cref="IEnumerable"/> - either because they should be using the indexer or because they are
    /// expected to be lazily evaluated.
    /// </summary>

    sealed class BreakingReadOnlyList<T> : BreakingReadOnlyCollection<T>, IReadOnlyList<T>
    {
        readonly IReadOnlyList<T> _list;

        public BreakingReadOnlyList(params T[] values) : this ((IReadOnlyList<T>) values) {}
        public BreakingReadOnlyList(IReadOnlyList<T> list) : base (list)
            => _list = list;

        public T this[int index] => _list[index];
    }
}
