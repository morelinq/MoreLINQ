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
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    [TestFixture]
    public class EndsWithTest
    {
        [TestCase(new[] {1, 2, 3}, new[] {2, 3}, ExpectedResult = true)]
        [TestCase(new[] {1, 2, 3}, new[] {1, 2, 3}, ExpectedResult = true)]
        [TestCase(new[] {1, 2, 3}, new[] {0, 1, 2, 3}, ExpectedResult = false)]
        public bool EndsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second)
        {
            return first.EndsWith(second);
        }

        [TestCase(new[] {'1', '2', '3'}, new[] {'2', '3'}, ExpectedResult = true)]
        [TestCase(new[] {'1', '2', '3'}, new[] {'1', '2', '3'}, ExpectedResult = true)]
        [TestCase(new[] {'1', '2', '3'}, new[] {'0', '1', '2', '3'}, ExpectedResult = false)]
        public bool EndsWithWithChars(IEnumerable<char> first, IEnumerable<char> second)
        {
            return first.EndsWith(second);
        }

        [TestCase("123", "23", ExpectedResult = true)]
        [TestCase("123", "123", ExpectedResult = true)]
        [TestCase("123", "0123", ExpectedResult = false)]
        public bool EndsWithWithStrings(string first, string second)
        {
            // Conflict with String.EndsWith(), which has precedence in this case
            return MoreEnumerable.EndsWith(first, second);
        }

        [Test]
        public void EndsWithReturnsTrueIfBothEmpty()
        {
            Assert.True(new int[0].EndsWith(new int[0]));
        }

        [Test]
        public void EndsWithReturnsFalseIfOnlyFirstIsEmpty()
        {
            Assert.False(new int[0].EndsWith(new[] {1,2,3}));
        }

        [TestCase("", "", ExpectedResult = true)]
        [TestCase("1", "", ExpectedResult = true)]
        public bool EndsWithReturnsTrueIfSecondIsEmpty(string first, string second)
        {
            // Conflict with String.EndsWith(), which has precedence in this case
            return MoreEnumerable.EndsWith(first, second);
        }

        [Test]
        public void EndsWithDisposesBothSequenceEnumerators()
        {
            using var first = TestingSequence.Of(1,2,3);
            using var second = TestingSequence.Of(1);

            first.EndsWith(second);
        }

        [Test]
        [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
        public void EndsWithUsesSpecifiedEqualityComparerOrDefault()
        {
            var first = new[] {1,2,3};
            var second = new[] {4,5,6};

            Assert.False(first.EndsWith(second));
            Assert.False(first.EndsWith(second, null));
            Assert.False(first.EndsWith(second, EqualityComparer.Create<int>(delegate { return false; })));
            Assert.True(first.EndsWith(second, EqualityComparer.Create<int>(delegate { return true; })));
        }

        [TestCase(SourceKind.BreakingCollection)]
        [TestCase(SourceKind.BreakingReadOnlyCollection)]
        public void EndsWithUsesCollectionsCountToAvoidUnnecessaryIteration(SourceKind sourceKind)
        {
            var first = new[] { 1, 2 }.ToSourceKind(sourceKind);
            var second = new[] { 1, 2, 3 }.ToSourceKind(sourceKind);

            Assert.False(first.EndsWith(second));
        }
    }
}
