﻿#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Leandro F. Vieira (leandromoh). All rights reserved.
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
        /// Generates a sequence of integral numbers within the specified range.
        /// </summary>
        /// <param name="start">The value of the first integer in the sequence.</param>
        /// <param name="stop">The value of the last integer in the sequence.</param>
        /// <returns>An <see cref="IEnumerable{Int32}"/> that contains a range of sequential integral numbers.</returns>
        /// <example>
        /// <code>
        /// string result = Sequence(6, 0);
        /// </code>
        /// The <c>result</c> variable will contain <c>{ 6, 5, 4, 3, 2, 1, 0 }</c>.
        /// </example>
        public static IEnumerable<int> Sequence(int start, int stop)
        {
            return Sequence(start, stop, start < stop ? 1 : -1);
        }

        /// <summary>
        /// Generates a sequence of integral numbers within the specified range.
        /// An additional argument specifies the step between the numbers.
        /// </summary>
        /// <param name="start">The value of the first integer in the sequence.</param>
        /// <param name="stop">The value of the last integer in the sequence.</param>
        /// <param name="step">The step to define the next number.</param>
        /// <returns>An <see cref="IEnumerable{Int32}"/> that contains a range of sequential integral numbers.</returns>
        /// <example>
        /// <code>
        /// string result = Sequence(6, 0, -2);
        /// </code>
        /// The <c>result</c> variable will contain <c>{ 6, 4, 2, 0 }</c>.
        /// </example>
        public static IEnumerable<int> Sequence(int start, int stop, int step)
        {
            var func = step >= 0 ? () => stop >= start : new Func<bool>(() => stop <= start);

            while (func())
            {
                yield return start;
                start = checked(start + step);
            }
        }
    }
}
