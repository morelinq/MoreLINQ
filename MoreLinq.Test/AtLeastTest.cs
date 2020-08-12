#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2015 "sholland". All rights reserved.
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

    [TestFixture]
    public class AtLeastTest
    {
        [Test]
        public void WithNegativeCount()
        {
            AssertThrowsArgument.OutOfRangeException("count", () =>
                new[] { 1 }.AtLeast(-1));
        }

        [TestCase(0, 0, SourceKind.Sequence                   , ExpectedResult = true )]
        [TestCase(0, 0, SourceKind.BreakingReadOnlyCollection , ExpectedResult = true )]
        [TestCase(0, 0, SourceKind.BreakingCollection         , ExpectedResult = true )]
        [TestCase(0, 1, SourceKind.Sequence                   , ExpectedResult = false)]
        [TestCase(0, 1, SourceKind.BreakingReadOnlyCollection , ExpectedResult = false)]
        [TestCase(0, 1, SourceKind.BreakingCollection         , ExpectedResult = false)]
        [TestCase(0, 2, SourceKind.Sequence                   , ExpectedResult = false)]
        [TestCase(0, 2, SourceKind.BreakingReadOnlyCollection , ExpectedResult = false)]
        [TestCase(0, 2, SourceKind.BreakingCollection         , ExpectedResult = false)]
        [TestCase(1, 0, SourceKind.Sequence                   , ExpectedResult = true )]
        [TestCase(1, 0, SourceKind.BreakingReadOnlyCollection , ExpectedResult = true )]
        [TestCase(1, 0, SourceKind.BreakingCollection         , ExpectedResult = true )]
        [TestCase(1, 1, SourceKind.Sequence                   , ExpectedResult = true )]
        [TestCase(1, 1, SourceKind.BreakingReadOnlyCollection , ExpectedResult = true )]
        [TestCase(1, 1, SourceKind.BreakingCollection         , ExpectedResult = true )]
        [TestCase(1, 2, SourceKind.Sequence                   , ExpectedResult = false)]
        [TestCase(1, 2, SourceKind.BreakingReadOnlyCollection , ExpectedResult = false)]
        [TestCase(1, 2, SourceKind.BreakingCollection         , ExpectedResult = false)]
        [TestCase(3, 0, SourceKind.Sequence                   , ExpectedResult = true )]
        [TestCase(3, 0, SourceKind.BreakingReadOnlyCollection , ExpectedResult = true )]
        [TestCase(3, 0, SourceKind.BreakingCollection         , ExpectedResult = true )]
        [TestCase(3, 1, SourceKind.Sequence                   , ExpectedResult = true )]
        [TestCase(3, 1, SourceKind.BreakingReadOnlyCollection , ExpectedResult = true )]
        [TestCase(3, 1, SourceKind.BreakingCollection         , ExpectedResult = true )]
        [TestCase(3, 2, SourceKind.Sequence                   , ExpectedResult = true )]
        [TestCase(3, 2, SourceKind.BreakingReadOnlyCollection , ExpectedResult = true )]
        [TestCase(3, 2, SourceKind.BreakingCollection         , ExpectedResult = true )]
        public bool Case(int count, int atLeast, SourceKind kind)
        {
            var xs = Enumerable.Range(1, count).ToSourceKind(kind);
            using var _ = xs as TestingSequence<int>;
            return xs.AtLeast(atLeast);
        }

        [Test]
        public void DoesNotIterateUnnecessaryElements()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => 2,
                                             () => throw new TestException());
            Assert.IsTrue(source.AtLeast(2));
        }
    }
}
