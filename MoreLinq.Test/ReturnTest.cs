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

namespace MoreLinq.Test
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;

    public class ReturnTest
    {
        static class SomeSingleton
        {
            public static readonly object Item = new();
            public static readonly IEnumerable<object> Sequence = MoreEnumerable.Return(Item);
            public static IList<object> List => (IList<object>)Sequence;
            public static ICollection<object> Collection => (ICollection<object>)Sequence;
        }

        static class NullSingleton
        {
            public static readonly IEnumerable<object?> Sequence = MoreEnumerable.Return<object?>(null);
            public static IList<object?> List => (IList<object?>)Sequence;
        }

        [Test]
        public void TestResultingSequenceContainsSingle()
        {
            Assert.That(SomeSingleton.Sequence.Count(), Is.EqualTo(1));
        }

        [Test]
        public void TestResultingSequenceContainsTheItemProvided()
        {
            Assert.That(SomeSingleton.Sequence, Has.Member(SomeSingleton.Item));
        }

        [Test]
        public void TestResultingListHasCountOne()
        {
            Assert.That(SomeSingleton.List.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestContainsReturnsTrueWhenTheResultingSequenceContainsTheItemProvided()
        {
            Assert.That(SomeSingleton.Sequence.Contains(SomeSingleton.Item), Is.True);
        }

        [Test]
        public void TestContainsDoesNotThrowWhenTheItemContainedIsNull()
        {
            Assert.That(() => SomeSingleton.Sequence.Contains(null), Throws.Nothing);
        }

        [Test]
        public void TestContainsDoesNotThrowWhenTheItemProvidedIsNull()
        {
            Assert.That(() => NullSingleton.Sequence.Contains(new object()), Throws.Nothing);
        }

        [Test]
        public void TestIndexOfDoesNotThrowWhenTheItemProvidedIsNull()
        {
            Assert.That(() => SomeSingleton.List.IndexOf(new object()), Throws.Nothing);
        }

        [Test]
        public void TestIndexOfDoesNotThrowWhenTheItemContainedIsNull()
        {
            Assert.That(() => NullSingleton.List.IndexOf(null), Throws.Nothing);
        }

        [Test]
        public void TestCopyToSetsTheValueAtTheIndexToTheItemContained()
        {
            var first = new object();
            var third = new object();

            var array = new[]
            {
                first,
                new object(),
                third
            };

            SomeSingleton.List.CopyTo(array, 1);

            Assert.That(array[0], Is.EqualTo(first));
            Assert.That(array[1], Is.EqualTo(SomeSingleton.Item));
            Assert.That(array[2], Is.EqualTo(third));
        }

        [Test]
        public void TestResultingCollectionIsReadOnly()
        {
            Assert.That(SomeSingleton.Collection.IsReadOnly, Is.True);
        }

        [Test]
        public void TestResultingCollectionHasCountOne()
        {
            Assert.That(SomeSingleton.Collection.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestIndexZeroContainsTheItemProvided()
        {
            Assert.That(SomeSingleton.List[0], Is.EqualTo(SomeSingleton.Item));
        }

        [Test]
        public void TestIndexOfTheItemProvidedIsZero()
        {
            Assert.That(SomeSingleton.List.IndexOf(SomeSingleton.Item), Is.EqualTo(0));
        }

        [Test]
        public void TestIndexOfAnItemNotContainedIsNegativeOne()
        {
            Assert.That(SomeSingleton.List.IndexOf(new object()), Is.EqualTo(-1));
        }

        static IEnumerable<ITestCaseData> UnsupportedActions(string testName) =>
            from ma in new (string MethodName, Action Action)[]
            {
                ("Add"     , () => SomeSingleton.List.Add(new object())),
                ("Clear"   ,       SomeSingleton.Collection.Clear),
                ("Remove"  , () => SomeSingleton.Collection.Remove(SomeSingleton.Item)),
                ("RemoveAt", () => SomeSingleton.List.RemoveAt(0)),
                ("Insert"  , () => SomeSingleton.List.Insert(0, new object())),
                ("Index"   , () => SomeSingleton.List[0] = new object()),
            }
            select new TestCaseData(ma.Action).SetName($"{testName}({ma.MethodName})");

        [TestCaseSource(nameof(UnsupportedActions), new object[] { nameof(TestUnsupportedMethodShouldThrow) })]
        public void TestUnsupportedMethodShouldThrow(Action unsupportedAction)
        {
            Assert.That(() => unsupportedAction(), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void TestIndexingPastZeroShouldThrow()
        {
            Assert.That(() => SomeSingleton.List[1], Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
    }
}
