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

    [TestFixture]
    public class ExactlyTest
    {
        [Test]
        public void ExactlyWithNegativeCount()
        {
            AssertThrowsArgument.OutOfRangeException("count", () =>
                new[] { 1 }.Exactly(-1));
        }

        [Test]
        public void ExactlyWithEmptySequenceHasExactlyZeroElements()
        {
            foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionTestCases())
                Assert.IsTrue(xs.Exactly(0));
        }

        [Test]
        public void ExactlyWithEmptySequenceHasExactlyOneElement()
        {
            foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionTestCases())
                Assert.IsFalse(xs.Exactly(1));
        }

        [Test]
        public void ExactlyWithSingleElementHasExactlyOneElements()
        {
            foreach (var xs in new[] { 1 }.ArrangeCollectionTestCases())
                Assert.IsTrue(xs.Exactly(1));
        }

        [Test]
        public void ExactlyWithManyElementHasExactlyOneElement()
        {
            foreach (var xs in new[] { 1, 2, 3 }.ArrangeCollectionTestCases())
                Assert.IsFalse(xs.Exactly(1));
        }

        [Test]
        public void ExactlyDoesNotIterateUnnecessaryElements()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => 2,
                                             () => 3,
                                             () => throw new TestException());
            Assert.IsFalse(source.Exactly(2));
        }
    }
}
