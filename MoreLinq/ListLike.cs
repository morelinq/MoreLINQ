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
    /// Represents a union over list types implementing either
    /// <see cref="IList{T}"/> or <see cref="IReadOnlyList{T}"/>, allowing
    /// both to be treated the same.
    /// </summary>

    readonly struct ListLike<T>
    {
        readonly IList<T>? _rw;
        readonly IReadOnlyList<T>? _rx;

        public ListLike(IList<T> list)
        {
            _rw = list ?? throw new ArgumentNullException(nameof(list));
            _rx = null;
        }

        public ListLike(IReadOnlyList<T> list)
        {
            _rw = null;
            _rx = list ?? throw new ArgumentNullException(nameof(list));
        }

        public int Count => _rw?.Count ?? _rx?.Count ?? 0;

        public T this[int index] => _rw is { } rw ? rw[index]
                                  : _rx is { } rx ? rx[index]
                                  : throw new ArgumentOutOfRangeException(nameof(index));
    }

    static class ListLike
    {
        public static ListLike<T> AsListLike<T>(this List<T> list) => new((IList<T>)list);
        public static ListLike<T> AsListLike<T>(this IList<T> list) => new(list);
        public static ListLike<T> AsListLike<T>(this IReadOnlyList<T> list) => new(list);

        public static ListLike<T> ToListLike<T>(this IEnumerable<T> source)
            => source.TryAsListLike() ?? source.ToList().AsListLike();

        public static ListLike<T>? TryAsListLike<T>(this IEnumerable<T> source) =>
            source switch
            {
                null => throw new ArgumentNullException(nameof(source)),
                IList<T> list => list.AsListLike(),
                IReadOnlyList<T> list => list.AsListLike(),
                _ => null
            };
    }
}
