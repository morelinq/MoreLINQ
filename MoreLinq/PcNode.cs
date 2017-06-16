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

    static class PcNode
    {
        public static PcNode<T>.Source Source<T>(IEnumerable<T> source) =>
            new PcNode<T>.Source(source);

        public static PcNode<T> Prepend<T>(this PcNode<T> node, T item) =>
            new PcNode<T>.Item(node, item, isPrepend: true);

        public static PcNode<T> Concat<T>(this PcNode<T> node, T item) =>
            new PcNode<T>.Item(node, item, isPrepend: false);
    }

    /// <summary>
    /// Prepend-Concat node is a single linked-list of the discriminated union
    /// of a prepend item, a concat item and the source.
    /// </summary>

    abstract class PcNode<T> : IEnumerable<T>
    {
        public PcNode<T> Next { get; }

        protected PcNode(PcNode<T> next) => Next = next;

        public IEnumerator<T> GetEnumerator()
        {
            var concats = new List();
            for (var current = this; current != null; current = current.Next)
            {
                switch (current)
                {
                    case Item item:
                    {
                        if (item.IsPrepend)
                            yield return item.Value;
                        else
                            concats = concats.Add(item.Value);
                        break;
                    }
                    case Source source:
                    {
                        foreach (var item in source.Value)
                            yield return item;
                        for (var i = 0; i < concats.Count; i++)
                            yield return concats[i];
                        break;
                    }
                    default:
                        throw new NotSupportedException("Unsupported node: " + current);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public sealed class Item : PcNode<T>
        {
            public T Value { get; }
            public bool IsPrepend { get; }

            public Item(PcNode<T> next, T item, bool isPrepend) :
                base(next)
            {
                Value = item;
                IsPrepend = isPrepend;
            }
        }

        public class Source : PcNode<T>
        {
            public IEnumerable<T> Value { get; }

            public Source(IEnumerable<T> source) :
                base(null) => Value = source;
        }

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