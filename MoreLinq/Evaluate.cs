#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Felipe Sateler. All rights reserved.
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

    partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a sequence containing the values resulting from invoking (in order) each function in the source sequence of functions.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results.
        /// If the resulting sequence is enumerated multiple times, the functions will be
        /// evaluated multiple times too.
        /// </remarks>
        /// <typeparam name="T">The type of the object returned by the functions.</typeparam>
        /// <param name="functions">The functions to evaluate.</param>
        /// <returns>A sequence with results from invoking <paramref name="functions"/>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="functions"/> is <c>null</c>.</exception>

        public static IEnumerable<T> Evaluate<T>(this IEnumerable<Func<T>> functions) =>
            from f in functions ?? throw new ArgumentNullException(nameof(functions))
            select f();
    }
}
