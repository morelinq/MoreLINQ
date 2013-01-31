#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
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
    using System.Diagnostics;

    static partial class MoreEnumerable
    {
        static TResult FoldImpl<T, TResult>(IEnumerable<T> source, int count, 
            Func<T, TResult> folder1,
            Func<T, T, TResult> folder2,
            Func<T, T, T, TResult> folder3,
            Func<T, T, T, T, TResult> folder4)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (   count == 1 && folder1 == null
                || count == 2 && folder2 == null
                || count == 3 && folder3 == null
                || count == 4 && folder4 == null)
            {                                                // ReSharper disable NotResolvedInText
                throw new ArgumentNullException("folder");   // ReSharper restore NotResolvedInText
            }

            using (var e = source.GetEnumerator())
            {
                T[] elements = null;
                for (var i = 0; i < count; i++)
                {
                    if (!e.MoveNext()) 
                        throw new Exception(string.Format(@"Sequence has fewer than {0:N0} element(s).", count));
                    (elements = elements??new T[count])[i] = e.Current;
                }
                
                if (e.MoveNext()) 
                    throw new Exception(string.Format(@"Sequence has more than {0:N0} element(s).", count));
                
                Debug.Assert(elements != null);
                
                switch (count)
                {
                    case 1: return folder1(elements[0]);
                    case 2: return folder2(elements[0], elements[1]);
                    case 3: return folder3(elements[0], elements[1], elements[2]);
                    case 4: return folder4(elements[0], elements[1], elements[2], elements[3]);
                    default: throw new NotSupportedException();
                }
            }
        }
    }
}
