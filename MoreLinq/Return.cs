#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Mitch Bodmer. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace MoreLinq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a single-element sequence containing the item provided.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="item">The item to return in a sequence.</param>
        /// <returns>A sequence containing only <paramref name="item"/>.</returns>

        public static IEnumerable<T> Return<T>(T item) => new SingleElementList<T>(item);

        sealed class SingleElementList<T> : IList<T>, IReadOnlyList<T>
        {
            readonly T _item;

            public SingleElementList(T item) => _item = item;

            public int Count       => 1;
            public bool IsReadOnly => true;

            public T this[int index]
            {
                get => index == 0 ? _item : throw new ArgumentOutOfRangeException(nameof(index));
                set => throw ReadOnlyException();
            }

            public IEnumerator<T> GetEnumerator() { yield return _item; }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public int IndexOf(T item) => Contains(item) ? 0 : -1;
            public bool Contains(T item) => EqualityComparer<T>.Default.Equals(_item, item);

            public void CopyTo(T[] array, int arrayIndex) => array[arrayIndex] = _item;

            // Following methods are unsupported as this is a read-only list.

            public void Add(T item)               => throw ReadOnlyException();
            public void Clear()                   => throw ReadOnlyException();
            public bool Remove(T item)            => throw ReadOnlyException();
            public void Insert(int index, T item) => throw ReadOnlyException();
            public void RemoveAt(int index)       => throw ReadOnlyException();

            static NotSupportedException ReadOnlyException() => new("Single element list is immutable.");
        }
    }
}
