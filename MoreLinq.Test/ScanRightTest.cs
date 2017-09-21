#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Leandro F. Vieira (leandromoh). All rights reserved.
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
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class ScanRightTest
    {
        // ScanRight(source, func)
        
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

        [Test]
        public void ScanRightIsLazy()
        {
            new BreakingSequence<int>().ScanRight(BreakingFunc.Of<int, int, int>());
        }

        // ScanRight(source, seed, func)
                
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

        [Test]
        public void ScanRightSeedIsLazy()
        {
            new BreakingSequence<int>().ScanRight(string.Empty, BreakingFunc.Of<int, string, string>());
        }
    }
}
