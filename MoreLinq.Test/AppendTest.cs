#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Tim Rehm (Timmitry). All rights reserved.
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

using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class AppendTest
    {
        [Test]
        public void AppendWithNonEmptyHeadSequence()
        {
            string[] head = { "first", "second" };
            var tail = "third";
            var whole = head.Append(tail);
            whole.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void AppendWithEmptyTailSequence()
        {
            string[] head = { };
            var tail = "first";
            var whole = head.Append(tail);
            whole.AssertSequenceEqual("first");
        }

        [Test]
        public void AppendWithNullTailSequence()
        {
            Assert.ThrowsArgumentNullException("source", () =>
                MoreEnumerable.Append(null, "tail"));
        }

        [Test]
        public void AppendWithNullTail()
        {
            string[] head = { "first", "second" };
            string tail = null;
            var whole = head.Append(tail);
            whole.AssertSequenceEqual("first", "second", null);
        }

        [Test]
        public void AppendIsLazyInHeadSequence()
        {
            new BreakingSequence<string>().Append("tail");
        }
    }
}
