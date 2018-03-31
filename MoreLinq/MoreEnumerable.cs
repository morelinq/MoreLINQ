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

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of static methods for querying objects that
    /// implement <see cref="IEnumerable{T}" />.
    /// </summary>

    public static partial class MoreEnumerable
    {
        static int? TryGetCollectionCount<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source is ICollection<T> collection ? collection.Count
                 : source is IReadOnlyCollection<T> readOnlyCollection ? readOnlyCollection.Count
                 : (int?)null;
        }

        /// <summary>
        /// Returns a <see cref="IEnumerable{T}"/> that lazily creates an in-memory
        /// cache of the enumeration on first iteration.
        /// </summary>
        /// <remarks>
        /// This operator is not thread-safe, since it is for internal use only.
        /// </remarks>
        static IEnumerable<T> Memoize<T>(IEnumerator<T> e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var disposed = false;
            var cache = new List<T>();

            return _(); IEnumerable<T> _()
            {
                var index = 0;
                var hasValue = false;

                while (true)
                {
                    if (index < cache.Count)
                    {
                        hasValue = true;
                    }

                    else if ((hasValue = !disposed && e.MoveNext()))
                    {
                        cache.Add(e.Current);
                    }

                    else if (!disposed)
                    {
                        disposed = true;
                        e.Dispose();
                    }

                    if (hasValue)
                        yield return cache[index];
                    else
                        break;

                    index++;
                }
            }
        }
    }
}
