using System;
using System.Collections.Generic;

namespace MoreLinq.Test.Pull
{
    /// <summary>
    /// Adds reader semantics to a sequence where <see cref="IEnumerator{T}.MoveNext"/>
    /// and <see cref="IEnumerator{T}.Current"/> are rolled into a single
    /// "read" operation.
    /// </summary>
    /// <typeparam name="T">Type of elements to read.</typeparam>

    [ Serializable ]
    internal class SequenceReader<T> : IDisposable
    {
        private IEnumerator<T> enumerator;

        /// <summary>
        /// Initializes a <see cref="SequenceReader{T}" /> instance 
        /// from an enumerable sequence.
        /// </summary>
        /// <param name="source">Source sequence.</param>

        public SequenceReader(IEnumerable<T> source) :
            this(GetEnumerator(source)) { }

        /// <summary>
        /// Initializes a <see cref="SequenceReader{T}" /> instance 
        /// from an enumerator.
        /// </summary>
        /// <param name="enumerator">Source enumerator.</param>

        public SequenceReader(IEnumerator<T> enumerator)
        {
            if (enumerator == null) throw new ArgumentNullException("enumerator");
            this.enumerator = enumerator;
        }

        private static IEnumerator<T> GetEnumerator(IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source.GetEnumerator();
        }

        /// <summary>
        /// Tires to read the next value.
        /// </summary>
        /// <param name="value">
        /// When this method returns, contains the value read on success.
        /// </param>
        /// <returns>
        /// Returns true if a value was successfully read; otherwise, false. 
        /// </returns>

        public virtual bool TryRead(out T value)
        {
            EnsureNotDisposed();

            value = default(T);

            var e = enumerator;
            if (!e.MoveNext())
                return false;

            value = e.Current;
            return true;
        }

        /// <summary>
        /// Tires to read the next value otherwise return the default.
        /// </summary>

        public T TryRead()
        {
            return TryRead(default(T));
        }

        /// <summary>
        /// Tires to read the next value otherwise return a given default.
        /// </summary>

        public T TryRead(T defaultValue)
        {
            T result;
            return TryRead(out result) ? result : defaultValue;
        }

        /// <summary>
        /// Reads a value otherwise throws <see cref="InvalidOperationException"/>
        /// if no more values are available.
        /// </summary>
        /// <returns>
        /// Returns the read value;
        /// </returns>

        public T Read()
        {
            T result;
            if (!TryRead(out result))
                throw new InvalidOperationException();

            return result;
        }

        /// <summary>
        /// Reads the end. If the end has not been reached then it
        /// throws <see cref="InvalidOperationException"/>.
        /// </summary>

        public virtual void ReadEnd()
        {
            EnsureNotDisposed();

            if (enumerator.MoveNext())
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Ensures that this object has not been disposed, that
        /// <see cref="Dispose"/> has not been previously called.
        /// </summary>

        protected void EnsureNotDisposed()
        {
            if (enumerator == null)
                throw new ObjectDisposedException(GetType().FullName);
        }

        /// <summary>
        /// Disposes this object and enumerator with which is was
        /// initialized.
        /// </summary>

        public virtual void Dispose()
        {
            var e = this.enumerator;
            if (e == null) return;
            this.enumerator = null;
            e.Dispose();
        }
    }
}