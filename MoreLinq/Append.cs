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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a sequence consisting of the head elements and the given tail element.
        /// </summary>
        /// <typeparam name="T">Type of sequence</typeparam>
        /// <param name="head">All elements of the head. Must not be null.</param>
        /// <param name="tail">Tail element of the new sequence.</param>
        /// <returns>A sequence consisting of the head elements and the given tail element.</returns>
        /// <remarks>This operator uses deferred execution and streams its results.</remarks>

#if NET471_OR_GREATER || NETSTANDARD1_6_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        public static IEnumerable<T> Append<T>(IEnumerable<T> head, T tail)
#else
        public static IEnumerable<T> Append<T>(this IEnumerable<T> head, T tail)
#endif
        {
            if (head == null) throw new ArgumentNullException(nameof(head));
            return head is PendNode<T> node
                ? node.Concat(tail)
                : PendNode<T>.WithSource(head).Concat(tail);
        }
    }
}
