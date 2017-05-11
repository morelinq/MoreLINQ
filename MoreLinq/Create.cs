#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All rights reserved.
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
        /// Creates an <see cref="IEnumerable{T}"/> given a factory function
        /// for an <see cref="IEnumerator{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of returned sequence elements.</typeparam>
        /// <param name="factory">
        /// Function that creates an <see cref="IEnumerator{T}"/>.</param>
        /// <remarks>
        /// This method uses deferred execution semantics and streams its
        /// results.
        /// </remarks>

        public static IEnumerable<T> Create<T>(Func<IEnumerator<T>> factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            return _(); IEnumerable<T> _()
            {
                using (var e = factory())
                    while (e.MoveNext())
                        yield return e.Current;
            }
        }
    }
}
