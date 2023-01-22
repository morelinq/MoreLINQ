#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Pierre Lando. All rights reserved.
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
    using System.Collections.Generic;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;

    [TestFixture]
    public class OrderedMergeTest
    {
        static IEnumerable<T> Seq<T>(params T[] values) => values;

        public static readonly IEnumerable<ITestCaseData> TestData =
            from e in new[]
            {
                new
                {
                    A = Seq(0, 2, 4),
                    B = Seq(0, 1, 2, 3, 4),
                    R = Seq(0, 1, 2, 3, 4)
                },
                new
                {
                    A = Seq(0, 1, 2, 3, 4),
                    B = Seq(0, 2, 4),
                    R = Seq(0, 1, 2, 3, 4)
                },
                new
                {
                    A = Seq(0, 2, 4),
                    B = Seq(1, 3, 5),
                    R = Seq(0, 1, 2, 3, 4, 5)
                },
                new
                {
                    A = Seq<int>(),
                    B = Seq(0, 1, 2),
                    R = Seq(0, 1, 2)
                },
                new
                {
                    A = Seq(0, 1, 2),
                    B = Seq<int>(),
                    R = Seq(0, 1, 2)
                }
            }
            select new TestCaseData(e.A.AsTestingSequence(), e.B.AsTestingSequence()).Returns(e.R);

        [Test, TestCaseSource(nameof(TestData))]
        public IEnumerable<int> OrderedMerge(IEnumerable<int> first,
            IEnumerable<int> second)
        {
            return first.OrderedMerge(second);
        }
    }
}
