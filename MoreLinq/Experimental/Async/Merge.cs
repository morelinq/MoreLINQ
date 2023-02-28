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
        /// Concurrently merges all the elements of multiple asynchronous streams into a single
        /// asynchronous stream.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements in <paramref name="sources"/>.</typeparam>
        /// <param name="sources">The sequence of asynchronous streams.</param>
        /// <returns>
        /// An asynchronous stream with all elements from all <paramref name="sources"/>.
        /// </returns>
        /// <remarks>
        /// <para>This operator uses deferred execution and streams its results.</para>
        /// <para>
        /// The elements in the resulting stream may appear in a different order than their order in
        /// <paramref name="sources"/>.</para>
        /// <para>
        /// When disposed part of the way, there is a best-effort attempt to cancel all iterations
        /// that are in flight. This requires that all asynchronous streams in <paramref
        /// name="sources"/> properly honour timely cancellation.</para>
        /// </remarks>

        public static IAsyncEnumerable<T> Merge<T>(this IEnumerable<IAsyncEnumerable<T>> sources) =>
            Merge(sources, int.MaxValue);

        /// <summary>
        /// Concurrently merges all the elements of multiple asynchronous streams into a single
        /// asynchronous stream. An additional parameter specifies the maximum concurrent operations
        /// that may be in flight at any give time.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements in <paramref name="sources"/>.</typeparam>
        /// <param name="sources">The sequence of asynchronous streams.</param>
        /// <param name="maxConcurrent">
        /// Maximum number of asynchronous operations to have in flight at any given time. A value
        /// of 1 (or below) disables concurrency.</param>
        /// <returns>
        /// An asynchronous stream with all elements from all <paramref name="sources"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// <para>
        /// When <paramref name="maxConcurrent"/> is 2 or greater then the elements in the resulting
        /// stream may appear in a different order than their order in <paramref
        /// name="sources"/>.</para>
        /// <para>
        /// When disposed part of the way, there is a best-effort attempt to cancel all iterations
        /// that are in flight. This requires that all asynchronous streams in <paramref
        /// name="sources"/> properly honour timely cancellation.</para>
        /// </remarks>

        public static IAsyncEnumerable<T> Merge<T>(this IEnumerable<IAsyncEnumerable<T>> sources,
                                                   int maxConcurrent)
        {
            if (sources is null) throw new ArgumentNullException(nameof(sources));
            if (maxConcurrent <= 0) throw new ArgumentOutOfRangeException(nameof(maxConcurrent));

            return Async();

            async IAsyncEnumerable<T> Async([EnumeratorCancellation]CancellationToken cancellationToken = default)
            {
                using var thisCancellationTokenSource = new CancellationTokenSource();

                using var cancellationTokenSource =
                    cancellationToken.CanBeCanceled
                    ? CancellationTokenSource.CreateLinkedTokenSource(thisCancellationTokenSource.Token, cancellationToken)
                    : thisCancellationTokenSource;

                cancellationToken = cancellationTokenSource.Token;

                var enumeratorList = new List<IAsyncEnumerator<T>>();
                var disposalTaskList = (List<Task>?)null;
                var pendingTaskList = (List<Task<(bool, IAsyncEnumerator<T>)>>?)null;

                try
                {
                    enumeratorList.AddRange(from s in sources
                                            select s.GetAsyncEnumerator(cancellationToken));

                    pendingTaskList = new List<Task<(bool, IAsyncEnumerator<T>)>>();

                    const bool some = true;

                    ValueTask? DisposeAsync(IAsyncEnumerator<T> enumerator)
                    {
                        _ = enumeratorList.Remove(enumerator);
                        var disposalTask = enumerator.DisposeAsync();
                        if (disposalTask.IsCompleted)
                            return disposalTask;
                        disposalTaskList ??= new List<Task>();
                        disposalTaskList.Add(disposalTask.AsTask());
                        return null;
                    }

                    async ValueTask<(bool, T)> ReadAsync(IAsyncEnumerator<T> enumerator)
                    {
                        var task = enumerator.MoveNextAsync();

                        if (task.IsCompleted)
                        {
                            if (await task.ConfigureAwait(false))
                                return (some, enumerator.Current);

                            if (DisposeAsync(enumerator) is { IsCompleted: true } completedDisposalTask)
                                await completedDisposalTask.ConfigureAwait(false);
                        }
                        else
                        {
                            pendingTaskList.Add(task.And(enumerator).AsTask());
                        }

                        return default;
                    }

                    while (enumeratorList.Count > 0)
                    {
                        while (pendingTaskList.Count is var ptc
                               && ptc < enumeratorList.Count
                               && ptc < maxConcurrent)
                        {
                            var i = pendingTaskList.Count;
                            var enumerator = enumeratorList[i];

                            while (await ReadAsync(enumerator).ConfigureAwait(false) is (some, var item))
                                yield return item;
                        }

                        while (pendingTaskList.Count > 0)
                        {
                            var completedTask = await Task.WhenAny(pendingTaskList).ConfigureAwait(false);
                            var (moved, enumerator) = await completedTask.ConfigureAwait(false);
                            _ = pendingTaskList.Remove(completedTask);

                            if (moved)
                            {
                                yield return enumerator.Current;

                                while (await ReadAsync(enumerator).ConfigureAwait(false) is (some, var item))
                                    yield return item;
                            }
                            else if (DisposeAsync(enumerator) is { IsCompleted: true } completedDisposalTask)
                            {
                                await completedDisposalTask.ConfigureAwait(false);
                            }
                        }
                    }
                }
                finally
                {
                    // Signal cancellation to those in flight. Unfortunately, this relies on all
                    // iterators to honour the cancellation.

                    thisCancellationTokenSource.Cancel();

                    // > The caller of an async-iterator method should only call `DisposeAsync()`
                    // > when the method completed or was suspended by a `yield return`.
                    //
                    // Source: https://github.com/dotnet/roslyn/blob/0e7b657bf6c019ec8019dcbd4f833f0dda50a97d/docs/features/async-streams.md#disposal
                    //
                    // > The result of invoking `DisposeAsync` from states -1 or N is unspecified.
                    // > This compiler generates `throw new NotSupportException()` for those cases.
                    //
                    // Source: https://github.com/dotnet/roslyn/blob/0e7b657bf6c019ec8019dcbd4f833f0dda50a97d/docs/features/async-streams.md#state-values-and-transitions
                    //
                    // As a result, wait for all pending tasks to complete, irrespective of how they
                    // complete (successfully, faulted or canceled). The goal is that the iterator
                    // is in some defined state before disposing it otherwise it could throw
                    // "NotSupportedException".

                    if (pendingTaskList is { Count: > 0 })
                    {
                        while (await Task.WhenAny(pendingTaskList)
                                         .ConfigureAwait(false) is { } completedTask)
                        {
                            _ = pendingTaskList.Remove(completedTask);
                        }
                    }

                    foreach (var enumerator in enumeratorList)
                    {
                        ValueTask task;

                        try
                        {
                            task = enumerator.DisposeAsync();
                        }
                        catch (NotSupportedException)
                        {
                            // Ignore just in case we hit an unspecified case;
                            // see quoted notes from Roslyn spec above.

                            continue;
                        }

                        if (task.IsCompleted)
                        {
                            await task.ConfigureAwait(false);
                        }
                        else
                        {
                            disposalTaskList ??= new List<Task>();
                            disposalTaskList.Add(task.AsTask());
                        }
                    }

                    if (disposalTaskList is { Count: > 0 })
                        await Task.WhenAll(disposalTaskList).ConfigureAwait(false);
                }
            }
        }

        static async ValueTask<(T1, T2)> And<T1, T2>(this ValueTask<T1> task, T2 second) =>
            (await task.ConfigureAwait(false), second);
    }
}

#endif // !NO_ASYNC_STREAMS
