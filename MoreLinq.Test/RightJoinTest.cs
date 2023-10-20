#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All rights reserved.
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
    using System;
    using NUnit.Framework;
    using static RightJoinTest.Side;

    [TestFixture]
    public class RightJoinTest
    {
        public enum Side { Right, Both }

        [Test]
        public void RightJoinWithHomogeneousSequencesIsLazy()
        {
            var xs = new BreakingSequence<int>();
            var ys = new BreakingSequence<int>();

            Assert.That(() =>
                xs.RightJoin(ys, e => e,
                    BreakingFunc.Of<int, object>(),
                    BreakingFunc.Of<int, int, object>()),
                Throws.Nothing);
        }

        [Test]
        public void RightJoinWithHomogeneousSequencesWithComparerIsLazy()
        {
            var xs = new BreakingSequence<int>();
            var ys = new BreakingSequence<int>();

            Assert.That(() =>
                xs.RightJoin(ys, e => e,
                    BreakingFunc.Of<int, object>(),
                    BreakingFunc.Of<int, int, object>(),
                    comparer: null),
                Throws.Nothing);
        }

        [Test]
        public void RightJoinIsLazy()
        {
            var xs = new BreakingSequence<int>();
            var ys = new BreakingSequence<object>();

            Assert.That(() =>
                xs.RightJoin(ys, x => x.GetHashCode(), y => y,
                    BreakingFunc.Of<object, object>(),
                    BreakingFunc.Of<int, object, object>()),
                Throws.Nothing);
        }

        [Test]
        public void RightJoinWithComparerIsLazy()
        {
            var xs = new BreakingSequence<int>();
            var ys = new BreakingSequence<object>();

            Assert.That(() =>
                xs.RightJoin(ys, x => x.GetHashCode(), y => y,
                    BreakingFunc.Of<object, object>(),
                    BreakingFunc.Of<int, object, object>(),
                    comparer: null),
                Throws.Nothing);
        }

        [Test]
        public void RightJoinResults()
        {
            var foo  = (1, "foo");
            var bar1 = (2, "bar");
            var bar2 = (2, "Bar");
            var bar3 = (2, "BAR");
            var baz  = (3, "baz");
            var qux  = (4, "qux");

            var xs = new[] { bar2, baz, bar3 };
            var ys = new[] { foo, bar1, qux };

            var missing = default((int, string));

            var result =
                xs.RightJoin(ys,
                             x => x.Item1,
                             y => y.Item1,
                             y => (Right, missing, y),
                             (x, y) => (Both, x, y));

            result.AssertSequenceEqual(
                (Right, missing, foo ),
                (Both , bar2   , bar1),
                (Both , bar3   , bar1),
                (Right, missing, qux ));
        }

        [Test]
        public void RightJoinWithComparerResults()
        {
            var foo  = ("one"  , "foo");
            var bar1 = ("two"  , "bar");
            var bar2 = ("Two"  , "bar");
            var bar3 = ("TWO"  , "bar");
            var baz  = ("three", "baz");
            var qux  = ("four" , "qux");

            var xs = new[] { bar2, baz, bar3 };
            var ys = new[] { foo, bar1, qux };

            var missing = default((string, string));

            var result =
                xs.RightJoin(ys,
                             x => x.Item1,
                             y => y.Item1,
                             y => (Right, missing, y),
                             (x, y) => (Both, x, y),
                             StringComparer.OrdinalIgnoreCase);

            result.AssertSequenceEqual(
                (Right, missing, foo ),
                (Both , bar2   , bar1),
                (Both , bar3   , bar1),
                (Right, missing, qux ));
        }

        [Test]
        public void RightJoinEmptyLeft()
        {
            var foo = (1, "foo");
            var bar = (2, "bar");
            var baz = (3, "baz");

            var xs = new (int, string)[0];
            var ys = new[] { foo, bar, baz };

            var missing = default((int, string));

            var result =
                xs.RightJoin(ys,
                             x => x.Item1,
                             y => y.Item1,
                             y => (Right, missing, y),
                             (x, y) => (Both, x, y));

            result.AssertSequenceEqual(
                (Right, missing, foo),
                (Right, missing, bar),
                (Right, missing, baz));
        }

        [Test]
        public void RightJoinEmptyRight()
        {
            var foo = (1, "foo");
            var bar = (2, "bar");
            var baz = (3, "baz");

            var xs = new[] { foo, bar, baz };
            var ys = new (int, string)[0];

            var missing = default((int, string));

            var result =
                xs.RightJoin(ys,
                            x => x.Item1,
                            y => y.Item1,
                            y => (Right, missing, y),
                            (x, y) => (Both, x, y));

            Assert.That(result, Is.Empty);
        }
    }
}
