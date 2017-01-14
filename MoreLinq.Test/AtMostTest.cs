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
    public class AtMostTest
    {
        [Test]
        public void AtMostWithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source",
                () => MoreEnumerable.AtMost<int>(null, 1));
        }

        [Test]
        public void AtMostWithNegativeCount()
        {
            Assert.ThrowsArgumentOutOfRangeException("count",
                () => new[] { 1 }.AtMost(-1));
        }

        [Test]
        public void AtMostWithEmptySequenceHasAtMostZeroElements()
        {
            Assert.IsTrue(LinqEnumerable.Empty<int>().AtMost(0));
        }

        [Test]
        public void AtMostWithEmptySequenceHasAtMostOneElement()
        {
            Assert.IsTrue(LinqEnumerable.Empty<int>().AtMost(1));
        }

        [Test]
        public void AtMostWithSingleElementHasAtMostZeroElements()
        {
            Assert.IsFalse(new[] { 1 }.AtMost(0));
        }

        [Test]
        public void AtMostWithSingleElementHasAtMostOneElement()
        {
            Assert.IsTrue(new[] { 1 }.AtMost(1));
        }

        [Test]
        public void AtMostWithSingleElementHasAtMostManyElements()
        {
            Assert.IsTrue(new[] { 1 }.AtMost(2));
        }

        [Test]
        public void AtMostWithManyElementsHasAtMostOneElements()
        {
            Assert.IsFalse(new[] { 1, 2, 3 }.AtMost(1));
        }
    }
}
