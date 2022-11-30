#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2020 Atif Aziz, Leandro F. Vieira (leandromoh). All rights reserved.
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

namespace MoreLinq.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// A minimal <see cref="System.Collections.Generic.Dictionary{TKey,TValue}"/> wrapper that
    /// allows a null key.
    /// </summary>

    // Add members if and when needed to keep coverage.

    sealed class Dictionary<TKey, TValue>
    {
        readonly System.Collections.Generic.Dictionary<ValueTuple<TKey>, TValue> _dict;

        public Dictionary(IEqualityComparer<TKey> comparer)
        {
            var keyComparer = ReferenceEquals(comparer, EqualityComparer<TKey>.Default)
                            ? null
                            : new ValueTupleItemComparer<TKey>(comparer);
            _dict = new System.Collections.Generic.Dictionary<ValueTuple<TKey>, TValue>(keyComparer);
        }

        public TValue this[TKey key]
        {
            get => _dict[ValueTuple.Create(key)];
            set => _dict[ValueTuple.Create(key)] = value;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) =>
            _dict.TryGetValue(ValueTuple.Create(key), out value);

        sealed class ValueTupleItemComparer<T> : IEqualityComparer<ValueTuple<T>>
        {
            readonly IEqualityComparer<T> _comparer;

            public ValueTupleItemComparer(IEqualityComparer<T> comparer) => _comparer = comparer;
            public bool Equals(ValueTuple<T> x, ValueTuple<T> y) => _comparer.Equals(x.Item1, y.Item1);
            public int GetHashCode(ValueTuple<T> obj) => obj.Item1 is { } some ? _comparer.GetHashCode(some) : 0;
        }
    }
}
