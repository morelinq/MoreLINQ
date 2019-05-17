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
                ReadWithCount(e, ref count, out item1);
                ReadWithCount(e, ref count, out item2);
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
                ReadWithCount(e, ref count, out item1);
                ReadWithCount(e, ref count, out item2);
                ReadWithCount(e, ref count, out item3);
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
                ReadWithCount(e, ref count, out item1);
                ReadWithCount(e, ref count, out item2);
                ReadWithCount(e, ref count, out item3);
                ReadWithCount(e, ref count, out item4);
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
                ReadWithCount(e, ref count, out item1);
                ReadWithCount(e, ref count, out item2);
                ReadWithCount(e, ref count, out item3);
                ReadWithCount(e, ref count, out item4);
                ReadWithCount(e, ref count, out item5);
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
                ReadWithCount(e, ref count, out item1);
                ReadWithCount(e, ref count, out item2);
                ReadWithCount(e, ref count, out item3);
                ReadWithCount(e, ref count, out item4);
                ReadWithCount(e, ref count, out item5);
                ReadWithCount(e, ref count, out item6);
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
                ReadWithCount(e, ref count, out item1);
                ReadWithCount(e, ref count, out item2);
                ReadWithCount(e, ref count, out item3);
                ReadWithCount(e, ref count, out item4);
                ReadWithCount(e, ref count, out item5);
                ReadWithCount(e, ref count, out item6);
                ReadWithCount(e, ref count, out item7);
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
                ReadWithCount(e, ref count, out item1);
                ReadWithCount(e, ref count, out item2);
                ReadWithCount(e, ref count, out item3);
                ReadWithCount(e, ref count, out item4);
                ReadWithCount(e, ref count, out item5);
                ReadWithCount(e, ref count, out item6);
                ReadWithCount(e, ref count, out item7);
                ReadWithCount(e, ref count, out item8);
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
                ReadWithCount(e, ref count, out item1);
                ReadWithCount(e, ref count, out item2);
                ReadWithCount(e, ref count, out item3);
                ReadWithCount(e, ref count, out item4);
                ReadWithCount(e, ref count, out item5);
                ReadWithCount(e, ref count, out item6);
                ReadWithCount(e, ref count, out item7);
                ReadWithCount(e, ref count, out item8);
                ReadWithCount(e, ref count, out item9);
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
                ReadWithCount(e, ref count, out item1);
                ReadWithCount(e, ref count, out item2);
                ReadWithCount(e, ref count, out item3);
                ReadWithCount(e, ref count, out item4);
                ReadWithCount(e, ref count, out item5);
                ReadWithCount(e, ref count, out item6);
                ReadWithCount(e, ref count, out item7);
                ReadWithCount(e, ref count, out item8);
                ReadWithCount(e, ref count, out item9);
                ReadWithCount(e, ref count, out item10);
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
                ReadWithCount(e, ref count, out item1);
                ReadWithCount(e, ref count, out item2);
                ReadWithCount(e, ref count, out item3);
                ReadWithCount(e, ref count, out item4);
                ReadWithCount(e, ref count, out item5);
                ReadWithCount(e, ref count, out item6);
                ReadWithCount(e, ref count, out item7);
                ReadWithCount(e, ref count, out item8);
                ReadWithCount(e, ref count, out item9);
                ReadWithCount(e, ref count, out item10);
                ReadWithCount(e, ref count, out item11);
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
                ReadWithCount(e, ref count, out item1);
                ReadWithCount(e, ref count, out item2);
                ReadWithCount(e, ref count, out item3);
                ReadWithCount(e, ref count, out item4);
                ReadWithCount(e, ref count, out item5);
                ReadWithCount(e, ref count, out item6);
                ReadWithCount(e, ref count, out item7);
                ReadWithCount(e, ref count, out item8);
                ReadWithCount(e, ref count, out item9);
                ReadWithCount(e, ref count, out item10);
                ReadWithCount(e, ref count, out item11);
                ReadWithCount(e, ref count, out item12);
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
                ReadWithCount(e, ref count, out item1);
                ReadWithCount(e, ref count, out item2);
                ReadWithCount(e, ref count, out item3);
                ReadWithCount(e, ref count, out item4);
                ReadWithCount(e, ref count, out item5);
                ReadWithCount(e, ref count, out item6);
                ReadWithCount(e, ref count, out item7);
                ReadWithCount(e, ref count, out item8);
                ReadWithCount(e, ref count, out item9);
                ReadWithCount(e, ref count, out item10);
                ReadWithCount(e, ref count, out item11);
                ReadWithCount(e, ref count, out item12);
                ReadWithCount(e, ref count, out item13);
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
                ReadWithCount(e, ref count, out item1);
                ReadWithCount(e, ref count, out item2);
                ReadWithCount(e, ref count, out item3);
                ReadWithCount(e, ref count, out item4);
                ReadWithCount(e, ref count, out item5);
                ReadWithCount(e, ref count, out item6);
                ReadWithCount(e, ref count, out item7);
                ReadWithCount(e, ref count, out item8);
                ReadWithCount(e, ref count, out item9);
                ReadWithCount(e, ref count, out item10);
                ReadWithCount(e, ref count, out item11);
                ReadWithCount(e, ref count, out item12);
                ReadWithCount(e, ref count, out item13);
                ReadWithCount(e, ref count, out item14);
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
                ReadWithCount(e, ref count, out item1);
                ReadWithCount(e, ref count, out item2);
                ReadWithCount(e, ref count, out item3);
                ReadWithCount(e, ref count, out item4);
                ReadWithCount(e, ref count, out item5);
                ReadWithCount(e, ref count, out item6);
                ReadWithCount(e, ref count, out item7);
                ReadWithCount(e, ref count, out item8);
                ReadWithCount(e, ref count, out item9);
                ReadWithCount(e, ref count, out item10);
                ReadWithCount(e, ref count, out item11);
                ReadWithCount(e, ref count, out item12);
                ReadWithCount(e, ref count, out item13);
                ReadWithCount(e, ref count, out item14);
                ReadWithCount(e, ref count, out item15);
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
                ReadWithCount(e, ref count, out item1);
                ReadWithCount(e, ref count, out item2);
                ReadWithCount(e, ref count, out item3);
                ReadWithCount(e, ref count, out item4);
                ReadWithCount(e, ref count, out item5);
                ReadWithCount(e, ref count, out item6);
                ReadWithCount(e, ref count, out item7);
                ReadWithCount(e, ref count, out item8);
                ReadWithCount(e, ref count, out item9);
                ReadWithCount(e, ref count, out item10);
                ReadWithCount(e, ref count, out item11);
                ReadWithCount(e, ref count, out item12);
                ReadWithCount(e, ref count, out item13);
                ReadWithCount(e, ref count, out item14);
                ReadWithCount(e, ref count, out item15);
                ReadWithCount(e, ref count, out item16);
            }
        }
    }
}
