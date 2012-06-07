#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008-2011 Jonathan Skeet. All rights reserved.
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

namespace MoreLinq
{
    /// <summary>
    /// Provides a set of static methods for querying objects that 
    /// implement <see cref="IEnumerable{T}" />. The actual methods
    /// are implemented in files reflecting the method name.
    /// </summary>
    public static partial class MoreEnumerable
    {
        static IEnumerator<T>[] GetEnumerators<T>(this IEnumerable<IEnumerable<T>> sources)
        {
            var array = sources.ToArray();
            var enumerators = new IEnumerator<T>[array.Length];
            try
            {
                for (var i = 0; i < array.Length; i++)
                    enumerators[i] = array[i].GetEnumerator();
                return enumerators;
            }
            catch
            {
                foreach (var enumerator in enumerators.TakeWhile(e => e != null))
                    enumerator.Dispose();
                throw;
            }
        }
    }
}
