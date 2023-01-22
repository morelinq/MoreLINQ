#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2010 Leopold Bushkin. All rights reserved.
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
    using Experimental;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Repeats the sequence the specified number of times.
        /// </summary>
        /// <typeparam name="T">Type of elements in sequence</typeparam>
        /// <param name="sequence">The sequence to repeat</param>
        /// <param name="count">Number of times to repeat the sequence</param>
        /// <returns>A sequence produced from the repetition of the original source sequence</returns>

        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> sequence, int count)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Repeat count must be greater than or equal to zero.");
            return RepeatImpl(sequence, count);
        }

        /// <summary>
        /// Repeats the sequence forever.
        /// </summary>
        /// <typeparam name="T">Type of elements in sequence</typeparam>
        /// <param name="sequence">The sequence to repeat</param>
        /// <returns>A sequence produced from the infinite repetition of the original source sequence</returns>

        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            return RepeatImpl(sequence, null);
        }


        static IEnumerable<T> RepeatImpl<T>(IEnumerable<T> sequence, int? count)
        {
            var memo = sequence.Memoize();
            using (memo as IDisposable)
            {
                while (count == null || count-- > 0)
                {
#pragma warning disable CA1851 // Possible multiple enumerations of 'IEnumerable' collection
                    foreach (var item in memo)
#pragma warning restore CA1851 // Possible multiple enumerations of 'IEnumerable' collection
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}
