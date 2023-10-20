#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Avi Levin. All rights reserved.
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

namespace MoreLinq.Test
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// This class implement <see cref="IReadOnlyList{T}"/> but specifically prohibits enumeration using GetEnumerator().
    /// It is provided to assist in testing extension methods that MUST NOT call the GetEnumerator()
    /// method of <see cref="IEnumerable"/> - either because they should be using the indexer or because they are
    /// expected to be lazily evaluated.
    /// </summary>

    sealed class BreakingReadOnlyList<T> : BreakingReadOnlyCollection<T>, IReadOnlyList<T>
    {
        readonly IReadOnlyList<T> _list;

        public BreakingReadOnlyList(params T[] values) : this((IReadOnlyList<T>)values) { }
        public BreakingReadOnlyList(IReadOnlyList<T> list) : base(list)
            => _list = list;

        public T this[int index] => _list[index];
    }
}
