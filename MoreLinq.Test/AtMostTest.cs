#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Leandro F. Vieira (leandromoh). All rights reserved.
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
    using System.Collections.Generic;

    [TestFixture]
    public class AtMostTest
    {
        [Test]
        public void AtMostWithNegativeCount()
        {
            AssertThrowsArgument.OutOfRangeException("count",
                () => new[] { 1 }.AtMost(-1));
        }

        [TestCaseSource(nameof(AtMostSource))]
        public bool AtMost(SourceKind sourceKind, int sequenceSize, int atMostAssertCount) =>
            Enumerable.Range(0, sequenceSize).ToSourceKind(sourceKind).AtMost(atMostAssertCount);

        public static IEnumerable<TestCaseData> AtMostSource => CountMethodsTestGenerator.GetTestCases(
            new[] {
                (0, 0),
                (0, 1),
                (1, 0),
                (1, 1),
                (1, 2),
                (3, 1)
            },
            (size, comparedTo) => size <= comparedTo
        );

        [Test]
        public void AtMostDoesNotIterateUnnecessaryElements()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => 2,
                                             () => 3,
                                             () => throw new TestException());
            Assert.IsFalse(source.AtMost(2));
        }
    }
}
