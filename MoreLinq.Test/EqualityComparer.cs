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

namespace MoreLinq.Test
{
    using System;
    using System.Collections.Generic;

    static class EqualityComparer
    {
        /// <summary>
        /// Creates an <see cref="IEqualityComparer{T}"/> given a
        /// <see cref="Func{T,T,Boolean}"/>.
        /// </summary>

        public static IEqualityComparer<T> Create<T>(Func<T?, T?, bool> comparer) =>
            new DelegatingComparer<T>(comparer);

        sealed class DelegatingComparer<T> : IEqualityComparer<T>
        {
            readonly Func<T?, T?, bool> _comparer;
            readonly Func<T, int> _hasher;

            public DelegatingComparer(Func<T?, T?, bool> comparer)
                : this(comparer, x => x == null ? 0 : x.GetHashCode()) { }

            DelegatingComparer(Func<T?, T?, bool> comparer, Func<T, int> hasher)
            {
                _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
                _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            }

            public bool Equals(T? x, T? y) => _comparer(x, y);
            public int GetHashCode(T obj) => _hasher(obj);
        }
    }
}
