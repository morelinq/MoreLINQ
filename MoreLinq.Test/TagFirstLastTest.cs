#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2013 Atif Aziz. All rights reserved.
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

    [TestFixture]
    public class TagFirstLastTest
    {
        [Test]
        public void TagFirstLastDoesOneLookAhead()
        {
            using var source = MoreEnumerable.From(() => 123, () => 456, BreakingFunc.Of<int>()).AsTestingSequence();
            var result = source.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast });

            result.Take(1).Consume();
        }

        [Test]
        public void TagFirstLastIsLazy()
        {
            _ = new BreakingSequence<object>().TagFirstLast(BreakingFunc.Of<object, bool, bool, object>());
        }

        [Test]
        public void TagFirstLastWithSourceSequenceOfZero()
        {
            using var source = Enumerable.Empty<int>().AsTestingSequence();
            var result = source.TagFirstLast(BreakingFunc.Of<int, bool, bool, int>());

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void TagFirstLastWithSourceSequenceOfOne()
        {
            using var source = new[] { 123 }.AsTestingSequence();
            var result = source.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast });

            result.AssertSequenceEqual(new { Item = 123, IsFirst = true, IsLast = true });
        }

        [Test]
        public void TagFirstLastWithSourceSequenceOfTwo()
        {
            using var source = new[] { 123, 456 }.AsTestingSequence();
            var result = source.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast });

            result.AssertSequenceEqual(new { Item = 123, IsFirst = true,  IsLast = false },
                                       new { Item = 456, IsFirst = false, IsLast = true });
        }

        [Test]
        public void TagFirstLastWithSourceSequenceOfThree()
        {
            using var source = new[] { 123, 456, 789 }.AsTestingSequence();
            var result = source.TagFirstLast((item, isFirst, isLast) => new { Item = item, IsFirst = isFirst, IsLast = isLast });

            result.AssertSequenceEqual(new { Item = 123, IsFirst = true,  IsLast = false },
                                       new { Item = 456, IsFirst = false, IsLast = false },
                                       new { Item = 789, IsFirst = false, IsLast = true  });
        }
    }
}
