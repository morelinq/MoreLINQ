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

    [TestFixture]
    public class AggregateRightTest
    {
        // Overload 1 Test

        [Test]
        public void AggregateRightWithEmptySequence()
        {
            Assert.That(() => new int[0].AggregateRight((a, b) => a + b),
                        Throws.InvalidOperationException);
        }

        [Test]
        public void AggregateRightFuncIsNotInvokedOnSingleElementSequence()
        {
            const int value = 1;

            var result = new[] { value }.AggregateRight(BreakingFunc.Of<int, int, int>());

            Assert.That(result, Is.EqualTo(value));
        }

        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        [TestCase(SourceKind.Sequence)]
        public void AggregateRight(SourceKind sourceKind)
        {
            var enumerable = Enumerable.Range(1, 5).Select(x => x.ToInvariantString()).ToSourceKind(sourceKind);

            var result = enumerable.AggregateRight((a, b) => $"({a}+{b})");

            Assert.That(result, Is.EqualTo("(1+(2+(3+(4+5))))"));
        }

        // Overload 2 Test

        [TestCase(5)]
        [TestCase("c")]
        [TestCase(true)]
        public void AggregateRightSeedWithEmptySequence(object defaultValue)
        {
            Assert.That(new int[0].AggregateRight(defaultValue, (_, b) => b), Is.EqualTo(defaultValue));
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
                                   .AggregateRight("5", (a, b) => $"({a}+{b})");

            Assert.That(result, Is.EqualTo("(1+(2+(3+(4+5))))"));
        }

        // Overload 3 Test

        [TestCase(5)]
        [TestCase("c")]
        [TestCase(true)]
        public void AggregateRightResultorWithEmptySequence(object defaultValue)
        {
            Assert.That(new int[0].AggregateRight(defaultValue, (_, b) => b, a => a == defaultValue), Is.EqualTo(true));
        }

        [Test]
        public void AggregateRightResultor()
        {
            var result = Enumerable.Range(1, 4)
                                   .AggregateRight("5", (a, b) => $"({a}+{b})", a => a.Length);

            Assert.That(result, Is.EqualTo("(1+(2+(3+(4+5))))".Length));
        }
    }
}
