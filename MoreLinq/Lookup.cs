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

#pragma warning disable IDE0040 // Add accessibility modifiers
#pragma warning disable IDE0032 // Use auto property
#pragma warning disable IDE0017 // Simplify object initialization

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
    internal sealed class Lookup<TKey, TElement> : ILookup<TKey, TElement>
    {
        readonly IEqualityComparer<TKey> _comparer;
        Grouping<TKey, TElement>[] _groupings;
        Grouping<TKey, TElement>? _lastGrouping;
        int _count;

        internal static Lookup<TKey, TElement> Create<TSource>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer)
        {
            Debug.Assert(source is not null);
            Debug.Assert(keySelector is not null);
            Debug.Assert(elementSelector is not null);

            var lookup = new Lookup<TKey, TElement>(comparer);

            foreach (var item in source)
            {
                var grouping = Assume.NotNull(lookup.GetGrouping(keySelector(item), create: true));
                grouping.Add(elementSelector(item));
            }

            return lookup;
        }

        internal static Lookup<TKey, TElement> Create(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            Debug.Assert(source is not null);
            Debug.Assert(keySelector is not null);

            var lookup = new Lookup<TKey, TElement>(comparer);

            foreach (var item in source)
            {
                var grouping = Assume.NotNull(lookup.GetGrouping(keySelector(item), create: true));
                grouping.Add(item);
            }

            return lookup;
        }

        internal static Lookup<TKey, TElement> CreateForJoin(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            var lookup = new Lookup<TKey, TElement>(comparer);

            foreach (var item in source)
            {
                if (keySelector(item) is { } key)
                {
                    var grouping = Assume.NotNull(lookup.GetGrouping(key, create: true));
                    grouping.Add(item);
                }
            }

            return lookup;
        }

        Lookup(IEqualityComparer<TKey>? comparer)
        {
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
            _groupings = new Grouping<TKey, TElement>[7];
        }

        public int Count => _count;

        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                var grouping = GetGrouping(key, create: false);
                return grouping ?? Enumerable.Empty<TElement>();
            }
        }

        public bool Contains(TKey key) => GetGrouping(key, create: false) is not null;

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            var g = _lastGrouping;
            if (g is not null)
            {
                do
                {
                    g = Assume.NotNull(g._next);
                    yield return g;
                }
                while (g != _lastGrouping);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        int InternalGetHashCode(TKey key) =>
            // Handle comparer implementations that throw when passed null
            key is null ? 0 : _comparer.GetHashCode(key) & 0x7FFFFFFF;

        internal Grouping<TKey, TElement>? GetGrouping(TKey key, bool create)
        {
            var hashCode = InternalGetHashCode(key);
            for (var g = _groupings[hashCode % _groupings.Length]; g is not null; g = g._hashNext)
            {
                if (g._hashCode == hashCode && _comparer.Equals(g._key, key))
                    return g;
            }

            if (create)
            {
                if (_count == _groupings.Length)
                {
                    Resize();
                }

                var index = hashCode % _groupings.Length;
                var g = new Grouping<TKey, TElement>(key, hashCode);
                g._hashNext = _groupings[index];
                _groupings[index] = g;
                if (_lastGrouping is null)
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

        void Resize()
        {
            var newSize = checked((_count * 2) + 1);
            var newGroupings = new Grouping<TKey, TElement>[newSize];
            var g = Assume.NotNull(_lastGrouping);
            do
            {
                g = Assume.NotNull(g._next);
                var index = g._hashCode % newSize;
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
    internal sealed class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IList<TElement>
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
                Array.Resize(ref _elements, checked(_count * 2));

            _elements[_count] = element;
            _count++;
        }

        internal void Trim()
        {
            if (_elements.Length != _count)
                Array.Resize(ref _elements, _count);
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            for (var i = 0; i < _count; i++)
                yield return _elements[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // DDB195907: implement IGrouping<>.Key implicitly
        // so that WPF binding works on this property.
        public TKey Key => _key;

        int ICollection<TElement>.Count => _count;

        bool ICollection<TElement>.IsReadOnly => true;

        bool ICollection<TElement>.Contains(TElement item) => Array.IndexOf(_elements, item, 0, _count) >= 0;

        void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex) =>
            Array.Copy(_elements, 0, array, arrayIndex, _count);

        int IList<TElement>.IndexOf(TElement item) => Array.IndexOf(_elements, item, 0, _count);

        TElement IList<TElement>.this[int index]
        {
            get => index < 0 || index >= _count
                   ? throw new ArgumentOutOfRangeException(nameof(index))
                   : _elements[index];

            set => ThrowModificationNotSupportedException();
        }

        void ICollection<TElement>.Add(TElement item) => ThrowModificationNotSupportedException();
        void ICollection<TElement>.Clear() => ThrowModificationNotSupportedException();
        bool ICollection<TElement>.Remove(TElement item) { ThrowModificationNotSupportedException(); return false; }
        void IList<TElement>.Insert(int index, TElement item) => ThrowModificationNotSupportedException();
        void IList<TElement>.RemoveAt(int index) => ThrowModificationNotSupportedException();

        [DoesNotReturn]
        static void ThrowModificationNotSupportedException() => throw new NotSupportedException("Grouping is immutable.");
    }
}
