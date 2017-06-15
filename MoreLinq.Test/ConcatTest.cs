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

    [TestFixture]
    public class ConcatTest
    {
        #region Concat with single head and tail sequence

        // NOTE: Concat with single head and tail sequence is now 
        // implemented in terms of Prepend so the tests are identical. 

        [Test]
        public void ConcatWithNonEmptyTailSequence()
        {
            var tail = new[] { "second", "third" };
            var head = "first";
            var whole = MoreEnumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void ConcatWithEmptyTailSequence()
        {
            string[] tail = { };
            var head = "first";
            var whole = MoreEnumerable.Concat(head, tail);
            whole.AssertSequenceEqual("first");
        }

        [Test]
        public void ConcatWithNullHead()
        {
            var tail = new[] { "second", "third" };
            string head = null;
            var whole = MoreEnumerable.Concat(head, tail);
            whole.AssertSequenceEqual(null, "second", "third");
        }

        [Test]
        public void ConcatIsLazyInTailSequence()
        {
            MoreEnumerable.Concat("head", new BreakingSequence<string>());
        }
        #endregion

        #region Concat with single head and tail sequence
        [Test]
        public void ConcatWithNonEmptyHeadSequence()
        {
            var head = new[] { "first", "second" };
            var tail = "third";
            var whole = head.Concat(tail);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void ConcatWithEmptyHeadSequence()
        {
            string[] head = { };
            var tail = "first";
            var whole = head.Concat(tail);
            whole.AssertSequenceEqual("first");
        }

        [Test]
        public void ConcatWithNullTail()
        {
            var head = new[] { "first", "second" };
            string tail = null;
            var whole = head.Concat(tail);
            whole.AssertSequenceEqual("first", "second", null);
        }

        [Test]
        public void ConcatIsLazyInHeadSequence()
        {
            new BreakingSequence<string>().Concat("tail");
        }
        #endregion

        [Test]
        public void ConcatMany()
        {
            IEnumerable<int> xs = new[] {  1,  2,  3,  4,  5,  6,  7,  8,  9, 10 };

            foreach (var e in new[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 })
                xs = xs.Concat(e);

            xs.AssertSequenceEqual( 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                                   11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
        }

        [Test]
        public void ConcatWithSharedSource()
        {
            var first  = new [] { 1 }.Concat(2);
            var second = first.Concat(3).Concat(4);
            var third  = first.Concat(4).Concat(8);

            second.AssertSequenceEqual(1, 2, 3, 4);
            third.AssertSequenceEqual(1, 2, 4, 8);
        }
    }
}
