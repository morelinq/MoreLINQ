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

namespace MoreLinq.Test
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class TransposeTest
    {
        [Test]
        public void TransposeIsLazy()
        {
            _ = new BreakingSequence<BreakingSequence<int>>().Transpose();
        }

        [Test]
        public void TransposeWithOneNullRow()
        {
            using var seq1 = TestingSequence.Of(10, 11);
            using var seq2 = TestingSequence.Of<int>();
            using var seq3 = TestingSequence.Of(30, 31, 32);
            using var matrix = TestingSequence.Of<IEnumerable<int>>(seq1, seq2, seq3, null!);

            Assert.That(() => matrix.Transpose().FirstOrDefault(),
                        Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void TransposeWithRowsOfSameLength()
        {
            var expectations = new[]
            {
                new [] { 10, 20, 30 },
                new [] { 11, 21, 31 },
                new [] { 12, 22, 32 },
                new [] { 13, 23, 33 },
            };

            using var row1 = TestingSequence.Of(10, 11, 12, 13);
            using var row2 = TestingSequence.Of(20, 21, 22, 23);
            using var row3 = TestingSequence.Of(30, 31, 32, 33);
            using var matrix = TestingSequence.Of(row1, row2, row3);

            AssertMatrix(expectations, matrix.Transpose());
        }

        [Test]
        public void TransposeWithRowsOfDifferentLengths()
        {
            var expectations = new[]
            {
                new[] { 10, 20, 30 },
                new[] { 11, 31 },
                new[] { 32 }
            };

            using var row1 = TestingSequence.Of(10, 11);
            using var row2 = TestingSequence.Of(20);
            using var row3 = TestingSequence.Of<int>();
            using var row4 = TestingSequence.Of(30, 31, 32);
            using var matrix = TestingSequence.Of(row1, row2, row3, row4);

            AssertMatrix(expectations, matrix.Transpose());
        }

        [Test]
        public void TransposeMaintainsCornerElements()
        {
            var matrix = new[]
            {
                new[] { 10, 11 },
                new[] { 20 },
                new int[0],
                new[] { 30, 31, 32 }
            };

            var traspose = matrix.Transpose();

            Assert.That(matrix.Last().Last(), Is.EqualTo(traspose.Last().Last()));
            Assert.That(matrix.First().First(), Is.EqualTo(traspose.First().First()));
        }

        [Test]
        public void TransposeWithAllRowsAsInfiniteSequences()
        {
            var matrix = MoreEnumerable.Generate(1, x => x + 1)
                                       .Where(IsPrime)
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

            AssertMatrix(expectations, result);
        }

        [Test]
        public void TransposeWithSomeRowsAsInfiniteSequences()
        {
            var matrix = MoreEnumerable.Generate(1, x => x + 1)
                                       .Where(IsPrime)
                                       .Take(3)
                                       .Select((x, i) => i == 1 ? MoreEnumerable.Generate(x, n => n * x).Take(2)
                                                                : MoreEnumerable.Generate(x, n => n * x));

            var result = matrix.Transpose().Take(5);

            var expectations = new[]
            {
                new[] { 2,    3,    5 },
                new[] { 4,    9,   25 },
                new[] { 8,        125 },
                new[] { 16,       625 },
                new[] { 32,      3125 }
            };

            AssertMatrix(expectations, result);
        }

        [Test]
        public void TransposeColumnTraversalOrderIsIrrelevant()
        {
            var matrix = new[]
            {
                new[] { 10, 11 },
                new[] { 20 },
                new int[0],
                new[] { 30, 31, 32 }
            };

            var transpose = matrix.Transpose().ToList();

            transpose[1].AssertSequenceEqual(11, 31);
            transpose[0].AssertSequenceEqual(10, 20, 30);
            transpose[2].AssertSequenceEqual(32);
        }

        [Test]
        public void TransposeConsumesRowsLazily()
        {
            var matrix = new[]
            {
                MoreEnumerable.From(() => 10, () => 11),
                MoreEnumerable.From(() => 20, () => 22),
                MoreEnumerable.From(() => 30, () => throw new TestException(), () => 31),
            };

            var result = matrix.Transpose();

            result.ElementAt(0).AssertSequenceEqual(10, 20, 30);

            Assert.That(() => result.ElementAt(1),
                        Throws.TypeOf<TestException>());
        }

        [Test]
        public void TransposeWithErroneousRowDisposesRowIterators()
        {
            using var row1 = TestingSequence.Of(10, 11);
            using var row2 = MoreEnumerable.From(() => 20,
                                                 () => throw new TestException())
                                           .AsTestingSequence();
            using var row3 = TestingSequence.Of(30, 32);
            using var matrix = TestingSequence.Of(row1, row2, row3);

            Assert.That(() => matrix.Transpose().Consume(),
                        Throws.TypeOf<TestException>());
        }

        static bool IsPrime(int number)
        {
            if (number == 1) return false;
            if (number == 2) return true;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (var i = 2; i <= boundary; ++i)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }

        static void AssertMatrix<T>(IEnumerable<IEnumerable<T>> expectation, IEnumerable<IEnumerable<T>> result)
        {
            // necessary because NUnitLite 3.6.1 (.NET 4.5) for Mono don't assert nested enumerables

            var resultList = result.ToList();
            var expectationList = expectation.ToList();

            Assert.That(resultList.Count, Is.EqualTo(expectationList.Count));

            expectationList.Zip(resultList, ValueTuple.Create)
                           .ForEach(t => t.Item1.AssertSequenceEqual(t.Item2));
        }
    }
}
