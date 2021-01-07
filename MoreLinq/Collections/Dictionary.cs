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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// A minimal <see cref="System.Collections.Generic.Dictionary{TKey,TValue}"/> wrapper that
    /// allows null keys when <typeparamref name="TKey"/> is a
    /// reference type.
    /// </summary>

    // Add members if and when needed to keep coverage.

    sealed class Dictionary<TKey, TValue>
    {
        readonly System.Collections.Generic.Dictionary<TKey, TValue> _dict;
        (bool, TValue) _null;

        public Dictionary(IEqualityComparer<TKey> comparer)
        {
            _dict = new System.Collections.Generic.Dictionary<TKey, TValue>(comparer);
            _null = default;
        }

        public TValue this[TKey key]
        {
            set
            {
                if (key is null)
                    _null = (true, value);
                else
                    _dict[key] = value;
            }
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (key is null)
            {
                switch (_null)
                {
                    case (true, {} v):
                        value = v;
                        return true;
                    case (false, _):
                        value = default!;
                        return false;
                }
            }

            return _dict.TryGetValue(key, out value);
        }
    }
}
