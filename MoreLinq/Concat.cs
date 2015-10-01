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
    using LinqEnumerable = System.Linq.Enumerable;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a sequence consisting of the head element and the given tail elements.
        /// </summary>
        /// <typeparam name="T">Type of sequence</typeparam>
        /// <param name="head">Head element of the new sequence.</param>
        /// <param name="tail">All elements of the tail. Must not be null.</param>
        /// <returns>A sequence consisting of the head elements and the given tail elements.</returns>
        /// <remarks>This operator uses deferred execution and streams its results.</remarks>
        
        public static IEnumerable<T> Concat<T>(this T head, IEnumerable<T> tail)
        {
            if (tail == null) throw new ArgumentNullException("tail");
            return tail.Prepend(head);
        }

        /// <summary>
        /// Returns a sequence consisting of the head elements and the given tail element.
        /// </summary>
        /// <typeparam name="T">Type of sequence</typeparam>
        /// <param name="head">All elements of the head. Must not be null.</param>
        /// <param name="tail">Tail element of the new sequence.</param>
        /// <returns>A sequence consisting of the head elements and the given tail element.</returns>
        /// <remarks>This operator uses deferred execution and streams its results.</remarks>
        
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> head, T tail)
        {
            if (head == null) throw new ArgumentNullException("head");
            return LinqEnumerable.Concat(head, LinqEnumerable.Repeat(tail, 1));
        }
    }
}