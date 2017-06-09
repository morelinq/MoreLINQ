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
        public void PadLeftNegativeWidth()
        {
            Assert.ThrowsArgumentException("width",() => new int[0].PadLeft(-1));
        }

        [Test]
        public void PadLeftIsLazy()
        {
            new BreakingSequence<int>().PadLeft(0);
        }

        [Test]
        public void PadLeftWideSourceSequence()
        {
            AssertPadLeft(new[] { 123, 456, 789 }, x => x.PadLeft(2), new[] { 123, 456, 789 });
        }

        [Test]
        public void PadLeftEqualSourceSequence()
        {
            AssertPadLeft(new[] { 123, 456, 789 }, x => x.PadLeft(3), new[] { 123, 456, 789 });
        }

        [Test]
        public void PadLeftNarrowSourceSequence()
        {
            AssertPadLeft(new[] { 123, 456, 789 }, x => x.PadLeft(5), new[] { 0, 0, 123, 456, 789 });
        }

        // PadLeft(source, width, padding)

        [Test]
        public void PadLeftPaddingNegativeWidth()
        {
            Assert.ThrowsArgumentException("width",() => new int[0].PadLeft(-1, 1));
        }

        [Test]
        public void PadLeftPaddingIsLazy()
        {
            new BreakingSequence<int>().PadLeft(0, -1);
        }

        [Test]
        public void PadLeftPaddingWideSourceSequence()
        {
            AssertPadLeft(new[] { 123, 456, 789 }, x => x.PadLeft(2, -1), new[] { 123, 456, 789 });
        }

        [Test]
        public void PadLeftPaddingEqualSourceSequence()
        {
            AssertPadLeft(new[] { 123, 456, 789 }, x => x.PadLeft(3, -1), new[] { 123, 456, 789 });
        }

        [Test]
        public void PadLeftPaddingNarrowSourceSequence()
        {
            AssertPadLeft(new[] { 123, 456, 789 }, x => x.PadLeft(5, -1), new[] { -1, -1, 123, 456, 789 });
        }

        // PadLeft(source, width, paddingSelector)
        
        [Test]
        public void PadLeftSelectorNegativeWidth()
        {
            Assert.ThrowsArgumentException("width",() => new int[0].PadLeft(-1, x => x));
        }

        [Test]
        public void PadLeftSelectorIsLazy()
        {
            new BreakingSequence<int>().PadLeft(0, x => x);
        }

        [Test]
        public void PadLeftSelectorWideSourceSequence()
        {
            AssertPadLeft(new[] { 123, 456, 789 }, x => x.PadLeft(2, y => y * y), new[] { 123, 456, 789 });
        }

        [Test]
        public void PadLeftSelectorEqualSourceSequence()
        {
            AssertPadLeft(new[] { 123, 456, 789 }, x => x.PadLeft(3, y => y * y), new[] { 123, 456, 789 });
        }

        [Test]
        public void PadLeftSelectorNarrowSourceSequence()
        {
            AssertPadLeft(new[] { 123, 456, 789 }, x => x.PadLeft(7, y => y * y), new[] { 0, 1, 4, 9, 123, 456, 789 });
        }

        static void AssertPadLeft<T>(ICollection<T> col, Func<IEnumerable<T>, IEnumerable<T>> selector, IEnumerable<T> expected) 
        {
            // test that the behaviour of PadLeft does not change when passed a collection
            selector(col).AssertSequenceEqual(expected);
            selector(col.Select(x => x)).AssertSequenceEqual(expected);
        }
    }
}
