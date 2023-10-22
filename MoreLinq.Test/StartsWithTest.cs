#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Andreas Gullberg Larsen (angularsen). All rights reserved.
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
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    [TestFixture]
    public class StartsWithTest
    {
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2 }, ExpectedResult = true)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, ExpectedResult = true)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4 }, ExpectedResult = false)]
        public bool StartsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second)
        {
            return first.StartsWith(second);
        }

        [TestCase(new[] { '1', '2', '3' }, new[] { '1', '2' }, ExpectedResult = true)]
        [TestCase(new[] { '1', '2', '3' }, new[] { '1', '2', '3' }, ExpectedResult = true)]
        [TestCase(new[] { '1', '2', '3' }, new[] { '1', '2', '3', '4' }, ExpectedResult = false)]
        public bool StartsWithWithChars(IEnumerable<char> first, IEnumerable<char> second)
        {
            return first.StartsWith(second);
        }

        [TestCase("123", "12", ExpectedResult = true)]
        [TestCase("123", "123", ExpectedResult = true)]
        [TestCase("123", "1234", ExpectedResult = false)]
        public bool StartsWithWithStrings(string first, string second)
        {
            // Conflict with String.StartsWith(), which has precedence in this case
            return MoreEnumerable.StartsWith(first, second);
        }

        [Test]
        public void StartsWithReturnsTrueIfBothEmpty()
        {
            Assert.That(new int[0].StartsWith(new int[0]), Is.True);
        }

        [Test]
        public void StartsWithReturnsFalseIfOnlyFirstIsEmpty()
        {
            Assert.That(new int[0].StartsWith(new[] { 1, 2, 3 }), Is.False);
        }

        [TestCase("", "", ExpectedResult = true)]
        [TestCase("1", "", ExpectedResult = true)]
        public bool StartsWithReturnsTrueIfSecondIsEmpty(string first, string second)
        {
            // Conflict with String.StartsWith(), which has precedence in this case
            return MoreEnumerable.StartsWith(first, second);
        }

        [Test]
        public void StartsWithDisposesBothSequenceEnumerators()
        {
            using var first = TestingSequence.Of(1, 2, 3);
            using var second = TestingSequence.Of(1);

            _ = first.StartsWith(second);
        }

        [Test]
        [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
        public void StartsWithUsesSpecifiedEqualityComparerOrDefault()
        {
            var first = new[] { 1, 2, 3 };
            var second = new[] { 4, 5, 6 };

            Assert.That(first.StartsWith(second), Is.False);
            Assert.That(first.StartsWith(second, null), Is.False);
            Assert.That(first.StartsWith(second, EqualityComparer.Create<int>(delegate { return false; })), Is.False);
            Assert.That(first.StartsWith(second, EqualityComparer.Create<int>(delegate { return true; })), Is.True);
        }

        [TestCase(SourceKind.BreakingCollection)]
        [TestCase(SourceKind.BreakingReadOnlyCollection)]
        public void StartsWithUsesCollectionsCountToAvoidUnnecessaryIteration(SourceKind sourceKind)
        {
            var first = new[] { 1, 2 }.ToSourceKind(sourceKind);
            var second = new[] { 1, 2, 3 }.ToSourceKind(sourceKind);

            Assert.That(first.StartsWith(second), Is.False);
        }
    }
}
