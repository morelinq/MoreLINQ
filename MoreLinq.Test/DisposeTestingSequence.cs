using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace MoreLinq.Test
{
    internal sealed class DisposeTestingSequence
    {
        internal static DisposeTestingSequence<T> Of<T>(params T[] elements)
        {
            return new DisposeTestingSequence<T>(elements);
        }
    }

    /// <summary>
    /// Sequence which records whether or not its iterator has been disposed.
    /// It assumes that GetEnumerator() is called exactly once before
    /// AssertDisposed is called to check the results.
    /// </summary>
    internal sealed class DisposeTestingSequence<T> : IEnumerable<T>
    {
        private bool disposed = false;
        private readonly IEnumerable<T> sequence;

        internal DisposeTestingSequence(IEnumerable<T> sequence)
        {
            this.sequence = sequence;
        }

        /// <summary>
        /// Checks that the iterator was disposed, and then resets.
        /// </summary>
        internal void AssertDisposed()
        {
            Assert.IsTrue(disposed, "Expected sequence to be disposed.");
            disposed = false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new DisposeTestingSequenceEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class DisposeTestingSequenceEnumerator : IEnumerator<T>
        {
            private readonly DisposeTestingSequence<T> parent;
            private readonly IEnumerator<T> sequence;

            internal DisposeTestingSequenceEnumerator(DisposeTestingSequence<T> parent)
            {
                this.parent = parent;
                sequence = parent.sequence.GetEnumerator();
            }

            public T Current
            {
                get { return sequence.Current; }
            }

            public void Dispose()
            {
                sequence.Dispose();
                parent.disposed = true;
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
