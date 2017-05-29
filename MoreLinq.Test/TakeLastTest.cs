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
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class TakeLastTest
    {
        [Test]
        public void TakeLast()
        {
            void Test(IEnumerable<int> source) 
                => source.TakeLast(3).AssertSequenceEqual(78, 910, 1112);
            
            var collection = new[]{ 12, 34, 56, 78, 910, 1112 };

            Test(collection);
            Test(collection.Select(x => x));
        }

        [Test]
        public void TakeLastOnSequenceShortOfCount()
        {
            void Test(IEnumerable<int> source) 
                => source.TakeLast(5).AssertSequenceEqual(12, 34, 56);

            var collection = new[] { 12, 34, 56 };

            Test(collection);
            Test(collection.Select(x => x));
        }

        [Test]
        public void TakeLastWithNegativeCount()
        {
            void Test(IEnumerable<int> source) 
                => Assert.IsFalse(source.TakeLast(-2).GetEnumerator().MoveNext());

            var collection = new[] { 12, 34, 56 };

            Test(collection);
            Test(collection.Select(x => x));
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

            using (var seq = new[]{ 1,2,3 }.AsTestingSequence())
            {
                seq.TakeLast(1).Consume();
            }
        }
    }
}
