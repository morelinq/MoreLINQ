#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Leandro F. Vieira (leandromoh). All rights reserved.
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
    using System.Collections;

    static partial class MoreEnumerable
    {
        delegate TResult Folder<in T, out TResult>(params T[] args);

        static IEnumerable<TResult> ZipImpl<T1, T2, T3, T4, TResult>(
            IEnumerable<T1>  s1,
            IEnumerable<T2>  s2,
            IEnumerable<T3>? s3,
            IEnumerable<T4>? s4,
            Func<T1, T2, T3, T4, TResult> resultSelector,
            int limit,
            Folder<IEnumerator?, Exception>? errorSelector = null)
        {
            IEnumerator<T1>? e1 = null;
            IEnumerator<T2>? e2 = null;
            IEnumerator<T3>? e3 = null;
            IEnumerator<T4>? e4 = null;
            var terminations = 0;

            try
            {
                e1 = s1 .GetEnumerator();
                e2 = s2 .GetEnumerator();
                e3 = s3?.GetEnumerator();
                e4 = s4?.GetEnumerator();

                while (true)
                {
                    var n = 0;
                    var v1 = Read(ref e1, ++n);
                    var v2 = Read(ref e2, ++n);
                    var v3 = Read(ref e3, ++n);
                    var v4 = Read(ref e4, ++n);

                    if (terminations <= limit)
                        yield return resultSelector(v1, v2, v3, v4);
                    else
                        yield break;
                }
            }
            finally
            {
                e1?.Dispose();
                e2?.Dispose();
                e3?.Dispose();
                e4?.Dispose();
            }

            T Read<T>(ref IEnumerator<T>? e, int n)
            {
                if (e == null || terminations > limit)
                    return default!;

                T value;
                if (e.MoveNext())
                {
                    value = e.Current;
                }
                else
                {
                    e.Dispose();
                    e = null;
                    terminations++;
                    value = default!;
                }

                if (errorSelector != null && terminations > 0 && terminations < n)
                    throw errorSelector(e1, e2, e3, e4);

                return value;
            }
        }
    }
}
