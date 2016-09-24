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
using System;
using System.Collections.Generic;
using System.Linq;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class AtMostTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AtMostWithNullSequence()
        {
            IEnumerable<int> sequence = null;
            sequence.AtMost(1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AtMostWithNegativeCount()
        {
            new[] { 1 }.AtMost(-1);
        }

        private static IEnumerable<int> GetSequence()
        {
            return new InfiniteSequence<int>(0);
        }
        [Test]
        public void AtMostWithEmptySequenceHasAtMostZeroElements()
        {
            Assert.IsTrue(GetEmptySequence().AtMost(0));
        }
        [Test]
        public void AtMostWithEmptySequenceHasAtMostOneElement()
        {
            Assert.IsTrue(GetEmptySequence().AtMost(1));
        }
        private static IEnumerable<int> GetEmptySequence()
        {
            return LinqEnumerable.Empty<int>();
        }

        [Test]
        public void AtMostWithSingleElementHasAtMostZeroElements()
        {
            Assert.IsFalse(GetSingleElementSequence().AtMost(0));
        }
        [Test]
        public void AtMostWithSingleElementHasAtMostOneElement()
        {
            Assert.IsTrue(GetSingleElementSequence().AtMost(1));
        }
        [Test]
        public void AtMostWithSingleElementHasAtMostManyElements()
        {
            Assert.IsTrue(GetSingleElementSequence().AtMost(2));
        }
        private static IEnumerable<int> GetSingleElementSequence()
        {
            return GetSequence().Take(1);
        }

        [Test]
        public void AtMostWithManyElementsHasAtMostOneElements()
        {
            Assert.IsFalse(GetManyElementSequence().AtMost(1));
        }

        private static IEnumerable<int> GetManyElementSequence()
        {
            return GetSequence().Take(3);
        }
    }
}
