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

using NUnit.Framework;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class ExactlyTest
    {
        [Test]
        public void ExactlyWithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source",
                () => MoreEnumerable.Exactly<int>(null, 1));
        }

        [Test]
        public void ExactlyWithNegativeCount()
        {
            Assert.ThrowsArgumentOutOfRangeException("count", () =>
                new[] { 1 }.Exactly(-1));
        }

        [Test]
        public void ExactlyWithEmptySequenceHasExactlyZeroElements()
        {
            Assert.IsTrue(LinqEnumerable.Empty<int>().Exactly(0));
        }

        [Test]
        public void ExactlyWithEmptySequenceHasExactlyOneElement()
        {
            Assert.IsFalse(LinqEnumerable.Empty<int>().Exactly(1));
        }

        [Test]
        public void ExactlyWithSingleElementHasExactlyOneElements()
        {
            Assert.IsTrue(new[] { 1 }.Exactly(1));
        }

        [Test]
        public void ExactlyWithManyElementHasExactlyOneElement()
        {
            Assert.IsFalse(new[] { 1, 2, 3 }.Exactly(1));
        }
    }
}
