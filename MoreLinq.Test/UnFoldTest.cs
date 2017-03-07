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

using System;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class UnfoldTest
    {
        [Test]
        public void UnfoldGenericWithNullGenerator()
        {
            Assert.ThrowsArgumentNullException("generator", () =>
                MoreEnumerable.Unfold<int, Tuple<int, int>, int>(0, null, _ => true, e => e.First, e => e.Second));
        }

        [Test]
        public void UnfoldGenericWithNullPredicate()
        {
            Assert.ThrowsArgumentNullException("predicate", () =>
                MoreEnumerable.Unfold(0, e => new Tuple<int, int>(e, e + 1), null, e => e.First, e => e.Second));
        }

        [Test]
        public void UnfoldGenericWithNullStateSelector()
        {
            Assert.ThrowsArgumentNullException("stateSelector", () =>
                MoreEnumerable.Unfold(0, e => new Tuple<int, int>(e, e + 1), _ => true, null, e => e.Second));
        }

        [Test]
        public void UnfoldGenericWithNullResultSelector()
        {
            Assert.ThrowsArgumentNullException("resultSelector", () =>
                MoreEnumerable.Unfold<int, Tuple<int, int>, int>(0, e => new Tuple<int, int>(e, e + 1), 
                                                                    _ => true, 
                                                                    e => e.First, 
                                                                    null));
        }

        [Test]
        public void UnfoldGenericInfiniteSequence()
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
        public void UnfoldGenericFiniteSequence()
        {
            var result = MoreEnumerable.Unfold(1, x => new { result = x, state = x + 1 },
                                                  e => e.result <= 100,
                                                  e => e.state,
                                                  e => e.result);

            var expectations = MoreEnumerable.Generate(1, x => x + 1).Take(100);

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void UnfoldGenericIsLazy()
        {
            MoreEnumerable.Unfold(0, BreakingFunc.Of<int, Tuple<int, int>>(),
                                     BreakingFunc.Of<Tuple<int, int>, bool>(),
                                     BreakingFunc.Of<Tuple<int, int>, int>(),
                                     BreakingFunc.Of<Tuple<int, int>, int>());
        }

        [Test]
        public void UnfoldTupleWithNullGenerator()
        {
            Assert.ThrowsArgumentNullException("generator", () =>
                MoreEnumerable.Unfold<int, int>(3, null));
        }

        [Test]
        public void UnfoldTupleInfiniteSequence()
        {
            var result = MoreEnumerable.Unfold(1, x => Tuple.Create(x, x + 1)).Take(100);
            var expectations = MoreEnumerable.Generate(1, x => x + 1).Take(100);

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void UnfoldTupleFiniteSequence()
        {
            var result = MoreEnumerable.Unfold(1, x => x <= 100 ? Tuple.Create(x, x + 1) : null);
            var expectations = MoreEnumerable.Generate(1, x => x + 1).Take(100);

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void UnfoldTupleIsLazy()
        {
            MoreEnumerable.Unfold(1, BreakingFunc.Of<int, System.Tuple<bool, int>>());
        }
    }
}
