using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
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
            Assert.IsTrue(resultAsc1.SequenceEqual(resultAsc2));

            var resultDes1 = sequenceDescending.OrderBy(x => x, OrderByDirection.Ascending);
            var resultDes2 = sequenceDescending.OrderBy(x => x);
            // ensure both order by operations produce identical results
            Assert.IsTrue(resultDes1.SequenceEqual(resultDes2));
        }

        /// <summary>
        /// Verify that OrderBy preserves the comparer
        /// </summary>
        [Test]
        public void TestOrderByComparerPreserved()
        {
            var sequence = Enumerable.Range(1, 100);
            var sequenceAscending = sequence.Select(x => x.ToString());
            var sequenceDescending = sequenceAscending.Reverse();

            var comparer = new ComparerFunc<string>((a, b) => int.Parse(a).CompareTo(int.Parse(b)));

            var resultAsc1 = sequenceAscending.OrderBy(x => x, comparer, OrderByDirection.Descending);
            var resultAsc2 = sequenceAscending.OrderByDescending(x => x, comparer);
            // ensure both order by operations produce identical results
            Assert.IsTrue(resultAsc1.SequenceEqual(resultAsc2));
            // ensure comparer was applied in the order by evaluation
            Assert.IsTrue(resultAsc1.SequenceEqual(sequenceDescending));

            var resultDes1 = sequenceDescending.OrderBy(x => x, comparer, OrderByDirection.Ascending);
            var resultDes2 = sequenceDescending.OrderBy(x => x, comparer);
            // ensure both order by operations produce identical results
            Assert.IsTrue(resultDes1.SequenceEqual(resultDes2));
            // ensure comparer was applied in the order by evaluation
            Assert.IsTrue(resultDes1.SequenceEqual(sequenceAscending));
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
            Assert.IsTrue(resultA1.SequenceEqual(resultA2));

            var resultB1 = sequence.OrderBy(x => x.A, OrderByDirection.Ascending)
                                     .ThenBy(y => y.B, OrderByDirection.Descending);
            var resultB2 = sequence.OrderBy(x => x.A)
                                   .ThenByDescending(y => y.B);
            // ensure both produce the same order
            Assert.IsTrue(resultB1.SequenceEqual(resultB2));
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

            var comparer = new ComparerFunc<string>((a, b) => int.Parse(a).CompareTo(int.Parse(b)));

            var resultA1 = sequence.OrderBy(x => x.A, comparer, OrderByDirection.Ascending)
                                     .ThenBy(y => y.B, comparer, OrderByDirection.Ascending);
            var resultA2 = sequence.OrderBy(x => x.A, comparer)
                                   .ThenBy(y => y.B, comparer);
            // ensure both produce the same order
            Assert.IsTrue(resultA1.SequenceEqual(resultA2));

            var resultB1 = sequence.OrderBy(x => x.A, comparer, OrderByDirection.Ascending)
                                     .ThenBy(y => y.B, comparer, OrderByDirection.Descending);
            var resultB2 = sequence.OrderBy(x => x.A, comparer)
                                   .ThenByDescending(y => y.B, comparer);
            // ensure both produce the same order
            Assert.IsTrue(resultB1.SequenceEqual(resultB2));
        }
    }
}