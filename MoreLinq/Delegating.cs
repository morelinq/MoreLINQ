#region Copyright (C) 2017 Atif Aziz. All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//
#endregion

// https://github.com/atifaziz/Delegating

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Delegating
{
    using System;
    using System.Threading;

    static class Delegate
    {
        public static IDisposable Disposable(Action delegatee) =>
            new DelegatingDisposable(delegatee);

        public static IObserver<T> Observer<T>(Action<T> onNext,
                                               Action<Exception>? onError = null,
                                               Action? onCompleted = null) =>
            new DelegatingObserver<T>(onNext, onError, onCompleted);
    }

    sealed class DelegatingDisposable : IDisposable
    {
        Action? _delegatee;

        public DelegatingDisposable(Action delegatee) =>
            _delegatee = delegatee ?? throw new ArgumentNullException(nameof(delegatee));

        public void Dispose()
        {
            var delegatee = _delegatee;
            if (delegatee == null || Interlocked.CompareExchange(ref _delegatee, null, delegatee) != delegatee)
                return;
            delegatee();
        }
    }

    sealed class DelegatingObserver<T> : IObserver<T>
    {
        readonly Action<T> _onNext;
        readonly Action<Exception>? _onError;
        readonly Action? _onCompleted;

        public DelegatingObserver(Action<T> onNext,
                                  Action<Exception>? onError = null,
                                  Action? onCompleted = null)
        {
            _onNext = onNext ?? throw new ArgumentNullException(nameof(onNext));
            _onError = onError;
            _onCompleted = onCompleted;
        }

        public void OnCompleted() => _onCompleted?.Invoke();
        public void OnError(Exception error) => _onError?.Invoke(error);
        public void OnNext(T value) => _onNext(value);
    }
}
