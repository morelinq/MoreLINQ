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
        static TResult FoldImpl<T, TResult>(IEnumerable<T> source, int count,
            Func<T, TResult> folder1 = null,
            Func<T, T, TResult> folder2 = null,
            Func<T, T, T, TResult> folder3 = null,
            Func<T, T, T, T, TResult> folder4 = null,
            Func<T, T, T, T, T, TResult> folder5 = null,
            Func<T, T, T, T, T, T, TResult> folder6 = null,
            Func<T, T, T, T, T, T, T, TResult> folder7 = null,
            Func<T, T, T, T, T, T, T, T, TResult> folder8 = null,
            Func<T, T, T, T, T, T, T, T, T, TResult> folder9 = null,
            Func<T, T, T, T, T, T, T, T, T, T, TResult> folder10 = null,
            Func<T, T, T, T, T, T, T, T, T, T, T, TResult> folder11 = null,
            Func<T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder12 = null,
            Func<T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder13 = null,
            Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder14 = null,
            Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder15 = null,
            Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder16 = null
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
                throw new ArgumentNullException("folder");   // ReSharper restore NotResolvedInText
            }

            var elements = new T[count];
            foreach (var e in AssertCountImpl(source.Index(), count, OnFolderSourceSizeErrorSelector))
                elements[e.Key] = e.Value;

            switch (count)
            {
                case  1: return folder1 (elements[0]);
                case  2: return folder2 (elements[0], elements[1]);
                case  3: return folder3 (elements[0], elements[1], elements[2]);
                case  4: return folder4 (elements[0], elements[1], elements[2], elements[3]);
                case  5: return folder5 (elements[0], elements[1], elements[2], elements[3], elements[4]);
                case  6: return folder6 (elements[0], elements[1], elements[2], elements[3], elements[4], elements[5]);
                case  7: return folder7 (elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6]);
                case  8: return folder8 (elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7]);
                case  9: return folder9 (elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8]);
                case 10: return folder10(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9]);
                case 11: return folder11(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10]);
                case 12: return folder12(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11]);
                case 13: return folder13(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12]);
                case 14: return folder14(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12], elements[13]);
                case 15: return folder15(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12], elements[13], elements[14]);
                case 16: return folder16(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12], elements[13], elements[14], elements[15]);
                default: throw new NotSupportedException();
            }
        }

        static readonly Func<int, int, Exception> OnFolderSourceSizeErrorSelector = OnFolderSourceSizeError;

        static Exception OnFolderSourceSizeError(int cmp, int count)
        {
            var message = cmp < 0
                        ? "Sequence contains too few elements when exactly {0} {1} expected."
                        : "Sequence contains too many elements when exactly {0} {1} expected.";
            return new InvalidOperationException(string.Format(message, count.ToString("N0"), count == 1 ? "was" : "were"));
        }
    }
}
