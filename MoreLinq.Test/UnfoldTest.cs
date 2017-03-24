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

using System.Linq;
using NUnit.Framework;
using System;

namespace MoreLinq.Test
{
    [TestFixture]
    public class UnfoldTest
    {
        [Test]
        public void UnfoldWithNullGenerator()
        {
            Assert.ThrowsArgumentNullException("generator", () =>
                MoreEnumerable.Unfold<int, System.Tuple<int, int>, int>(0, null, _ => true, e => e.Item1, e => e.Item2));
        }

        [Test]
        public void UnfoldWithNullPredicate()
        {
            Assert.ThrowsArgumentNullException("predicate", () =>
                MoreEnumerable.Unfold(0, e => Tuple.Create(e, e + 1), null, e => e.Item1, e => e.Item2));
        }

        [Test]
        public void UnfoldWithNullStateSelector()
        {
            Assert.ThrowsArgumentNullException("stateSelector", () =>
                MoreEnumerable.Unfold(0, e => Tuple.Create(e, e + 1), _ => true, null, e => e.Item2));
        }

        [Test]
        public void UnfoldWithNullResultSelector()
        {
            Assert.ThrowsArgumentNullException("resultSelector", () =>
                MoreEnumerable.Unfold<int, System.Tuple<int, int>, int>(0, e => Tuple.Create(e, e + 1),
                                                                    _ => true,
                                                                    e => e.Item1,
                                                                    null));
        }

        [Test]
        public void UnfoldInfiniteSequence()
        {
            var result = MoreEnumerable.Unfold(1, x => new { result = x, state = x + 1 },
                                                  _ => true,
                                                  e => e.state,
                                                  e => e.result)
                                       .Take(100);

            var expectations = MoreEnumerable.Generate(1, x => x + 1).Take(100);

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void UnfoldFiniteSequence()
        {
            var result = MoreEnumerable.Unfold(1, x => new { result = x, state = x + 1 },
                                                  e => e.result <= 100,
                                                  e => e.state,
                                                  e => e.result);

            var expectations = MoreEnumerable.Generate(1, x => x + 1).Take(100);

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void UnfoldIsLazy()
        {
            MoreEnumerable.Unfold(0, BreakingFunc.Of<int, Tuple<int, int>>(),
                                     BreakingFunc.Of<Tuple<int, int>, bool>(),
                                     BreakingFunc.Of<Tuple<int, int>, int>(),
                                     BreakingFunc.Of<Tuple<int, int>, int>());
        }


        [Test]
        public void UnfoldSingleElementSequence()
        {
            var result = MoreEnumerable.Unfold(0, x => new { result = x, state = x + 1 },
                                                  x => x.result == 0,
                                                  e => e.state,
                                                  e => e.result);

            var expectations = new[] { 0 };

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void UnfoldEmptySequence()
        {
            var result = MoreEnumerable.Unfold(0, x => new { result = x, state = x + 1 },
                                                  x => x.result < 0,
                                                  e => e.state,
                                                  e => e.result);

            var expectations = new int[] { };

            Assert.That(result, Is.EqualTo(expectations));
        }
    }
}
