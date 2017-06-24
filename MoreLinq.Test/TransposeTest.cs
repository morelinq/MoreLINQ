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

using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MoreLinq.Test
{
    [TestFixture]
    public class TransposeTest
    {
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
                Assert.That(matrix.Transpose(), Is.EqualTo(expectations));
            }

            innerTestSequences.ForEach(seq => (seq as IDisposable).Dispose());
        }

        [Test]
        public void TransposeWithDifferentsLengthEnumerables()
        {
            var matrix = new[]
            {
                new int[] { 10, 11 },
                new int[] { 20 },
                new int[] { },
                new int[] { 30, 31, 32 }
            };

            var innerTestSequences = matrix.Select(x => x.AsTestingSequence()).ToList();
            
            var expectations = new[]
            {
                new int[] { 10, 20, 30 },
                new int[] { 11, 31 },
                new int[] { 32 }
            };

            using (var test = innerTestSequences.AsTestingSequence())
            {
                Assert.That(matrix.Transpose(), Is.EqualTo(expectations));
            }

            innerTestSequences.ForEach(seq => (seq as IDisposable).Dispose());
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
        public void TransposeWithAllEnumerablesInfinite()
        {
            const int takeRow = 10, takeLine = 20;

            var matrix = MoreEnumerable.Generate(1, n => n + 1)
                                       .Select(n => new[] { n }.Repeat());

            var result = matrix.Transpose().Take(takeRow).Select(x => x.Take(takeLine));

            var expectations = Enumerable.Repeat(Enumerable.Range(1, takeLine), takeRow);

            Assert.That(result, Is.EqualTo(expectations));
        }


        // add test pra quando inner sequence é nulo null

        [TestCase(2, 3, 5)]
        [TestCase(5, 10, 15)]
        [TestCase(4, 6, 3)]
        public void TransposeWithSomeEnumerablesInfinite(int count, int takeRow, int takeLine)
        {
            var naturals = MoreEnumerable.Generate(1, n => n + 1);
            var matrix = naturals.Select(x => x % 2 == 0 ? Enumerable.Repeat(x, count)
                                                         : new [] { x }.Repeat());

            var tranposedSequenceWithoutPadding = naturals.Where(x => x % 2 != 0).Take(takeLine);

            var expectations = Enumerable.Repeat(Enumerable.Range(1, takeLine), count)
                                         .Concat(new [] {tranposedSequenceWithoutPadding }.Repeat())
                                         .Take(takeRow);

            var result = matrix.Transpose().Take(takeRow).Select(x => x.Take(takeLine));

            Assert.That(result, Is.EqualTo(expectations));
        }


        [Test]
        public void TransposeOrderSequenceAreIteratedIsIrrelevant()
        {
            var matrix = new[]
            {
                new int[] { 10, 11 },
                new int[] { 20 },
                new int[] { },
                new int[] { 30, 31, 32 }
            };

            var transpose = matrix.Transpose().Take(2).ToList();

            Assert.That(transpose[1], Is.EqualTo(new int[] { 11, 31 }));
            Assert.That(transpose[0], Is.EqualTo(new int[] { 10, 20, 30 }));
        }

        [Test]
        public void TransposeSequencesAreLazies()
        {
            var matrix = new[]
            {
                new int[] { 10, 11, 12 },
                new int[] { 30, 31, 32 }.Select<int, int>(x => throw new Exception())
            };

            var first = matrix.First();
            var count = first.Count();

            matrix.Transpose().Take(count).ForEach((seq, i) => 
            {
                Assert.That(seq.First(), Is.EqualTo(first.ElementAt(i)));
            });
        }
    }
}
