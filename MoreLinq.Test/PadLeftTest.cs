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
    using System.Linq;
    using System.Collections.Generic;

    [TestFixture]
    public class PadLeftTest
    {
        // PadLeft(source, width)

        [Test]
        public void PadLeftWithNegativeWidth()
        {
            Assert.ThrowsArgumentException("width", () => new int[0].PadLeft(-1));
        }

        [Test]
        public void PadLeftIsLazy()
        {
            new BreakingSequence<int>().PadLeft(0);
        }

        [TestCase(new[] { 123, 456, 789 }, 2, new[] {           123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 3, new[] {           123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 4, new[] {        0, 123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 5, new[] {   0,   0, 123, 456, 789 })]
        public void PadLeft(ICollection<int> source, int width, IEnumerable<int> expected)
        {
            AssertPadLeft(source, x => x.PadLeft(width), expected);
        }

        // PadLeft(source, width, padding)

        [Test]
        public void PadLeftWithPaddingWithNegativeWidth()
        {
            Assert.ThrowsArgumentException("width", () => new int[0].PadLeft(-1, 1));
        }

        [Test]
        public void PadLeftWithPaddingIsLazy()
        {
            new BreakingSequence<int>().PadLeft(0, -1);
        }

        [TestCase(new[] { 123, 456, 789 }, 2, new[] {           123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 3, new[] {           123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 4, new[] {       -1, 123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 5, new[] {  -1,  -1, 123, 456, 789 })]
        public void PadLeftWithPadding(ICollection<int> source, int width, IEnumerable<int> expected)
        {
            AssertPadLeft(source, x => x.PadLeft(width, -1), expected);
        }

        // PadLeft(source, width, paddingSelector)

        [Test]
        public void PadLeftWithSelectorWithNegativeWidth()
        {
            Assert.ThrowsArgumentException("width", () => new int[0].PadLeft(-1, x => x));
        }

        [Test]
        public void PadLeftWithSelectorIsLazy()
        {
            new BreakingSequence<int>().PadLeft(0, x => x);
        }

        [TestCase(new[] { 123, 456, 789 }, 2, new[] {                    123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 3, new[] {                    123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 4, new[] {                 0, 123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 5, new[] {            0,  -1, 123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 6, new[] {        0, -1,  -4, 123, 456, 789 })]
        [TestCase(new[] { 123, 456, 789 }, 7, new[] {   0,  -1, -4,  -9, 123, 456, 789 })]
        public void PadLeftWithSelector(ICollection<int> source, int width, IEnumerable<int> expected)
        {
            AssertPadLeft(source, x => x.PadLeft(width, y => y * -y), expected);
        }

        static void AssertPadLeft<T>(ICollection<T> col, Func<IEnumerable<T>, IEnumerable<T>> selector, IEnumerable<T> expected) 
        {
            // test that the behaviour of PadLeft does not change when passed a collection
            selector(col).AssertSequenceEqual(expected);
            selector(col.Select(x => x)).AssertSequenceEqual(expected);
        }
    }
}
