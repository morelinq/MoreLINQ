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

    sealed class BreakingCollection<T> : BreakingSequence<T>, ICollection<T>
    {
        public BreakingCollection(int count) => Count = count;

        public int Count { get; }

        public void Add(T item)      => throw new NotImplementedException();
        public void Clear()          => throw new NotImplementedException();
        public bool Contains(T item) => throw new NotImplementedException();
        public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();
        public bool Remove(T item)   => throw new NotImplementedException();
        public bool IsReadOnly       => throw new NotImplementedException();
    }
}
