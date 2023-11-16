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

namespace MoreLinq.Test
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class AggregateRightTest
    {
        // Overload 1 Test

        [Test]
        public void AggregateRightWithEmptySequence()
        {
            using var ts = TestingSequence.Of<int>();
            Assert.That(
                () => ts.AggregateRight((a, b) => a + b),
                Throws.InvalidOperationException);
        }

        [Test]
        public void AggregateRightFuncIsNotInvokedOnSingleElementSequence()
        {
            using var ts = TestingSequence.Of(1);
            var result = ts.AggregateRight(BreakingFunc.Of<int, int, int>());

            Assert.That(result, Is.EqualTo(1));
        }

        public static IEnumerable<TestCaseData> AggregateRightSource =>
            Enumerable.Range(1, 5)
                .Select(x => x.ToInvariantString())
                .ToTestData(SourceKinds.Sequence.Concat(SourceKinds.List));

        [TestCaseSource(nameof(AggregateRightSource))]
        public void AggregateRight(IEnumerable<string> sequence)
        {
            using var d = sequence as IDisposable;
            var result = sequence.AggregateRight((a, b) => $"({a}+{b})");

            Assert.That(result, Is.EqualTo("(1+(2+(3+(4+5))))"));
        }

        // Overload 2 Test

        [TestCase(5)]
        [TestCase("c")]
        [TestCase(true)]
        public void AggregateRightSeedWithEmptySequence(object defaultValue)
        {
            using var ts = TestingSequence.Of<int>();
            var result = ts.AggregateRight(defaultValue, (_, b) => b);

            Assert.That(result, Is.EqualTo(defaultValue));
        }

        [Test]
        public void AggregateRightSeedFuncIsNotInvokedOnEmptySequence()
        {
            using var ts = TestingSequence.Of<int>();
            var result = ts.AggregateRight(1, BreakingFunc.Of<int, int, int>());

            Assert.That(result, Is.EqualTo(1));
        }

        public static IEnumerable<TestCaseData> AggregateRightSeedSource =>
            Enumerable.Range(1, 4)
                .ToTestData(SourceKinds.Sequence.Concat(SourceKinds.List));

        [TestCaseSource(nameof(AggregateRightSeedSource))]
        public void AggregateRightSeed(IEnumerable<int> sequence)
        {
            using var d = sequence as IDisposable;
            var result = sequence.AggregateRight("5", (a, b) => $"({a}+{b})");

            Assert.That(result, Is.EqualTo("(1+(2+(3+(4+5))))"));
        }

        // Overload 3 Test

        [TestCase(5)]
        [TestCase("c")]
        [TestCase(true)]
        public void AggregateRightResultorWithEmptySequence(object defaultValue)
        {
            using var ts = TestingSequence.Of<int>();
            var result = ts.AggregateRight(defaultValue, (_, b) => b, a => a == defaultValue);
            Assert.That(result, Is.EqualTo(true));
        }

        [TestCaseSource(nameof(AggregateRightSeedSource))]
        public void AggregateRightResultor(IEnumerable<int> sequence)
        {
            using var d = sequence as IDisposable;
            var result = sequence.AggregateRight("5", (a, b) => $"({a}+{b})", a => a.Length);

            Assert.That(result, Is.EqualTo("(1+(2+(3+(4+5))))".Length));
        }
    }
}
