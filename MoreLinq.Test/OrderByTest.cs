#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2010 Leopold Bushkin. All rights reserved.
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
    using System.Globalization;
    using NUnit.Framework;

    /// <summary>
    /// Verify the behavior of the OrderBy/ThenBy operators
    /// </summary>
    [TestFixture]
    public class OrderByTests
    {
        /// <summary>
        /// Verify that OrderBy preserves the selector
        /// </summary>
        [Test]
        public void TestOrderBySelectorPreserved()
        {
            var sequenceAscending = Enumerable.Range(1, 100);
            var sequenceDescending = sequenceAscending.Reverse();

            var resultAsc1 = sequenceAscending.OrderBy(x => x, OrderByDirection.Descending);
            var resultAsc2 = sequenceAscending.OrderByDescending(x => x);
            // ensure both order by operations produce identical results
            Assert.That(resultAsc1, Is.EqualTo(resultAsc2));

            var resultDes1 = sequenceDescending.OrderBy(x => x, OrderByDirection.Ascending);
            var resultDes2 = sequenceDescending.OrderBy(x => x);
            // ensure both order by operations produce identical results
            Assert.That(resultDes1, Is.EqualTo(resultDes2));
        }

        static readonly IComparer<string> NumericStringComparer =
            Comparer<string>.Create((a, b) => int.Parse(a, CultureInfo.InvariantCulture).CompareTo(int.Parse(b, CultureInfo.InvariantCulture)));

        /// <summary>
        /// Verify that OrderBy preserves the comparer
        /// </summary>
        [Test]
        public void TestOrderByComparerPreserved()
        {
            var sequence = Enumerable.Range(1, 100);
            var sequenceAscending = sequence.Select(x => x.ToInvariantString());
            var sequenceDescending = sequenceAscending.Reverse();

            var comparer = NumericStringComparer;

            var resultAsc1 = sequenceAscending.OrderBy(x => x, comparer, OrderByDirection.Descending);
            var resultAsc2 = sequenceAscending.OrderByDescending(x => x, comparer);
            // ensure both order by operations produce identical results
            Assert.That(resultAsc1, Is.EqualTo(resultAsc2));
            // ensure comparer was applied in the order by evaluation
            Assert.That(resultAsc1, Is.EqualTo(sequenceDescending));

            var resultDes1 = sequenceDescending.OrderBy(x => x, comparer, OrderByDirection.Ascending);
            var resultDes2 = sequenceDescending.OrderBy(x => x, comparer);
            // ensure both order by operations produce identical results
            Assert.That(resultDes1, Is.EqualTo(resultDes2));
            // ensure comparer was applied in the order by evaluation
            Assert.That(resultDes1, Is.EqualTo(sequenceAscending));
        }

        /// <summary>
        /// Verify that ThenBy preserves the selector
        /// </summary>
        [Test]
        public void TestThenBySelectorPreserved()
        {
            var sequence = new[]
                               {
                                   new {A = 2, B = 0},
                                   new {A = 1, B = 5},
                                   new {A = 2, B = 2},
                                   new {A = 1, B = 3},
                                   new {A = 1, B = 4},
                                   new {A = 2, B = 1},
                               };

            var resultA1 = sequence.OrderBy(x => x.A, OrderByDirection.Ascending)
                                     .ThenBy(y => y.B, OrderByDirection.Ascending);
            var resultA2 = sequence.OrderBy(x => x.A)
                                   .ThenBy(y => y.B);
            // ensure both produce the same order
            Assert.That(resultA1, Is.EqualTo(resultA2));

            var resultB1 = sequence.OrderBy(x => x.A, OrderByDirection.Ascending)
                                     .ThenBy(y => y.B, OrderByDirection.Descending);
            var resultB2 = sequence.OrderBy(x => x.A)
                                   .ThenByDescending(y => y.B);
            // ensure both produce the same order
            Assert.That(resultB1, Is.EqualTo(resultB2));
        }

        /// <summary>
        /// Verify that ThenBy preserves the comparer
        /// </summary>
        [Test]
        public void TestThenByComparerPreserved()
        {
            var sequence = new[]
                               {
                                   new {A = "2", B = "0"},
                                   new {A = "1", B = "5"},
                                   new {A = "2", B = "2"},
                                   new {A = "1", B = "3"},
                                   new {A = "1", B = "4"},
                                   new {A = "2", B = "1"},
                               };

            var comparer = NumericStringComparer;

            var resultA1 = sequence.OrderBy(x => x.A, comparer, OrderByDirection.Ascending)
                                     .ThenBy(y => y.B, comparer, OrderByDirection.Ascending);
            var resultA2 = sequence.OrderBy(x => x.A, comparer)
                                   .ThenBy(y => y.B, comparer);
            // ensure both produce the same order
            Assert.That(resultA1, Is.EqualTo(resultA2));

            var resultB1 = sequence.OrderBy(x => x.A, comparer, OrderByDirection.Ascending)
                                     .ThenBy(y => y.B, comparer, OrderByDirection.Descending);
            var resultB2 = sequence.OrderBy(x => x.A, comparer)
                                   .ThenByDescending(y => y.B, comparer);
            // ensure both produce the same order
            Assert.That(resultB1, Is.EqualTo(resultB2));
        }
    }
}
