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
    public class ExactlyTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExactlyWithNullSequence()
        {
            IEnumerable<int> sequence = null;
            sequence.Exactly(1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExactlyWithNegativeCount()
        {
            new[] { 1 }.Exactly(-1);
        }

        private static IEnumerable<int> GetSequence()
        {
            return new InfiniteSequence<int>(0);
        }
        [Test]
        public void ExactlyWithEmptySequenceHasExactlyZeroElements()
        {
            Assert.IsTrue(GetEmptySequence().Exactly(0));
        }
        private static IEnumerable<int> GetEmptySequence()
        {
            return LinqEnumerable.Empty<int>();
        }
        [Test]
        public void ExactlyWithEmptySequenceHasExactlyOneElement()
        {
            Assert.IsFalse(GetEmptySequence().Exactly(1));
        }
        [Test]
        public void ExactlyWithSingleElementHasExactlyOneElements()
        {
            Assert.IsTrue(GetSingleElementSequence().Exactly(1));
        }
        [Test]
        public void ExactlyWithManyElementHasExactlyOneElement()
        {
            Assert.IsFalse(GetManyElementSequence().Exactly(1));
        }
        private static IEnumerable<int> GetSingleElementSequence()
        {
            return GetSequence().Take(1);
        }

        private static IEnumerable<int> GetManyElementSequence()
        {
            return GetSequence().Take(3);
        }
    }
}
