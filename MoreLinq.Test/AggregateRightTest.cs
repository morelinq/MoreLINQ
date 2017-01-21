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

namespace MoreLinq.Test
{
    [TestFixture]
    public class AggregateRightTest
    {
        // Overload 1 Test

        [Test]
        public void AggregateRight1WithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source", 
                () => MoreEnumerable.AggregateRight<int>(null, (a, b) => a + b));
        }

        [Test]
        public void AggregateRight1WithNullFunc()
        {
            Assert.ThrowsArgumentNullException("func", 
                () => Enumerable.Range(1, 5).AggregateRight(null));
        }

        [Test]
        public void AggregateRight1WithEmptySequence()
        {
            Assert.Throws<InvalidOperationException>(
                () => new int[] { }.AggregateRight((a, b) => a + b));
        }

        [Test]
        public void AggregateRight1SimpleTest()
        {
            var numbersAsString = Enumerable.Range(1, 5).Select(x => x.ToString());

            Assert.That(numbersAsString.AggregateRight((a, b) => string.Format("({0}+{1})", a, b)), Is.EqualTo("(1+(2+(3+(4+5))))"));
        }

        // Overload 2 Test

        [Test]
        public void AggregateRight2WithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source",
                () => MoreEnumerable.AggregateRight<int, int>(null, 1, (a, b) => a + b));
        }

        [Test]
        public void AggregateRight2WithNullFunc()
        {
            Assert.ThrowsArgumentNullException("func",
                () => Enumerable.Range(1, 5).AggregateRight(6, null));
        }

        [Test]
        public void AggregateRight2WithEmptySequence()
        {
            Assert.That(new int[] { }.AggregateRight(5, (a, b) => a + b), Is.EqualTo(5));
            Assert.That(new string[] { }.AggregateRight("c", (a, b) => a + b), Is.EqualTo("c"));
        }

        [Test]
        public void AggregateRight2SimpleTest()
        {
            Assert.That(Enumerable.Range(1, 4).AggregateRight("5", (a, b) => string.Format("({0}+{1})", a, b)), Is.EqualTo("(1+(2+(3+(4+5))))"));
        }

        // Overload 3 Test

        [Test]
        public void AggregateRight3WithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source",
                () => MoreEnumerable.AggregateRight<int, int, bool>(null, 1, (a, b) => a + b, a => a % 2 == 0));
        }

        [Test]
        public void AggregateRight3WithNullFunc()
        {
            Assert.ThrowsArgumentNullException("func",
                () => Enumerable.Range(1, 5).AggregateRight(6, null, a => a % 2 == 0));
        }

        [Test]
        public void AggregateRight3WithNullResultSelector()
        {
            Assert.ThrowsArgumentNullException("resultSelector",
                () => Enumerable.Range(1, 5).AggregateRight(6, (a, b) => a + b, (Func<int, bool>)null));
        }

        [Test]
        public void AggregateRight3WithEmptySequence()
        {
            Assert.That(new int[] { }.AggregateRight(5, (a, b) => a + b, a => a == 5), Is.EqualTo(true));
            Assert.That(new string[] { }.AggregateRight("ab", (a, b) => a + b, a => a.Length == 2), Is.EqualTo(true));
        }

        [Test]
        public void AggregateRight3SimpleTest()
        {
            Assert.That(Enumerable.Range(1, 4).AggregateRight("5", (a, b) => string.Format("({0}+{1})", a, b), a => a.Length), Is.EqualTo("(1+(2+(3+(4+5))))".Length));
        }
    }
}
