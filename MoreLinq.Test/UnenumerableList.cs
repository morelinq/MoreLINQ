using System;
using System.Collections;
using System.Collections.Generic;

namespace MoreLinq.Test
{
    /// <summary>
    /// This class implement <see cref="IList{T}"/> but specifically prohibits enumeration using GetEnumerator().
    /// It is provided to assist in testing extension methods that MUST NOT call the GetEnumerator()
    /// method of <see cref="IEnumerable"/> - either because they should be using the indexer or because they are
    /// expected to be lazily evaluated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class UnenumerableList<T> : IList<T>
    {
        private readonly List<T> m_List = new List<T>();

        // intentionally implemented to thow exception - ensures iteration is not used in Slice
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public IEnumerator<T> GetEnumerator() { throw new NotImplementedException(); }
        // all other IList<T> members are forwarded back to the underlying private list
        public void Add(T item) { m_List.Add(item); }
        public void Clear() { m_List.Clear(); }
        public bool Contains(T item) { return m_List.Contains(item); }
        public void CopyTo(T[] array, int arrayIndex) { m_List.CopyTo(array, arrayIndex); }
        public bool Remove(T item) { return m_List.Remove(item); }
        public int Count { get { return m_List.Count; } }
        public bool IsReadOnly { get { return ((ICollection<T>)m_List).IsReadOnly; } }
        public int IndexOf(T item) { return m_List.IndexOf(item); }
        public void Insert(int index, T item) { m_List.Insert(index, item); }
        public void RemoveAt(int index) { m_List.RemoveAt(index); }
        public T this[int index]
        {
            get { return m_List[index]; }
            set { m_List[index] = value; }
        }
    }
}