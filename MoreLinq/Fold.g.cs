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

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 5 elements.
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
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 5 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, TResult> folder)
        {
            return FoldImpl(source, 5, folder5: folder);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 6 elements.
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
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 6 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, TResult> folder)
        {
            return FoldImpl(source, 6, folder6: folder);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 7 elements.
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
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 7 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, TResult> folder)
        {
            return FoldImpl(source, 7, folder7: folder);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 8 elements.
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
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 8 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, TResult> folder)
        {
            return FoldImpl(source, 8, folder8: folder);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 9 elements.
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
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 9 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            return FoldImpl(source, 9, folder9: folder);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 10 elements.
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
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 10 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            return FoldImpl(source, 10, folder10: folder);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 11 elements.
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
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 11 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            return FoldImpl(source, 11, folder11: folder);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 12 elements.
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
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 12 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            return FoldImpl(source, 12, folder12: folder);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 13 elements.
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
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 13 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            return FoldImpl(source, 13, folder13: folder);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 14 elements.
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
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 14 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            return FoldImpl(source, 14, folder14: folder);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 15 elements.
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
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 15 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            return FoldImpl(source, 15, folder15: folder);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 16 elements.
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
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 16 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            return FoldImpl(source, 16, folder16: folder);
        }

    }
}
