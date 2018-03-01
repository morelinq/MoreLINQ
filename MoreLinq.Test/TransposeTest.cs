#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Leandro F. Vieira (leandromoh). All rights reserved.
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

using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace MoreLinq.Test
{
    [TestFixture]
    public class TransposeTest
    {
        [Test]
        public void TransposeWithNullInnerSequence()
        {
            var matrix = new int[][]
            {
                new int[] { 10, 11 },
                null,
                new int[] { },
                new int[] { 30, 31, 32 }
            };

            var traspose = matrix.Transpose();

            Assert.That(matrix.First().First(), Is.EqualTo(traspose.First().First()));

            Assert.AreEqual(30, traspose.First().ElementAt(1));
        }

        [Test]
        public void TransposeWithEqualsLengthEnumerables()
        {
            var matrix = new[]
            {
                new [] { 10, 11, 12, 13 },
                new [] { 20, 21, 22, 23 },
                new [] { 30, 31, 32, 33 },
            };

            var expectations = new[]
            {
                new [] { 10, 20, 30 },
                new [] { 11, 21, 31 },
                new [] { 12, 22, 32 },
                new [] { 13, 23, 33 },
            };

            var innerTestSequences = matrix.Select(x => x.AsTestingSequence()).ToList();

            using (var test = innerTestSequences.AsTestingSequence())
            {
                AssertMatrix(test.Transpose(), expectations);
            }

            innerTestSequences.Cast<IDisposable>().ForEach(seq => seq.Dispose());
        }

        [Test]
        public void TransposeWithDifferentsLengthEnumerables()
        {
            var expectations = new[]
            {
                new int[] { 10, 20, 30 },
                new int[] { 11, 31 },
                new int[] { 32 }
            };

            using (var seq1 = TestingSequence.Of(10, 11))
            using (var seq2 = TestingSequence.Of(20))
            using (var seq3 = TestingSequence.Of<int>())
            using (var seq4 = TestingSequence.Of( 30, 31, 32 ))
            using (var matrix = TestingSequence.Of( seq1, seq2, seq3, seq4 ))
            {
                AssertMatrix(matrix.Transpose(), expectations);
            }
        }

        [Test]
        public void TransposeAssertPositions()
        {
            var matrix = new[]
            {
                new int[] { 10, 11 },
                new int[] { 20 },
                new int[] { },
                new int[] { 30, 31, 32 }
            };

            var traspose = matrix.Transpose();

            Assert.That(matrix.Last().Last(), Is.EqualTo(traspose.Last().Last()));
            Assert.That(matrix.First().First(), Is.EqualTo(traspose.First().First()));
        }

        [Test]
        public void TransposeWithAllInnerSequencesInfinite()
        {
            var matrix = MoreEnumerable.Generate(1, x => x + 1)
                                       .Where(x => isPrime(x))
                                       .Take(3)
                                       .Select(x => MoreEnumerable.Generate(x, n => n * x));

            var result = matrix.Transpose().Take(5);

            var expectations = new[]
            {
                new[] { 2,    3,    5 },
                new[] { 4,    9,   25 },
                new[] { 8,   27,  125 },
                new[] { 16,  81,  625 },
                new[] { 32, 243, 3125 }
            };

            AssertMatrix(result, expectations);
        }

        public void TransposeWithSomeSequencesInfinite()
        {
            var matrix = MoreEnumerable.Generate(1, x => x + 1)
                                       .Where(x => isPrime(x))
                                       .Take(3)
                                       .Select(x => x == 3 ? MoreEnumerable.Generate(x, n => n * x)
                                                           : MoreEnumerable.Generate(x, n => n * x).Take(3));

            var result = matrix.Transpose().Take(5);

            var expectations = new[]
            {
                new[] { 2,    3,    5 },
                new[] { 4,    9,   25 },
                new[] { 8,   27,  125 },
                new[] { 16,       625 },
                new[] { 32,      3125 }
            };

            AssertMatrix(result, expectations);
        }

        [Test]
        public void TransposeOrderSequencesAreIteratedIsIrrelevant()
        {
            var matrix = new[]
            {
                new int[] { 10, 11 },
                new int[] { 20 },
                new int[] { },
                new int[] { 30, 31, 32 }
            };

            var transpose = matrix.Transpose().ToList();

            transpose[1].AssertSequenceEqual(11, 31);
            transpose[0].AssertSequenceEqual(10, 20, 30);
            transpose[2].AssertSequenceEqual(32);
        }

        [Test]
        public void TransposeInnerSequencesAreConsumedLazies()
        {
            var matrix = new[]
            {
                MoreEnumerable.From(() => 10, () => 11),
                MoreEnumerable.From(() => 20, () => 22),
                MoreEnumerable.From(() => 30, () => throw new InvalidOperationException(), () => 31),
            };

            var result = matrix.Transpose();

            result.ElementAt(0).AssertSequenceEqual(10, 20, 30);

            Assert.Throws<InvalidOperationException>(() =>
                result.ElementAt(1));
        }

        // [Test]
        // public void TransposeSequencesAreLazies()
        // {
        //     var matrix = new[]
        //     {
        //         new int[] { 10, 11, 12 },
        //         new int[] { 30, 31, 32 }.Select<int, int>(x => throw new Exception())
        //     };

        //     var first = matrix.First();
        //     var count = first.Count();

        //     matrix.Transpose().Take(count).ForEach((seq, i) =>
        //     {
        //         Assert.That(seq.First(), Is.EqualTo(first.ElementAt(i)));
        //     });
        // }

        public static bool isPrime(int number)
        {
            if (number == 1) return false;
            if (number == 2) return true;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 2; i <= boundary; ++i)
            {
                if (number % i == 0)  return false;
            }

            return true;
        }

        public static void AssertMatrix<T>(IEnumerable<IEnumerable<T>> result, IEnumerable<IEnumerable<T>> expectation)
        {
            // necessary because NUnitLite 3.6.1 (.NET 4.5) for Mono don't assert nested enumerables

            var resultList = result.ToList();
            var expectationList = expectation.ToList();

            Assert.AreEqual(expectationList.Count, resultList.Count);

            expectationList.Zip(resultList, ValueTuple.Create)
                           .ForEach(t => t.Item1.AssertSequenceEqual(t.Item2));
        }
    }
}
