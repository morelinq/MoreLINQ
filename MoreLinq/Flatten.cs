#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Leandro F. Vieira (leandromoh). All rights reserved.
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
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Flattens a sequence containing arbitrarily-nested sequences.
        /// </summary>
        /// <param name="source">The sequence that will be flattened.</param>
        /// <returns>
        /// A sequence that contains the elements of <paramref name="source"/>
        /// and from all its inner sequences. 
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IEnumerable<object> Flatten(this IEnumerable source)
        {
            return Flatten(source, obj => !(obj is string));
        }

        /// <summary>
        /// Flattens a sequence containing arbitrarily-nested sequences.
        /// </summary>
        /// <param name="source">The sequence that will be flattened.</param>
        /// <param name="predicate">
        /// A function that receives the elements that implements <see cref="IEnumerable"/>
        /// and returns <c>true</c> case the element must be treat as an inner sequence
        /// or <c>false</c> case the element must be yielded in the resulting sequence.
        /// </param>
        /// <returns>
        /// A sequence that contains the elements of <paramref name="source"/>
        /// and from all its inner sequences. 
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null.</exception>
        public static IEnumerable<object> Flatten(this IEnumerable source, Func<IEnumerable, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return _(); IEnumerable<object> _()
            {
                var e = source.GetEnumerator();
                var stack = new Stack<IEnumerator>();

                stack.Push(e);

                try
                {
                    while (stack.Any())
                    {
                        e = stack.Pop();

                        bool next;
                        while (next = e.MoveNext())
                        {
                            if (e.Current is IEnumerable inner && predicate(inner))
                            {
                                stack.Push(e);
                                stack.Push(inner.GetEnumerator());
                                break;
                            }
                            else
                            {
                                yield return e.Current;
                            }
                        }

                        if (!next)
                        {
                            (e as IDisposable)?.Dispose();
                        }
                    }
                }
                finally
                {
                    stack.Prepend(e)
                         .OfType<IDisposable>()
                         .ForEach(x => x.Dispose());
                }
            };
        }
    }
}
