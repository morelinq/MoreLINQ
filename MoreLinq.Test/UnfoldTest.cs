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
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using NUnit.Framework;

    [TestFixture]
    public class UnfoldTest
    {
        static IEnumerable<TestCaseData>
            TestCaseData<T>(IEnumerable<T> source1,
                            IEnumerable<T> source2,
                            [CallerMemberName] string? callerMemberName = null) =>
            new TestCaseData[]
            {
                new(source1) { TestName = $"{callerMemberName}(generator)" },
                new(source2) { TestName = $"{callerMemberName}(state, generator, predicate, stateSelector, resultSelector)" },
            };

        static IEnumerable<TestCaseData> UnfoldInfiniteSequenceData() =>
            TestCaseData(MoreEnumerable.Unfold(1, x => (true, Result: x, State: x + 1)),
                         MoreEnumerable.Unfold(1, x => (Result: x, State: x + 1),
                                                  _ => true,
                                                  e => e.State,
                                                  e => e.Result));

        [Test]
        [TestCaseSource(nameof(UnfoldInfiniteSequenceData))]
        public void UnfoldInfiniteSequence(IEnumerable<int> unfold)
        {
            var result = unfold.Take(100);
            var expectations = MoreEnumerable.Generate(1, x => x + 1).Take(100);
            Assert.That(result, Is.EqualTo(expectations));
        }

        static IEnumerable<TestCaseData> UnfoldFiniteSequenceData() =>
            TestCaseData(MoreEnumerable.Unfold(1, x => x <= 100 ? (true, Result: x, State: x + 1) : default),
                         MoreEnumerable.Unfold(1, x => (Result: x, State: x + 1),
                                                  e => e.Result <= 100,
                                                  e => e.State,
                                                  e => e.Result));

        [Test]
        [TestCaseSource(nameof(UnfoldFiniteSequenceData))]
        public void UnfoldFiniteSequence(IEnumerable<int> unfold)
        {
            var result = unfold;
            var expectations = MoreEnumerable.Generate(1, x => x + 1).Take(100);
            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void UnfoldIsLazy()
        {
            _ = MoreEnumerable.Unfold(0, BreakingFunc.Of<int, (bool, int, int)>());

            _ = MoreEnumerable.Unfold(0, BreakingFunc.Of<int, (int, int)>(),
                                         BreakingFunc.Of<(int, int), bool>(),
                                         BreakingFunc.Of<(int, int), int>(),
                                         BreakingFunc.Of<(int, int), int>());
        }

        static IEnumerable<TestCaseData> UnfoldSingleElementSequenceData() =>
            TestCaseData(MoreEnumerable.Unfold(0, x => x == 0 ? (true, Result: x, State: x + 1) : default),
                         MoreEnumerable.Unfold(0, x => (Result: x, State: x + 1),
                                                  x => x.Result == 0,
                                                  e => e.State,
                                                  e => e.Result));

        [Test]
        [TestCaseSource(nameof(UnfoldSingleElementSequenceData))]
        public void UnfoldSingleElementSequence(IEnumerable<int> unfold)
        {
            var result = unfold;
            var expectations = new[] { 0 };
            Assert.That(result, Is.EqualTo(expectations));
        }

        static IEnumerable<TestCaseData> UnfoldEmptySequenceData() =>
            TestCaseData(MoreEnumerable.Unfold(0, x => x < 0 ? (true, Result: x, State: x + 1) : default),
                         MoreEnumerable.Unfold(0, x => (Result: x, State: x + 1),
                                                  x => x.Result < 0,
                                                  e => e.State,
                                                  e => e.Result));

        [Test]
        [TestCaseSource(nameof(UnfoldEmptySequenceData))]
        public void UnfoldEmptySequence(IEnumerable<int> unfold)
        {
            var result = unfold;
            Assert.That(result, Is.Empty);
        }

        static IEnumerable<TestCaseData> UnfoldReiterationsReturnsSameResultData() =>
            TestCaseData(MoreEnumerable.Unfold(1, n => (true, Result: n, State: n + 1)),
                         MoreEnumerable.Unfold(1, n => (Result: n, Next: n + 1),
                                                  _ => true,
                                                  n => n.Next,
                                                  n => n.Result));

        [Test(Description = "https://github.com/morelinq/MoreLINQ/issues/990")]
        [TestCaseSource(nameof(UnfoldReiterationsReturnsSameResultData))]
        public void UnfoldReiterationsReturnsSameResult(IEnumerable<int> unfold)
        {
            var xs = unfold.Take(5);
            xs.AssertSequenceEqual(1, 2, 3, 4, 5);
            xs.AssertSequenceEqual(1, 2, 3, 4, 5);
        }
    }
}
