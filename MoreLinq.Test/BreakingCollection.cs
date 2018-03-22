#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
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
    using System;
    using System.Collections.Generic;

    class BreakingCollection<T> : BreakingSequence<T>, ICollection<T>
    {
        protected readonly List<T> _list;

        public BreakingCollection() : this(new List<T>()) { }
        public BreakingCollection(List<T> list) => _list = list;
        public BreakingCollection(int count)
            : this(Enumerable.Repeat(default(T), count).ToList()){ }

        public int Count => _list.Count;

        public void Add(T item) => _list.Add(item);
        public void Clear() => _list.Clear();
        public bool Contains(T item) => _list.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
        public bool Remove(T item) => _list.Remove(item);
        public bool IsReadOnly => ((ICollection<T>)_list).IsReadOnly;
    }
}
