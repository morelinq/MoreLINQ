#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Atif Aziz. All rights reserved.
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
    /// Verify the behavior of the Window operator
    /// </summary>
    [TestFixture]
    public class WindowTests
    {
        /// <summary>
        /// Verify that Window behaves in a lazy manner
        /// </summary>
        [Test]
        public void TestWindowIsLazy()
        {
            _ = new BreakingSequence<int>().Window(1);
        }

        [Test]
        public void WindowModifiedBeforeMoveNextDoesNotAffectNextWindow()
        {
            var sequence = Enumerable.Range(0, 3);
            using var reader = sequence.Window(2).Read();

            var window1 = reader.Read();
            window1[1] = -1;
            var window2 = reader.Read();

            Assert.That(window2[0], Is.EqualTo(1));
        }

        [Test]
        public void WindowModifiedAfterMoveNextDoesNotAffectNextWindow()
        {
            var sequence = Enumerable.Range(0, 3);
            using var reader = sequence.Window(2).Read();

            var window1 = reader.Read();
            window1[1] = -1;
            var window2 = reader.Read();

            Assert.That(window2[0], Is.EqualTo(1));
        }

        [Test]
        public void WindowModifiedDoesNotAffectPreviousWindow()
        {
            var sequence = Enumerable.Range(0, 3);
            using var reader = sequence.Window(2).Read();

            var window1 = reader.Read();
            var window2 = reader.Read();
            window2[0] = -1;

            Assert.That(window1[1], Is.EqualTo(1));
        }

        /// <summary>
        /// Verify that a negative window size results in an exception
        /// </summary>
        [Test]
        public void TestWindowNegativeWindowSizeException()
        {
            var sequence = Enumerable.Repeat(1, 10);

            Assert.That(() => sequence.Window(-5),
                        Throws.ArgumentOutOfRangeException("size"));
        }

        /// <summary>
        /// Verify that a sliding window of an any size over an empty sequence
        /// is an empty sequence
        /// </summary>
        [Test]
        public void TestWindowEmptySequence()
        {
            var sequence = Enumerable.Empty<int>();
            var result = sequence.Window(5);

            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Verify that decomposing a sequence into windows of a single item
        /// degenerates to the original sequence.
        /// </summary>
        [Test]
        public void TestWindowOfSingleElement()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Window(1);

            // number of windows should be equal to the source sequence length
            Assert.That(result.Count(), Is.EqualTo(count));
            // each window should contain single item consistent of element at that offset
            var index = -1;
            foreach (var window in result)
                Assert.That(window.Single(), Is.EqualTo(sequence.ElementAt(++index)));
        }

        /// <summary>
        /// Verify that asking for a window large than the source sequence results
        /// in a empty sequence.
        /// </summary>
        [Test]
        public void TestWindowLargerThanSequence()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Window(count + 1);

            // there should only be one window whose contents is the same
            // as the source sequence
            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Verify that asking for a window smaller than the source sequence results
        /// in N sequences, where N = (source.Count() - windowSize) + 1.
        /// </summary>
        [Test]
        public void TestWindowSmallerThanSequence()
        {
            const int count = 100;
            const int windowSize = count / 3;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Window(windowSize);

            // ensure that the number of windows is correct
            Assert.That(result.Count(), Is.EqualTo(count - windowSize + 1));
            // ensure each window contains the correct set of items
            var index = -1;
            foreach (var window in result)
                Assert.That(window, Is.EqualTo(sequence.Skip(++index).Take(windowSize)));
        }

        /// <summary>
        /// Verify that later windows do not modify any of the previous ones.
        /// </summary>

        [Test]
        public void TestWindowWindowsImmutability()
        {
            using var windows = Enumerable.Range(1, 5).Window(2).AsTestingSequence();

            using var reader = windows.ToArray().Read();
            reader.Read().AssertSequenceEqual(1, 2);
            reader.Read().AssertSequenceEqual(2, 3);
            reader.Read().AssertSequenceEqual(3, 4);
            reader.Read().AssertSequenceEqual(4, 5);
            reader.ReadEnd();
        }
    }
}
