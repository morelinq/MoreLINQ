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

using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace MoreLinq.Test
{
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

        internal TestingSequence(IEnumerable<T> sequence)
        {
            _sequence = sequence;
        }

        void IDisposable.Dispose()
        {
            AssertDisposed();
        }

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
            var enumerator = new DisposeTestingSequenceEnumerator(_sequence.GetEnumerator());
            _disposed = false;
            enumerator.Disposed += delegate { _disposed = true; };
            _sequence = null;
            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        class DisposeTestingSequenceEnumerator : IEnumerator<T>
        {
            readonly IEnumerator<T> _sequence;

            public event EventHandler Disposed;

            public DisposeTestingSequenceEnumerator(IEnumerator<T> sequence)
            {
                _sequence = sequence;
            }

            public T Current => _sequence.Current;
            object IEnumerator.Current => Current;
            public bool MoveNext() => _sequence.MoveNext();
            public void Reset() => _sequence.Reset();

            public void Dispose()
            {
                _sequence.Dispose();
                Disposed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
