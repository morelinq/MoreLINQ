#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Atif Aziz. All rights reserved.
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
        /// Enables a sequence's elements to be deconstructed into matching
        /// number of variables.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <typeparam name="T">The type of the source elements.</typeparam>
        /// <returns>
        /// The same sequence that can be deconstructed.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution semantics and streams its
        /// results unless deconstructed.
        /// </remarks>

        public static DeconstructibleEnumerable<T> AsDeconstructible<T>(this IEnumerable<T> source)
            => source == null
             ? throw new ArgumentNullException(nameof(source))
             : new DeconstructibleEnumerable<T>(source);
    }
}