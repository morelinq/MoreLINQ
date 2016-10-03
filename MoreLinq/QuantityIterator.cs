#region License and Terms
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreLinq
{
    static partial class MoreEnumerable
    {
        private static bool QuantityIterator<T>(IEnumerable<T> source, int limit, Func<int, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");

            var col = source as ICollection<T>;

            if (col != null)
            {
                return predicate(col.Count);
            }

            var count = 0;

            using (var e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (++count == limit)
                    {
                        break;
                    }
                }
            }

            return predicate(count);
        }
    }
}
