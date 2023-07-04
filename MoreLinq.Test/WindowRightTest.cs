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
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class WindowRightTest
    {
        [Test]
        public void WindowRightIsLazy()
        {
            _ = new BreakingSequence<int>().WindowRight(1);
        }

        [Test]
        public void WindowModifiedBeforeMoveNextDoesNotAffectNextWindow()
        {
            var sequence = Enumerable.Range(0, 3);
            using var reader = sequence.WindowRight(2).Read();

            var window1 = reader.Read();
            window1[0] = -1;
            var window2 = reader.Read();

            Assert.That(window2[0], Is.EqualTo(0));
        }

        [Test]
        public void WindowModifiedAfterMoveNextDoesNotAffectNextWindow()
        {
            var sequence = Enumerable.Range(0, 3);
            using var reader = sequence.WindowRight(2).Read();

            var window1 = reader.Read();
            window1[0] = -1;
            var window2 = reader.Read();

            Assert.That(window2[0], Is.EqualTo(0));
        }

        [Test]
        public void WindowModifiedDoesNotAffectPreviousWindow()
        {
            var sequence = Enumerable.Range(0, 3);
            using var reader = sequence.WindowRight(2).Read();

            var window1 = reader.Read();
            var window2 = reader.Read();
            window2[0] = -1;

            Assert.That(window1[0], Is.EqualTo(0));
        }

        [Test]
        public void WindowRightWithNegativeWindowSize()
        {
            Assert.That(() => Enumerable.Repeat(1, 10).WindowRight(-5),
                        Throws.ArgumentOutOfRangeException("size"));
        }

        [Test]
        public void WindowRightWithEmptySequence()
        {
            using var xs = Enumerable.Empty<int>().AsTestingSequence();

            var result = xs.WindowRight(5);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void WindowRightWithSingleElement()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count).ToArray();

            IList<int>[] result;
            using (var ts = sequence.AsTestingSequence())
                result = ts.WindowRight(1).ToArray();

            // number of windows should be equal to the source sequence length
            Assert.That(result.Length, Is.EqualTo(count));

            // each window should contain single item consistent of element at that offset
            foreach (var window in result.Index())
                Assert.That(sequence.ElementAt(window.Key), Is.EqualTo(window.Value.Single()));
        }

        [Test]
        public void WindowRightWithWindowSizeLargerThanSequence()
        {
            using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

            using var reader = sequence.WindowRight(10).Read();
            reader.Read().AssertSequenceEqual(            1);
            reader.Read().AssertSequenceEqual(         1, 2);
            reader.Read().AssertSequenceEqual(      1, 2, 3);
            reader.Read().AssertSequenceEqual(   1, 2, 3, 4);
            reader.Read().AssertSequenceEqual(1, 2, 3, 4, 5);
            reader.ReadEnd();
        }

        [Test]
        public void WindowRightWithWindowSizeSmallerThanSequence()
        {
            using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

            using var reader = sequence.WindowRight(3).Read();
            reader.Read().AssertSequenceEqual(      1);
            reader.Read().AssertSequenceEqual(   1, 2);
            reader.Read().AssertSequenceEqual(1, 2, 3);
            reader.Read().AssertSequenceEqual(2, 3, 4);
            reader.Read().AssertSequenceEqual(3, 4, 5);
            reader.ReadEnd();
        }
    }
}
