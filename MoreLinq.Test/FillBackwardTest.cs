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

namespace MoreLinq.Test
{
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using System.Collections.Generic;

    [TestFixture]
    public class FillBackwardTest
    {
        private static IEnumerable<T> Seq<T>(params T[] values) => values;

        [TestCase(TestName = "FillBackward is lazy")]
        public void FillBackwardIsLazy()
        {
            var source = new BreakingSequence<object>();
            source.FillBackward();
        }

        public static readonly IEnumerable<ITestCaseData> FillBackwardTestData =
            from e in new[]
            {
                new
                {
                    Name = "FillBackward: Input sequence is empty",
                    Source = Enumerable.Empty<int?>(),
                    Expected = Enumerable.Empty<int?>()
                },
                new
                {
                    Name = "FillBackward: Input sequence has no null values",
                    Source = Seq<int?>(1, 2, 3),
                    Expected = Seq<int?>(1, 2, 3)
                },
                new
                {
                    Name = "FillBackward: Input sequence has only nulls but last value",
                    Source = Seq<int?>(null, null, 0),
                    Expected = Seq<int?>(0, 0, 0)
                },
                new
                {
                    Name = "FillBackward: Input sequence ends with nulls",
                    Source = Seq<int?>(null, null, 0, null, null),
                    Expected = Seq<int?>(0, 0, 0, null, null)
                },
                new
                {
                    Name = "FillBackward: Input sequence with distributed nulls",
                    Source = Seq<int?>(0, null, null, 1, 2, null, null, 3, 4, null, null),
                    Expected = Seq<int?>(0, 1, 1, 1, 2, 3, 3, 3, 4, null, null)
                }
            }
            select new TestCaseData(e.Source) {ExpectedResult = e.Expected, TestName = e.Name};

        [TestCaseSource(nameof(FillBackwardTestData))]
        public IEnumerable<int?> FillBackward(IEnumerable<int?> source)
        {
            return source.AsTestingSequence().FillBackward();
        }

        [TestCase(TestName = "FillBackward with predicate is lazy")]
        public void FillBackwardWithPredicateIsLazy()
        {
            var source = new BreakingSequence<object>();
            var predicate = BreakingFunc.Of<object, bool>();
            source.FillBackward(predicate);
        }

        public static readonly IEnumerable<ITestCaseData> FillBackwardWithPredicateTestData =
            from e in new[]
            {
                new
                {
                    Name = "FillBackward, with predicate: Input sequence is empty",
                    Source = Enumerable.Empty<int>(),
                    Expected = Enumerable.Empty<int>()
                },
                new
                {
                    Name = "FillBackward, with predicate: Input sequence has no rejected values",
                    Source = Seq(1, 2, 3),
                    Expected = Seq(1, 2, 3)
                },
                new
                {
                    Name = "FillBackward, with predicate: Input sequence has only rejected values but last value",
                    Source = Seq(-1, -1, 0),
                    Expected = Seq(0, 0, 0)
                },
                new
                {
                    Name = "FillBackward, with predicate: Input sequence ends with rejected values",
                    Source = Seq(-1, -1, 0, -1, -1),
                    Expected = Seq(0, 0, 0, -1, -1)
                },
                new
                {
                    Name = "FillBackward, with predicate: Input sequence with distributed rejected values",
                    Source = Seq(0, -1, -1, 1, 2, -1, -1, 3, 4, -1, -1),
                    Expected = Seq(0, 1, 1, 1, 2, 3, 3, 3, 4, -1, -1)
                }
            }
            select new TestCaseData(e.Source) {ExpectedResult = e.Expected, TestName = e.Name};

        [TestCaseSource(nameof(FillBackwardWithPredicateTestData))]
        public IEnumerable<int> FillBackwardWithPredicate(IEnumerable<int> source)
        {
            return source.AsTestingSequence().FillBackward(v => v < 0);
        }

        [TestCase(TestName = "FillBackward with predicate and fill selector is lazy")]
        public void FillBackwardWithPredicateAndFillSelectorIsLazy()
        {
            var source = new BreakingSequence<object>();
            var predicate = BreakingFunc.Of<object, bool>();
            var fillSelector = BreakingFunc.Of<object, object, object>();
            source.FillBackward(predicate, fillSelector);
        }

        public static readonly IEnumerable<ITestCaseData>
            FillBackwardWithPredicateAndFillSelectorTestData =
                from e in new[]
                {
                    new
                    {
                        Name = "FillBackward, with predicate and fill selector: Input sequence is empty",
                        Source = Enumerable.Empty<int>(),
                        Expected = Enumerable.Empty<int>()
                    },
                    new
                    {
                        Name = "FillBackward, with predicate and fill selector: Input sequence has no rejected values",
                        Source = Seq(1, 2, 3),
                        Expected = Seq(1, 2, 3)
                    },
                    new
                    {
                        Name = "FillBackward, with predicate and fill selector: Input sequence has only rejected values but last value",
                        Source = Seq(-2, -1, 5),
                        Expected = Seq(25, 15, 5)
                    },
                    new
                    {
                        Name = "FillBackward, with predicate and fill selector: Input sequence ends with rejected values",
                        Source = Seq(-2, -1, 5, -1, -1),
                        Expected = Seq(25, 15, 5, -1, -1)
                    },
                    new
                    {
                        Name = "FillBackward, with predicate and fill selector: Input sequence with distributed rejected values",
                        Source = Seq(0, -2, -1, 1, 2, -2, -1, 3, 4, -1, -1),
                        Expected = Seq(0, 21, 11, 1, 2, 23, 13, 3, 4, -1, -1)
                    }
                }
                select new TestCaseData(e.Source) {ExpectedResult = e.Expected, TestName = e.Name};

        [TestCaseSource(nameof(FillBackwardWithPredicateAndFillSelectorTestData))]
        public IEnumerable<int> FillBackwardWithPredicateAndFillSelector(IEnumerable<int> source)
        {
            static bool Predicate(int v) => v < 0;
            static int FillSelector(int missing, int nonMissing) => nonMissing - 10 * missing;
            return source.AsTestingSequence().FillBackward(Predicate, FillSelector);
        }
    }
}
