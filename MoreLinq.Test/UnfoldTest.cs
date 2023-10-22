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
    public class UnfoldTest
    {
        [Test]
        public void UnfoldInfiniteSequence()
        {
            var result = MoreEnumerable.Unfold(1, x => (Result: x, State: x + 1),
                                                  _ => true,
                                                  e => e.State,
                                                  e => e.Result)
                                       .Take(100);

            var expectations = MoreEnumerable.Generate(1, x => x + 1).Take(100);

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void UnfoldFiniteSequence()
        {
            var result = MoreEnumerable.Unfold(1, x => (Result: x, State: x + 1),
                                                  e => e.Result <= 100,
                                                  e => e.State,
                                                  e => e.Result);

            var expectations = MoreEnumerable.Generate(1, x => x + 1).Take(100);

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void UnfoldIsLazy()
        {
            _ = MoreEnumerable.Unfold(0, BreakingFunc.Of<int, (int, int)>(),
                                         BreakingFunc.Of<(int, int), bool>(),
                                         BreakingFunc.Of<(int, int), int>(),
                                         BreakingFunc.Of<(int, int), int>());
        }


        [Test]
        public void UnfoldSingleElementSequence()
        {
            var result = MoreEnumerable.Unfold(0, x => (Result: x, State: x + 1),
                                                  x => x.Result == 0,
                                                  e => e.State,
                                                  e => e.Result);

            var expectations = new[] { 0 };

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void UnfoldEmptySequence()
        {
            var result = MoreEnumerable.Unfold(0, x => (Result: x, State: x + 1),
                                                  x => x.Result < 0,
                                                  e => e.State,
                                                  e => e.Result);
            Assert.That(result, Is.Empty);
        }

        [Test(Description = "https://github.com/morelinq/MoreLINQ/issues/990")]
        public void UnfoldReiterationsReturnsSameResult()
        {
            var xs = MoreEnumerable.Unfold(1, n => (Result: n, Next: n + 1),
                                           _ => true,
                                           n => n.Next,
                                           n => n.Result)
                                   .Take(5);

            xs.AssertSequenceEqual(1, 2, 3, 4, 5);
            xs.AssertSequenceEqual(1, 2, 3, 4, 5);
        }
    }
}
