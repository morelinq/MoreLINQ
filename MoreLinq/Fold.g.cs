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

#nullable enable // required for auto-generated sources (see below why)

// > Older code generation strategies may not be nullable aware. Setting the
// > project-level nullable context to "enable" could result in many
// > warnings that a user is unable to fix. To support this scenario any syntax
// > tree that is determined to be generated will have its nullable state
// > implicitly set to "disable", regardless of the overall project state.
//
// Source: https://github.com/dotnet/roslyn/blob/70e158ba6c2c99bd3c3fc0754af0dbf82a6d353d/docs/features/nullable-reference-types.md#generated-code

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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 1 element.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(1);
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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 2 elements.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(2);
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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 3 elements.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(3);
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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 4 elements.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(4);
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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 5 elements.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(5);
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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 6 elements.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(6);
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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 7 elements.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(7);
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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 8 elements.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(8);
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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 9 elements.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(9);
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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 10 elements.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(10);
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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 11 elements.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(11);
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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 12 elements.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(12);
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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 13 elements.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(13);
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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 14 elements.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(14);
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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 15 elements.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(15);
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
        /// <typeparam name="T">Type of element in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="source"/> or <paramref name="folder"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 16 elements.</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var elements = source.Fold(16);
            return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12], elements[13], elements[14], elements[15]);
        }
    }
}
