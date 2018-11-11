#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2009 Jonathan Skeet. All rights reserved.
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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a sequence of values based on indexes.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the value returned by <paramref name="generator"/>
        /// and therefore the elements of the generated sequence.</typeparam>
        /// <param name="generator">
        /// Generation function to apply to each index.</param>
        /// <returns>A sequence of generated results.</returns>
        /// <remarks>
        /// <para>
        /// The sequence is (practically) infinite where the index ranges from
        /// zero to <see cref="int.MaxValue"/> inclusive.</para>
        /// <para>
        /// This function defers execution and streams the results.</para>
        /// </remarks>

        public static IEnumerable<TResult> GenerateByIndex<TResult>(Func<int, TResult> generator)
        {
            if (generator == null) throw new ArgumentNullException(nameof(generator));

            return MoreEnumerable.Sequence(0, int.MaxValue)
                                 .Select(generator);
        }
    }
}
