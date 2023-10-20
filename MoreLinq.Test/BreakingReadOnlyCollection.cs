#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Leandro F. Vieira. All rights reserved.
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
    using System.Collections.Generic;

    class BreakingReadOnlyCollection<T> : BreakingSequence<T>, IReadOnlyCollection<T>
    {
        readonly IReadOnlyCollection<T> _collection;

        public BreakingReadOnlyCollection(params T[] values) : this((IReadOnlyCollection<T>)values) { }
        public BreakingReadOnlyCollection(IReadOnlyCollection<T> collection) => _collection = collection;
        public int Count => _collection.Count;
    }
}
