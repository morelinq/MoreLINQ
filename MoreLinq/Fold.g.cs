#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2013 Atif Aziz. All rights reserved.
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

    partial class MoreEnumerable
    {
        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 1 element.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 1 element</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, TResult> folder)
        {
            return FoldImpl(source, 1, folder1: folder);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 2 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 2 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, TResult> folder)
        {
            return FoldImpl(source, 2, folder2: folder);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 3 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 3 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, TResult> folder)
        {
            return FoldImpl(source, 3, folder3: folder);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 4 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 4 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, TResult> folder)
        {
            return FoldImpl(source, 4, folder4: folder);
        }

    }
}
