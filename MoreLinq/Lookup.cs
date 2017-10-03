#region License and Terms
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// The MIT License (MIT)
//
// Copyright(c) Microsoft Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

namespace MoreLinq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A <see cref="ILookup{TKey, TElement}"/> implementation that preserves insertion order
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the <see cref="Lookup{TKey, TElement}"/></typeparam>
    /// <typeparam name="TElement">The type of the elements in the <see cref="IEnumerable{T}"/> sequences that make up the values in the <see cref="Lookup{TKey, TElement}"/></typeparam>
    /// <remarks>
    /// This implementation preserves insertion order of keys and elements within each <see cref="IEnumerable{T}"/>
    /// Copied over from CoreFX on 2015-10-27
    /// https://github.com/dotnet/corefx/blob/6f1c2a86fb8fa1bdaee7c6e70a684d27842d804c/src/System.Linq/src/System/Linq/Enumerable.cs#L3230-L3403
    /// Modified to remove internal interfaces
    /// </remarks>
    internal class Lookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>, ILookup<TKey, TElement>
    {
        private IEqualityComparer<TKey> _comparer;
        private Grouping<TKey, TElement>[] _groupings;
        private Grouping<TKey, TElement> _lastGrouping;
        private int _count;

        internal static Lookup<TKey, TElement> Create<TSource>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));
            Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
            foreach (TSource item in source) {
                lookup.GetGrouping(keySelector(item), true).Add(elementSelector(item));
            }
            return lookup;
        }

        internal static Lookup<TKey, TElement> CreateForJoin(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
            foreach (TElement item in source) {
                TKey key = keySelector(item);
                if (key != null) lookup.GetGrouping(key, true).Add(item);
            }
            return lookup;
        }

        private Lookup(IEqualityComparer<TKey> comparer)
        {
            if (comparer == null) comparer = EqualityComparer<TKey>.Default;
            _comparer = comparer;
            _groupings = new Grouping<TKey, TElement>[7];
        }

        public int Count
        {
            get { return _count; }
        }

        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                Grouping<TKey, TElement> grouping = GetGrouping(key, false);
                if (grouping != null) return grouping;
                return Enumerable.Empty<TElement>();
            }
        }

        public bool Contains(TKey key)
        {
            return _count > 0 && GetGrouping(key, false) != null;
        }

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            Grouping<TKey, TElement> g = _lastGrouping;
            if (g != null) {
                do {
                    g = g.next;
                    yield return g;
                } while (g != _lastGrouping);
            }
        }

        public IEnumerable<TResult> ApplyResultSelector<TResult>(Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            Grouping<TKey, TElement> g = _lastGrouping;
            if (g != null) {
                do {
                    g = g.next;
                    if (g.count != g.elements.Length) { Array.Resize<TElement>(ref g.elements, g.count); }
                    yield return resultSelector(g.key, g.elements);
                } while (g != _lastGrouping);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal int InternalGetHashCode(TKey key)
        {
            // Handle comparer implementations that throw when passed null
            return (key == null) ? 0 : _comparer.GetHashCode(key) & 0x7FFFFFFF;
        }

        internal Grouping<TKey, TElement> GetGrouping(TKey key, bool create)
        {
            int hashCode = InternalGetHashCode(key);
            for (Grouping<TKey, TElement> g = _groupings[hashCode % _groupings.Length]; g != null; g = g.hashNext)
                if (g.hashCode == hashCode && _comparer.Equals(g.key, key)) return g;
            if (create) {
                if (_count == _groupings.Length) Resize();
                int index = hashCode % _groupings.Length;
                Grouping<TKey, TElement> g = new Grouping<TKey, TElement>();
                g.key = key;
                g.hashCode = hashCode;
                g.elements = new TElement[1];
                g.hashNext = _groupings[index];
                _groupings[index] = g;
                if (_lastGrouping == null) {
                    g.next = g;
                }
                else {
                    g.next = _lastGrouping.next;
                    _lastGrouping.next = g;
                }
                _lastGrouping = g;
                _count++;
                return g;
            }
            return null;
        }

        private void Resize()
        {
            int newSize = checked(_count * 2 + 1);
            Grouping<TKey, TElement>[] newGroupings = new Grouping<TKey, TElement>[newSize];
            Grouping<TKey, TElement> g = _lastGrouping;
            do {
                g = g.next;
                int index = g.hashCode % newSize;
                g.hashNext = newGroupings[index];
                newGroupings[index] = g;
            } while (g != _lastGrouping);
            _groupings = newGroupings;
        }
    }

    internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IList<TElement>
    {
        internal TKey key;
        internal int hashCode;
        internal TElement[] elements;
        internal int count;
        internal Grouping<TKey, TElement> hashNext;
        internal Grouping<TKey, TElement> next;

        internal Grouping()
        {
        }

        internal void Add(TElement element)
        {
            if (elements.Length == count) Array.Resize(ref elements, checked(count * 2));
            elements[count] = element;
            count++;
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            for (int i = 0; i < count; i++) yield return elements[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // DDB195907: implement IGrouping<>.Key implicitly
        // so that WPF binding works on this property.
        public TKey Key
        {
            get { return key; }
        }

        int ICollection<TElement>.Count
        {
            get { return count; }
        }

        bool ICollection<TElement>.IsReadOnly
        {
            get { return true; }
        }

        void ICollection<TElement>.Add(TElement item)
        {
            throw new NotSupportedException("Lookup is immutable");
        }

        void ICollection<TElement>.Clear()
        {
            throw new NotSupportedException("Lookup is immutable");
        }

        bool ICollection<TElement>.Contains(TElement item)
        {
            return Array.IndexOf(elements, item, 0, count) >= 0;
        }

        void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex)
        {
            Array.Copy(elements, 0, array, arrayIndex, count);
        }

        bool ICollection<TElement>.Remove(TElement item)
        {
            throw new NotSupportedException("Lookup is immutable");
        }

        int IList<TElement>.IndexOf(TElement item)
        {
            return Array.IndexOf(elements, item, 0, count);
        }

        void IList<TElement>.Insert(int index, TElement item)
        {
            throw new NotSupportedException("Lookup is immutable");
        }

        void IList<TElement>.RemoveAt(int index)
        {
            throw new NotSupportedException("Lookup is immutable");
        }

        TElement IList<TElement>.this[int index]
        {
            get
            {
                if (index < 0 || index >= count) throw new ArgumentOutOfRangeException(nameof(index));
                return elements[index];
            }
            set
            {
                throw new NotSupportedException("Lookup is immutable");
            }
        }
    }
}
