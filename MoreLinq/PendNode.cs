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

    /// <summary>
    /// Prepend-Append node is a single linked-list of the discriminated union
    /// of an item to prepend, an item to append and the source.
    /// </summary>

    abstract class PendNode<T> : IEnumerable<T>
    {
        public static PendNode<T> WithSource(IEnumerable<T> source) => new Source(source);

        public PendNode<T> Prepend(T item) => new Item(item, isPrepend: true , next: this);
        public PendNode<T> Concat(T item)  => new Item(item, isPrepend: false, next: this);

        sealed class Item : PendNode<T>
        {
            public T Value { get; }
            public bool IsPrepend { get; }
            public int ConcatCount { get; }
            public PendNode<T> Next { get; }

            public Item(T item, bool isPrepend, PendNode<T> next)
            {
                if (next == null) throw new ArgumentNullException(nameof(next));

                Value       = item;
                IsPrepend   = isPrepend;
                ConcatCount = next is Item nextItem
                            ? nextItem.ConcatCount + (isPrepend ? 0 : 1)
                            : 1;
                Next        = next;
            }
        }

        sealed class Source : PendNode<T>
        {
            public IEnumerable<T> Value { get; }
            public Source(IEnumerable<T> source) => Value = source;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var i = 0;
            T[]? concats = null;      // Array for > 4 concatenations
            var concat1 = default(T); // Slots for up to 4 concatenations
            var concat2 = default(T);
            var concat3 = default(T);
            var concat4 = default(T);

            var current = this;
            for (; current is Item item; current = item.Next)
            {
                if (item.IsPrepend)
                {
                    yield return item.Value;
                }
                else
                {
                    if (concats == null)
                    {
                        if (i == 0 && item.ConcatCount > 4)
                        {
                            concats = new T[item.ConcatCount];
                        }
                        else
                        {
                            switch (i++)
                            {
                                case 0: concat1 = item.Value; break;
                                case 1: concat2 = item.Value; break;
                                case 2: concat3 = item.Value; break;
                                case 3: concat4 = item.Value; break;
                                default: throw new UnreachableException();
                            }
                            continue;
                        }
                    }

                    concats[i++] = item.Value;
                }
            }

            var source = (Source)current;

            foreach (var item in source.Value)
                yield return item;

            if (concats == null)
            {
                if (i == 4) { yield return concat4!; i--; }
                if (i == 3) { yield return concat3!; i--; }
                if (i == 2) { yield return concat2!; i--; }
                if (i == 1) { yield return concat1!; }
                yield break;
            }

            for (i--; i >= 0; i--)
                yield return concats[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
