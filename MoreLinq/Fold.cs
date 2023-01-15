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

    static partial class MoreEnumerable
    {
        const string folder = nameof(folder);

        static TResult FoldImpl<T, TResult>(IEnumerable<T> source, int count,
            Func<T, TResult>? folder1 = null,
            Func<T, T, TResult>? folder2 = null,
            Func<T, T, T, TResult>? folder3 = null,
            Func<T, T, T, T, TResult>? folder4 = null,
            Func<T, T, T, T, T, TResult>? folder5 = null,
            Func<T, T, T, T, T, T, TResult>? folder6 = null,
            Func<T, T, T, T, T, T, T, TResult>? folder7 = null,
            Func<T, T, T, T, T, T, T, T, TResult>? folder8 = null,
            Func<T, T, T, T, T, T, T, T, T, TResult>? folder9 = null,
            Func<T, T, T, T, T, T, T, T, T, T, TResult>? folder10 = null,
            Func<T, T, T, T, T, T, T, T, T, T, T, TResult>? folder11 = null,
            Func<T, T, T, T, T, T, T, T, T, T, T, T, TResult>? folder12 = null,
            Func<T, T, T, T, T, T, T, T, T, T, T, T, T, TResult>? folder13 = null,
            Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult>? folder14 = null,
            Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult>? folder15 = null,
            Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult>? folder16 = null
            )
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (   count ==  1 && folder1  == null
                || count ==  2 && folder2  == null
                || count ==  3 && folder3  == null
                || count ==  4 && folder4  == null
                || count ==  5 && folder5  == null
                || count ==  6 && folder6  == null
                || count ==  7 && folder7  == null
                || count ==  8 && folder8  == null
                || count ==  9 && folder9  == null
                || count == 10 && folder10 == null
                || count == 11 && folder11 == null
                || count == 12 && folder12 == null
                || count == 13 && folder13 == null
                || count == 14 && folder14 == null
                || count == 15 && folder15 == null
                || count == 16 && folder16 == null
                )
            {                                                // ReSharper disable NotResolvedInText
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException("folder");   // ReSharper restore NotResolvedInText
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }

            var elements = new T[count];
            foreach (var e in AssertCountImpl(source.Index(), count, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            return count switch
            {
                 1 => Assume.NotNull(folder1 )(elements[0]),
                 2 => Assume.NotNull(folder2 )(elements[0], elements[1]),
                 3 => Assume.NotNull(folder3 )(elements[0], elements[1], elements[2]),
                 4 => Assume.NotNull(folder4 )(elements[0], elements[1], elements[2], elements[3]),
                 5 => Assume.NotNull(folder5 )(elements[0], elements[1], elements[2], elements[3], elements[4]),
                 6 => Assume.NotNull(folder6 )(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5]),
                 7 => Assume.NotNull(folder7 )(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6]),
                 8 => Assume.NotNull(folder8 )(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7]),
                 9 => Assume.NotNull(folder9 )(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8]),
                10 => Assume.NotNull(folder10)(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9]),
                11 => Assume.NotNull(folder11)(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10]),
                12 => Assume.NotNull(folder12)(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11]),
                13 => Assume.NotNull(folder13)(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12]),
                14 => Assume.NotNull(folder14)(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12], elements[13]),
                15 => Assume.NotNull(folder15)(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12], elements[13], elements[14]),
                16 => Assume.NotNull(folder16)(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12], elements[13], elements[14], elements[15]),
                _ => throw new NotSupportedException()
            };
        }

        static readonly Func<int, int, Exception> OnFolderSourceSizeErrorSelector = OnFolderSourceSizeError;

        static Exception OnFolderSourceSizeError(int cmp, int count)
        {
            var message = cmp < 0
                        ? "Sequence contains too few elements when exactly {0} {1} expected."
                        : "Sequence contains too many elements when exactly {0} {1} expected.";
            return new InvalidOperationException(string.Format(null, message, count.ToString("N0", null), count == 1 ? "was" : "were"));
        }
    }
}
