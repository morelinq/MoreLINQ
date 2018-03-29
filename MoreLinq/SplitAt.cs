#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2009 Atif Aziz. All rights reserved.
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
    using System.Diagnostics;
    using System.Linq;

    static partial class MoreEnumerable
    {
        #if !NO_VALUE_TUPLES

        /// <summary>
        /// Splits the sequence in two at the given index.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="index">
        /// The zero-based index at which to split the source sequence.</param>
        /// <typeparam name="T">
        /// The type of the element in the source sequence.</typeparam>
        /// <returns>
        /// A tuple containing two sequences resulting from the split. The
        /// first sequence contains elements up to but not including the
        /// element at the index specified by <paramref name="index"/>. The
        /// second sequence contains elements from the index specified by
        /// <paramref name="index"/> and onwards.
        /// </returns>
        /// <remarks>
        /// This method uses deferred execution semantics but iterates the
        /// <paramref name="source"/> entirely when either of the sequences
        /// resulting from split are iterated.
        /// </remarks>

        public static (IEnumerable<T> First, IEnumerable<T> Second) SplitAt<T>(
            this IEnumerable<T> source, int index) =>
            source.SplitAt(index, ValueTuple.Create);

        #endif

        /// <summary>
        /// Splits the sequence in two at the given index. An additional
        /// parameter specifies a function that projects the result given the
        /// two sequences resulting from the split.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="index">
        /// The zero-based index at which to split the source sequence.</param>
        /// <param name="resultSelector">
        /// The function that projects the result given the two sequences
        /// resulting from the split. The first sequence will contain elements
        /// up to but not including the element at the index specified by
        /// <paramref name="index"/>. The second sequence will contain
        /// elements from the index specified by <paramref name="index"/>
        /// and onwards.</param>
        /// <typeparam name="T">
        /// The type of the element in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <returns>
        /// The result returned by <paramref name="resultSelector"/>.
        /// </returns>
        /// <remarks>
        /// This method uses deferred execution semantics but iterates the
        /// <paramref name="source"/> entirely when either of the sequences
        /// resulting from split are iterated.
        /// </remarks>

        public static TResult SplitAt<T, TResult>(
            this IEnumerable<T> source, int index,
            Func<IEnumerable<T>, IEnumerable<T>, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            if (index <= 0)
                return resultSelector(Enumerable.Empty<T>(), source);

            // TODO Thread-safety?

            if (source is ICollection<T> collection)
            {
                if (index >= collection.Count)
                    return resultSelector(source, Enumerable.Empty<T>());

                T[] part1 = null; T[] part2 = null;

                return resultSelector(new LazyList<int, T[], T>(0, SplitCollection),
                                      new LazyList<int, T[], T>(1, SplitCollection));

                T[] SplitCollection(int id)
                {
                    if (source != null)
                    {
                        var i  = 0;
                        var ai = 0; var a = new T[index];
                        var bi = 0; var b = new T[collection.Count - index];

                        foreach (var item in source)
                            if (i++ < index)
                                a[ai++] = item;
                            else
                                b[bi++] = item;

                        part1 = a; part2 = b;
                        source = null;
                    }

                    Debug.Assert(id == 0 || id == 1);
                    return id == 0 ? part1 : part2;
                }
            }
            else
            {
                List<T> part1 = null; List<T> part2 = null;

                return resultSelector(new LazyList<int, List<T>, T>(0, SplitSequence),
                                      new LazyList<int, List<T>, T>(1, SplitSequence));

                List<T> SplitSequence(int id)
                {
                    if (source != null)
                    {
                        var i = 0;
                        var a = new List<T>();
                        var b = new List<T>();

                        foreach (var item in source)
                            (i++ < index ? a : b).Add(item);

                        part1 = a; part2 = b;
                        source = null;
                    }

                    Debug.Assert(id == 0 || id == 1);
                    return id == 0 ? part1 : part2;
                }
            }
        }

        sealed class LazyList<TArg, TList, TItem> : IList<TItem>
            where TList : class, IList<TItem>
        {
            readonly TArg _arg;
            Func<TArg, TList> _factory;
            TList _list;

            public LazyList(TArg arg, Func<TArg, TList> factory)
            {
                _arg = arg;
                _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            }

            TList List
            {
                get // Not tread-safe!
                {
                    return _list ?? (_list = CreateList());
                    TList CreateList()
                    {
                        var list = (_factory ?? throw new InvalidOperationException())(_arg);
                        _factory = null;
                        return list;
                    }
                }
            }

            public int Count => List.Count;
            public TItem this[int index] { get => List[index]; set => throw UnsupportedError(); }

            public IEnumerator<TItem> GetEnumerator() => List.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public int IndexOf(TItem item) => List.IndexOf(item);
            public bool Contains(TItem item) => List.Contains(item);
            public void CopyTo(TItem[] array, int arrayIndex) => List.CopyTo(array, arrayIndex);

            // read-only ...

            public bool IsReadOnly => true;

            static Exception UnsupportedError() =>
                new NotSupportedException("Collection is read-only.");

            public void Add(TItem item)               => throw UnsupportedError();
            public void Clear()                       => throw UnsupportedError();
            public bool Remove(TItem item)            => throw UnsupportedError();
            public void Insert(int index, TItem item) => throw UnsupportedError();
            public void RemoveAt(int index)           => throw UnsupportedError();
        }
    }
}
