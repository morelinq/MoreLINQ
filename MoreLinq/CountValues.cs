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

    partial class MoreEnumerable{

        /// <summary>
        /// Returns a sequence of key/value pairs, 
        /// where each key is a distinct value from the source sequence,
        /// and each value is the number of times that value occurred in the source sequence.
        /// </summary>
        /// <typeparam name="T">Type of sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <returns>Sequence of key/value pairs, 
        /// where each key is a distinct value from the source sequence,
        /// and each value is the number of times that value occurred in the source sequence.</returns>
        /// <remarks>
        /// This operator immediately counts all source values, then streams the results.
        /// </remarks>
        public static IEnumerable<KeyValuePair<T, int>> CountValues<T>(this IEnumerable<T> sequence) {
            if (sequence == null) throw new ArgumentNullException("sequence");

            return CountValuesImpl(sequence, null);        
        }

        /// <summary>
        /// Returns a sequence of key/value pairs, 
        /// where each key is a distinct value from the source sequence, according to the given comparer,
        /// and each value is the number of times that value occurred in the source sequence.
        /// </summary>
        /// <typeparam name="T">Type of sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="comparer">Element equality comparer.</param>
        /// <returns>Sequence of key/value pairs, 
        /// where each key is a distinct value from the source sequence, according to the given comparer.
        /// and each value is the number of times that value occurred in the source sequence.</returns>
        /// <remarks>
        /// This operator immediately counts all source values, then streams the results.
        /// </remarks>
        public static IEnumerable<KeyValuePair<T, int>> CountValues<T>(this IEnumerable<T> sequence,
            IEqualityComparer<T> comparer) {
            if (sequence == null) throw new ArgumentNullException("sequence");

            return CountValuesImpl(sequence, comparer);
        }

        private static IEnumerable<KeyValuePair<T, int>> CountValuesImpl<T>(
            IEnumerable<T> sequence, 
            IEqualityComparer<T> comparer) {

            return new Counter<T>(sequence, comparer);
        }
    }
}
