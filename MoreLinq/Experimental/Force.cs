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

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    static partial class ExperimentalEnumerable
    {
        /// <summary>
		/// Forces the specified enumeration to be evaluated, if it is not yet.
        /// If collection is well known and already forced, than does nothing.
		/// </summary>
        /// <typeparam name="T">
        /// Type of elements in <paramref name="source"/>.
        /// </typeparam>
        /// <param name="source">The source sequence.</param>
		/// <returns>Same enumeration as countable if countable already or forces <see cref="Enumerable.ToArray"/> else.</returns>
        /// <remarks>Eager version of <seealso href="Memoize"/> and no returning version of <seealso href="Consume"/>.</remarks>
        public static IReadOnlyCollection<T>? Force<T>(this IEnumerable<T>? source)
        {
            switch (source)
            {
                case T[] _:
                case List<T> _:
                case HashSet<T> _:
                case Stack<T> _:
                case Queue<T> _:
                case SortedSet<T> _:
                case LinkedList<T> _:
                case null:
                    return (IReadOnlyCollection<T>?)source;
                default:
                    return source.ToArray();
            }
        }
    }


}
