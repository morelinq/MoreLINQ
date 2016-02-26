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
    using System.Linq;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Creates a right-aligned sliding window over the source sequence
        /// of a given size.
        /// </summary>

        public static IEnumerable<IEnumerable<TSource>> WindowRight<TSource>(this IEnumerable<TSource> source, int size)
        {
            return source.WindowRight(size, (_, w) => w);
        }

        /// <summary>
        /// Creates a right-aligned sliding window over the source sequence
        /// of a given size.
        /// </summary>

        public static IEnumerable<TResult> WindowRight<TSource, TResult>(this IEnumerable<TSource> source, int size, Func<TSource, IEnumerable<TSource>, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (size <= 0) throw new ArgumentOutOfRangeException("size");

            return WindowRightImpl(source, size, resultSelector);
        }

        static IEnumerable<TResult> WindowRightImpl<TSource, TResult>(IEnumerable<TSource> source, int size, Func<TSource, IEnumerable<TSource>, TResult> resultSelector)
        {
            var window = new List<TSource>();
            foreach (var item in source)
            {
                window.Add(item);
                yield return resultSelector(item, window);
                window = new List<TSource>(window.Count == size ? window.Skip(1) : window);
            }
        }
    }
}