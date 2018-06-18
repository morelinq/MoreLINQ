#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Avi Levin. All rights reserved.
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

namespace MoreLinq.Test
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    static class CountMethodsTestGenerator
    {
        public static IEnumerable<TestCaseData> GetTestCases(
            IEnumerable<(int Size, int ComparedTo)> counts,
            Func<int, int, bool> expectedResultCalculator)
        {
            return
                from count in counts
                from type in new[] { SourceKind.Sequence, SourceKind.BreakingCollection, SourceKind.BreakingReadOnlyCollection }
                select new TestCaseData(type, count.Size, count.ComparedTo)
                    .Returns(expectedResultCalculator(count.Size, count.ComparedTo))
                    .SetName($"{{m}}({type}[{count.Size}], {count.ComparedTo})");
        }
    }
}
