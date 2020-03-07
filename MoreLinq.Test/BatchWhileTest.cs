#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2020 Kir_Antipov. All rights reserved.
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
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class BatchWhileTest
    {
        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingCollection)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        public void BatchEmptySource(SourceKind kind)
        {
            var batches = Enumerable.Empty<object>().ToSourceKind(kind).BatchWhile((element, bucket) => true);
            Assert.That(batches, Is.Empty);
        }

        [Test]
        public void BatchUnevenlyDivisbleSequenceIntoSeparateBuckets()
        {
            var result = new[] { 1, 2, 3, 4, 5 }.BatchWhile((element, bucket) => false);
            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1);
            reader.Read().AssertSequenceEqual(2);
            reader.Read().AssertSequenceEqual(3);
            reader.Read().AssertSequenceEqual(4);
            reader.Read().AssertSequenceEqual(5);
            reader.ReadEnd();
        }

        [Test]
        public void BatchEvenlyDivisbleSequenceIntoSeparateBuckets()
        {
            var result = new[] { 1, 2, 3, 4 }.BatchWhile((element, bucket) => false);
            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1);
            reader.Read().AssertSequenceEqual(2);
            reader.Read().AssertSequenceEqual(3);
            reader.Read().AssertSequenceEqual(4);
            reader.ReadEnd();
        }

        [Test]
        public void BatchUnevenlyDivisbleSequenceIntoOneBucket()
        {
            var result = new[] { 1, 2, 3, 4, 5 }.BatchWhile((element, bucket) => true);
            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1, 2, 3, 4, 5);
            reader.ReadEnd();
        }

        [Test]
        public void BatchEvenlyDivisbleSequenceIntoOneBucket()
        {
            var result = new[] { 1, 2, 3, 4 }.BatchWhile((element, bucket) => true);
            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1, 2, 3, 4);
            reader.ReadEnd();
        }

        [Test]
        public void BatchWhileSequenceTransformingResult()
        {
            var result = new[] { 1, 2, 3, 1, 2, 3, 4, 1, 2, 3, 4, 5 }.BatchWhile((element, bucket) => element > bucket[bucket.Count - 1], bucket => bucket.Sum());
            result.AssertSequenceEqual(6, 10, 15);
        }

        [TestCase(new[] { 1, 2, 3, 4 }                  , ExpectedResult = 1)]
        [TestCase(new[] { 1, 2, 2, 3, 4 }               , ExpectedResult = 2)]
        [TestCase(new[] { 1, 2, 2, 3, 4, 4, 4 }         , ExpectedResult = 3)]
        [TestCase(new[] { 1, 1, 1, 1, 2, 2, 3, 4, 4, 4 }, ExpectedResult = 4)]
        public int FindMaxLengthOfSubsequenceOfRepeatingNumbersInArray(int[] numbers)
        {
            return numbers.BatchWhile((x, bucket) => x == bucket[bucket.Count - 1], bucket => bucket.Count).Max();
        }

        [TestCase(new[] { 1, 2, 3, 4 }                  , ExpectedResult = 1)]
        [TestCase(new[] { 1, 2, 2, 3, 4 }               , ExpectedResult = 2)]
        [TestCase(new[] { 1, 2, 2, 3, 4, 4, 4 }         , ExpectedResult = 3)]
        [TestCase(new[] { 1, 1, 1, 1, 2, 2, 3, 4, 4, 4 }, ExpectedResult = 4)]
        public int FindMaxLengthOfSubsequenceOfRepeatingNumbersInSequence(IEnumerable<int> numbers)
        {
            return numbers.Select(x => x).BatchWhile((x, bucket) => x == bucket[bucket.Count - 1], bucket => bucket.Count).Max();
        }
    }
}
