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
        static T[] Fold<T>(this IEnumerable<T> source, int count)
        {
            var elements = new T[count];
            foreach (var e in source.AssertCount(count, static (cmp, count) => new InvalidOperationException(FormatSequenceLengthErrorMessage(cmp, count))).Index())
                elements[e.Key] = e.Value;

            return elements;
        }
    }
}
