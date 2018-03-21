namespace MoreLinq.Test
{
    using System.Collections;
    using System.Collections.Generic;

    partial class TestExtensions
    {
        internal static IEnumerable<T> ToBreakingList<T>(this IEnumerable<T> enumerable, bool readOnly) =>
            readOnly
            ? (IEnumerable<T>)new BreakingReadOnlyList<T>(enumerable.ToList())
            : new BreakingList<T>(enumerable.ToList());
    }

    /// <summary>
    /// This class implement <see cref="IList{T}"/> but specifically prohibits enumeration using GetEnumerator().
    /// It is provided to assist in testing extension methods that MUST NOT call the GetEnumerator()
    /// method of <see cref="IEnumerable"/> - either because they should be using the indexer or because they are
    /// expected to be lazily evaluated.
    /// </summary>

    sealed class BreakingList<T> : BreakingSequence<T>, IList<T>
    {
        readonly List<T> _list;

        public BreakingList() : this(new List<T>()) {}
        public BreakingList(List<T> list) => _list = list;

        public void Add(T item) => _list.Add(item);
        public void Clear() => _list.Clear();
        public bool Contains(T item) => _list.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
        public bool Remove(T item) => _list.Remove(item);
        public int Count => _list.Count;
        public bool IsReadOnly => ((ICollection<T>)_list).IsReadOnly;
        public int IndexOf(T item) => _list.IndexOf(item);
        public void Insert(int index, T item) => _list.Insert(index, item);
        public void RemoveAt(int index) => _list.RemoveAt(index);

        public T this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }
    }
}
