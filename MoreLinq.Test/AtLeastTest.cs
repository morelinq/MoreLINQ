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
    using System.Collections.Generic;

    [TestFixture]
    public class AtLeastTest
    {
        [Test]
        public void AtLeastWithNegativeCount()
        {
            AssertThrowsArgument.OutOfRangeException("count", () =>
                new[] { 1 }.AtLeast(-1));
        }

        public static IEnumerable<TestCaseData> AtLeastSource => CountMethodsTestGenerator.GetTestCases(
            MoreEnumerable.Cartesian(
                new[] { 0, 1, 3 },
                new[] { 0, 1, 2 },
                (size, comparedTo) => (size, comparedTo)
            ),
            (size, comparedTo) => size >= comparedTo
        );

        [TestCaseSource(nameof(AtLeastSource))]
        public bool AtLeast(SourceKind sourceKind, int sequenceSize, int atLeastAssertCount) =>
            Enumerable.Range(0, sequenceSize).ToSourceKind(sourceKind).AtLeast(atLeastAssertCount);

        [Test]
        public void AtLeastDoesNotIterateUnnecessaryElements()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => 2,
                                             () => throw new TestException());
            Assert.IsTrue(source.AtLeast(2));
        }
    }
}
