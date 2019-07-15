#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Atif Aziz. All rights reserved.
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

namespace MoreLinq.Reactive
{
    using System;
    using System.Collections.Generic;
    using Delegate = Delegating.Delegate;

    sealed class Subject<T> : IObservable<T>, IObserver<T>
    {
        List<IObserver<T>> _observers;
        bool _completed;
        Exception _error;

        bool HasObservers => (_observers?.Count ?? 0) > 0;
        List<IObserver<T>> Observers => _observers ?? (_observers = new List<IObserver<T>>());

        bool IsMuted => _completed || _error != null;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));

            if (_error is Exception error)
            {
                observer.OnError(error);
                return Disposable.Nop;
            }

            if (_completed)
            {
                observer.OnCompleted();
                return Disposable.Nop;
            }

            Observers.Add(observer);
            return Delegate.Disposable(() => Observers.Remove(observer));
        }

        public void OnNext(T value)
        {
            if (!HasObservers)
                return;

            foreach (var observer in Observers)
                observer.OnNext(value);
        }

        public void OnError(Exception error)
        {
            if (IsMuted)
                return;

            _error = error;

            // Once an error occurs, no other method of the subject is expected
            // to be called so release the list of observers from the subject's
            // state. The list of observers will be garbage once this method
            // returns.

            if (!(Assignment.Set(ref _observers, default) is List<IObserver<T>> observers))
                return;

            foreach (var observer in observers)
                observer.OnError(error);
        }

        public void OnCompleted()
        {
            if (IsMuted)
                return;

            _completed = true;

            // Once an error occurs, no other method of the subject is expected
            // to be called so release the list of observers from the subject's
            // state. The list of observers will be garbage once this method
            // returns.

            if (!(Assignment.Set(ref _observers, default) is List<IObserver<T>> observers))
                return;

            foreach (var observer in observers)
                observer.OnCompleted();
        }
    }
}
