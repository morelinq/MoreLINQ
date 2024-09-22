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

namespace MoreLinq.Test.Async
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.ExceptionServices;
    using System.Threading;
    using NUnit.Framework;

    static class TestingAsyncSequence
    {
        public static TestingAsyncSequence<T> Of<T>(params T[] elements) =>
            new(elements.ToAsyncEnumerable());

        public static TestingAsyncSequence<T> AsTestingSequence<T>(this IAsyncEnumerable<T> source) =>
            source is not null
            ? new TestingAsyncSequence<T>(source)
            : throw new ArgumentNullException(nameof(source));
    }

    /// <summary>
    /// Sequence that asserts whether its iterator has been disposed
    /// when it is disposed itself and also whether GetEnumerator() is
    /// called exactly once or not.
    /// </summary>

    sealed class TestingAsyncSequence<T> : IAsyncEnumerable<T>, IDisposable
    {
        bool? disposed;
        IAsyncEnumerable<T>? source;
        ExceptionDispatchInfo? disposeErrorInfo;

        internal TestingAsyncSequence(IAsyncEnumerable<T> sequence) =>
            this.source = sequence;

        public bool IsDisposed => this.disposed == true;
        public int MoveNextCallCount { get; private set; }

        void IDisposable.Dispose() =>
            AssertDisposed();

        /// <summary>
        /// Checks that the iterator was disposed, and then resets.
        /// </summary>

        void AssertDisposed()
        {
            this.disposeErrorInfo?.Throw();

            if (this.disposed is null)
                return;

            Assert.That(this.disposed, Is.True, "Expected sequence to be disposed.");
            this.disposed = null;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            Assert.That(this.source, Is.Not.Null,
                        "LINQ operators should not enumerate a sequence more than once.");

            // Dammit (!) below is okay since we assert above it's not null.
            var enumerator = this.source!.GetAsyncEnumerator(cancellationToken).AsWatchable();

            this.disposed = false;
            enumerator.Disposed += delegate
            {
                // If the following assertion fails the capture the error
                // and re-throw later during the disposal of the test
                // sequence. This is done so because "DisposeAsync" is never
                // expected to throw and could interfere with how an operator
                // builds on that assumption.

                try
                {
                    Assert.That(this.disposed, Is.False, "LINQ operators should not dispose a sequence more than once.");
                }
                catch (AssertionException e)
                {
                    this.disposeErrorInfo = ExceptionDispatchInfo.Capture(e);
                }

                this.disposed = true;
            };

            var ended = false;
            enumerator.MoveNextCalled += (_, moved) =>
            {
                Assert.That(ended, Is.False, "LINQ operators should not continue iterating a sequence that has terminated.");
                ended = !moved;
                MoveNextCallCount++;
            };

            this.source = null;
            return enumerator;
        }
    }
}
