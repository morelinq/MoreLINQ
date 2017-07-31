namespace MoreLinq.Test
{
    using System.Linq;
    using NUnit.Framework;

    /// <summary>
    /// Verify the behavior of the WhereIf operator
    /// </summary>
    [TestFixture]
    public class WhereIfTest
    {
        /// <summary>
        /// Verify that WhereIf behaves in a lazy manner
        /// </summary>
        [Test]
        public void TestWhereIfIsLazy()
        {
            new BreakingSequence<int>().WhereIf(true, it => true);
            new BreakingSequence<int>().WhereIf(false, it => true);
        }

        /// <summary>
        /// Verify that WhereIf won't modify source if condition is false
        /// </summary>
        [Test]
        public void TestWhereIfWillNotModifySourceIfConditionIsFalse()
        {
            var sourceSequence = Enumerable.Empty<int>();

            var resultSequence = sourceSequence.WhereIf(false, it => true);

            Assert.AreEqual(sourceSequence, resultSequence);
        }

        /// <summary>
        /// Verify that WhereIf filters source if condition is true
        /// </summary>
        [Test]
        public void TestWhereIfFiltersSourceIfConditionIsTrue()
        {
            var sequence = Enumerable.Range(1, 10);
            var result = sequence.WhereIf(true, it => it % 2 == 0);

            Assert.IsTrue(result.SequenceEqual(new[] { 2, 4, 6, 8, 10 }));
        }
    }
}