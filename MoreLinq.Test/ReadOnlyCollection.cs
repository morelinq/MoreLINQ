#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Atif Aziz. All rights reserved.
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

    static class ReadOnlyCollection
    {
        public static IReadOnlyCollection<T> From<T>(params T[] items) =>
            new ListCollection<T[], T>(items);

        sealed class ListCollection<TList, T>(TList list) :
            IReadOnlyCollection<T>
            where TList : IList<T>
        {
            readonly TList list = list;

            public IEnumerator<T> GetEnumerator() => this.list.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public int Count => this.list.Count;
        }
    }
}
