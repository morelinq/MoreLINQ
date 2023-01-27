#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2009 Atif Aziz. All rights reserved.
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using NUnit.Framework;
    using static TestingSequence;

    static class TestingSequence
    {
        internal static TestingSequence<T> Of<T>(params T[] elements) =>
            new(elements, Options.None, maxEnumerations: 1);

        internal static TestingSequence<T> Of<T>(Options options, params T[] elements) =>
            elements.AsTestingSequence(options, maxEnumerations: 1);

        internal static TestingSequence<T> AsTestingSequence<T>(this IEnumerable<T> source,
                                                                Options options = Options.None,
                                                                int maxEnumerations = 1) =>
            source != null
            ? new TestingSequence<T>(source, options, maxEnumerations)
            : throw new ArgumentNullException(nameof(source));

        internal const string ExpectedDisposal = "Expected sequence to be disposed.";
        internal const string TooManyEnumerations = "Sequence should not be enumerated more than expected.";
        internal const string TooManyDisposals = "Sequence should not be disposed more than once per enumeration.";
        internal const string SimultaneousEnumerations = "Sequence should not have simultaneous enumeration.";
        internal const string MoveNextPostDisposal = "LINQ operators should not call MoveNext() on a disposed sequence.";
        internal const string MoveNextPostEnumeration = "LINQ operators should not continue iterating a sequence that has terminated.";
        internal const string CurrentPostDisposal = "LINQ operators should not attempt to get the Current value on a disposed sequence.";
        internal const string CurrentPostEnumeration = "LINQ operators should not attempt to get the Current value on a completed sequence.";

        [Flags]
        public enum Options
        {
            None,
            AllowRepeatedDisposals = 0x2,
            AllowRepeatedMoveNexts = 0x4,
        }
    }

    /// <summary>
    /// Sequence that asserts whether its iterator has been disposed
    /// when it is disposed itself and also whether GetEnumerator() is
    /// called exactly once or not.
    /// </summary>
    sealed class TestingSequence<T> : IEnumerable<T>, IDisposable
    {
        readonly IEnumerable<T> _sequence;
        readonly Options _options;
        readonly int _maxEnumerations;

        int _disposedCount;
        int _enumerationCount;

        internal TestingSequence(IEnumerable<T> sequence, Options options, int maxEnumerations)
        {
            _sequence = sequence;
            _maxEnumerations = maxEnumerations;
            _options = options;
        }

        public int MoveNextCallCount { get; private set; }
        public bool IsDisposed => _enumerationCount > 0 && _disposedCount == _enumerationCount;

        void IDisposable.Dispose()
        {
            if (_enumerationCount > 0)
                Assert.That(_disposedCount, Is.EqualTo(_enumerationCount), ExpectedDisposal);
        }

        public IEnumerator<T> GetEnumerator()
        {
            Assert.That(_enumerationCount, Is.LessThan(_maxEnumerations), TooManyEnumerations);
            Assert.That(_enumerationCount, Is.EqualTo(_disposedCount), SimultaneousEnumerations);
            _enumerationCount++;

            var enumerator = _sequence.GetEnumerator().AsWatchable();
            var disposed = false;
            enumerator.Disposed += delegate
            {
                if (!disposed)
                {
                    _disposedCount++;
                    disposed = true;
                }
                else if (!_options.HasFlag(Options.AllowRepeatedDisposals))
                {
                    Assert.Fail(TooManyDisposals);
                }
            };

            var ended = false;
            enumerator.MoveNextCalled += (_, moved) =>
            {
                Assert.That(disposed, Is.False, MoveNextPostDisposal);
                if (!_options.HasFlag(Options.AllowRepeatedMoveNexts))
                    Assert.That(ended, Is.False, MoveNextPostEnumeration);

                ended = !moved;
                MoveNextCallCount++;
            };

            enumerator.GetCurrentCalled += delegate
            {
                Assert.That(disposed, Is.False, CurrentPostDisposal);
                Assert.That(ended, Is.False, CurrentPostEnumeration);
            };

            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [TestFixture]
    public class TestingSequenceTest
    {
        [Test]
        public void TestingSequencePublicPropertiesTest()
        {
            using var sequence = Of(1, 2, 3, 4);
            Assert.That(sequence.IsDisposed, Is.False);
            Assert.That(sequence.MoveNextCallCount, Is.EqualTo(0));

            var iter = sequence.GetEnumerator();
            Assert.That(sequence.IsDisposed, Is.False);
            Assert.That(sequence.MoveNextCallCount, Is.EqualTo(0));

            for (var i = 1; i <= 4; i++)
            {
                _ = iter.MoveNext();
                Assert.That(sequence.IsDisposed, Is.False);
                Assert.That(sequence.MoveNextCallCount, Is.EqualTo(i));
            }

            iter.Dispose();
            Assert.That(sequence.IsDisposed, Is.True);
        }

        [Test]
        public void TestingSequenceShouldValidateDisposal()
        {
            static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
            {
                var _ = enumerable.GetEnumerator();

                yield break;
            }

            static void Act()
            {
                using var xs = Enumerable.Range(1, 10).AsTestingSequence();
                InvalidUsage(xs).Consume();
            }

            AssertTestingSequenceException(Act, ExpectedDisposal);
        }

        [Test]
        public void TestingSequenceShouldValidateNumberOfUsages()
        {
            static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
            {
                using (enumerable.GetEnumerator())
                    yield return 1;
                using (enumerable.GetEnumerator())
                    yield return 2;
                using (enumerable.GetEnumerator())
                    yield return 3;
            }

            static void Act()
            {
                using var xs = Enumerable.Range(1, 10).AsTestingSequence(maxEnumerations: 2);
                InvalidUsage(xs).Consume();
            }

            AssertTestingSequenceException(Act, TooManyEnumerations);
        }

        [Test]
        public void TestingSequenceShouldValidateDisposeOnDisposedSequence()
        {
            static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
            {
                var enumerator = enumerable.GetEnumerator();
                enumerator.Dispose();
                enumerator.Dispose();

                yield break;
            }

            static void Act()
            {
                using var xs = Enumerable.Range(1, 10).AsTestingSequence();
                InvalidUsage(xs).Consume();
            }

            AssertTestingSequenceException(Act, TooManyDisposals);
        }

        [Test]
        public void TestingSequenceShouldValidateMoveNextOnDisposedSequence()
        {
            static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
            {
                var enumerator = enumerable.GetEnumerator();
                enumerator.Dispose();
                _ = enumerator.MoveNext();

                yield break;
            }

            static void Act()
            {
                using var xs = Enumerable.Range(1, 10).AsTestingSequence();
                InvalidUsage(xs).Consume();
            }

            AssertTestingSequenceException(Act, MoveNextPostDisposal);
        }

        [Test]
        public void TestingSequenceShouldValidateMoveNextOnCompletedSequence()
        {
            static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
            {
                using var enumerator = enumerable.GetEnumerator();
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
                _ = enumerator.MoveNext();
            }

            static void Act()
            {
                using var xs = Enumerable.Range(1, 10).AsTestingSequence();
                InvalidUsage(xs).Consume();
            }

            AssertTestingSequenceException(Act, MoveNextPostEnumeration);
        }

        [Test]
        public void TestingSequenceShouldValidateCurrentOnDisposedSequence()
        {
            static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
            {
                var enumerator = enumerable.GetEnumerator();
                enumerator.Dispose();
                yield return enumerator.Current;
            }

            static void Act()
            {
                using var xs = Enumerable.Range(1, 10).AsTestingSequence();
                InvalidUsage(xs).Consume();
            }

            AssertTestingSequenceException(Act, CurrentPostDisposal);
        }

        [Test]
        public void TestingSequenceShouldValidateCurrentOnEndedSequence()
        {
            static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
            {
                using var enumerator = enumerable.GetEnumerator();
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
                yield return enumerator.Current;
            }

            static void Act()
            {
                using var xs = Enumerable.Range(1, 10).AsTestingSequence();
                InvalidUsage(xs).Consume();
            }

            AssertTestingSequenceException(Act, CurrentPostEnumeration);
        }

        [Test]
        public void TestingSequenceShouldValidateSimultaneousEnumeration()
        {
            static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
            {
                using var enum1 = enumerable.GetEnumerator();
                using var enum2 = enumerable.GetEnumerator();

                yield break;
            }

            static void Act()
            {
                using var xs = Enumerable.Range(1, 10).AsTestingSequence(maxEnumerations: 2);
                InvalidUsage(xs).Consume();
            }

            AssertTestingSequenceException(Act, SimultaneousEnumerations);
        }

        static void AssertTestingSequenceException(TestDelegate code, string message) =>
            Assert.That(code, Throws.InstanceOf<AssertionException>().With.Message.Matches(@"^\s*" + Regex.Escape(message)));
    }
}
