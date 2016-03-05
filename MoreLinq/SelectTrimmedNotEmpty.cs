#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Sergiy Zinovyev. All rights reserved.
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
    using System.Collections.Generic;
    using System.Linq;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Shuffles the source sequence.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <returns>A sequence of all elements from the original sequence but in random order.</returns>
        /// <remarks>Each element from the source is used only and only once for result sequence.</remarks>
        public static IEnumerable<string> SelectTrimmedNotEmpty(this IEnumerable<string> source)
        {
            return source.Where(v => !string.IsNullOrEmpty(v)).Select(v => v.Trim()).Where(v => !string.IsNullOrEmpty(v));
        }
    }
}
