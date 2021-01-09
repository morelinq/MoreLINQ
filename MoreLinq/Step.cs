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
        /// Steps through a sequence based on another sequence of zero or
        /// positive steps to take between elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="steps">
        /// Sequence of zero or positive steps to take where a negative step is
        /// treated the same as zero.</param>
        /// <returns>
        /// A sequence of items from <paramref name="source"/> paired with
        /// steps from <paramref name="steps"/>.
        /// </returns>
        /// <remarks>This operator uses deferred execution and streams its results.</remarks>

        public static IEnumerable<(T Item, int Step)>
            Step<T>(this IEnumerable<T> source, IEnumerable<int> steps)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (steps is null) throw new ArgumentNullException(nameof(steps));

            return _(); IEnumerable<(T, int)> _()
            {
                using var item = source.GetEnumerator();

                if (!item.MoveNext())
                    yield break;
                yield return (item.Current, 1);

                foreach (var step in steps)
                {
                    if (step > 0)
                    {
                        for (var i = 0; i < step; i++)
                        {
                            if (!item.MoveNext())
                                yield break;
                        }

                        yield return (item.Current, step);
                    }
                    else
                    {
                        yield return (item.Current, 0);
                    }
                }
            }
        }
    }
}
