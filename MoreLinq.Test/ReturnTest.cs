#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Mitch Bodmer. All rights reserved.
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

using System;

namespace MoreLinq.Test
{
    using NUnit.Framework;
    using System.Collections.Generic;

    class ReturnTest
    {
        [Test]
        public void TestResultingSequenceContainsSingle()
        {
            Assert.That(MoreEnumerable.Return(new object()).Count(), Is.EqualTo(1));
        }

        [Test]
        public void TestResultingSequenceContainsTheItemProvided()
        {
            var item = new object();
            Assert.That(MoreEnumerable.Return(item), Has.Member(item));
        }

        [Test]
        public void TestResultingListHasCountOne()
        {
            Assert.That(((IList<object>) MoreEnumerable.Return(new object())).Count, Is.EqualTo(1));
        }

        [Test]
        public void TestContainsReturnsTrueWhenTheResultingSequenceContainsTheItemProvided()
        {
            var item = new object();
            Assert.That(MoreEnumerable.Return(item).Contains(item), Is.True);
        }

        [Test]
        public void TestContainsDoesNotThrowWhenTheItemContainedIsNull()
        {
            Assert.That(() => MoreEnumerable.Return(new object()).Contains(null), Throws.Nothing);
        }

        [Test]
        public void TestContainsDoesNotThrowWhenTheItemProvidedIsNull()
        {
            Assert.That(() => MoreEnumerable.Return<object>(null).Contains(new object()), Throws.Nothing);
        }

        [Test]
        public void TestIndexOfDoesNotThrowWhenTheItemProvidedIsNull()
        {
            Assert.That(() => ((IList<object>) MoreEnumerable.Return<object>(null)).IndexOf(new object()),
                Throws.Nothing);
        }

        [Test]
        public void TestIndexOfDoesNotThrowWhenTheItemContainedIsNull()
        {
            Assert.That(() => ((IList<object>)MoreEnumerable.Return(new object())).IndexOf(null),
                Throws.Nothing);
        }

        [Test]
        public void TestCopyToSetsTheValueAtTheIndexToTheItemContained()
        {
            var array = new object[1];
            var item = new object();

            ((IList<object>)MoreEnumerable.Return(item)).CopyTo(array, 0);

            Assert.That(array[0], Is.EqualTo(item));
        }

        [Test]
        public void TestResultingCollectionIsReadOnly()
        {
            Assert.That(((ICollection<object>)MoreEnumerable.Return(new object())).IsReadOnly, Is.True);
        }

        [Test]
        public void TestResultingCollectionHasCountOne()
        {
            Assert.That(((ICollection<object>)MoreEnumerable.Return(new object())).Count, Is.EqualTo(1));
        }

        [Test]
        public void TestIndexZeroContainsTheItemProvided()
        {
            var item = new object();
            Assert.That(((IList<object>) MoreEnumerable.Return(item))[0], Is.EqualTo(item));
        }

        [Test]
        public void TestIndexOfTheItemProvidedIsZero()
        {
            var item = new object();
            Assert.That(((IList<object>)MoreEnumerable.Return(item)).IndexOf(item), Is.EqualTo(0));
        }

        [Test]
        public void TestIndexOfAnItemNotContainedIsNegativeOne()
        {
            Assert.That(((IList<object>)MoreEnumerable.Return(new object())).IndexOf(new object()), Is.EqualTo(-1));
        }

        private static readonly IEnumerable<Action> UnsupportedActions =
            new Action[]
            {
                () => ((IList<object>) MoreEnumerable.Return(new object())).Add(new object()),
                () => ((IList<object>) MoreEnumerable.Return(new object())).Clear(),
                () => ((ICollection<object>) MoreEnumerable.Return(new object())).Remove(new object()),
                () => ((IList<object>) MoreEnumerable.Return(new object())).RemoveAt(0),
                () => ((IList<object>) MoreEnumerable.Return(new object())).Insert(0, new object()),
                () => ((IList<object>) MoreEnumerable.Return(new object()))[0] = new object(),
            };

        [TestCaseSource(nameof(UnsupportedActions))]
        public void TestUnsupportedMethodsShouldThrow(Action unsupportedAction)
        {
            Assert.That(() => unsupportedAction(), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void TestIndexingPastZeroShouldThrow()
        {
            Assert.That(() => ((IList<object>)MoreEnumerable.Return(new object()))[1], Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
    }
}
