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

using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MoreLinq.Test
{
    [TestFixture]
    public class StartsWithTest
    {
        [TestCase(null, null)]
        [TestCase(null, new[] {1})]
        public void StartsWithThrowsIfFirstIsNull(IEnumerable<int> first, IEnumerable<int> second)
        {
            Assert.ThrowsArgumentNullException("first", () =>
                first.StartsWith(second));
        }

        [TestCase(new[] {1}, null)]
        public void StartsWithThrowsIfSecondAIsNull(IEnumerable<int> first, IEnumerable<int> second)
        {
            Assert.ThrowsArgumentNullException("second", () =>
                first.StartsWith(second));
        }

        [TestCase(new[] {1, 2, 3}, new[] {1, 2}, ExpectedResult = true)]
        [TestCase(new[] {1, 2, 3}, new[] {1, 2, 3}, ExpectedResult = true)]
        [TestCase(new[] {1, 2, 3}, new[] {1, 2, 3, 4}, ExpectedResult = false)]
        public bool StartsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second)
        {
            return first.StartsWith(second);
        }

        [TestCase(new[] {'1', '2', '3'}, new[] {'1', '2'}, ExpectedResult = true)]
        [TestCase(new[] {'1', '2', '3'}, new[] {'1', '2', '3'}, ExpectedResult = true)]
        [TestCase(new[] {'1', '2', '3'}, new[] {'1', '2', '3', '4'}, ExpectedResult = false)]
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
            Assert.True(new int[0].StartsWith(new int[0]));
        }

        [Test]
        public void StartsWithReturnsFalseIfOnlyFirstIsEmpty()
        {
            Assert.False(new int[0].StartsWith(new[] {1,2,3}));
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
            using (var first = TestingSequence.Of(1,2,3))
            using (var second = TestingSequence.Of(1))
            {
                first.StartsWith(second);
            }
        }

        [Test]
        [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
        public void StartsWithUsesSpecifiedEqualityComparerOrDefault()
        {
            var first = new[] {1,2,3};
            var second = new[] {4,5,6};

            Assert.False(first.StartsWith(second));
            Assert.False(first.StartsWith(second, null));
            Assert.False(first.StartsWith(second, new EqualityComparerFunc<int>((f, s) => false)));
            Assert.True(first.StartsWith(second, new EqualityComparerFunc<int>((f, s) => true)));
        }
    }
}