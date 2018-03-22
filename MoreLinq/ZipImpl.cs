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
        static IEnumerable<TResult> ZipImpl<T1, T2, T3, T4, TResult>(
            IEnumerable<T1> s1,
            IEnumerable<T2> s2,
            IEnumerable<T3> s3,
            IEnumerable<T4> s4,
            Func<T1, T2, T3, T4, TResult> resultSelector,
            int limit,
            Func<IEnumerator[], Exception> errorSelector = null
            )
        {
            var e1 = s1?.GetEnumerator();
            var e2 = s2?.GetEnumerator();
            var e3 = s3?.GetEnumerator();
            var e4 = s4?.GetEnumerator();
            var disposed = 0;
            int calls;

            try
            {
                while (true)
                {
                    calls = 0;
                    var v1 = GetValue(ref e1);
                    var v2 = GetValue(ref e2);
                    var v3 = GetValue(ref e3);
                    var v4 = GetValue(ref e4);

                    if (disposed <= limit)
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

            T GetValue<T>(ref IEnumerator<T> e)
            {
                calls++;
                if (e == null || disposed > limit)
                {
                    return default;
                }
                else if (e.MoveNext())
                {
                    return ValidateEquiZip(e.Current);
                }
                else
                {
                    e.Dispose();
                    e = null;
                    disposed++;
                    return ValidateEquiZip(default);
                }

                T ValidateEquiZip(T value)
                {
                    if (errorSelector != null && disposed > 0 && disposed < call)
                        throw errorSelector(new IEnumerator[]{ e1, e2, e3, e4 });

                    return value;
                }
            }
        }
    }
}
