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

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Repeats the specific sequences <paramref name="count"/> times.
        /// </summary>
        /// <param name="sequence">The sequence to repeat</param>
        /// <param name="count">Number of times to repeat the sequence</param>
        /// <returns>A sequence produced from the repetition of the original source sequence</returns>
        
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> sequence, int count)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (count < 0) throw new ArgumentOutOfRangeException("count", "Repeat count must be greater than or equal to zero.");
            return RepeatImpl(sequence, count);
        }

        private static IEnumerable<T> RepeatImpl<T>(this IEnumerable<T> sequence, int count)
        {
            while (count-- > 0)
            {
                // TODO buffer to avoid multiple enumerations
                foreach (var item in sequence)
                    yield return item;
            }
        }
    }
}