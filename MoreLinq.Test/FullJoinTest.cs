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
    using static FullJoinTest.Side;

    [TestFixture]
    public class FullJoinTest
    {
        public enum Side { Left, Right, Both }

        [Test]
        public void FullJoinWithHomogeneousSequencesIsLazy()
        {
            var xs = new BreakingSequence<int>();
            var ys = new BreakingSequence<int>();

            Assert.That(() =>
                xs.FullJoin(ys, e => e,
                    BreakingFunc.Of<int, object>(),
                    BreakingFunc.Of<int, object>(),
                    BreakingFunc.Of<int, int, object>()),
                Throws.Nothing);
        }

        [Test]
        public void FullJoinWithHomogeneousSequencesWithComparerIsLazy()
        {
            var xs = new BreakingSequence<int>();
            var ys = new BreakingSequence<int>();

            Assert.That(() =>
                xs.FullJoin(ys, e => e,
                    BreakingFunc.Of<int, object>(),
                    BreakingFunc.Of<int, object>(),
                    BreakingFunc.Of<int, int, object>(),
                    comparer: null),
                Throws.Nothing);
        }

        [Test]
        public void FullJoinIsLazy()
        {
            var xs = new BreakingSequence<int>();
            var ys = new BreakingSequence<object>();

            Assert.That(() =>
                xs.FullJoin(ys, x => x, y => y.GetHashCode(),
                    BreakingFunc.Of<int, object>(),
                    BreakingFunc.Of<object, object>(),
                    BreakingFunc.Of<int, object, object>()),
                Throws.Nothing);
        }

        [Test]
        public void FullJoinWithComparerIsLazy()
        {
            var xs = new BreakingSequence<int>();
            var ys = new BreakingSequence<object>();

            Assert.That(() =>
                xs.FullJoin(ys, x => x, y => y.GetHashCode(),
                    BreakingFunc.Of<int, object>(),
                    BreakingFunc.Of<object, object>(),
                    BreakingFunc.Of<int, object, object>(),
                    comparer: null),
                Throws.Nothing);
        }

        [Test]
        public void FullJoinResults()
        {
            var foo  = (1, "foo");
            var bar1 = (2, "bar");
            var bar2 = (2, "Bar");
            var bar3 = (2, "BAR");
            var baz  = (3, "baz");
            var qux  = (4, "qux");
            var quux = (5, "quux");
            var quuz = (6, "quuz");

            var xs = new[] { foo, bar1, qux };
            var ys = new[] { quux, bar2, baz, bar3, quuz };

            var missing = default((int, string));

            var result =
                xs.FullJoin(ys,
                            x => x.Item1,
                            y => y.Item1,
                            x => (Left, x, missing),
                            y => (Right, missing, y),
                            (x, y) => (Both, x, y));

            result.AssertSequenceEqual(
                (Left , foo    , missing),
                (Both , bar1   , bar2   ),
                (Both , bar1   , bar3   ),
                (Left , qux    , missing),
                (Right, missing, quux   ),
                (Right, missing, baz    ),
                (Right, missing, quuz   ));
        }

        [Test]
        public void FullJoinWithComparerResults()
        {
            var foo  = ("one"  , "foo");
            var bar1 = ("two"  , "bar");
            var bar2 = ("Two"  , "bar");
            var bar3 = ("TWO"  , "bar");
            var baz  = ("three", "baz");
            var qux  = ("four" , "qux");
            var quux = ("five" , "quux");
            var quuz = ("six"  , "quuz");

            var xs = new[] { foo, bar1, qux };
            var ys = new[] { quux, bar2, baz, bar3, quuz };

            var missing = default((string, string));

            var result =
                xs.FullJoin(ys,
                            x => x.Item1,
                            y => y.Item1,
                            x => (Left, x, missing),
                            y => (Right, missing, y),
                            (x, y) => (Both, x, y),
                            StringComparer.OrdinalIgnoreCase);

            result.AssertSequenceEqual(
                (Left , foo    , missing),
                (Both , bar1   , bar2   ),
                (Both , bar1   , bar3   ),
                (Left , qux    , missing),
                (Right, missing, quux   ),
                (Right, missing, baz    ),
                (Right, missing, quuz   ));
        }

        [Test]
        public void FullJoinEmptyLeft()
        {
            var foo = (1, "foo");
            var bar = (2, "bar");
            var baz = (3, "baz");

            var xs = new (int, string)[0];
            var ys = new[] { foo, bar, baz };

            var missing = default((int, string));

            var result =
                xs.FullJoin(ys,
                            x => x.Item1,
                            y => y.Item1,
                            x => (Left, x, missing),
                            y => (Right, missing, y),
                            (x, y) => (Both, x, y));

            result.AssertSequenceEqual(
                (Right, missing, foo),
                (Right, missing, bar),
                (Right, missing, baz));
        }

        [Test]
        public void FullJoinEmptyRight()
        {
            var foo = (1, "foo");
            var bar = (2, "bar");
            var baz = (3, "baz");

            var xs = new[] { foo, bar, baz };
            var ys = new (int, string)[0];

            var missing = default((int, string));

            var result =
                xs.FullJoin(ys,
                            x => x.Item1,
                            y => y.Item1,
                            x => (Left, x, missing),
                            y => (Right, missing, y),
                            (x, y) => (Both, x, y));

            result.AssertSequenceEqual(
                (Left, foo, missing),
                (Left, bar, missing),
                (Left, baz, missing));
        }
    }
}
