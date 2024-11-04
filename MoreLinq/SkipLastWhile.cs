#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2024 Andy Romero (armorynode). All rights reserved.
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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Removes elements from the end of a sequence as long as a specified condition is true.
        /// </summary>
        /// <typeparam name="T">Type of the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="predicate">The predicate to use to remove items from the tail of the sequence.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> containing the source sequence elements except for the bypassed ones at the end.
        /// </returns>
        /// <exception cref="ArgumentNullException">The source sequence is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">The predicate is <see langword="null"/>.</exception>

        public static IEnumerable<T> SkipLastWhile<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return source.TryAsListLike() switch
            {
                { Count: 0 } => source,
                { } list => IterateList(list, predicate),
                _ => IterateSequence(source, predicate),
            };

            static IEnumerable<T> IterateList(ListLike<T> list, Func<T, bool> predicate)
            {
                var i = list.Count - 1;
                while (i >= 0 && predicate(list[i]))
                {
                    i--;
                }

                for (var j = 0; j <= i; j++)
                {
                    yield return list[j];
                }
            }

            static IEnumerable<T> IterateSequence(IEnumerable<T> source, Func<T, bool> predicate)
            {
                Queue<T>? queue = null;
                foreach (var item in source)
                {
                    if (predicate(item))
                    {
                        queue ??= new Queue<T>();
                        queue.Enqueue(item);
                    }
                    else
                    {
                        while (queue?.Count > 0)
                        {
                            yield return queue.Dequeue();
                        }
                        yield return item;
                    }
                }
            }
        }
    }
}
