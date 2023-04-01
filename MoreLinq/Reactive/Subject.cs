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
        List<IObserver<T>>? _observers;
        bool _completed;
        Exception? _error;

        bool HasObservers => (_observers?.Count ?? 0) > 0;
        List<IObserver<T>> Observers => _observers ??= new List<IObserver<T>>();

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

            Observers.Add(observer);

            return Delegate.Disposable(() =>
            {
                var observers = Observers;

                // Could do the following to find the index of the
                // the observer:
                //
                // var i = observers.FindIndex(o => o == observer);
                //
                // but it would require a closure allocation.

                for (var i = 0; i < observers.Count; i++)
                {
                    if (observers[i] == observer)
                    {
                        if (_shouldDeleteObserver)
                            observers[i] = null!;
                        else
                            observers.RemoveAt(i);
                        break;
                    }
                }
            });
        }

        bool _shouldDeleteObserver; // delete (null) or remove an observer?

        public void OnNext(T value)
        {
            if (!HasObservers)
                return;

            var observers = Observers;

            // Set a flag around iteration to indicate that an observer that
            // disposes their subscription should be marked for deletion
            // instead of being removed from the list of observers. The actual
            // removal is then deferred until after the iteration is complete.

            _shouldDeleteObserver = true;

            try
            {
                // DO NOT change the following loop into the for-each variant
                // because an observer might dispose its subscription during
                // the call to "OnNext" and List<T>'s enumerator will throw
                // seeing that as a modification of the collection during
                // enumeration.

                for (var i = 0; i < observers.Count; i++)
                    observers[i].OnNext(value);
            }
            finally
            {
                _shouldDeleteObserver = false;

                // Remove any observers that were marked for deletion during
                // iteration.

                _ = observers.RemoveAll(o => o == null);
            }
        }

        public void OnError(Exception error) =>
            OnFinality(ref _error, error, (observer, err) => observer.OnError(err));

        public void OnCompleted() =>
            OnFinality(ref _completed, true, (observer, _) => observer.OnCompleted());

        void OnFinality<TState>(ref TState? state, TState value, Action<IObserver<T>, TState> action)
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
    }
}
