#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2020 Atif Aziz. All rights reserved.
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

#if !NO_ASYNC_STREAMS

namespace MoreLinq.Experimental.Async
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Projects each element of a sequence to an asynchronous stream,
        /// flattens the resulting asynchronous stream into one asynchronous stream,
        /// and invokes a result selector function on each element therein.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TCollection">
        /// The type of the intermediate elements collected by
        /// <paramref name="collectionSelector"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements of the resulting sequence.</typeparam>
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="collectionSelector">
        /// A transform function to apply to each element of the input sequence.</param>
        /// <param name="resultSelector">
        /// A transform function to apply to each element of the intermediate
        /// asynchronous stream.</param>
        /// <returns>
        /// An <see cref="IAsyncEnumerable{T}"/> whose elements are the result of
        /// invoking the one-to-many transform function <paramref name="collectionSelector"/>
        /// on each element of source and then mapping each of those sequence elements
        /// and their corresponding source element to a result element.
        /// </returns>

        public static IAsyncQuery<TResult>
            SelectMany<TSource, TCollection, TResult>(
                IEnumerable<TSource> source,
                Func<TSource, IAsyncEnumerable<TCollection>> collectionSelector,
                Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (collectionSelector is null) throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return AsyncQuery.Create(options =>
                SelectMany(source, collectionSelector, resultSelector, options, default));
        }

        // TODO Convert to local function when moving to C# 9 or later

        static async IAsyncEnumerable<TResult>
            SelectMany<TSource, TCollection, TResult>(
                IEnumerable<TSource> source,
                Func<TSource, IAsyncEnumerable<TCollection>> collectionSelector,
                Func<TSource, TCollection, TResult> resultSelector,
                AsyncQueryOptions options,
                [EnumeratorCancellation]CancellationToken cancellationToken)
        {
            var enumeratorList = new List<(TSource, IAsyncEnumerator<TCollection>)>();

            try
            {
                var enumerators =
                    from item in source
                    select (item, collectionSelector(item).GetAsyncEnumerator(cancellationToken));

                enumeratorList.AddRange(enumerators);

                var pendingTaskList = new List<Task<(bool, TSource, IAsyncEnumerator<TCollection>)>>();

                const bool some = true;

                async ValueTask<(bool, TResult)>
                    ReadAsync(TSource item, IAsyncEnumerator<TCollection> enumerator)
                {
                    var task = enumerator.MoveNextAsync();

                    if (task.IsCompleted)
                    {
                        if (await task.ConfigureAwait(false))
                            return (some, resultSelector(item, enumerator.Current));

                        await enumerator.DisposeAsync().ConfigureAwait(false);
                        enumeratorList.Remove((item, enumerator));
                    }
                    else
                    {
                        pendingTaskList.Add(task.And(item, enumerator).AsTask());
                    }

                    return default;
                }

                var maxConcurrency = options.MaxConcurrency;

                while (enumeratorList.Count > 0)
                {
                    while (pendingTaskList.Count < enumeratorList.Count &&
                           (maxConcurrency is null || pendingTaskList.Count < maxConcurrency))
                    {
                        var i = pendingTaskList.Count;
                        var (item, enumerator) = enumeratorList[i];

                        while (await ReadAsync(item, enumerator).ConfigureAwait(false)
                               is (some, var resultItem))
                        {
                            yield return resultItem;
                        }
                    }

                    while (pendingTaskList.Count > 0)
                    {
                        var completedTask = await Task.WhenAny(pendingTaskList).ConfigureAwait(false);
                        var (moved, item, enumerator) = await completedTask.ConfigureAwait(false);
                        pendingTaskList.Remove(completedTask);

                        if (moved)
                        {
                            yield return resultSelector(item, enumerator.Current);

                            while (await ReadAsync(item, enumerator).ConfigureAwait(false)
                                   is (some, var resultItem))
                            {
                                yield return resultItem;
                            }
                        }
                        else
                        {
                            await enumerator.DisposeAsync().ConfigureAwait(false);
                            enumeratorList.Remove((item, enumerator));
                            break;
                        }
                    }
                }
            }
            finally
            {
                foreach (var (_, enumerator) in enumeratorList)
                    await enumerator.DisposeAsync().ConfigureAwait(false);
            }
        }

        static async ValueTask<(T1, T2, T3)> And<T1, T2, T3>(this ValueTask<T1> task, T2 second, T3 third) =>
            (await task, second, third);
    }
}

#endif // !NO_ASYNC_STREAMS
