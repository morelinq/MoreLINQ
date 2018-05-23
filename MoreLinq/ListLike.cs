#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Atif Aziz. All rights reserved.
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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an list-like (indexable) data structure.
    /// </summary>

    interface IListLike<out T>
    {
        int Count { get; }
        T this[int index] { get; }
    }

    static class ListLike
    {
        public static IListLike<T> ToListLike<T>(this IEnumerable<T> source)
            => source.TryAsListLike() ?? new List<T>(source.ToList());

        public static IListLike<T> TryAsListLike<T>(this IEnumerable<T> source)
            => source is null ? throw new ArgumentNullException(nameof(source))
             : source is IList<T> list ? new List<T>(list)
             : source is IReadOnlyList<T> readOnlyList ? new ReadOnlyList<T>(readOnlyList)
             : (IListLike<T>) null;

        sealed class List<T> : IListLike<T>
        {
            readonly IList<T> _list;
            public List(IList<T> list) => _list = list ?? throw new ArgumentNullException(nameof(list));
            public int Count => _list.Count;
            public T this[int index] => _list[index];
        }

        sealed class ReadOnlyList<T> : IListLike<T>
        {
            readonly IReadOnlyList<T> _list;
            public ReadOnlyList(IReadOnlyList<T> list) => _list = list ?? throw new ArgumentNullException(nameof(list));
            public int Count => _list.Count;
            public T this[int index] => _list[index];
        }
    }
}
