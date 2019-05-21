#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
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

    static class TestingSequence
    {
        internal static TestingSequence<T> Of<T>(params T[] elements) =>
            new TestingSequence<T>(elements);

        internal static TestingSequence<T> AsTestingSequence<T>(this IEnumerable<T> source) =>
            source != null
            ? new TestingSequence<T>(source)
            : throw new ArgumentNullException(nameof(source));
    }

    /// <summary>
    /// Sequence that asserts whether its iterator has been disposed
    /// when it is disposed itself and also whether GetEnumerator() is
    /// called exactly once or not.
    /// </summary>
    sealed class TestingSequence<T> : IEnumerable<T>, IDisposable
    {
        bool? _disposed;
        IEnumerable<T> _sequence;

        internal TestingSequence(IEnumerable<T> sequence) =>
            _sequence = sequence;

        public int MoveNextCallCount { get; private set; }

        void IDisposable.Dispose() =>
            AssertDisposed();

        /// <summary>
        /// Checks that the iterator was disposed, and then resets.
        /// </summary>
        void AssertDisposed()
        {
            if (_disposed == null)
                return;
            Assert.IsTrue(_disposed, "Expected sequence to be disposed.");
            _disposed = null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Assert.That(_sequence, Is.Not.Null, "LINQ operators should not enumerate a sequence more than once.");
            var enumerator = _sequence.GetEnumerator().AsWatchtable();
            _disposed = false;
            enumerator.Disposed += delegate
            {
                Assert.That(_disposed, Is.False, "LINQ operators should not dispose a sequence more than once.");
                _disposed = true;
            };
            var ended = false;
            enumerator.MoveNextCalled += (_, moved) =>
            {
                Assert.That(ended, Is.False, "LINQ operators should not continue iterating a sequence that has terminated.");
                ended = !moved;
                MoveNextCallCount++;
            };
            _sequence = null;
            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
