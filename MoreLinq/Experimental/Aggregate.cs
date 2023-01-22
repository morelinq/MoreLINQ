#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All rights reserved.
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

namespace MoreLinq.Experimental
{
    using System;
    using Reactive;

    static partial class ExperimentalEnumerable
    {
        static IDisposable SubscribeSingle<T, TResult>(Func<IObservable<T>, IObservable<TResult>> aggregatorSelector, IObservable<T> subject, (bool Defined, TResult)[] r, string paramName)
        {
            var aggregator = aggregatorSelector(subject);
            return ReferenceEquals(aggregator, subject)
                 ? throw new ArgumentException("Aggregator cannot be identical to the source.", paramName)
                 : aggregator.Subscribe(s =>
                     r[0] = r[0].Defined
                         ? throw new InvalidOperationException(
                             $"Aggregator supplied for parameter \"{paramName}\" produced multiple results when only one is allowed.")
                         : (true, s));
        }

        static T GetAggregateResult<T>((bool Defined, T Value) result, string paramName) =>
            !result.Defined
            ? throw new InvalidOperationException($"Aggregator supplied for parameter \"{paramName}\" has an undefined result.")
            : result.Value;
    }
}
