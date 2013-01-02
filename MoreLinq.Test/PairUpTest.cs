using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    /// <summary>
    /// Verify the behavior of the PairUp operator.
    /// </summary>
    [TestFixture]
    public class PairUpTest
    {
        /// <summary>
        /// Verify that PairUp behaves in a lazy manner.
        /// </summary>
        [Test]
        public void TestPairUpIsLazy()
        {
            new BreakingSequence<int>().PairUp((a, b) => a + b);
            new BreakingSequence<int>().PairUp((a, b) => a + b, PairUpImbalanceStrategy.Pad);
            new BreakingSequence<int>().PairUp((a, b) => a + b, PairUpImbalanceStrategy.Skip);
        }

        /// <summary>
        /// Verify that PairUp throws an exception if invoked on a <c>null</c> sequence.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPairUpNullSequenceException()
        {
            const IEnumerable<int> sequence = null;
            sequence.PairUp((a, b) => a + b);
        }

        /// <summary>
        /// Verify that PairUp throws an exception if invoked with a <c>null</c> projection function.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPairUpNullProjectionException()
        {
            var sequence = Enumerable.Range(1, 10);
            const Func<int, int, int> projection = null;
            sequence.PairUp(projection);
        }

        /// <summary>
        /// Verify that pairing up an empty sequence results in an empty sequence.
        /// </summary>
        [Test]
        public void TestPairUpEmptySequence()
        {
            var sequence = Enumerable.Empty<int>();
            var resultA = sequence.PairUp((a, b) => a + b);
            var resultB = sequence.PairUp((a, b) => a + b, PairUpImbalanceStrategy.Pad);
            var resultC = sequence.PairUp((a, b) => a + b, PairUpImbalanceStrategy.Skip);

            Assert.IsTrue(resultA.SequenceEqual(sequence));
            Assert.IsTrue(resultB.SequenceEqual(sequence));
            Assert.IsTrue(resultC.SequenceEqual(sequence));
        }

        /// <summary>
        /// Verify that pairing up a sequence of a single element with the Pad imbalance
        /// strategy results in a single result.
        /// </summary>
        [Test]
        public void TestPairUpSingleItemSequencePad()
        {
            const int value = 1;
            var sequence = Enumerable.Repeat(value, 1);
            var result = sequence.PairUp((a, b) => a + b, PairUpImbalanceStrategy.Pad);

            Assert.AreEqual( value, result.Single() );
        }

        /// <summary>
        /// Verify that pairing up a sequence of a single element with the Skip imbalance
        /// strategy results in an empty sequence.
        /// </summary>
        [Test]
        public void TestPairUpSingleItemSequenceSkip()
        {
            const int value = 1;
            var sequence = Enumerable.Repeat(value, 1);
            var result = sequence.PairUp((a, b) => a + b, PairUpImbalanceStrategy.Skip);

            Assert.AreEqual( 0, result.Count() );
        }

        /// <summary>
        /// Verify that the result of pairing a known sequence is correct.
        /// </summary>
        [Test]
        public void TestPairingBehavior()
        {
            var sequenceToPair = new[] { -1, 1, -2, 2, -3, 3, -4, 4, -5, 5, -6, 6, -7, 7, };
            var expectedPairs = new[]
                                    {
                                        new {A = -1, B = 1}, 
                                        new {A = -2, B = 2}, 
                                        new {A = -3, B = 3}, 
                                        new {A = -4, B = 4},
                                        new {A = -5, B = 5}, 
                                        new {A = -6, B = 6},
                                        new {A = -7, B = 7},
                                    };

            var pairedSequence = sequenceToPair.PairUp((a, b) => new { A = a, B = b });

            Assert.IsTrue(pairedSequence.SequenceEqual(expectedPairs));
        }

        /// <summary>
        /// Verify that the Pad imbalance strategy works correctly with value types.
        /// </summary>
        [Test]
        public void TestPadImbalanceStrategyValueType()
        {
            // test the bahavior with value types
            var sequenceToPair = new[] { -1, 1, -2, 2, -3, 3, -4, 4, -5 };
            var pairedSequence = sequenceToPair.PairUp((a, b) => a + b, PairUpImbalanceStrategy.Pad);
            Assert.AreEqual((sequenceToPair.Count() + 1) / 2, pairedSequence.Count());
            Assert.IsTrue(pairedSequence.Last() == sequenceToPair.Last());
        }

        /// <summary>
        /// Verify that the Pad imbalance strategy works correctly with reference types.
        /// </summary>
        [Test]
        public void TestPadImbalanceStrategyReferenceType()
        {
            // test the bahavior with refrence types
            var sequenceToPair = new[] { "-1", "1", "-2", "2", "-3", "3", "-4" };
            var pairedSequence = sequenceToPair.PairUp((a, b) => new { A = a, B = b }, PairUpImbalanceStrategy.Pad);
            Assert.AreEqual((sequenceToPair.Count() + 1) / 2, pairedSequence.Count());
            Assert.AreEqual(null, pairedSequence.Last().B);
        }

        /// <summary>
        /// Verify that the Skip imbalance strategy works correctly with value types.
        /// </summary>
        [Test]
        public void TestSkipImbalanceStrategyValueType()
        {
            var sequenceToPair = new[] { 1, 1, 2, 2, 3, 3, 4, 4, 5 };
            var pairedSequence = sequenceToPair.PairUp((a, b) => a, PairUpImbalanceStrategy.Skip);
            Assert.IsFalse(pairedSequence.Contains(sequenceToPair.Last()));
        }

        /// <summary>
        /// Verify that the Skip imbalance strategy works correctly with reference types.
        /// </summary>
        [Test]
        public void TestSkipImbalanceStrategyReferenceType()
        {
            var sequenceToPair = new[] { "1", "1", "2", "2", "3", "3", "4", "4", "5" };
            var pairedSequence = sequenceToPair.PairUp((a, b) => a, PairUpImbalanceStrategy.Skip);
            Assert.IsFalse(pairedSequence.Contains(sequenceToPair.Last()));
        }
    }
}