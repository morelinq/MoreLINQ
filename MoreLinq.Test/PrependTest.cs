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
    using System.Collections.Generic;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using static MoreLinq.Extensions.PrependExtension;

    [TestFixture]
    public class PrependTest
    {
        [Test]
        public void PrependWithNonEmptyTailSequence()
        {
            string[] tail = { "second", "third" };
            var head = "first";
            var whole = tail.Prepend(head);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void PrependWithEmptyTailSequence()
        {
            string[] tail = { };
            var head = "first";
            var whole = tail.Prepend(head);
            whole.AssertSequenceEqual("first");
        }

        [Test]
        public void PrependWithNullHead()
        {
            string[] tail = { "second", "third" };
            string? head = null;
            var whole = tail.Prepend(head);
            whole.AssertSequenceEqual(null, "second", "third");
        }

        [Test]
        public void PrependIsLazyInTailSequence()
        {
            _ = new BreakingSequence<string>().Prepend("head");
        }

        [TestCaseSource(nameof(PrependManySource))]
        public int[] PrependMany(int[] head, int[] tail)
        {
            return tail.Aggregate(head.AsEnumerable(), MoreEnumerable.Prepend).ToArray();
        }

        public static IEnumerable<ITestCaseData> PrependManySource =>
            from x in Enumerable.Range(0, 11)
            from y in Enumerable.Range(1, 11)
            select new
            {
                Head = Enumerable.Range(0, y).Select(n => 0 - n).ToArray(),
                Tail = Enumerable.Range(1, x).ToArray(),
            }
            into e
            select new TestCaseData(e.Head, e.Tail)
                .SetName("Head = [" + string.Join(", ", e.Head) + "], " +
                         "Tail = [" + string.Join(", ", e.Tail) + "]")
                .Returns(e.Tail.Reverse().Concat(e.Head).ToArray());

        [Test]
        public void PrependWithSharedSource()
        {
            var first  = new[] { 1 }.Prepend(2);
            var second = first.Prepend(3).Prepend(4);
            var third  = first.Prepend(4).Prepend(8);

            second.AssertSequenceEqual(4, 3, 2, 1);
            third.AssertSequenceEqual(8, 4, 2, 1);
        }
    }
}
