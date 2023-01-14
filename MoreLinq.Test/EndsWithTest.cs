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
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class EndsWithTest
    {
        [TestCase(new[] {1, 2, 3}, new[] {2, 3}, ExpectedResult = true)]
        [TestCase(new[] {1, 2, 3}, new[] {1, 2, 3}, ExpectedResult = true)]
        [TestCase(new[] {1, 2, 3}, new[] {0, 1, 2, 3}, ExpectedResult = false)]
        public bool EndsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second)
        {
            using var fts = first.AsTestingSequence();
            using var sts = second.AsTestingSequence();
            return fts.EndsWith(sts);
        }

        [TestCase(new[] {'1', '2', '3'}, new[] {'2', '3'}, ExpectedResult = true)]
        [TestCase(new[] {'1', '2', '3'}, new[] {'1', '2', '3'}, ExpectedResult = true)]
        [TestCase(new[] {'1', '2', '3'}, new[] {'0', '1', '2', '3'}, ExpectedResult = false)]
        public bool EndsWithWithChars(IEnumerable<char> first, IEnumerable<char> second)
        {
            using var fts = first.AsTestingSequence();
            using var sts = second.AsTestingSequence();
            return fts.EndsWith(sts);
        }

        [TestCase("123", "23", ExpectedResult = true)]
        [TestCase("123", "123", ExpectedResult = true)]
        [TestCase("123", "0123", ExpectedResult = false)]
        public bool EndsWithWithStrings(string first, string second)
        {
            using var fts = first.AsTestingSequence();
            using var sts = second.AsTestingSequence();
            return fts.EndsWith(sts);
        }

        [Test]
        public void EndsWithReturnsTrueIfBothEmpty()
        {
            using var fts = TestingSequence.Of<int>();
            using var sts = TestingSequence.Of<int>();
            Assert.That(fts.EndsWith(sts), Is.True);
        }

        [Test]
        public void EndsWithReturnsFalseIfOnlyFirstIsEmpty()
        {
            using var fts = TestingSequence.Of<int>();
            using var sts = TestingSequence.Of(1, 2, 3);
            Assert.That(fts.EndsWith(sts), Is.False);
        }

        [TestCase("", "", ExpectedResult = true)]
        [TestCase("1", "", ExpectedResult = true)]
        public bool EndsWithReturnsTrueIfSecondIsEmpty(string first, string second)
        {
            using var fts = first.AsTestingSequence();
            using var sts = second.AsTestingSequence();
            return fts.EndsWith(sts);
        }

        [Test]
        public void EndsWithDisposesBothSequenceEnumerators()
        {
            using var first = TestingSequence.Of(1, 2, 3);
            using var second = TestingSequence.Of(1);

            first.EndsWith(second);
        }

        [Test]
        public void EndsWithUsesDefaultEqualityComparerByDefault()
        {
            using var first = TestingSequence.Of(1, 2, 3);
            using var second = TestingSequence.Of(4, 5, 6);

            Assert.That(first.EndsWith(second), Is.False);
        }

        [Test]
        public void EndsWithUsesDefaultEqualityComparerWhenNullSpecified()
        {
            using var first = TestingSequence.Of(1, 2, 3);
            using var second = TestingSequence.Of(4, 5, 6);

            Assert.That(first.EndsWith(second, null), Is.False);
        }

        [Test]
        [TestCase(false, ExpectedResult = false)]
        [TestCase(true, ExpectedResult = true)]
        public bool EndsWithUsesSpecifiedEqualityComparer(bool result)
        {
            using var first = TestingSequence.Of(1, 2, 3);
            using var second = TestingSequence.Of(4, 5, 6);
            return first.EndsWith(second, EqualityComparer.Create<int>((_, _) => result));
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
