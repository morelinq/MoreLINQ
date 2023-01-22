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
    using Delegate = Delegating.Delegate;

    /// <summary>
    /// Provides a set of static methods for writing in-memory queries over observable sequences.
    /// </summary>

    static partial class Observable
    {
        /// <summary>
        /// Subscribes an element handler and a completion handler to an
        /// observable sequence.
        /// </summary>
        /// <typeparam name="T">Type of elements in <paramref name="source"/>.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">
        /// Action to invoke for each element in <paramref name="source"/>.</param>
        /// <param name="onError">
        /// Action to invoke upon exceptional termination of the
        /// <paramref name="source"/>.</param>
        /// <param name="onCompleted">
        /// Action to invoke upon graceful termination of <paramref name="source"/>.</param>
        /// <returns>The subscription, which when disposed, will unsubscribe
        /// from <paramref name="source"/>.</returns>

        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception>? onError = null, Action? onCompleted = null) =>
            source == null
            ? throw new ArgumentNullException(nameof(source))
            : source.Subscribe(Delegate.Observer(onNext, onError, onCompleted));
    }
}
