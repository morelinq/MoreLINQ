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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[1];
            foreach (var e in AssertCountImpl(source.Index(), 1, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0]);
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[2];
            foreach (var e in AssertCountImpl(source.Index(), 2, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0], elements[1]);
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[3];
            foreach (var e in AssertCountImpl(source.Index(), 3, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0], elements[1], elements[2]);
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[4];
            foreach (var e in AssertCountImpl(source.Index(), 4, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0], elements[1], elements[2], elements[3]);
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[5];
            foreach (var e in AssertCountImpl(source.Index(), 5, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0], elements[1], elements[2], elements[3], elements[4]);
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[6];
            foreach (var e in AssertCountImpl(source.Index(), 6, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5]);
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[7];
            foreach (var e in AssertCountImpl(source.Index(), 7, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6]);
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[8];
            foreach (var e in AssertCountImpl(source.Index(), 8, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7]);
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[9];
            foreach (var e in AssertCountImpl(source.Index(), 9, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8]);
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[10];
            foreach (var e in AssertCountImpl(source.Index(), 10, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9]);
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[11];
            foreach (var e in AssertCountImpl(source.Index(), 11, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10]);
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[12];
            foreach (var e in AssertCountImpl(source.Index(), 12, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11]);
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[13];
            foreach (var e in AssertCountImpl(source.Index(), 13, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12]);
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[14];
            foreach (var e in AssertCountImpl(source.Index(), 14, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12], elements[13]);
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[15];
            foreach (var e in AssertCountImpl(source.Index(), 15, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12], elements[13], elements[14]);
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
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = new T[16];
            foreach (var e in AssertCountImpl(source.Index(), 16, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12], elements[13], elements[14], elements[15]);
        }

    }
}
