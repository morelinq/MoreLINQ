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

    sealed class Comparer
    {
        /// <summary>
        /// Creates an <see cref="IComparer{T}"/> given a
        /// <see cref="Func{T,T,Int32}"/>.
        /// </summary>

        public static IComparer<T> Create<T>(Func<T, T, int> compare) =>
            new DelegatingComparer<T>(compare);

        sealed class DelegatingComparer<T> : IComparer<T>
        {
            readonly Func<T, T, int> _comparer;

            public DelegatingComparer(Func<T, T, int> comparer)
            {
                _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            }

            public int Compare(T x, T y) => _comparer(x, y);
        }
    }
}
