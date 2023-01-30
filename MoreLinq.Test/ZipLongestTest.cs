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

namespace MoreLinq.Test
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using Tuple = System.ValueTuple;

    [TestFixture]
    public class ZipLongestTest
    {
        static IEnumerable<T> Seq<T>(params T[] values) => values;

        public static readonly IEnumerable<ITestCaseData> TestData =
            from e in new[]
            {
                new { A = Seq<int>(  ), B = Seq("foo", "bar", "baz"), Result = Seq<(int, string?)>((0, "foo"), (0, "bar"), (0, "baz")) },
                new { A = Seq(1      ), B = Seq("foo", "bar", "baz"), Result = Seq<(int, string?)>((1, "foo"), (0, "bar"), (0, "baz")) },
                new { A = Seq(1, 2   ), B = Seq("foo", "bar", "baz"), Result = Seq<(int, string?)>((1, "foo"), (2, "bar"), (0, "baz")) },
                new { A = Seq(1, 2, 3), B = Seq<string>(           ), Result = Seq<(int, string?)>((1, null ), (2, null ), (3, null )) },
                new { A = Seq(1, 2, 3), B = Seq("foo"              ), Result = Seq<(int, string?)>((1, "foo"), (2, null ), (3, null )) },
                new { A = Seq(1, 2, 3), B = Seq("foo", "bar"       ), Result = Seq<(int, string?)>((1, "foo"), (2, "bar"), (3, null )) },
                new { A = Seq(1, 2, 3), B = Seq("foo", "bar", "baz"), Result = Seq<(int, string?)>((1, "foo"), (2, "bar"), (3, "baz")) },
            }
            select new TestCaseData(e.A, e.B)
                .Returns(e.Result);


        [Test, TestCaseSource(nameof(TestData))]
        public IEnumerable<(int, string)> ZipLongest(int[] first, string[] second)
        {
            using var ts1 = TestingSequence.Of(first);
            using var ts2 = TestingSequence.Of(second);
            return ts1.ZipLongest(ts2, Tuple.Create).ToArray();
        }

        [Test]
        public void ZipLongestIsLazy()
        {
            var bs = new BreakingSequence<int>();
            _ = bs.ZipLongest(bs, BreakingFunc.Of<int, int, int>());
        }

        [Test]
        public void ZipLongestDisposeSequencesEagerly()
        {
            using var shorter = TestingSequence.Of(1, 2, 3);
            var longer = MoreEnumerable.Generate(1, x => x + 1);
            var zipped = shorter.ZipLongest(longer, Tuple.Create);

            var count = 0;
            foreach (var _ in zipped.Take(10))
            {
                if (++count == 4)
                    ((IDisposable)shorter).Dispose();
            }
        }

        [Test]
        public void ZipLongestDisposesInnerSequencesCaseGetEnumeratorThrows()
        {
            using var s1 = TestingSequence.Of(1, 2);

            Assert.That(() => s1.ZipLongest(new BreakingSequence<int>(), Tuple.Create).Consume(),
                        Throws.BreakException);
        }
    }
}
