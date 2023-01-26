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

namespace MoreLinq.Test
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class PadStartTest
    {
        // PadStart(source, width)

        [Test]
        public void PadStartWithNegativeWidth()
        {
            Assert.That(() => new int[0].PadStart(-1), Throws.ArgumentException("width"));
        }

        [Test]
        public void PadStartIsLazy()
        {
            _ = new BreakingSequence<int>().PadStart(0);
        }

        public class PadStartWithDefaultPadding
        {
            [TestCase(new[] { 123, 456, 789 }, 2, new[] {           123, 456, 789 })]
            [TestCase(new[] { 123, 456, 789 }, 3, new[] {           123, 456, 789 })]
            [TestCase(new[] { 123, 456, 789 }, 4, new[] {        0, 123, 456, 789 })]
            [TestCase(new[] { 123, 456, 789 }, 5, new[] {   0,   0, 123, 456, 789 })]
            public void ValueTypeElements(ICollection<int> source, int width, IEnumerable<int> expected)
            {
                AssertEqual(source, x => x.PadStart(width), expected);
            }

            [TestCase(new[] { "foo", "bar", "baz" }, 2, new[] {             "foo", "bar", "baz" })]
            [TestCase(new[] { "foo", "bar", "baz" }, 3, new[] {             "foo", "bar", "baz" })]
            [TestCase(new[] { "foo", "bar", "baz" }, 4, new[] {       null, "foo", "bar", "baz" })]
            [TestCase(new[] { "foo", "bar", "baz" }, 5, new[] { null, null, "foo", "bar", "baz" })]
            public void ReferenceTypeElements(ICollection<string?> source, int width, IEnumerable<string?> expected)
            {
                AssertEqual(source, x => x.PadStart(width), expected);
            }
        }

        // PadStart(source, width, padding)

        [Test]
        public void PadStartWithPaddingWithNegativeWidth()
        {
            Assert.That(() => new int[0].PadStart(-1, 1), Throws.ArgumentException("width"));
        }

        [Test]
        public void PadStartWithPaddingIsLazy()
        {
            _ = new BreakingSequence<int>().PadStart(0, -1);
        }

        public class PadStartWithPadding
        {
            [TestCase(new[] { 123, 456, 789 }, 2, new[] {           123, 456, 789 })]
            [TestCase(new[] { 123, 456, 789 }, 3, new[] {           123, 456, 789 })]
            [TestCase(new[] { 123, 456, 789 }, 4, new[] {       -1, 123, 456, 789 })]
            [TestCase(new[] { 123, 456, 789 }, 5, new[] {  -1,  -1, 123, 456, 789 })]
            public void ValueTypeElements(ICollection<int> source, int width, IEnumerable<int> expected)
            {
                AssertEqual(source, x => x.PadStart(width, -1), expected);
            }

            [TestCase(new[] { "foo", "bar", "baz" }, 2, new[] {         "foo", "bar", "baz" })]
            [TestCase(new[] { "foo", "bar", "baz" }, 3, new[] {         "foo", "bar", "baz" })]
            [TestCase(new[] { "foo", "bar", "baz" }, 4, new[] {     "", "foo", "bar", "baz" })]
            [TestCase(new[] { "foo", "bar", "baz" }, 5, new[] { "", "", "foo", "bar", "baz" })]
            public void ReferenceTypeElements(ICollection<string> source, int width, IEnumerable<string> expected)
            {
                AssertEqual(source, x => x.PadStart(width, string.Empty), expected);
            }
        }

        // PadStart(source, width, paddingSelector)

        [Test]
        public void PadStartWithSelectorWithNegativeWidth()
        {
            Assert.That(() => new int[0].PadStart(-1, x => x), Throws.ArgumentException("width"));
        }

        [Test]
        public void PadStartWithSelectorIsLazy()
        {
            _ = new BreakingSequence<int>().PadStart(0, BreakingFunc.Of<int, int>());
        }

        public class PadStartWithSelector
        {
            [TestCase(new[] { 123, 456, 789 }, 2, new[] {                    123, 456, 789 })]
            [TestCase(new[] { 123, 456, 789 }, 3, new[] {                    123, 456, 789 })]
            [TestCase(new[] { 123, 456, 789 }, 4, new[] {                 0, 123, 456, 789 })]
            [TestCase(new[] { 123, 456, 789 }, 5, new[] {            0,  -1, 123, 456, 789 })]
            [TestCase(new[] { 123, 456, 789 }, 6, new[] {        0, -1,  -4, 123, 456, 789 })]
            [TestCase(new[] { 123, 456, 789 }, 7, new[] {   0,  -1, -4,  -9, 123, 456, 789 })]
            public void ValueTypeElements(ICollection<int> source, int width, IEnumerable<int> expected)
            {
                AssertEqual(source, x => x.PadStart(width, y => y * -y), expected);
            }

            [TestCase(new[] { "foo", "bar", "baz" }, 2, new[] {                           "foo", "bar", "baz" })]
            [TestCase(new[] { "foo", "bar", "baz" }, 3, new[] {                           "foo", "bar", "baz" })]
            [TestCase(new[] { "foo", "bar", "baz" }, 4, new[] {                      "+", "foo", "bar", "baz" })]
            [TestCase(new[] { "foo", "bar", "baz" }, 5, new[] {              "+",   "++", "foo", "bar", "baz" })]
            [TestCase(new[] { "foo", "bar", "baz" }, 6, new[] {       "+",  "++",  "+++", "foo", "bar", "baz" })]
            [TestCase(new[] { "foo", "bar", "baz" }, 7, new[] { "+", "++", "+++", "++++", "foo", "bar", "baz" })]
            public void ReferenceTypeElements(ICollection<string> source, int width, IEnumerable<string> expected)
            {
                AssertEqual(source, x => x.PadStart(width, y => new string('+', y + 1)), expected);
            }
        }

        [Test]
        public void PadStartUsesCollectionCountAtIterationTime()
        {
            var queue = new Queue<int>(Enumerable.Range(1, 3));
            var result = queue.PadStart(4, -1);
            queue.Enqueue(4);
            result.AssertSequenceEqual(1, 2, 3, 4);
        }

        static void AssertEqual<T>(ICollection<T> input, Func<IEnumerable<T>, IEnumerable<T>> op, IEnumerable<T> expected)
        {
            // Test that the behaviour does not change whether a collection
            // or a sequence is used as the source.

            op(input).AssertSequenceEqual(expected);
            op(input.Select(x => x)).AssertSequenceEqual(expected);
        }
    }
}
