#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008-2011 Jonathan Skeet. All rights reserved.
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
using NUnit.Framework.SyntaxHelpers;

namespace MoreLinq.Test
{
    internal static class TestingSequence
    {
        internal static TestingSequence<T> Of<T>(params T[] elements)
        {
            return new TestingSequence<T>(elements);
        }
    }

    /// <summary>
    /// Sequence that asserts whether its iterator has been disposed
    /// when it is disposed itself and also whether GetEnumerator() is
    /// called exactly once or not.
    /// </summary>
    internal sealed class TestingSequence<T> : IEnumerable<T>, IDisposable
    {
        private bool disposed;
        private IEnumerable<T> sequence;

        internal TestingSequence(IEnumerable<T> sequence)
        {
            this.sequence = sequence;
        }

        void IDisposable.Dispose()
        {
            AssertDisposed();
        }

        /// <summary>
        /// Checks that the iterator was disposed, and then resets.
        /// </summary>
        private void AssertDisposed()
        {
            Assert.IsTrue(disposed, "Expected sequence to be disposed.");
            disposed = false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Assert.That(this.sequence, Is.Not.Null, "LINQ operators should not enumerate a sequence more than once.");
            var enumerator = new DisposeTestingSequenceEnumerator(this.sequence.GetEnumerator());
            enumerator.Disposed += delegate { disposed = true; };
            this.sequence = null;
            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class DisposeTestingSequenceEnumerator : IEnumerator<T>
        {
            private readonly IEnumerator<T> sequence;

            public event EventHandler Disposed;

            public DisposeTestingSequenceEnumerator(IEnumerator<T> sequence)
            {
                this.sequence = sequence;
            }

            public T Current
            {
                get { return sequence.Current; }
            }

            public void Dispose()
            {
                sequence.Dispose();
                var disposed = Disposed;
                if (disposed != null) 
                    disposed(this, EventArgs.Empty);
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                return sequence.MoveNext();
            }

            public void Reset()
            {
                sequence.Reset();
            }
        }
    }
}
