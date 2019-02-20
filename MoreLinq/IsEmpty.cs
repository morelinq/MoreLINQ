#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 RyotaMurohoshi. All rights reserved.
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
    using System.Linq;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Determines whether source is empty or not.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the source sequences</typeparam>
        /// <param name="source">The sequence to check whether is empty or not</param>
        /// <returns><c>true</c> if the sequence is empty or <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return !source.Any ();
        }
    }
}
