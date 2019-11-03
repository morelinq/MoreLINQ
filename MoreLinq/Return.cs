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

using System.Collections;

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a single-element sequence containing the item provided.
        /// </summary>
        /// <typeparam name="T">The type of the item provided.</typeparam>
        /// <param name="item">The item to wrap in a sequence.</param>
        /// <returns>A sequence containing only the <paramref name="item"/>.</returns>

        public static IEnumerable<T> Return<T>(T item) => new SingleElementList<T>(item);

        internal sealed class SingleElementList<T> : IList<T>, IReadOnlyList<T>
        {
            private readonly T _item;

            public SingleElementList(T item) => _item = item;

            public IEnumerator<T> GetEnumerator() => new SingleElementEnumerator(_item);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public void Add(T item) => throw new NotSupportedException("Single element list is immutable");

            public void Clear() => throw new NotSupportedException("Single element list is immutable");

            public bool Contains(T item) => _item.Equals(item);

            public void CopyTo(T[] array, int arrayIndex) => array[arrayIndex] = _item;

            bool ICollection<T>.Remove(T item) => throw new NotSupportedException("Single element list is immutable");

            int ICollection<T>.Count => 1;

            public bool IsReadOnly => true;

            public int IndexOf(T item) => _item.Equals(item) ? 0 : -1;

            void IList<T>.Insert(int index, T item) => throw new NotSupportedException("Single element list is immutable");

            void IList<T>.RemoveAt(int index) => throw new NotSupportedException("Single element list is immutable");

            public T this[int index]
            {
                get => index == 0 ? _item : throw new ArgumentOutOfRangeException();
                set => throw new NotSupportedException("Single element list is immutable");
            }

            int IReadOnlyCollection<T>.Count => 1;

            internal sealed class SingleElementEnumerator : IEnumerator<T>
            {
                private readonly T _item;

                private int _count = -1;

                public SingleElementEnumerator(T item)
                {
                    _item = item;
                }

                public void Dispose()
                {
                }

                public bool MoveNext()
                {
                    _count++;
                    return _count > 0;
                }

                public void Reset()
                {
                    _count = -1;
                }

                public T Current => _count == 0 ? _item : throw new InvalidOperationException();

                object IEnumerator.Current => Current;
            }
        }
    }
}
