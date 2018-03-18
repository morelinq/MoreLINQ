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
            Action<IEnumerator[]> validation = null
            )
        {
            var t1 = (e: s1?.GetEnumerator(), disposed: false);
            var t2 = (e: s2?.GetEnumerator(), disposed: false);
            var t3 = (e: s3?.GetEnumerator(), disposed: false);
            var t4 = (e: s4?.GetEnumerator(), disposed: false);
            var disposed = 0; 

            try
            {
                while (true)
                {
                    var v1 = GetValue(ref t1);
                    var v2 = GetValue(ref t2);
                    var v3 = GetValue(ref t3);
                    var v4 = GetValue(ref t4);

                    if (disposed <= limit)
                    {
                        if (validation != null && disposed != 0)
                            validation(new IEnumerator[]{ t1.e, t2.e, t3.e, t4.e });

                        yield return resultSelector(v1, v2, v3, v4);
                    }
                    else
                        yield break;
                }
            }
            finally
            {
                t1.e?.Dispose();
                t2.e?.Dispose();
                t3.e?.Dispose();
                t4.e?.Dispose();
            }

            T GetValue<T>(ref (IEnumerator<T>, bool) t)
            {
                if (t.Item1 == null || disposed > limit)
                {
                    return default;
                }
                else if (!t.Item2 && t.Item1.MoveNext())
                {
                    return t.Item1.Current;
                }
                else
                {
                    if (!t.Item2)
                    {
                        t.Item1.Dispose();
                        t.Item1 = null;
                        t.Item2 = true;
                        disposed++;
                    }
                    return default;
                }
            }
        }
    }
}
