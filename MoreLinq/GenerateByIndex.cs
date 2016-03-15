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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a sequence of values based on indexes.
        /// </summary>
        /// <remarks>
        /// The sequence is (practically) infinite
        /// - the index ranges from 0 to <c>int.MaxValue</c> inclusive. This function defers
        /// execution and streams the results.
        /// </remarks>
        /// <typeparam name="TResult">Type of result to generate</typeparam>
        /// <param name="generator">Generation function to apply to each index</param>
        /// <returns>A sequence </returns>

        public static IEnumerable<TResult> GenerateByIndex<TResult>(Func<int, TResult> generator)
        {
            // Would just use Enumerable.Range(0, int.MaxValue).Select(generator) but that doesn't
            // include int.MaxValue. Picky, I know...
            if (generator == null) throw new ArgumentNullException("generator");
            return GenerateByIndexImpl(generator);
        }

        private static IEnumerable<TResult> GenerateByIndexImpl<TResult>(Func<int, TResult> generator)
        {
            // Looping over 0...int.MaxValue inclusive is a pain. Simplest is to go exclusive,
            // then go again for int.MaxValue.
            for (var i = 0; i < int.MaxValue; i++)
            {
                yield return generator(i);
            }
            yield return generator(int.MaxValue);
        }
    }
}
