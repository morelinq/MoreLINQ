#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Atif Aziz. All rights reserved.
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

namespace MoreLinq.Test
{
    using System;

    static class Observer
    {
        public static IObserver<T>
            Create<T>(Action<T> onNext,
                      Action<Exception> onError = null,
                      Action onCompleted = null) =>
            new Observer<T>(onNext, onError, onCompleted);
    }

    sealed class Observer<T> : IObserver<T>
    {
        public static readonly IObserver<T> None = Observer.Create<T>(delegate { });

        readonly Action<T> _onNext;
        readonly Action<Exception> _onError;
        readonly Action _onCompleted;

        public Observer(Action<T> onNext,
                        Action<Exception> onError = null,
                        Action onCompleted = null)
        {
            _onNext      = onNext ?? throw new ArgumentNullException(nameof(onNext));
            _onError     = onError ?? (e => throw e);
            _onCompleted = onCompleted;
        }

        public void OnCompleted() => _onCompleted?.Invoke();
        public void OnError(Exception error) => _onError(error);
        public void OnNext(T value) => _onNext(value);
    }
}
