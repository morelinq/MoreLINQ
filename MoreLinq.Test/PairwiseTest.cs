#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2012 Atif Aziz. All rights reserved.
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
    public static class PairwiseTest
    {
        public class ReturningTuples
        {
            [Test]
            public void PairwiseIsLazy()
            {
                _ = new BreakingSequence<object>().Pairwise();
            }

            [TestCase(0)]
            [TestCase(1)]
            public void PairwiseWithSequenceShorterThanTwo(int count)
            {
                var source = Enumerable.Range(0, count);
                var result = source.Pairwise();

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void PairwiseWideSourceSequence()
            {
                using var source = new[] { "a", "b", "c", "d" }.AsTestingSequence();
                var result = source.Pairwise();
                result.AssertSequenceEqual(("a", "b"), ("b", "c"), ("c", "d"));
            }
        }

        public class ReturningSomeResults
        {
            [Test]
            public void PairwiseIsLazy()
            {
                _ = new BreakingSequence<object>().Pairwise(BreakingFunc.Of<object, object, int>());
            }

            [TestCase(0)]
            [TestCase(1)]
            public void PairwiseWithSequenceShorterThanTwo(int count)
            {
                var source = Enumerable.Range(0, count);
                var result = source.Pairwise(BreakingFunc.Of<int, int, int>());

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void PairwiseWideSourceSequence()
            {
                using var source = new[] { "a", "b", "c", "d" }.AsTestingSequence();
                var result = source.Pairwise((x, y) => x + y);
                result.AssertSequenceEqual("ab", "bc", "cd");
            }
        }
    }
}
