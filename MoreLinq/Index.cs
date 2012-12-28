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
    using System.Collections.Generic;
    using System.Linq;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a sequence of <see cref="KeyValuePair{TKey,TValue}"/> 
        /// where the key is the zero-based index of the value in the source 
        /// sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>A sequence of <see cref="KeyValuePair{TKey,TValue}"/>.</returns>
        /// <remarks>This operator uses deferred execution and streams its 
        /// results.</remarks>
        public static IEnumerable<KeyValuePair<int, TSource>> Index<TSource>(this IEnumerable<TSource> source)
        {
            return source.Index(0);
        }

        /// <summary>
        /// Returns a sequence of <see cref="KeyValuePair{TKey,TValue}"/> 
        /// where the key is the index of the value in the source sequence.
        /// An additional parameter specifies the starting index.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="startIndex"></param>
        /// <returns>A sequence of <see cref="KeyValuePair{TKey,TValue}"/>.</returns>
        /// <remarks>This operator uses deferred execution and streams its 
        /// results.</remarks>
        public static IEnumerable<KeyValuePair<int, TSource>> Index<TSource>(this IEnumerable<TSource> source, int startIndex)
        {
            return source.Select((item, index) => new KeyValuePair<int, TSource>(startIndex + index, item));
        }
    }
}
