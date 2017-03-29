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
        public void AggregateRightWithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source",
                () => MoreEnumerable.AggregateRight<int>(null, (a, b) => a + b));
        }

        [Test]
        public void AggregateRightWithNullFunc()
        {
            Assert.ThrowsArgumentNullException("func",
                () => Enumerable.Range(1, 5).AggregateRight(null));
        }

        [Test]
        public void AggregateRightWithEmptySequence()
        {
            Assert.Throws<InvalidOperationException>(
                () => new int[0].AggregateRight((a, b) => a + b));
        }

        [Test]
        public void AggregateRightFuncIsNotInvokedOnSingleElementSequence()
        {
            const int value = 1;

            var result = new[] { value }.AggregateRight(BreakingFunc.Of<int, int, int>());

            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public void AggregateRight()
        {
            var result = Enumerable.Range(1, 5)
                                   .Select(x => x.ToString())
                                   .AggregateRight((a, b) => string.Format("({0}+{1})", a, b));

            Assert.That(result, Is.EqualTo("(1+(2+(3+(4+5))))"));
        }

        // Overload 2 Test

        [Test]
        public void AggregateRightSeedWithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source",
                () => MoreEnumerable.AggregateRight<int, int>(null, 1, (a, b) => a + b));
        }

        [Test]
        public void AggregateRightSeedWithNullFunc()
        {
            Assert.ThrowsArgumentNullException("func",
                () => Enumerable.Range(1, 5).AggregateRight(6, null));
        }

        [TestCase(5)]
        [TestCase("c")]
        [TestCase(true)]
        public void AggregateRightSeedWithEmptySequence(object defaultValue)
        {
            Assert.That(new int[0].AggregateRight(defaultValue, (a, b) => b), Is.EqualTo(defaultValue));
        }

        [Test]
        public void AggregateRightSeedFuncIsNotInvokedOnEmptySequence()
        {
            const int value = 1;

            var result = new int[0].AggregateRight(value, BreakingFunc.Of<int, int, int>());

            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public void AggregateRightSeed()
        {
            var result = Enumerable.Range(1, 4)
                                   .AggregateRight("5", (a, b) => string.Format("({0}+{1})", a, b));

            Assert.That(result, Is.EqualTo("(1+(2+(3+(4+5))))"));
        }

        // Overload 3 Test

        [Test]
        public void AggregateRightResultorWithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source",
                () => MoreEnumerable.AggregateRight<int, int, bool>(null, 1, (a, b) => a + b, a => a % 2 == 0));
        }

        [Test]
        public void AggregateRightResultorWithNullFunc()
        {
            Assert.ThrowsArgumentNullException("func",
                () => Enumerable.Range(1, 5).AggregateRight(6, null, a => a % 2 == 0));
        }

        [Test]
        public void AggregateRightResultorWithNullResultSelector()
        {
            Assert.ThrowsArgumentNullException("resultSelector",
                () => Enumerable.Range(1, 5).AggregateRight(6, (a, b) => a + b, (Func<int, bool>)null));
        }

        [TestCase(5)]
        [TestCase("c")]
        [TestCase(true)]
        public void AggregateRightResultorWithEmptySequence(object defaultValue)
        {
            Assert.That(new int[0].AggregateRight(defaultValue, (a, b) => b, a => a == defaultValue), Is.EqualTo(true));
        }

        [Test]
        public void AggregateRightResultor()
        {
            var result = Enumerable.Range(1, 4)
                                   .AggregateRight("5", (a, b) => string.Format("({0}+{1})", a, b), a => a.Length);

            Assert.That(result, Is.EqualTo("(1+(2+(3+(4+5))))".Length));
        }
    }
}
