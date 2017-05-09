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
    public class ScanRightTest
    {
        // Overload 1 Test

        [Test]
        public void ScanRightWithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source",
                () => MoreEnumerable.ScanRight<int>(null, (a, b) => a + b));
        }

        [Test]
        public void ScanRightWithNullFunc()
        {
            Assert.ThrowsArgumentNullException("func",
                () => Enumerable.Range(1, 5).ScanRight(null));
        }

        [Test]
        public void ScanRightWithEmptySequence()
        {
            var result = new int[0].ScanRight((a, b) => a + b);

            Assert.That(result, Is.EqualTo(new int[0]));
        }

        [Test]
        public void ScanRightFuncIsNotInvokedOnSingleElementSequence()
        {
            const int value = 1;

            var result = new[] { value }.ScanRight(BreakingFunc.Of<int, int, int>());

            Assert.That(result, Is.EqualTo(new[] { value }));
        }

        [Test]
        public void ScanRight()
        {
            var result = Enumerable.Range(1, 5)
                                   .Select(x => x.ToString())
                                   .ScanRight((a, b) => string.Format("({0}+{1})", a, b));

            var expectations = new[] { "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" };

            Assert.That(result, Is.EqualTo(expectations));
        }

        // Overload 2 Test

        [Test]
        public void ScanRightSeedWithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source",
                () => MoreEnumerable.ScanRight<int, int>(null, 1, (a, b) => a + b));
        }

        [Test]
        public void ScanRightSeedWithNullFunc()
        {
            Assert.ThrowsArgumentNullException("func",
                () => Enumerable.Range(1, 5).ScanRight(6, null));
        }

        [TestCase(5)]
        [TestCase("c")]
        [TestCase(true)]
        public void ScanRightSeedWithEmptySequence(object defaultValue)
        {
            Assert.That(new int[0].ScanRight(defaultValue, (a, b) => b), Is.EqualTo(new[] { defaultValue }));
        }

        [Test]
        public void ScanRightSeedFuncIsNotInvokedOnEmptySequence()
        {
            const int value = 1;

            var result = new int[0].ScanRight(value, BreakingFunc.Of<int, int, int>());

            Assert.That(result, Is.EqualTo(new[] { value }));
        }

        [Test]
        public void ScanRightSeed()
        {
            var result = Enumerable.Range(1, 4)
                                   .ScanRight("5", (a, b) => string.Format("({0}+{1})", a, b));

            var expectations = new[] { "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" };

            Assert.That(result, Is.EqualTo(expectations));
        }
    }
}
