#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
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

using System.Collections;
using NUnit.Framework.Internal;

namespace MoreLinq.Test
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using Tuple = System.ValueTuple;

    [TestFixture]
    public class ZipWhileTest
    {
        static IEnumerable<T> Seq<T>(params T[] values) => values;

        public static readonly IEnumerable<ITestCaseData> TestData =
            from e in new[]
            {
                new { A = Seq<int>(  ), B = Seq("foo", "bar", "baz"), C = new bool[3], Result = Seq((0, "foo", false), (0, "bar", false), (0, "baz", false)) },
                new { A = Seq(1      ), B = Seq("foo", "bar", "baz"), C = new bool[3], Result = Seq((1, "foo", false), (0, "bar", false), (0, "baz", false)) },
                new { A = Seq(1, 2   ), B = Seq("foo", "bar", "baz"), C = new bool[3], Result = Seq((1, "foo", false), (2, "bar", false), (0, "baz", false)) },
                new { A = Seq(1, 2, 3), B = Seq<string>(           ), C = new bool[3], Result = Seq((1, null, false), (2, null, false), (3, (string) null, false)) },
                new { A = Seq(1, 2, 3), B = Seq("foo"              ), C = new bool[3], Result = Seq((1, "foo", false), (2, null, false), (3, null, false)) },
                new { A = Seq(1, 2, 3), B = Seq("foo", "bar"       ), C = new bool[3], Result = Seq((1, "foo", false), (2, "bar", false), (3, null, false)) },
                new { A = Seq(1, 2, 3), B = Seq("foo", "bar", "baz"), C = new bool[3], Result = Seq((1, "foo", false), (2, "bar", false), (3, "baz", false)) },
                new { A = Seq<int>(  ), B = Seq("foo", "bar", "baz"), C = new bool[3], Result = Seq((0, "foo", false), (0, "bar", false), (0, "baz", false)) },
                new { A = Seq(1      ), B = Seq("foo", "bar", "baz"), C = new bool[1], Result = Seq((1, "foo", false)) },
                new { A = Seq(1, 2   ), B = Seq("foo", "bar", "baz"), C = new bool[2], Result = Seq((1, "foo", false), (2, "bar", false)) },
                new { A = Seq(1, 2, 3), B = Seq("foo", "bar", "baz"), C = new bool[3], Result = Seq((1, "foo", false), (2, "bar", false), (3, "baz", false)) },
            }
            select new TestCaseData(e.A, e.B, e.C)
                .Returns(e.Result);


        [Test, TestCaseSource(nameof(TestData))]
        public IEnumerable<(int, string, bool)> ZipWhile(int[] first, string[] second, bool[] third)
        {
            using var ts1 = TestingSequence.Of(first);
            using var ts2 = TestingSequence.Of(second);
            using var ts3 = TestingSequence.Of(third);
            return ts1.ZipWhile(ts2, ts3, Tuple.Create, es => es.Contains(3)).ToArray();
        }

        [Test]
        public void ZipWhileIsLazy()
        {
            var bs = new BreakingSequence<int>();
            bs.ZipWhile(bs, BreakingFunc.Of<int, int, int>(), es => true);
        }

        [Test]
        public void ZipWhileDisposeSequencesEagerly()
        {
            var shorter = TestingSequence.Of(1, 2, 3);
            var longer = MoreEnumerable.Generate(1, x => x + 1);
            var zipped = shorter.ZipWhile(longer, Tuple.Create, es => true);

            var count = 0;
            foreach (var _ in zipped.Take(10))
            {
                if (++count == 4)
                    ((IDisposable)shorter).Dispose();
            }
        }

        [Test]
        public void ZipWhileDoesNotCallPredicateOnSameLengthSources()
        {
            var first = TestingSequence.Of(1, 2, 3);
            var second = TestingSequence.Of(1, 2, 3);
            var third = TestingSequence.Of(1, 2, 3);
            var zipped = first.ZipWhile(second, third, Tuple.Create, es => throw new InvalidOperationException());

            zipped.Consume();
        }

        [TestCase(0, 0, 0, ExpectedResult = 0)]
        [TestCase(1, 1, 1, ExpectedResult = 0)]
        [TestCase(1, 0, 0, ExpectedResult = 1)]
        [TestCase(0, 1, 0, ExpectedResult = 1)]
        [TestCase(0, 0, 1, ExpectedResult = 1)]
        [TestCase(1, 1, 0, ExpectedResult = 1)]
        [TestCase(1, 0, 1, ExpectedResult = 1)]
        [TestCase(0, 1, 1, ExpectedResult = 1)]
        [TestCase(1, 2, 0, ExpectedResult = 2)]
        [TestCase(2, 1, 0, ExpectedResult = 2)]
        [TestCase(1, 0, 2, ExpectedResult = 2)]
        [TestCase(2, 0, 1, ExpectedResult = 2)]
        [TestCase(0, 1, 2, ExpectedResult = 2)]
        [TestCase(0, 2, 1, ExpectedResult = 2)]
        public int ZipWhilePredicateCalledRightNumberOfTimes(int length1, int length2, int length3)
        {
            var count = 0;

            bool Continue(IReadOnlyList<int> enumerablePositions)
            {
                count++;
                return true;
            }

            var first = Enumerable.Repeat(new object(), length1);
            var second = Enumerable.Repeat(new object(), length2);
            var third = Enumerable.Repeat(new object(), length3);
            var zipped = first.ZipWhile(second, third, Tuple.Create, Continue);

            zipped.Consume();

            return count;
        }

        [TestCase(1, 1, 0, 0, ExpectedResult = 1)]
        [TestCase(2, 0, 1, 0, ExpectedResult = 1)]
        [TestCase(3, 0, 0, 1, ExpectedResult = 1)]
        [TestCase(1, 1, 2, 2, ExpectedResult = 1)]
        [TestCase(2, 2, 1, 2, ExpectedResult = 1)]
        [TestCase(3, 2, 2, 1, ExpectedResult = 1)]
        [TestCase(1, 1, 2, 0, ExpectedResult = 1)]
        [TestCase(2, 0, 1, 2, ExpectedResult = 1)]
        [TestCase(3, 2, 0, 1, ExpectedResult = 1)]
        public int ZipWhileStopWhenPredicateReturnFalse(int mainSourcePosition, int length1, int length2, int length3)
        {
            bool Continue(IReadOnlyList<int> enumerablePositions)
            {
                return enumerablePositions.Contains(mainSourcePosition);
            }

            var first = Enumerable.Repeat(new object(), length1);
            var second = Enumerable.Repeat(new object(), length2);
            var third = Enumerable.Repeat(new object(), length3);
            var zipped = first.ZipWhile(second, third, Tuple.Create, Continue);

            return zipped.Count();
        }

        [Test]
        public void ZipWhileDisposesInnerSequencesCaseGetEnumeratorThrows()
        {
            using (var s1 = TestingSequence.Of(1, 2))
            {
                Assert.Throws<InvalidOperationException>(() =>
                    s1.ZipWhile(new BreakingSequence<int>(), Tuple.Create, es => true).Consume());
            }
        }
    }
}
