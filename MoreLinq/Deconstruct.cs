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
    using System.Collections.Generic;
    using System.Diagnostics;

    static partial class MoreEnumerable
    {
        static T ReadWithCount<T>(this IEnumerator<T> enumerator, ref int count, ref bool ended)
        {
            Debug.Assert(enumerator != null);

            if (ended || (ended = !enumerator.MoveNext()))
                return default;

            count++;
            return enumerator.Current;
        }
    }
}
