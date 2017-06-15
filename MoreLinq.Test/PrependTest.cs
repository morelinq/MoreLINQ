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
            string head = null;
            var whole = tail.Prepend(head);
            whole.AssertSequenceEqual(null, "second", "third");
        }

        [Test]
        public void PrependIsLazyInTailSequence()
        {
            new BreakingSequence<string>().Prepend("head");
        }

        [Test]
        public void PrependMany()
        {
            IEnumerable<int> xs = new[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            foreach (var e in new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 }.Index())
            {
                var s = xs.Prepend(e.Value);
                Assert.That(s, e.Key == 0 ? Is.Not.SameAs(xs) : Is.SameAs(xs));
                xs = s;
            }
            xs.AssertSequenceEqual( 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                                   11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
        }
    }
}
