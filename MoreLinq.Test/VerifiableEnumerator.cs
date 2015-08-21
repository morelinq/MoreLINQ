using System;
using System.Collections;
using System.Collections.Generic;

namespace MoreLinq.Test
{
    /// <summary>
    /// Interface representing an <see cref="IEnumerable{T}"/> that exposed verification checkpoints for testing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IVerifiableEnumerable<T> : IEnumerable<T>
    {
        /// <summary>
        /// Called when the the enumerator is about to be disposed.
        /// </summary>
        IVerifiableEnumerable<T> WhenDisposed(Action<IEnumerable<T>> action);
        /// <summary>
        /// Called when GetEnumerator() is called on the sequence.
        /// </summary>
        IVerifiableEnumerable<T> WhenEnumerated(Action<IEnumerable<T>> action);
        /// <summary>
        /// Called when MoveNext() is called on any enumerator for this sequence.
        /// </summary>
        IVerifiableEnumerable<T> WhenMoveNext(Action<IEnumerable<T>> action);
        /// <summary>
        /// Called when Reset() is called on any enumerator for this sequence.
        /// </summary>
        IVerifiableEnumerable<T> WhenReset(Action<IEnumerable<T>> action);
        /// <summary>
        /// Called when the Current property is accessed on any enumerator for this sequence.
        /// </summary>
        IVerifiableEnumerable<T> WhenCurrent(Action<IEnumerable<T>> action);
    }

    /// <summary>
    /// Extension class that allows verification injection points to be added to any existing <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class VerifyEnumeratorDisposalExt
    {
        private class VerifiableEnumerable<T> : IVerifiableEnumerable<T>
        {
            private readonly IEnumerable<T> m_Enumerable;

            private static readonly Action<IEnumerable<T>> DefaultAction = s => { return; };

            private Action<IEnumerable<T>> OnEnumerateAction = DefaultAction;
            private Action<IEnumerable<T>> OnDisposeAction = DefaultAction;
            private Action<IEnumerable<T>> OnMoveNextAction = DefaultAction;
            private Action<IEnumerable<T>> OnResetAction = DefaultAction;
            private Action<IEnumerable<T>> OnCurrentAction = DefaultAction;

            public VerifiableEnumerable(IEnumerable<T> enumerable)
            {
                m_Enumerable = enumerable;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new VerifiableEnumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            IVerifiableEnumerable<T> IVerifiableEnumerable<T>.WhenDisposed(Action<IEnumerable<T>> action)
            {
                OnDisposeAction = action ?? DefaultAction;
                return this;
            }

            IVerifiableEnumerable<T> IVerifiableEnumerable<T>.WhenEnumerated(Action<IEnumerable<T>> action)
            {
                OnEnumerateAction = action ?? DefaultAction;
                return this;
            }

            IVerifiableEnumerable<T> IVerifiableEnumerable<T>.WhenMoveNext(Action<IEnumerable<T>> action)
            {
                OnMoveNextAction = action ?? DefaultAction;
                return this;
            }

            IVerifiableEnumerable<T> IVerifiableEnumerable<T>.WhenReset(Action<IEnumerable<T>> action)
            {
                OnResetAction = action ?? DefaultAction;
                return this;
            }

            IVerifiableEnumerable<T> IVerifiableEnumerable<T>.WhenCurrent(Action<IEnumerable<T>> action)
            {
                OnCurrentAction = action ?? DefaultAction;
                return this;
            }

            private class VerifiableEnumerator : IEnumerator<T>
            {
                private readonly IEnumerator<T> m_Enumerator;
                private readonly VerifiableEnumerable<T> m_Enumerable;

                public VerifiableEnumerator(VerifiableEnumerable<T> enumerable)
                {
                    m_Enumerable = enumerable;
                    m_Enumerator = enumerable.m_Enumerable.GetEnumerator();
                    m_Enumerable.OnEnumerateAction(m_Enumerable);
                }

                public void Dispose()
                {
                    m_Enumerable.OnDisposeAction(m_Enumerable);
                    m_Enumerator.Dispose();
                }

                public bool MoveNext()
                {
                    m_Enumerable.OnMoveNextAction(m_Enumerable);
                    return m_Enumerator.MoveNext();
                }

                public void Reset()
                {
                    m_Enumerable.OnResetAction(m_Enumerable);
                    m_Enumerator.Reset();
                }

                public T Current
                {
                    get
                    {
                        m_Enumerable.OnCurrentAction(m_Enumerable);
                        return m_Enumerator.Current;
                    }
                }

                object IEnumerator.Current
                {
                    get { return Current; }
                }
            }
        }

        /// <summary>
        /// Extension method that wraps an existing <see cref="IEnumerable{T}"/> in a verification wrapper.
        /// </summary>
        /// <typeparam name="T">Type of the element being enumerated</typeparam>
        /// <param name="sequence">The enumerable sequence to wrap verification around</param>
        /// <returns>A verifiable enumerator that wraps <paramref name="sequence"/></returns>
        public static IVerifiableEnumerable<T> AsVerifiable<T>(this IEnumerable<T> sequence)
        {
            return sequence as IVerifiableEnumerable<T> ?? new VerifiableEnumerable<T>(sequence);
        }
    }
}