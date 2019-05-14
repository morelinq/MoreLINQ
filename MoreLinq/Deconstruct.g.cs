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

    partial struct DeconstructibleEnumerable<T>
    {
        /// <summary>
        /// Deconstructs exactly 2 elements into separate variables.
        /// </summary>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <exception cref="InvalidOperationException">
        /// The source sequence either has too few or too many elements
        /// than being deconstructed.
        /// </exception>

        public void Deconstruct(out T item1, out T item2)
        {
            const int count = 2;

            using (var e = GetEnumerator())
            {
                item1 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item2 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                if (e.MoveNext())
                    throw CreateTooLongError(count);
            }
        }
        /// <summary>
        /// Deconstructs exactly 3 elements into separate variables.
        /// </summary>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <exception cref="InvalidOperationException">
        /// The source sequence either has too few or too many elements
        /// than being deconstructed.
        /// </exception>

        public void Deconstruct(out T item1, out T item2, out T item3)
        {
            const int count = 3;

            using (var e = GetEnumerator())
            {
                item1 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item2 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item3 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                if (e.MoveNext())
                    throw CreateTooLongError(count);
            }
        }
        /// <summary>
        /// Deconstructs exactly 4 elements into separate variables.
        /// </summary>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <exception cref="InvalidOperationException">
        /// The source sequence either has too few or too many elements
        /// than being deconstructed.
        /// </exception>

        public void Deconstruct(out T item1, out T item2, out T item3, out T item4)
        {
            const int count = 4;

            using (var e = GetEnumerator())
            {
                item1 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item2 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item3 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item4 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                if (e.MoveNext())
                    throw CreateTooLongError(count);
            }
        }
        /// <summary>
        /// Deconstructs exactly 5 elements into separate variables.
        /// </summary>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <exception cref="InvalidOperationException">
        /// The source sequence either has too few or too many elements
        /// than being deconstructed.
        /// </exception>

        public void Deconstruct(out T item1, out T item2, out T item3, out T item4, out T item5)
        {
            const int count = 5;

            using (var e = GetEnumerator())
            {
                item1 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item2 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item3 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item4 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item5 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                if (e.MoveNext())
                    throw CreateTooLongError(count);
            }
        }
        /// <summary>
        /// Deconstructs exactly 6 elements into separate variables.
        /// </summary>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="item6">The value of the sixth element.</param>
        /// <exception cref="InvalidOperationException">
        /// The source sequence either has too few or too many elements
        /// than being deconstructed.
        /// </exception>

        public void Deconstruct(out T item1, out T item2, out T item3, out T item4, out T item5, out T item6)
        {
            const int count = 6;

            using (var e = GetEnumerator())
            {
                item1 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item2 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item3 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item4 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item5 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item6 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                if (e.MoveNext())
                    throw CreateTooLongError(count);
            }
        }
        /// <summary>
        /// Deconstructs exactly 7 elements into separate variables.
        /// </summary>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="item6">The value of the sixth element.</param>
        /// <param name="item7">The value of the seventh element.</param>
        /// <exception cref="InvalidOperationException">
        /// The source sequence either has too few or too many elements
        /// than being deconstructed.
        /// </exception>

        public void Deconstruct(out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7)
        {
            const int count = 7;

            using (var e = GetEnumerator())
            {
                item1 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item2 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item3 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item4 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item5 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item6 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item7 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                if (e.MoveNext())
                    throw CreateTooLongError(count);
            }
        }
        /// <summary>
        /// Deconstructs exactly 8 elements into separate variables.
        /// </summary>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="item6">The value of the sixth element.</param>
        /// <param name="item7">The value of the seventh element.</param>
        /// <param name="item8">The value of the eighth element.</param>
        /// <exception cref="InvalidOperationException">
        /// The source sequence either has too few or too many elements
        /// than being deconstructed.
        /// </exception>

        public void Deconstruct(out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8)
        {
            const int count = 8;

            using (var e = GetEnumerator())
            {
                item1 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item2 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item3 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item4 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item5 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item6 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item7 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item8 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                if (e.MoveNext())
                    throw CreateTooLongError(count);
            }
        }
        /// <summary>
        /// Deconstructs exactly 9 elements into separate variables.
        /// </summary>
        /// <param name="item1">The value of the first element.</param>
        /// <param name="item2">The value of the second element.</param>
        /// <param name="item3">The value of the third element.</param>
        /// <param name="item4">The value of the fourth element.</param>
        /// <param name="item5">The value of the fifth element.</param>
        /// <param name="item6">The value of the sixth element.</param>
        /// <param name="item7">The value of the seventh element.</param>
        /// <param name="item8">The value of the eighth element.</param>
        /// <param name="item9">The value of the ninth element.</param>
        /// <exception cref="InvalidOperationException">
        /// The source sequence either has too few or too many elements
        /// than being deconstructed.
        /// </exception>

        public void Deconstruct(out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9)
        {
            const int count = 9;

            using (var e = GetEnumerator())
            {
                item1 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item2 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item3 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item4 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item5 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item6 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item7 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item8 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item9 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                if (e.MoveNext())
                    throw CreateTooLongError(count);
            }
        }
        /// <summary>
        /// Deconstructs exactly 10 elements into separate variables.
        /// </summary>
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
        /// <exception cref="InvalidOperationException">
        /// The source sequence either has too few or too many elements
        /// than being deconstructed.
        /// </exception>

        public void Deconstruct(out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10)
        {
            const int count = 10;

            using (var e = GetEnumerator())
            {
                item1 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item2 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item3 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item4 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item5 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item6 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item7 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item8 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item9 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item10 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                if (e.MoveNext())
                    throw CreateTooLongError(count);
            }
        }
        /// <summary>
        /// Deconstructs exactly 11 elements into separate variables.
        /// </summary>
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
        /// <exception cref="InvalidOperationException">
        /// The source sequence either has too few or too many elements
        /// than being deconstructed.
        /// </exception>

        public void Deconstruct(out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10, out T item11)
        {
            const int count = 11;

            using (var e = GetEnumerator())
            {
                item1 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item2 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item3 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item4 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item5 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item6 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item7 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item8 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item9 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item10 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item11 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                if (e.MoveNext())
                    throw CreateTooLongError(count);
            }
        }
        /// <summary>
        /// Deconstructs exactly 12 elements into separate variables.
        /// </summary>
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
        /// <exception cref="InvalidOperationException">
        /// The source sequence either has too few or too many elements
        /// than being deconstructed.
        /// </exception>

        public void Deconstruct(out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10, out T item11, out T item12)
        {
            const int count = 12;

            using (var e = GetEnumerator())
            {
                item1 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item2 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item3 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item4 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item5 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item6 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item7 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item8 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item9 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item10 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item11 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item12 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                if (e.MoveNext())
                    throw CreateTooLongError(count);
            }
        }
        /// <summary>
        /// Deconstructs exactly 13 elements into separate variables.
        /// </summary>
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
        /// <exception cref="InvalidOperationException">
        /// The source sequence either has too few or too many elements
        /// than being deconstructed.
        /// </exception>

        public void Deconstruct(out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10, out T item11, out T item12, out T item13)
        {
            const int count = 13;

            using (var e = GetEnumerator())
            {
                item1 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item2 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item3 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item4 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item5 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item6 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item7 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item8 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item9 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item10 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item11 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item12 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item13 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                if (e.MoveNext())
                    throw CreateTooLongError(count);
            }
        }
        /// <summary>
        /// Deconstructs exactly 14 elements into separate variables.
        /// </summary>
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
        /// <exception cref="InvalidOperationException">
        /// The source sequence either has too few or too many elements
        /// than being deconstructed.
        /// </exception>

        public void Deconstruct(out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10, out T item11, out T item12, out T item13, out T item14)
        {
            const int count = 14;

            using (var e = GetEnumerator())
            {
                item1 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item2 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item3 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item4 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item5 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item6 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item7 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item8 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item9 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item10 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item11 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item12 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item13 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item14 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                if (e.MoveNext())
                    throw CreateTooLongError(count);
            }
        }
        /// <summary>
        /// Deconstructs exactly 15 elements into separate variables.
        /// </summary>
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
        /// <exception cref="InvalidOperationException">
        /// The source sequence either has too few or too many elements
        /// than being deconstructed.
        /// </exception>

        public void Deconstruct(out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10, out T item11, out T item12, out T item13, out T item14, out T item15)
        {
            const int count = 15;

            using (var e = GetEnumerator())
            {
                item1 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item2 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item3 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item4 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item5 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item6 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item7 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item8 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item9 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item10 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item11 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item12 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item13 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item14 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item15 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                if (e.MoveNext())
                    throw CreateTooLongError(count);
            }
        }
        /// <summary>
        /// Deconstructs exactly 16 elements into separate variables.
        /// </summary>
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
        /// <exception cref="InvalidOperationException">
        /// The source sequence either has too few or too many elements
        /// than being deconstructed.
        /// </exception>

        public void Deconstruct(out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10, out T item11, out T item12, out T item13, out T item14, out T item15, out T item16)
        {
            const int count = 16;

            using (var e = GetEnumerator())
            {
                item1 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item2 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item3 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item4 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item5 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item6 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item7 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item8 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item9 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item10 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item11 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item12 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item13 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item14 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item15 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                item16 = e.MoveNext() ? e.Current : throw CreateTooShortError(count);
                if (e.MoveNext())
                    throw CreateTooLongError(count);
            }
        }
    }
}
