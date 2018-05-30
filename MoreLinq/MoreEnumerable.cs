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

        static int CountUpTo<T>(this IEnumerable<T> source, int max)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (max < 0) throw new ArgumentOutOfRangeException(nameof(max), "The maximum count argument cannot be negative.");

            var count = 0;

            using (var e = source.GetEnumerator())
            {
                while (count < max && e.MoveNext())
                {
                    count++;
                }
            }

            return count;
        }
    }
}
