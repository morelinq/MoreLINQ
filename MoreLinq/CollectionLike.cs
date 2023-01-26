#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2023 Atif Aziz. All rights reserved.
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
    /// Represents a union over list types implementing either <see
    /// cref="ICollection{T}"/> or <see cref="IReadOnlyCollection{T}"/>,
    /// allowing both to be treated the same.
    /// </summary>

    readonly struct CollectionLike<T>
    {
        readonly ICollection<T>? _rw;
        readonly IReadOnlyCollection<T>? _rx;

        public CollectionLike(ICollection<T> collection)
        {
            _rw = collection ?? throw new ArgumentNullException(nameof(collection));
            _rx = null;
        }

        public CollectionLike(IReadOnlyCollection<T> collection)
        {
            _rw = null;
            _rx = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        public int Count => _rw?.Count ?? _rx?.Count ?? 0;

        public IEnumerator<T> GetEnumerator() =>
            _rw?.GetEnumerator() ?? _rx?.GetEnumerator() ?? Enumerable.Empty<T>().GetEnumerator();
    }
}
