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
    using NUnit.Framework;
    using static TestingSequence;

    static class TestingSequence
    {
        internal static TestingSequence<T> Of<T>(params T[] elements) =>
            new(elements, Options.None, numEnumerations: 1);

        internal static TestingSequence<T> Of<T>(Options options, params T[] elements) =>
            elements.AsTestingSequence(options, numEnumerations: 1);

        internal static TestingSequence<T> AsTestingSequence<T>(this IEnumerable<T> source,
                                                                Options options = Options.None,
                                                                int numEnumerations = 1) =>
            source != null
            ? new TestingSequence<T>(source, options, numEnumerations)
            : throw new ArgumentNullException(nameof(source));

        internal const string ExpectedDisposal = "Expected sequence to be disposed.";
        internal const string TooManyEnumerations = "Sequence should not be enumerated more than expected.";
        internal const string TooManyDisposals = "Sequence should not be disposed more than once per enumeration.";
        internal const string SimultaneousEnumerations = "Sequence should not have simultaneous enumeration.";
        internal const string MoveNextDisposed = "LINQ operators should not call MoveNext() on a disposed sequence.";
        internal const string MoveNextCompleted = "LINQ operators should not continue iterating a sequence that has terminated.";
        internal const string CurrentDisposed = "LINQ operators should not attempt to get the Current value on a disposed sequence.";
        internal const string CurrentCompleted = "LINQ operators should not attempt to get the Current value on a completed sequence.";

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
        private IEnumerable<T>? _sequence;
        private readonly Options _options;
        private readonly int _numEnumerations;

        private bool _hasEnumerated;
        private bool _currentlyEnumerating;
        private int _disposedCount;
        private int _enumerationCount;

        internal TestingSequence(IEnumerable<T> sequence, Options options, int numEnumerations)
        {
            _sequence = sequence;
            _numEnumerations = numEnumerations;
            _options = options;
        }

        public int MoveNextCallCount { get; private set; }
        public bool IsDisposed => !_currentlyEnumerating;

        void IDisposable.Dispose()
        {
            if (_hasEnumerated)
                Assert.That(_disposedCount, Is.EqualTo(_enumerationCount), ExpectedDisposal);
        }

        public IEnumerator<T> GetEnumerator()
        {
            Assert.That(_sequence is null, Is.False, TooManyEnumerations);

            Debug.Assert(_sequence is not null);

            Assert.That(_currentlyEnumerating, Is.False, SimultaneousEnumerations);
            _currentlyEnumerating = true;

            _hasEnumerated = true;

            var enumerator = _sequence.GetEnumerator().AsWatchable();
            var disposed = false;
            enumerator.Disposed += delegate
            {
                if (!disposed)
                {
                    _disposedCount++;
                    _currentlyEnumerating = false;
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
                Assert.That(disposed, Is.False, MoveNextDisposed);
                if (!_options.HasFlag(Options.AllowRepeatedMoveNexts))
                    Assert.That(ended, Is.False, MoveNextCompleted);

                ended = !moved;
                MoveNextCallCount++;
            };

            enumerator.GetCurrentCalled += delegate
            {
                Assert.That(disposed, Is.False, CurrentDisposed);
                Assert.That(ended, Is.False, CurrentCompleted);
            };

            if (++_enumerationCount == _numEnumerations)
                _sequence = null;
            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [TestFixture]
    public class TestingSequenceTest
    {
        [Test]
        public void TestingSequenceShouldValidateDisposal()
        {
            static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
            {
                var enumerator = enumerable.GetEnumerator();

                yield break;
            }

            Assert.That(
                () =>
                {
                    using var xs = Enumerable.Range(1, 10).AsTestingSequence();
                    InvalidUsage(xs).Consume();
                },
                Throws.InstanceOf<AssertionException>()
                    .With.Message.Contains(ExpectedDisposal));
        }

        [Test]
        public void TestingSequenceShouldValidateNumberOfUsages()
        {
            static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
            {
                using (var enumerator = enumerable.GetEnumerator())
                    yield return 1;
                using (var enumerator = enumerable.GetEnumerator())
                    yield return 2;
                using (var enumerator = enumerable.GetEnumerator())
                    yield return 3;

                yield break;
            }

            Assert.That(
                () =>
                {
                    using var xs = Enumerable.Range(1, 10).AsTestingSequence(numEnumerations: 2);
                    InvalidUsage(xs).Consume();
                },
                Throws.InstanceOf<AssertionException>()
                    .With.Message.Contains(TooManyEnumerations));
        }

        [Test]
        public void TestingSequenceShouldValidateDisposeOnDisposedSequence()
        {
            static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
            {
                using var enumerator = enumerable.GetEnumerator();
                enumerator.Dispose();
                enumerator.Dispose();

                yield break;
            }

            Assert.That(
                () =>
                {
                    using var xs = Enumerable.Range(1, 10).AsTestingSequence();
                    InvalidUsage(xs).Consume();
                },
                Throws.InstanceOf<AssertionException>()
                    .With.Message.Contains(TooManyDisposals));
        }

        [Test]
        public void TestingSequenceShouldValidateMoveNextOnDisposedSequence()
        {
            static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
            {
                using var enumerator = enumerable.GetEnumerator();
                enumerator.Dispose();
                enumerator.MoveNext();

                yield break;
            }

            Assert.That(
                () =>
                {
                    using var xs = Enumerable.Range(1, 10).AsTestingSequence();
                    InvalidUsage(xs).Consume();
                },
                Throws.InstanceOf<AssertionException>()
                    .With.Message.Contains(MoveNextDisposed));
        }

        [Test]
        public void TestingSequenceShouldValidateMoveNextOnCompletedSequence()
        {
            static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
            {
                using var enumerator = enumerable.GetEnumerator();
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
                enumerator.MoveNext();

                yield break;
            }

            Assert.That(
                () =>
                {
                    using var xs = Enumerable.Range(1, 10).AsTestingSequence();
                    InvalidUsage(xs).Consume();
                },
                Throws.InstanceOf<AssertionException>()
                    .With.Message.Contains(MoveNextCompleted));
        }

        [Test]
        public void TestingSequenceShouldValidateCurrentOnDisposedSequence()
        {
            static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
            {
                using var enumerator = enumerable.GetEnumerator();
                enumerator.Dispose();
                yield return enumerator.Current;

                yield break;
            }

            Assert.That(
                () =>
                {
                    using var xs = Enumerable.Range(1, 10).AsTestingSequence();
                    InvalidUsage(xs).Consume();
                },
                Throws.InstanceOf<AssertionException>()
                    .With.Message.Contains(CurrentDisposed));
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

                yield break;
            }

            Assert.That(
                () =>
                {
                    using var xs = Enumerable.Range(1, 10).AsTestingSequence();
                    InvalidUsage(xs).Consume();
                },
                Throws.InstanceOf<AssertionException>()
                    .With.Message.Contains(CurrentCompleted));
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

            Assert.That(
                () =>
                {
                    using var xs = Enumerable.Range(1, 10).AsTestingSequence(numEnumerations: 2);
                    InvalidUsage(xs).Consume();
                },
                Throws.InstanceOf<AssertionException>()
                    .With.Message.Contains(SimultaneousEnumerations));
        }
    }
}
