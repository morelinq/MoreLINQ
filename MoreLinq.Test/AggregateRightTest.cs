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
    public class AggregateRightTest
    {
        // Overload 1 Test

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AggregateRight1WithNullSequence()
        {
            (null as IEnumerable<int>).AggregateRight((a, b) => a + b);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AggregateRight1WithNullFunc()
        {
            Enumerable.Range(1, 5).AggregateRight(null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AggregateRight1WithEmptySequence()
        {
            new int[] { }.AggregateRight((a, b) => a + b);
        }

        [Test]
        public void AggregateRight1SimpleTest()
        {
            var numbersAsString = Enumerable.Range(1, 5).Select(x => x.ToString());

            Assert.IsTrue("(1+(2+(3+(4+5))))" == numbersAsString.AggregateRight((a, b) => string.Format("({0}+{1})", a, b)));
        }

        // Overload 2 Test

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AggregateRight2WithNullSequence()
        {
            (null as IEnumerable<int>).AggregateRight(1, (a, b) => a + b);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AggregateRight2WithNullFunc()
        {
            Enumerable.Range(1, 5).AggregateRight(6, null);
        }

        [Test]
        public void AggregateRight2WithEmptySequence()
        {
            Assert.IsTrue(5 == new int[] { }.AggregateRight(5, (a, b) => a + b));
            Assert.IsTrue("c" == new string[] { }.AggregateRight("c", (a, b) => a + b));
        }

        [Test]
        public void AggregateRight2SimpleTest()
        {
            var numbers = Enumerable.Range(1, 4);

            Assert.IsTrue("(1+(2+(3+(4+5))))" == numbers.AggregateRight("5", (a, b) => string.Format("({0}+{1})", a, b)));
        }

        // Overload 3 Test

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AggregateRight3WithNullSequence()
        {
            (null as IEnumerable<int>).AggregateRight(1, (a, b) => a + b, a => a % 2 == 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AggregateRight3WithNullFunc()
        {
            Enumerable.Range(1, 5).AggregateRight(6, null, a => a % 2 == 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AggregateRight3WithNullResultSelector()
        {
            Enumerable.Range(1, 5).AggregateRight(6, (a, b) => a + b, (Func<int, bool>) null);
        }

        [Test]
        public void AggregateRight3WithEmptySequence()
        {
            Assert.IsFalse(new int[] { }.AggregateRight(5, (a, b) => a + b, a => a % 2 == 0));
            Assert.IsTrue(new string[] { }.AggregateRight("ab", (a, b) => a + b, a => a.Length % 2 == 0));
        }

        [Test]
        public void AggregateRight3SimpleTest()
        {
            var numbers = Enumerable.Range(1, 4);

            Assert.IsTrue("(1+(2+(3+(4+5))))".Length == numbers.AggregateRight("5", (a, b) => string.Format("({0}+{1})", a, b), a => a.Length));
        }
    }
}
