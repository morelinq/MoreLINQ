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

namespace MoreLinq {

    using System;
    using System.Collections.Generic;

    partial class MoreEnumerable {

        /// <summary>
        /// Iterates the values of the source sequence 
        /// and caches the results for repeated access.
        /// </summary>
        /// <typeparam name="T">Type of sequence.</typeparam>
        /// <param name="sequence">The source sequence.</param>
        /// <returns>Sequence of elements from source sequence.</returns>
        /// <remarks>The first iteration through the result will lazily fetch elements from 
        /// the source collection and cache the results. 
        /// Further iterations will fetch elements from the internal cache.</remarks>
        public static IEnumerable<T> Memoize<T>(this IEnumerable<T> sequence) {
            if (sequence == null) throw new ArgumentNullException("sequence");
            return new MemoizedSequence<T>(sequence);
        }

    }
}
