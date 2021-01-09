#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2021 Atif Aziz. All rights reserved.
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

    [TestFixture]
    public class StepTest
    {
        static readonly IEnumerable<TestCaseData> TestData = new []
        {
            new TestCaseData(new int[0], new[] { 1, 1 })
            {
                ExpectedResult = new(bool, int)[0]
            },
            new TestCaseData(new int[0], new[] { 0, 0 })
            {
                ExpectedResult = new(bool, int)[0]
            },
            new TestCaseData(new[] { 42 }, new[] { 0, 0, 1 })
            {
                ExpectedResult = new[] { (42, 1), (42, 0), (42, 0) }
            },
            new TestCaseData(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 1, 1, 1, 1 })
            {
                ExpectedResult = new[] { (1, 1), (2, 1), (3, 1), (4, 1), (5, 1) }
            },
            new TestCaseData(new[] { 1, 2, 3, 4, 5 }, new[] { 2, 2, 2, 2, 2 })
            {
                ExpectedResult = new[] { (1, 1), (3, 2), (5, 2) }
            },
            new TestCaseData(new[] { 1, 2, 3, 4, 5 }, new[] { 10 })
            {
                ExpectedResult = new[] { (1, 1) }
            },
            new TestCaseData(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 1 })
            {
                ExpectedResult = new[] { (1, 1), (2, 1), (3, 1) }
            },
            new TestCaseData(new[] { 1, 2, 3 }, new[] { 1, 1, 1, 1, 1 })
            {
                ExpectedResult = new[] { (1, 1), (2, 1), (3, 1) }
            },
            new TestCaseData(new[] { 42 }, new[] { 0, 0, 0 })
            {
                ExpectedResult = new[] { (42, 1), (42, 0), (42, 0), (42, 0) }
            },
            new TestCaseData(new[] { 1, 2, 3 }, new[] { 1, 0, 1, 0, 1, 0 })
            {
                ExpectedResult = new[] { (1, 1), (2, 1), (2, 0), (3, 1), (3, 0) }
            },
            new TestCaseData(new[] { 1, 2, 3 }, new[] { 1, -1, 1, -1, 1, -1 })
            {
                ExpectedResult = new[] { (1, 1), (2, 1), (2, 0), (3, 1), (3, 0) }
            }
        };

        [TestCaseSource(nameof(TestData))]
        public object Step(int[] xs, int[] steps)
        {
            using var source = xs.AsTestingSequence();
            using var ts = steps.AsTestingSequence();
            return source.Step(ts).ToArray();
        }
    }
}
