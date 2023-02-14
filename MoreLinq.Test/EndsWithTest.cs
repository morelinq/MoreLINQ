#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Andreas Gullberg Larsen. All rights reserved.
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
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture(SourceKind.Sequence, SourceKind.Sequence)]
    [TestFixture(SourceKind.Sequence, SourceKind.Collection)]
    [TestFixture(SourceKind.Sequence, SourceKind.List)]
    [TestFixture(SourceKind.Collection, SourceKind.Sequence)]
    [TestFixture(SourceKind.Collection, SourceKind.Collection)]
    [TestFixture(SourceKind.Collection, SourceKind.List)]
    [TestFixture(SourceKind.List, SourceKind.Sequence)]
    [TestFixture(SourceKind.List, SourceKind.Collection)]
    [TestFixture(SourceKind.List, SourceKind.List)]
    public class EndsWithTest
    {
        readonly SourceKind _firstSourceKind;
        readonly SourceKind _secondSourceKind;

        public EndsWithTest(SourceKind firstSourceKind, SourceKind secondSourceKind)
        {
            _firstSourceKind = firstSourceKind;
            _secondSourceKind = secondSourceKind;
        }

        [TestCase(new int[0], new int[0], ExpectedResult = true)]
        [TestCase(new int[0], new[] { 1, 2, 3 }, ExpectedResult = false)]
        [TestCase(new[] { 1, 2, 3 }, new int[0], ExpectedResult = true)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 2, 3 }, ExpectedResult = true)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, ExpectedResult = true)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 0, 1, 2, 3 }, ExpectedResult = false)]
        public bool EndsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second)
        {
            return first.ToSourceKind(_firstSourceKind).EndsWith(second.ToSourceKind(_secondSourceKind));
        }

        [TestCase("", "", ExpectedResult = true)]
        [TestCase("", "1", ExpectedResult = false)]
        [TestCase("1", "", ExpectedResult = true)]
        [TestCase("123", "23", ExpectedResult = true)]
        [TestCase("123", "123", ExpectedResult = true)]
        [TestCase("123", "0123", ExpectedResult = false)]
        public bool EndsWithWithStrings(string first, string second)
        {
            return first.ToSourceKind(_firstSourceKind).EndsWith(second.ToSourceKind(_secondSourceKind));
        }

        [Test]
        public void EndsWithDisposesBothSequenceEnumerators()
        {
            using var first = TestingSequence.Of(1, 2, 3);
            using var second = TestingSequence.Of(1);

            _ = first.EndsWith(second);
        }

        [TestCaseSource(nameof(EndsWithUsesSpecifiedEqualityComparerOrDefaultTestCases))]
        public bool EndsWithUsesSpecifiedEqualityComparerOrDefault(IEqualityComparer<int>? equalityComparer)
        {
            var first = new[] { 1, 2, 3 }.ToSourceKind(_firstSourceKind);
            var second = new[] { 4, 5, 6 }.ToSourceKind(_secondSourceKind);

            return first.EndsWith(second, equalityComparer);
        }

        public static IEnumerable EndsWithUsesSpecifiedEqualityComparerOrDefaultTestCases
        {
            get
            {
                yield return new TestCaseData(null).Returns(false);
                yield return new TestCaseData(EqualityComparer.Create<int>(delegate { return false; })).Returns(false);
                yield return new TestCaseData(EqualityComparer.Create<int>(delegate { return true; })).Returns(true);
            }
        }

        [TestCase(SourceKind.BreakingCollection)]
        [TestCase(SourceKind.BreakingReadOnlyCollection)]
        public void EndsWithUsesCollectionsCountToAvoidUnnecessaryIteration(SourceKind sourceKind)
        {
            var first = new[] { 1, 2 }.ToSourceKind(sourceKind);
            var second = new[] { 1, 2, 3 }.ToSourceKind(sourceKind);

            Assert.That(first.EndsWith(second), Is.False);
        }
    }
}
