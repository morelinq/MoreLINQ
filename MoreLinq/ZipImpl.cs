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
            Func<IEnumerator[], Exception> errorSelector = null)
        {
            IEnumerator<T1> e1;
            IEnumerator<T2> e2;
            IEnumerator<T3> e3;
            IEnumerator<T4> e4;
            var disposals = 0;
            int calls;

            using (e1 = s1 .GetEnumerator())
            using (e2 = s2 .GetEnumerator())
            using (e3 = s3?.GetEnumerator())
            using (e4 = s4?.GetEnumerator())
            {
                while (true)
                {
                    calls = 0;
                    var v1 = GetValue(ref e1);
                    var v2 = GetValue(ref e2);
                    var v3 = GetValue(ref e3);
                    var v4 = GetValue(ref e4);

                    if (disposals <= limit)
                        yield return resultSelector(v1, v2, v3, v4);
                    else
                        yield break;
                }
            }

            T GetValue<T>(ref IEnumerator<T> e)
            {
                calls++;
                if (e == null || disposals > limit)
                {
                    return default;
                }

                T value;
                if (e.MoveNext())
                {
                    value = e.Current;
                }
                else
                {
                    e.Dispose();
                    e = null;
                    disposals++;
                    value = default;
                }

                if (errorSelector != null && disposals > 0 && disposals < calls)
                    throw errorSelector(new IEnumerator[] { e1, e2, e3, e4 });

                return value;
            }
        }
    }
}
