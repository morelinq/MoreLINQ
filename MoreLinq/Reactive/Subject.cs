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
        List<Observer> _observers;
        bool _completed;
        Exception _error;

        bool HasObservers => (_observers?.Count ?? 0) > 0;
        List<Observer> Observers => _observers ?? (_observers = new List<Observer>());

        bool IsMuted => _completed || _error != null;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));

            if (_error != null)
            {
                observer.OnError(_error);
                return Disposable.Nop;
            }

            if (_completed)
            {
                observer.OnCompleted();
                return Disposable.Nop;
            }

            Observers.Add(new Observer(observer));

            return Delegate.Disposable(() =>
            {
                var observers = Observers;
                for (var i = 0; i < observers.Count; i++)
                {
                    if (observers[i].Is(observer))
                    {
                        if (_shouldDeleteObserver)
                            observers[i].Delete();
                        else
                            observers.RemoveAt(i);
                        break;
                    }
                }
            });
        }

        bool _shouldDeleteObserver;

        public void OnNext(T value)
        {
            if (!HasObservers)
                return;

            var observers = Observers;

            // An observer can dispose its subscription during the call to
            // OnNext but the observers collection cannot be modified while
            // its being iterated. Set a flag around iteration to indicate
            // that observer that dispose their subscription should be marked
            // for deletion.

            _shouldDeleteObserver = true;

            try
            {
                foreach (var observer in observers)
                    observer.OnNext(value);
            }
            finally
            {
                _shouldDeleteObserver = false;

                // Remove any observers that were marked for deletion during
                // iteration.

                for (var i = observers.Count - 1; i >= 0; i--)
                {
                    if (observers[i].IsDeleted)
                        observers.RemoveAt(i);
                }
            }
        }

        public void OnError(Exception error) =>
            OnFinality(ref _error, error, (observer, err) => observer.OnError(err));

        public void OnCompleted() =>
            OnFinality(ref _completed, true, (observer, _) => observer.OnCompleted());

        void OnFinality<TState>(ref TState state, TState value, Action<IObserver<T>, TState> action)
        {
            if (IsMuted)
                return;

            state = value;

            // Once an error occurs, no other method of the subject is expected
            // to be called so release the list of observers of this subject.
            // The list of observers will be garbage once this method returns.

            var observers = _observers;
            _observers = null;

            if (observers == null)
                return;

            foreach (var observer in observers)
                action(observer, value);
        }

        /// <summary>
        /// An entry record holding an <see cref="IObserver{T}"/> along with a
        /// flag indicating whether the entry is marked for deletion or not.
        /// </summary>

        sealed class Observer : IObserver<T>
        {
            readonly IObserver<T> _observer;

            public Observer(IObserver<T> observer) =>
                _observer = observer;

            public bool Is(IObserver<T> other) => _observer == other;

            public bool IsDeleted { get; private set; }
            public void Delete() => IsDeleted = true;

            public void OnCompleted()            { if (!IsDeleted) _observer.OnCompleted();  }
            public void OnError(Exception error) { if (!IsDeleted) _observer.OnError(error); }
            public void OnNext(T value)          { if (!IsDeleted) _observer.OnNext(value);  }
        }
    }
}
