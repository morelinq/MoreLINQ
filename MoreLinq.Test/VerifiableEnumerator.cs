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
        class VerifiableEnumerable<T> : IVerifiableEnumerable<T>
        {
            readonly IEnumerable<T> _enumerable;

            static readonly Action<IEnumerable<T>> DefaultAction = s => { return; };

            Action<IEnumerable<T>> _onEnumerateAction = DefaultAction;
            Action<IEnumerable<T>> _onDisposeAction = DefaultAction;
            Action<IEnumerable<T>> _onMoveNextAction = DefaultAction;
            Action<IEnumerable<T>> _onResetAction = DefaultAction;
            Action<IEnumerable<T>> _onCurrentAction = DefaultAction;

            public VerifiableEnumerable(IEnumerable<T> enumerable)
            {
                _enumerable = enumerable;
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
                _onDisposeAction = action ?? DefaultAction;
                return this;
            }

            IVerifiableEnumerable<T> IVerifiableEnumerable<T>.WhenEnumerated(Action<IEnumerable<T>> action)
            {
                _onEnumerateAction = action ?? DefaultAction;
                return this;
            }

            IVerifiableEnumerable<T> IVerifiableEnumerable<T>.WhenMoveNext(Action<IEnumerable<T>> action)
            {
                _onMoveNextAction = action ?? DefaultAction;
                return this;
            }

            IVerifiableEnumerable<T> IVerifiableEnumerable<T>.WhenReset(Action<IEnumerable<T>> action)
            {
                _onResetAction = action ?? DefaultAction;
                return this;
            }

            IVerifiableEnumerable<T> IVerifiableEnumerable<T>.WhenCurrent(Action<IEnumerable<T>> action)
            {
                _onCurrentAction = action ?? DefaultAction;
                return this;
            }

            class VerifiableEnumerator : IEnumerator<T>
            {
                readonly IEnumerator<T> _enumerator;
                readonly VerifiableEnumerable<T> _enumerable;

                public VerifiableEnumerator(VerifiableEnumerable<T> enumerable)
                {
                    _enumerable = enumerable;
                    _enumerator = enumerable._enumerable.GetEnumerator();
                    _enumerable._onEnumerateAction(_enumerable);
                }

                public void Dispose()
                {
                    _enumerable._onDisposeAction(_enumerable);
                    _enumerator.Dispose();
                }

                public bool MoveNext()
                {
                    _enumerable._onMoveNextAction(_enumerable);
                    return _enumerator.MoveNext();
                }

                public void Reset()
                {
                    _enumerable._onResetAction(_enumerable);
                    _enumerator.Reset();
                }

                public T Current
                {
                    get
                    {
                        _enumerable._onCurrentAction(_enumerable);
                        return _enumerator.Current;
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