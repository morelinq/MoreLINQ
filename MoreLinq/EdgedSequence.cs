#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All rights reserved.
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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Sequence where elements may be prepended or appended to another
        /// source of elements with which it is initialized. It is primarily
        /// designed for flattened iteration of consecutive compositions of
        /// <see cref="MoreEnumerable.Prepend{TSource}"/> and/or
        /// <see cref="MoreEnumerable.Concat{T}(IEnumerable{T},T)"/>.
        /// </summary>

        sealed class EdgedSequence<T> : IEnumerable<T>
        {
            readonly IEnumerable<T> _source;
            List _heads;
            List _tails;

            public EdgedSequence(IEnumerable<T> source) => _source = source;

            public EdgedSequence<T> Prepending(T item)
            {
                _heads = _heads.Add(item);
                return this;
            }

            public EdgedSequence<T> Appending(T item)
            {
                _tails = _tails.Add(item);
                return this;
            }

            public IEnumerator<T> GetEnumerator()
            {
                // Equivalent to:
                //     foreach (var item in _heads)
                //         yield return item;
                // but avoids allocation of an enumerator

                for (var i = 0; i < _heads.Count; i++)
                    yield return _heads[i];

                foreach (var item in _source)
                    yield return item;

                // Equivalent to:
                //     foreach (var item in _tails)
                //         yield return item;
                // but avoids allocation of an enumerator

                for (var i = 0; i < _tails.Count; i++)
                    yield return _tails[i];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            /// <summary>
            /// A list that encapsulates one or more values.
            /// </summary>
            /// <remarks>
            /// While each modification of the list returns a new list, the
            /// underlying buffer is shared. It is therefore not appropriate
            /// or designed to be used as an immutable list. The immutability
            /// of the structure is purely a tactical implementation detail.
            /// </remarks>

            struct List : IEnumerable<T>
            {
                readonly T _item;
                readonly T[] _items;

                List(T singleton) : this(1, singleton, null) { }
                List(int count, T[] items) : this(count, default(T), items) { }
                List(int count, T item, T[] items)
                {
                    Count = count;
                    _item = item;
                    _items = items;
                }

                public int Count { get; }

                public T this[int index]
                    => index < 0 || index >= Count ? throw new IndexOutOfRangeException(nameof(index))
                     : Count == 1 ? _item : _items[index];

                public List Add(T item)
                {
                    switch (Count)
                    {
                        case 0: return new List(item);
                        case 1: return new List(2, new[] { _item, item, default(T), default(T) });
                        default:
                            var items = _items;
                            var capacity = items.Length;
                            var count = Count + 1;
                            if (count > capacity)
                                capacity *= 2;
                            Array.Resize(ref items, capacity);
                            items[count - 1] = item;
                            return new List(count, items);
                    }
                }

                public IEnumerator<T> GetEnumerator()
                {
                    for (var i = 0; i < Count; i++)
                        yield return this[i];
                }

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }
        }
    }
}