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
    using static MoreLinq.Extensions.AppendExtension;

    [TestFixture]
    public class AppendTest
    {
        #region Append with single head and tail sequence
        [Test]
        public void AppendWithNonEmptyHeadSequence()
        {
            var head = new[] { "first", "second" };
            var tail = "third";
            var whole = head.Append(tail);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void AppendWithEmptyHeadSequence()
        {
            string[] head = { };
            var tail = "first";
            var whole = head.Append(tail);
            whole.AssertSequenceEqual("first");
        }

        [Test]
        public void AppendWithNullTail()
        {
            var head = new[] { "first", "second" };
            string? tail = null;
            var whole = head.Append(tail);
            whole.AssertSequenceEqual("first", "second", null);
        }

        [Test]
        public void AppendIsLazyInHeadSequence()
        {
            _ = new BreakingSequence<string>().Append("tail");
        }
        #endregion

        [TestCaseSource(nameof(ContactManySource))]
        public void AppendMany(int[] head, int[] tail)
        {
            tail.Aggregate(head.AsEnumerable(), (xs, x) => xs.Append(x))
                .AssertSequenceEqual(head.Concat(tail));
        }

        public static IEnumerable<object> ContactManySource =>
            from x in Enumerable.Range(0, 11)
            from y in Enumerable.Range(1, 20 - x)
            select new
            {
                Head = Enumerable.Range(1, x).ToArray(),
                Tail = Enumerable.Range(x + 1, y).ToArray(),
            }
            into e
            select new TestCaseData(e.Head,
                                    e.Tail).SetName("Head = [" + string.Join(", ", e.Head) + "], " +
                                                    "Tail = [" + string.Join(", ", e.Tail) + "]");

        [Test]
        public void AppendWithSharedSource()
        {
            var first  = new[] { 1 }.Append(2);
            var second = first.Append(3).Append(4);
            var third  = first.Append(4).Append(8);

            second.AssertSequenceEqual(1, 2, 3, 4);
            third.AssertSequenceEqual(1, 2, 4, 8);
        }
    }
}
