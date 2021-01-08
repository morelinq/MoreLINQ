#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2021 Atif Aziz. All rights reserved.
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

    partial class MoreEnumerable
    {
        /// <summary>
        /// Iterates through a sequence based on another sequence of Boolean
        /// values indicating when to step to the next element.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="steps">Sequence of Boolean values.</param>
        /// <returns>
        /// A sequence of elements from <paramref name="source"/> where
        /// each element is duplicated while the Boolean from
        /// <paramref name="steps"/> is <c>false</c>.
        /// </returns>
        /// <remarks>This operator uses deferred execution and streams its results.</remarks>

        public static IEnumerable<(bool Moved, T Item)>
            Step<T>(this IEnumerable<T> source, IEnumerable<bool> steps)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (steps is null) throw new ArgumentNullException(nameof(steps));

            return _(); IEnumerable<(bool, T)> _()
            {
                using var item = source.GetEnumerator();

                if (!item.MoveNext())
                    yield break;
                yield return (true, item.Current);

                foreach (var step in steps)
                {
                    if (step)
                    {
                        if (!item.MoveNext())
                            break;
                        yield return (true, item.Current);
                    }
                    else
                    {
                        yield return (false, item.Current);
                    }
                }
            }
        }
    }
}
