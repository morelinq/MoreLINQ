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
            AssertThrowsArgument.Exception("width", () => new int[0].PadStart(-1));
        }

        [Test]
        public void PadStartIsLazy()
        {
            new BreakingSequence<int>().PadStart(0);
        }

        [TestCase(new[] { 123, 456, 789 }, 2, new[] {           123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 3, new[] {           123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 4, new[] {        0, 123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 5, new[] {   0,   0, 123, 456, 789 })]
        public void PadStart(ICollection<int> source, int width, IEnumerable<int> expected)
        {
            AssertEqual(source, x => x.PadStart(width), expected);
        }

        // PadStart(source, width, padding)

        [Test]
        public void PadStartWithPaddingWithNegativeWidth()
        {
            AssertThrowsArgument.Exception("width", () => new int[0].PadStart(-1, 1));
        }

        [Test]
        public void PadStartWithPaddingIsLazy()
        {
            new BreakingSequence<int>().PadStart(0, -1);
        }

        [TestCase(new[] { 123, 456, 789 }, 2, new[] {           123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 3, new[] {           123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 4, new[] {       -1, 123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 5, new[] {  -1,  -1, 123, 456, 789 })]
        public void PadStartWithPadding(ICollection<int> source, int width, IEnumerable<int> expected)
        {
            AssertEqual(source, x => x.PadStart(width, -1), expected);
        }

        // PadStart(source, width, paddingSelector)

        [Test]
        public void PadStartWithSelectorWithNegativeWidth()
        {
            AssertThrowsArgument.Exception("width", () => new int[0].PadStart(-1, x => x));
        }

        [Test]
        public void PadStartWithSelectorIsLazy()
        {
            new BreakingSequence<int>().PadStart(0, BreakingFunc.Of<int, int>());
        }

        [TestCase(new[] { 123, 456, 789 }, 2, new[] {                    123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 3, new[] {                    123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 4, new[] {                 0, 123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 5, new[] {            0,  -1, 123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 6, new[] {        0, -1,  -4, 123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 7, new[] {   0,  -1, -4,  -9, 123, 456, 789 })]
        public void PadStartWithSelector(ICollection<int> source, int width, IEnumerable<int> expected)
        {
            AssertEqual(source, x => x.PadStart(width, y => y * -y), expected);
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
