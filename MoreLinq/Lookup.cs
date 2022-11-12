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

#if !NET6_0_OR_GREATER
#nullable enable annotations
#pragma warning disable 8602 // Dereference of a possibly null reference.
#pragma warning disable 8603 // Possible null reference return.
#endif

namespace MoreLinq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// A <see cref="ILookup{TKey, TElement}"/> implementation that preserves insertion order
    /// </summary>
    /// <remarks>
    /// This implementation preserves insertion order of keys and elements within each <see
    /// cref="IEnumerable{T}"/>. Copied and modified from
    /// <c><a href="https://github.com/dotnet/runtime/blob/v7.0.0/src/libraries/System.Linq/src/System/Linq/Lookup.cs">Lookup.cs</a></c>
    /// </remarks>

    [DebuggerDisplay("Count = {Count}")]
    sealed class Lookup<TKey, TElement> : ILookup<TKey, TElement>
    {
        private readonly IEqualityComparer<TKey> _comparer;
        private Grouping<TKey, TElement>[] _groupings;
        private Grouping<TKey, TElement>? _lastGrouping;
        private int _count;

        internal static Lookup<TKey, TElement> Create<TSource>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer)
        {
            Debug.Assert(source != null);
            Debug.Assert(keySelector != null);
            Debug.Assert(elementSelector != null);

            Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
            foreach (TSource item in source)
            {
                lookup.GetGrouping(keySelector(item), create: true)!.Add(elementSelector(item));
            }

            return lookup;
        }

        internal static Lookup<TKey, TElement> Create(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            Debug.Assert(source != null);
            Debug.Assert(keySelector != null);

            Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
            foreach (TElement item in source)
            {
                lookup.GetGrouping(keySelector(item), create: true)!.Add(item);
            }

            return lookup;
        }

        internal static Lookup<TKey, TElement> CreateForJoin(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
            foreach (TElement item in source)
            {
                TKey key = keySelector(item);
                if (key != null)
                {
                    lookup.GetGrouping(key, create: true)!.Add(item);
                }
            }

            return lookup;
        }

        private Lookup(IEqualityComparer<TKey>? comparer)
        {
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
            _groupings = new Grouping<TKey, TElement>[7];
        }

        public int Count => _count;

        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                Grouping<TKey, TElement>? grouping = GetGrouping(key, create: false);
                return grouping ?? Enumerable.Empty<TElement>();
            }
        }

        public bool Contains(TKey key) => GetGrouping(key, create: false) != null;

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            Grouping<TKey, TElement>? g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;

                    Debug.Assert(g != null);
                    yield return g;
                }
                while (g != _lastGrouping);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private int InternalGetHashCode(TKey key)
        {
            // Handle comparer implementations that throw when passed null
            return (key == null) ? 0 : _comparer.GetHashCode(key) & 0x7FFFFFFF;
        }

        internal Grouping<TKey, TElement>? GetGrouping(TKey key, bool create)
        {
            int hashCode = InternalGetHashCode(key);
            for (Grouping<TKey, TElement>? g = _groupings[hashCode % _groupings.Length]; g != null; g = g._hashNext)
            {
                if (g._hashCode == hashCode && _comparer.Equals(g._key, key))
                {
                    return g;
                }
            }

            if (create)
            {
                if (_count == _groupings.Length)
                {
                    Resize();
                }

                int index = hashCode % _groupings.Length;
                Grouping<TKey, TElement> g = new Grouping<TKey, TElement>(key, hashCode);
                g._hashNext = _groupings[index];
                _groupings[index] = g;
                if (_lastGrouping == null)
                {
                    g._next = g;
                }
                else
                {
                    g._next = _lastGrouping._next;
                    _lastGrouping._next = g;
                }

                _lastGrouping = g;
                _count++;
                return g;
            }

            return null;
        }

        private void Resize()
        {
            int newSize = checked((_count * 2) + 1);
            Grouping<TKey, TElement>[] newGroupings = new Grouping<TKey, TElement>[newSize];
            Grouping<TKey, TElement> g = _lastGrouping!;
            do
            {
                g = g._next!;
                int index = g._hashCode % newSize;
                g._hashNext = newGroupings[index];
                newGroupings[index] = g;
            }
            while (g != _lastGrouping);

            _groupings = newGroupings;
        }
    }

    // Modified from:
    // https://github.com/dotnet/runtime/blob/v7.0.0/src/libraries/System.Linq/src/System/Linq/Grouping.cs#L48-L141

    [DebuggerDisplay("Key = {Key}")]
    sealed class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IList<TElement>
    {
        internal readonly TKey _key;
        internal readonly int _hashCode;
        internal TElement[] _elements;
        internal int _count;
        internal Grouping<TKey, TElement>? _hashNext;
        internal Grouping<TKey, TElement>? _next;

        internal Grouping(TKey key, int hashCode)
        {
            _key = key;
            _hashCode = hashCode;
            _elements = new TElement[1];
        }

        internal void Add(TElement element)
        {
            if (_elements.Length == _count)
            {
                Array.Resize(ref _elements, checked(_count * 2));
            }

            _elements[_count] = element;
            _count++;
        }

        internal void Trim()
        {
            if (_elements.Length != _count)
            {
                Array.Resize(ref _elements, _count);
            }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _elements[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // DDB195907: implement IGrouping<>.Key implicitly
        // so that WPF binding works on this property.
        public TKey Key => _key;

        int ICollection<TElement>.Count => _count;

        bool ICollection<TElement>.IsReadOnly => true;

        void ICollection<TElement>.Add(TElement item) => ThrowHelper.ThrowNotSupportedException();

        void ICollection<TElement>.Clear() => ThrowHelper.ThrowNotSupportedException();

        bool ICollection<TElement>.Contains(TElement item) => Array.IndexOf(_elements, item, 0, _count) >= 0;

        void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex) =>
            Array.Copy(_elements, 0, array, arrayIndex, _count);

        bool ICollection<TElement>.Remove(TElement item)
        {
            ThrowHelper.ThrowNotSupportedException();
            return false;
        }

        int IList<TElement>.IndexOf(TElement item) => Array.IndexOf(_elements, item, 0, _count);

        void IList<TElement>.Insert(int index, TElement item) => ThrowHelper.ThrowNotSupportedException();

        void IList<TElement>.RemoveAt(int index) => ThrowHelper.ThrowNotSupportedException();

        TElement IList<TElement>.this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return _elements[index];
            }

            set
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        static class ThrowHelper
        {
            [DoesNotReturn]
            internal static void ThrowNotSupportedException() => throw new NotSupportedException("Grouping is immutable.");
        }
    }
}
