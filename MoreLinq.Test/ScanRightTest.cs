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

        //
        // The first two cases are commented out intentionally for the
        // following reason:
        //
        // ScanRight internally skips ToList materialization if the source is
        // already list-like. Any test to make sure that is occurring would
        // have to fail if and only if the optimization is removed and ToList
        // is called. Such detection is tricky, hack-ish and brittle at best;
        // it would mean relying on current and internal implementation
        // details of Enumerable.ToList that can and have changed.
        // For further discussion, see:
        //
        // https://github.com/morelinq/MoreLINQ/pull/476#discussion_r185191063
        //
        // [TestCase(SourceKind.BreakingList)]
        // [TestCase(SourceKind.BreakingReadOnlyList)]
        [TestCase(SourceKind.Sequence)]
        public void ScanRight(SourceKind sourceKind)
        {
            var result = Enumerable.Range(1, 5)
                                   .Select(x => x.ToInvariantString())
                                   .ToSourceKind(sourceKind)
                                   .ScanRight((a, b) => $"({a}+{b})");

            var expectations = new[] { "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" };

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void ScanRightIsLazy()
        {
            _ = new BreakingSequence<int>().ScanRight(BreakingFunc.Of<int, int, int>());
        }

        // ScanRight(source, seed, func)

        [TestCase(5)]
        [TestCase("c")]
        [TestCase(true)]
        public void ScanRightSeedWithEmptySequence(object defaultValue)
        {
            Assert.That(new int[0].ScanRight(defaultValue, (_, b) => b), Is.EqualTo(new[] { defaultValue }));
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
                                   .ScanRight("5", (a, b) => $"({a}+{b})");

            var expectations = new[] { "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" };

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void ScanRightSeedIsLazy()
        {
            _ = new BreakingSequence<int>().ScanRight(string.Empty, BreakingFunc.Of<int, string, string>());
        }
    }
}
