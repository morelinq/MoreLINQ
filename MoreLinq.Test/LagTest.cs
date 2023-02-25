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
    using NUnit.Framework;

    /// <summary>
    /// Verify the behavior of the Lag operator
    /// </summary>
    [TestFixture]
    public class LagTests
    {
        /// <summary>
        /// Verify that lag behaves in a lazy manner.
        /// </summary>
        [Test]
        public void TestLagIsLazy()
        {
            _ = new BreakingSequence<int>().Lag(5, BreakingFunc.Of<int, int, int>());
            _ = new BreakingSequence<int>().Lag(5, -1, BreakingFunc.Of<int, int, int>());
        }

        /// <summary>
        /// Verify that lagging by a negative offset results in an exception.
        /// </summary>
        [Test]
        public void TestLagNegativeOffsetException()
        {
            Assert.That(() => Enumerable.Repeat(1, 10).Lag(-10, (val, _) => val),
                        Throws.ArgumentOutOfRangeException("offset"));
        }

        /// <summary>
        /// Verify that attempting to lag by a zero offset will result in an exception
        /// </summary>
        [Test]
        public void TestLagZeroOffset()
        {
            Assert.That(() => Enumerable.Range(1, 10).Lag(0, (val, lagVal) => val + lagVal),
                        Throws.ArgumentOutOfRangeException("offset"));
        }

        /// <summary>
        /// Verify that lag can accept an propagate a default value passed to it.
        /// </summary>
        [Test]
        public void TestLagExplicitDefaultValue()
        {
            const int count = 100;
            const int lagBy = 10;
            const int lagDefault = -1;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lag(lagBy, lagDefault, (_, lagVal) => lagVal);

            Assert.That(result.Count(), Is.EqualTo(count));
            Assert.That(result.Take(lagBy), Is.EqualTo(Enumerable.Repeat(lagDefault, lagBy)));
        }

        /// <summary>
        /// Verify that lag will use default(T) if a specific default value is not supplied for the lag value.
        /// </summary>
        [Test]
        public void TestLagImplicitDefaultValue()
        {
            const int count = 100;
            const int lagBy = 10;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lag(lagBy, (_, lagVal) => lagVal);

            Assert.That(result.Count(), Is.EqualTo(count));
            Assert.That(result.Take(lagBy), Is.EqualTo(Enumerable.Repeat(default(int), lagBy)));
        }

        /// <summary>
        /// Verify that if the lag offset is greater than the sequence length lag
        /// still yields all of the elements of the source sequence.
        /// </summary>
        [Test]
        public void TestLagOffsetGreaterThanSequenceLength()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lag(count + 1, (a, _) => a);

            Assert.That(result.Count(), Is.EqualTo(count));
            Assert.That(result, Is.EqualTo(sequence));
        }

        /// <summary>
        /// Verify that lag actually yields the correct pair of values from the sequence
        /// when offsetting by a single item.
        /// </summary>
        [Test]
        public void TestLagPassesCorrectLagValueOffsetBy1()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lag(1, (a, b) => new { A = a, B = b });

            Assert.That(result.Count(), Is.EqualTo(count));
            Assert.That(result.All(x => x.B == (x.A - 1)), Is.True);
        }

        /// <summary>
        /// Verify that lag yields the correct pair of values from the sequence when
        /// offsetting by more than a single item.
        /// </summary>
        [Test]
        public void TestLagPassesCorrectLagValuesOffsetBy2()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lag(2, (a, b) => new { A = a, B = b });

            Assert.That(result.Count(), Is.EqualTo(count));
            Assert.That(result.Skip(2).All(x => x.B == (x.A - 2)), Is.True);
            Assert.That(result.Take(2).All(x => (x.A - x.B) == x.A), Is.True);
        }

        [Test]
        public void TestLagWithNullableReferences()
        {
            var words = new[] { "foo", "bar", "baz", "qux" };
            var result = words.Lag(2, (a, b) => new { A = a, B = b });
            result.AssertSequenceEqual(
                new { A = "foo", B = (string?)null  },
                new { A = "bar", B = (string?)null  },
                new { A = "baz", B = (string?)"foo" },
                new { A = "qux", B = (string?)"bar" });
        }

        [Test]
        public void TestLagWithNonNullableReferences()
        {
            var words = new[] { "foo", "bar", "baz", "qux" };
            var empty = string.Empty;
            var result = words.Lag(2, empty, (a, b) => new { A = a, B = b });
            result.AssertSequenceEqual(
                new { A = "foo", B = empty },
                new { A = "bar", B = empty },
                new { A = "baz", B = "foo" },
                new { A = "qux", B = "bar" });
        }
    }
}
