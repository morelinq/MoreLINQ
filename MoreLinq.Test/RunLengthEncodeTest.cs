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
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    /// <summary>
    /// Verify the behavior of the RunLengthEncode() operator
    /// </summary>
    [TestFixture]
    public class RunLengthEncodeTests
    {
        /// <summary>
        /// Verify that the RunLengthEncode() methods behave in a lazy manner.
        /// </summary>
        [Test]
        public void TestRunLengthEncodeIsLazy()
        {
            _ = new BreakingSequence<int>().RunLengthEncode();
            _ = new BreakingSequence<int>().RunLengthEncode(EqualityComparer<int>.Default);
        }

        /// <summary>
        /// Verify that run-length encoding an empty sequence results in an empty sequence.
        /// </summary>
        [Test]
        public void TestRunLengthEncodeEmptySequence()
        {
            var sequence = Enumerable.Empty<int>();
            var result = sequence.RunLengthEncode();

            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Verify that run-length encoding correctly accepts and uses custom equality comparers.
        /// </summary>
        [Test]
        public void TestRunLengthEncodeCustomComparer()
        {
            var sequence = new[] { "a", "A", "a", "b", "b", "B", "B" };

            var result = sequence.RunLengthEncode(StringComparer.InvariantCultureIgnoreCase)
                                 .Select(kvp => KeyValuePair.Create(kvp.Key.ToLowerInvariant(), kvp.Value));
            var expectedResult = new[]
            {
                KeyValuePair.Create("a", 3),
                KeyValuePair.Create("b", 4)
            };

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Verify that run-length encoding a known sequence produced a correct result.
        /// </summary>
        [Test]
        public void TestRunLengthEncodeResults()
        {
            var sequence = new[] { 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6 };
            var expectedResult = Enumerable.Range(1, 6).Select(x => KeyValuePair.Create(x, x));
            var result = sequence.RunLengthEncode();

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Verify that run-length encoding a sequence with no runs produces a correct result.
        /// </summary>
        [Test]
        public void TestRunLengthEncodeNoRuns()
        {
            var sequence = Enumerable.Range(1, 10);
            var result = sequence.RunLengthEncode();
            var expectedResult = sequence.Select(x => KeyValuePair.Create(x, 1));

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Verify that run-length encoding a sequence consisting of a single repeated value
        /// produces a correct result.
        /// </summary>
        [Test]
        public void TestRunLengthEncodeOneRun()
        {
            const char value = 'q';
            const int repeatCount = 10;
            var sequence = Enumerable.Repeat(value, repeatCount);
            var result = sequence.RunLengthEncode();
            var expectedResult = new[] { KeyValuePair.Create(value, repeatCount) };

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
