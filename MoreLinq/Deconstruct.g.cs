#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Atif Aziz. All rights reserved.
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
        /// Deconstructs first 2 elements of the sequence into given variables.
        /// </summary>
        /// <param name="source">The sequence to deconstruct.</param>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="count">The actual count of elements deconstructed.</param>
        /// <remarks>
        /// If <paramref name="source"/> contains fewer elements then the
        /// remaining item variables will be initialized to the default value
        /// of <typeparamref name="T"/>.
        /// </remarks>

        public static void Deconstruct<T>(this IEnumerable<T> source, out T item1, out T item2, out int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            count = 0;

            using (var e = source.GetEnumerator())
            {
                (count, item1) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item2) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
            }
        }

        /// <summary>
        /// Deconstructs first 3 elements of the sequence into given variables.
        /// </summary>
        /// <param name="source">The sequence to deconstruct.</param>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="count">The actual count of elements deconstructed.</param>
        /// <remarks>
        /// If <paramref name="source"/> contains fewer elements then the
        /// remaining item variables will be initialized to the default value
        /// of <typeparamref name="T"/>.
        /// </remarks>

        public static void Deconstruct<T>(this IEnumerable<T> source, out T item1, out T item2, out T item3, out int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            count = 0;

            using (var e = source.GetEnumerator())
            {
                (count, item1) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item2) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item3) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
            }
        }

        /// <summary>
        /// Deconstructs first 4 elements of the sequence into given variables.
        /// </summary>
        /// <param name="source">The sequence to deconstruct.</param>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="count">The actual count of elements deconstructed.</param>
        /// <remarks>
        /// If <paramref name="source"/> contains fewer elements then the
        /// remaining item variables will be initialized to the default value
        /// of <typeparamref name="T"/>.
        /// </remarks>

        public static void Deconstruct<T>(this IEnumerable<T> source, out T item1, out T item2, out T item3, out T item4, out int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            count = 0;

            using (var e = source.GetEnumerator())
            {
                (count, item1) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item2) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item3) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item4) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
            }
        }

        /// <summary>
        /// Deconstructs first 5 elements of the sequence into given variables.
        /// </summary>
        /// <param name="source">The sequence to deconstruct.</param>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="count">The actual count of elements deconstructed.</param>
        /// <remarks>
        /// If <paramref name="source"/> contains fewer elements then the
        /// remaining item variables will be initialized to the default value
        /// of <typeparamref name="T"/>.
        /// </remarks>

        public static void Deconstruct<T>(this IEnumerable<T> source, out T item1, out T item2, out T item3, out T item4, out T item5, out int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            count = 0;

            using (var e = source.GetEnumerator())
            {
                (count, item1) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item2) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item3) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item4) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item5) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
            }
        }

        /// <summary>
        /// Deconstructs first 6 elements of the sequence into given variables.
        /// </summary>
        /// <param name="source">The sequence to deconstruct.</param>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="item6">The value of the sixth element.</param>
        /// <param name="count">The actual count of elements deconstructed.</param>
        /// <remarks>
        /// If <paramref name="source"/> contains fewer elements then the
        /// remaining item variables will be initialized to the default value
        /// of <typeparamref name="T"/>.
        /// </remarks>

        public static void Deconstruct<T>(this IEnumerable<T> source, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            count = 0;

            using (var e = source.GetEnumerator())
            {
                (count, item1) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item2) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item3) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item4) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item5) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item6) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
            }
        }

        /// <summary>
        /// Deconstructs first 7 elements of the sequence into given variables.
        /// </summary>
        /// <param name="source">The sequence to deconstruct.</param>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="item6">The value of the sixth element.</param>
        /// <param name="item7">The value of the seventh element.</param>
        /// <param name="count">The actual count of elements deconstructed.</param>
        /// <remarks>
        /// If <paramref name="source"/> contains fewer elements then the
        /// remaining item variables will be initialized to the default value
        /// of <typeparamref name="T"/>.
        /// </remarks>

        public static void Deconstruct<T>(this IEnumerable<T> source, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            count = 0;

            using (var e = source.GetEnumerator())
            {
                (count, item1) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item2) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item3) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item4) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item5) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item6) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item7) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
            }
        }

        /// <summary>
        /// Deconstructs first 8 elements of the sequence into given variables.
        /// </summary>
        /// <param name="source">The sequence to deconstruct.</param>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="item6">The value of the sixth element.</param>
        /// <param name="item7">The value of the seventh element.</param>
        /// <param name="item8">The value of the eighth element.</param>
        /// <param name="count">The actual count of elements deconstructed.</param>
        /// <remarks>
        /// If <paramref name="source"/> contains fewer elements then the
        /// remaining item variables will be initialized to the default value
        /// of <typeparamref name="T"/>.
        /// </remarks>

        public static void Deconstruct<T>(this IEnumerable<T> source, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            count = 0;

            using (var e = source.GetEnumerator())
            {
                (count, item1) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item2) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item3) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item4) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item5) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item6) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item7) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item8) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
            }
        }

        /// <summary>
        /// Deconstructs first 9 elements of the sequence into given variables.
        /// </summary>
        /// <param name="source">The sequence to deconstruct.</param>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="item6">The value of the sixth element.</param>
        /// <param name="item7">The value of the seventh element.</param>
        /// <param name="item8">The value of the eighth element.</param>
        /// <param name="item9">The value of the ninth element.</param>
        /// <param name="count">The actual count of elements deconstructed.</param>
        /// <remarks>
        /// If <paramref name="source"/> contains fewer elements then the
        /// remaining item variables will be initialized to the default value
        /// of <typeparamref name="T"/>.
        /// </remarks>

        public static void Deconstruct<T>(this IEnumerable<T> source, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            count = 0;

            using (var e = source.GetEnumerator())
            {
                (count, item1) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item2) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item3) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item4) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item5) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item6) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item7) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item8) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item9) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
            }
        }

        /// <summary>
        /// Deconstructs first 10 elements of the sequence into given variables.
        /// </summary>
        /// <param name="source">The sequence to deconstruct.</param>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="item6">The value of the sixth element.</param>
        /// <param name="item7">The value of the seventh element.</param>
        /// <param name="item8">The value of the eighth element.</param>
        /// <param name="item9">The value of the ninth element.</param>
        /// <param name="item10">The value of the tenth element.</param>
        /// <param name="count">The actual count of elements deconstructed.</param>
        /// <remarks>
        /// If <paramref name="source"/> contains fewer elements then the
        /// remaining item variables will be initialized to the default value
        /// of <typeparamref name="T"/>.
        /// </remarks>

        public static void Deconstruct<T>(this IEnumerable<T> source, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10, out int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            count = 0;

            using (var e = source.GetEnumerator())
            {
                (count, item1) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item2) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item3) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item4) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item5) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item6) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item7) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item8) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item9) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item10) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
            }
        }

        /// <summary>
        /// Deconstructs first 11 elements of the sequence into given variables.
        /// </summary>
        /// <param name="source">The sequence to deconstruct.</param>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="item6">The value of the sixth element.</param>
        /// <param name="item7">The value of the seventh element.</param>
        /// <param name="item8">The value of the eighth element.</param>
        /// <param name="item9">The value of the ninth element.</param>
        /// <param name="item10">The value of the tenth element.</param>
        /// <param name="item11">The value of the eleventh element.</param>
        /// <param name="count">The actual count of elements deconstructed.</param>
        /// <remarks>
        /// If <paramref name="source"/> contains fewer elements then the
        /// remaining item variables will be initialized to the default value
        /// of <typeparamref name="T"/>.
        /// </remarks>

        public static void Deconstruct<T>(this IEnumerable<T> source, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10, out T item11, out int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            count = 0;

            using (var e = source.GetEnumerator())
            {
                (count, item1) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item2) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item3) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item4) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item5) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item6) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item7) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item8) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item9) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item10) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item11) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
            }
        }

        /// <summary>
        /// Deconstructs first 12 elements of the sequence into given variables.
        /// </summary>
        /// <param name="source">The sequence to deconstruct.</param>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="item6">The value of the sixth element.</param>
        /// <param name="item7">The value of the seventh element.</param>
        /// <param name="item8">The value of the eighth element.</param>
        /// <param name="item9">The value of the ninth element.</param>
        /// <param name="item10">The value of the tenth element.</param>
        /// <param name="item11">The value of the eleventh element.</param>
        /// <param name="item12">The value of the twelfth element.</param>
        /// <param name="count">The actual count of elements deconstructed.</param>
        /// <remarks>
        /// If <paramref name="source"/> contains fewer elements then the
        /// remaining item variables will be initialized to the default value
        /// of <typeparamref name="T"/>.
        /// </remarks>

        public static void Deconstruct<T>(this IEnumerable<T> source, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10, out T item11, out T item12, out int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            count = 0;

            using (var e = source.GetEnumerator())
            {
                (count, item1) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item2) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item3) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item4) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item5) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item6) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item7) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item8) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item9) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item10) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item11) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item12) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
            }
        }

        /// <summary>
        /// Deconstructs first 13 elements of the sequence into given variables.
        /// </summary>
        /// <param name="source">The sequence to deconstruct.</param>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="item6">The value of the sixth element.</param>
        /// <param name="item7">The value of the seventh element.</param>
        /// <param name="item8">The value of the eighth element.</param>
        /// <param name="item9">The value of the ninth element.</param>
        /// <param name="item10">The value of the tenth element.</param>
        /// <param name="item11">The value of the eleventh element.</param>
        /// <param name="item12">The value of the twelfth element.</param>
        /// <param name="item13">The value of the thirteenth element.</param>
        /// <param name="count">The actual count of elements deconstructed.</param>
        /// <remarks>
        /// If <paramref name="source"/> contains fewer elements then the
        /// remaining item variables will be initialized to the default value
        /// of <typeparamref name="T"/>.
        /// </remarks>

        public static void Deconstruct<T>(this IEnumerable<T> source, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10, out T item11, out T item12, out T item13, out int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            count = 0;

            using (var e = source.GetEnumerator())
            {
                (count, item1) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item2) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item3) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item4) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item5) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item6) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item7) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item8) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item9) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item10) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item11) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item12) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item13) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
            }
        }

        /// <summary>
        /// Deconstructs first 14 elements of the sequence into given variables.
        /// </summary>
        /// <param name="source">The sequence to deconstruct.</param>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="item6">The value of the sixth element.</param>
        /// <param name="item7">The value of the seventh element.</param>
        /// <param name="item8">The value of the eighth element.</param>
        /// <param name="item9">The value of the ninth element.</param>
        /// <param name="item10">The value of the tenth element.</param>
        /// <param name="item11">The value of the eleventh element.</param>
        /// <param name="item12">The value of the twelfth element.</param>
        /// <param name="item13">The value of the thirteenth element.</param>
        /// <param name="item14">The value of the fourteenth element.</param>
        /// <param name="count">The actual count of elements deconstructed.</param>
        /// <remarks>
        /// If <paramref name="source"/> contains fewer elements then the
        /// remaining item variables will be initialized to the default value
        /// of <typeparamref name="T"/>.
        /// </remarks>

        public static void Deconstruct<T>(this IEnumerable<T> source, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10, out T item11, out T item12, out T item13, out T item14, out int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            count = 0;

            using (var e = source.GetEnumerator())
            {
                (count, item1) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item2) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item3) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item4) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item5) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item6) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item7) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item8) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item9) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item10) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item11) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item12) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item13) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item14) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
            }
        }

        /// <summary>
        /// Deconstructs first 15 elements of the sequence into given variables.
        /// </summary>
        /// <param name="source">The sequence to deconstruct.</param>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="item6">The value of the sixth element.</param>
        /// <param name="item7">The value of the seventh element.</param>
        /// <param name="item8">The value of the eighth element.</param>
        /// <param name="item9">The value of the ninth element.</param>
        /// <param name="item10">The value of the tenth element.</param>
        /// <param name="item11">The value of the eleventh element.</param>
        /// <param name="item12">The value of the twelfth element.</param>
        /// <param name="item13">The value of the thirteenth element.</param>
        /// <param name="item14">The value of the fourteenth element.</param>
        /// <param name="item15">The value of the fifteenth element.</param>
        /// <param name="count">The actual count of elements deconstructed.</param>
        /// <remarks>
        /// If <paramref name="source"/> contains fewer elements then the
        /// remaining item variables will be initialized to the default value
        /// of <typeparamref name="T"/>.
        /// </remarks>

        public static void Deconstruct<T>(this IEnumerable<T> source, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10, out T item11, out T item12, out T item13, out T item14, out T item15, out int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            count = 0;

            using (var e = source.GetEnumerator())
            {
                (count, item1) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item2) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item3) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item4) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item5) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item6) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item7) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item8) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item9) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item10) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item11) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item12) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item13) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item14) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item15) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
            }
        }

        /// <summary>
        /// Deconstructs first 16 elements of the sequence into given variables.
        /// </summary>
        /// <param name="source">The sequence to deconstruct.</param>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="item6">The value of the sixth element.</param>
        /// <param name="item7">The value of the seventh element.</param>
        /// <param name="item8">The value of the eighth element.</param>
        /// <param name="item9">The value of the ninth element.</param>
        /// <param name="item10">The value of the tenth element.</param>
        /// <param name="item11">The value of the eleventh element.</param>
        /// <param name="item12">The value of the twelfth element.</param>
        /// <param name="item13">The value of the thirteenth element.</param>
        /// <param name="item14">The value of the fourteenth element.</param>
        /// <param name="item15">The value of the fifteenth element.</param>
        /// <param name="item16">The value of the sixteenth element.</param>
        /// <param name="count">The actual count of elements deconstructed.</param>
        /// <remarks>
        /// If <paramref name="source"/> contains fewer elements then the
        /// remaining item variables will be initialized to the default value
        /// of <typeparamref name="T"/>.
        /// </remarks>

        public static void Deconstruct<T>(this IEnumerable<T> source, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10, out T item11, out T item12, out T item13, out T item14, out T item15, out T item16, out int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            count = 0;

            using (var e = source.GetEnumerator())
            {
                (count, item1) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item2) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item3) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item4) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item5) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item6) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item7) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item8) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item9) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item10) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item11) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item12) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item13) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item14) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item15) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
                (count, item16) = e.MoveNext() ? (count + 1, e.Current) : (count, default);
            }
        }
    }
}
