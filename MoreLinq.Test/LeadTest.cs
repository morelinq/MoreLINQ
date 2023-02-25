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
    /// Verify the behavior of the Lead operator.
    /// </summary>
    [TestFixture]
    public class LeadTests
    {
        /// <summary>
        /// Verify that Lead() behaves in a lazy manner.
        /// </summary>
        [Test]
        public void TestLeadIsLazy()
        {
            _ = new BreakingSequence<int>().Lead(5, BreakingFunc.Of<int, int, int>());
            _ = new BreakingSequence<int>().Lead(5, -1, BreakingFunc.Of<int, int, int>());
        }

        /// <summary>
        /// Verify that attempting to lead by a negative offset will result in an exception.
        /// </summary>
        [Test]
        public void TestLeadNegativeOffset()
        {
            Assert.That(() => Enumerable.Range(1, 100).Lead(-5, (val, leadVal) => val + leadVal),
                        Throws.ArgumentOutOfRangeException("offset"));
        }

        /// <summary>
        /// Verify that attempting to lead by a zero offset will result in an exception.
        /// </summary>
        [Test]
        public void TestLeadZeroOffset()
        {
            Assert.That(() => Enumerable.Range(1, 100).Lead(0, (val, leadVal) => val + leadVal),
                        Throws.ArgumentOutOfRangeException("offset"));
        }

        /// <summary>
        /// Verify that lead can accept and propagate a default value passed to it.
        /// </summary>
        [Test]
        public void TestLeadExplicitDefaultValue()
        {
            const int count = 100;
            const int leadBy = 10;
            const int leadDefault = -1;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lead(leadBy, leadDefault, (_, leadVal) => leadVal);

            Assert.That(result.Count(), Is.EqualTo(count));
            Assert.That(result.Skip(count - leadBy), Is.EqualTo(Enumerable.Repeat(leadDefault, leadBy)));
        }

        /// <summary>
        /// Verify that Lead() will use default(T) if a specific default value is not supplied for the lead value.
        /// </summary>
        [Test]
        public void TestLeadImplicitDefaultValue()
        {
            const int count = 100;
            const int leadBy = 10;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lead(leadBy, (_, leadVal) => leadVal);

            Assert.That(result.Count(), Is.EqualTo(count));
            Assert.That(result.Skip(count - leadBy), Is.EqualTo(Enumerable.Repeat(default(int), leadBy)));
        }

        /// <summary>
        /// Verify that if the lead offset is greater than the length of the sequence
        /// Lead() still yield all of the elements of the source sequence.
        /// </summary>
        [Test]
        public void TestLeadOffsetGreaterThanSequenceLength()
        {
            const int count = 100;
            const int leadDefault = -1;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lead(count + 1, leadDefault, (val, leadVal) => new { A = val, B = leadVal });

            Assert.That(result.Count(), Is.EqualTo(count));
            Assert.That(result, Is.EqualTo(sequence.Select(x => new { A = x, B = leadDefault })));
        }

        /// <summary>
        /// Verify that Lead() actually yields the correct pair of values from the sequence
        /// when the lead offset is 1.
        /// </summary>
        [Test]
        public void TestLeadPassesCorrectValueOffsetBy1()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lead(1, count + 1, (val, leadVal) => new { A = val, B = leadVal });

            Assert.That(result.Count(), Is.EqualTo(count));
            Assert.That(result.All(x => x.B == (x.A + 1)), Is.True);
        }

        /// <summary>
        /// Verify that Lead() yields the correct pair of values from the sequence
        /// when the lead offset is greater than 1.
        /// </summary>
        [Test]
        public void TestLeadPassesCorrectValueOffsetBy2()
        {
            const int count = 100;
            const int leadDefault = count + 1;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Lead(2, leadDefault, (val, leadVal) => new { A = val, B = leadVal });

            Assert.That(result.Count(), Is.EqualTo(count));
            Assert.That(result.Take(count - 2).All(x => x.B == (x.A + 2)), Is.True);
            Assert.That(result.Skip(count - 2).All(x => x.B == leadDefault && x.A is count or count - 1), Is.True);
        }

        [Test]
        public void TestLagWithNullableReferences()
        {
            var words = new[] { "foo", "bar", "baz", "qux" };
            var result = words.Lead(2, (a, b) => new { A = a, B = b });
            result.AssertSequenceEqual(
                new { A = "foo", B = (string?)"baz" },
                new { A = "bar", B = (string?)"qux" },
                new { A = "baz", B = (string?)null  },
                new { A = "qux", B = (string?)null  });
        }

        [Test]
        public void TestLagWithNonNullableReferences()
        {
            var words = new[] { "foo", "bar", "baz", "qux" };
            var empty = string.Empty;
            var result = words.Lead(2, empty, (a, b) => new { A = a, B = b });
            result.AssertSequenceEqual(
                new { A = "foo", B = "baz" },
                new { A = "bar", B = "qux" },
                new { A = "baz", B = empty },
                new { A = "qux", B = empty });
        }
    }
}
