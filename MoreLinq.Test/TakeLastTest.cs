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
    using NUnit.Framework;
    using System.Linq;
    using System.Collections.Generic;
    using System;

    [TestFixture]
    public class TakeLastTest
    {
        [Test]
        public void TakeLast()
        {
            AssertEqual(new[]{ 12, 34, 56, 78, 910, 1112 }, x => x.TakeLast(3), result =>
            result.AssertSequenceEqual(78, 910, 1112));
        }

        [Test]
        public void TakeLastOnSequenceShortOfCount()
        {
            AssertEqual(new[] { 12, 34, 56 }, x => x.TakeLast(5), result =>
            result.AssertSequenceEqual(12, 34, 56));
        }

        [Test]
        public void TakeLastWithNegativeCount()
        {
            AssertEqual(new[] { 12, 34, 56 }, x => x.TakeLast(-2), result =>
            Assert.IsFalse(result.GetEnumerator().MoveNext()));
        }

        [Test]
        public void TakeLastIsLazy()
        {
            new BreakingSequence<object>().TakeLast(1);
        }

        [Test]
        public void TakeLastDisposesSequenceEnumerator()
        {
            using (var seq = TestingSequence.Of(1,2,3))
            {
                seq.TakeLast(1).Consume();
            }
        }

        [Test]
        public void TakeLastOptimizedForCollections()
        {
            var collection = new OnceEnumerationCollection<int>(Enumerable.Range(1, 10));

            foreach (var _ in collection.TakeLast(3))
            {
                break;
            }

            Assert.IsTrue(collection.Count > 0);
        }

        static void AssertEqual<T>(ICollection<T> input, Func<IEnumerable<T>, IEnumerable<T>> op, Action<IEnumerable<T>> action)
        {
            // Test that the behaviour does not change whether a collection
            // or a sequence is used as the source.

            action(op(input));
            action(op(input.Select(x => x)));
        }
    }
}