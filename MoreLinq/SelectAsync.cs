#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Atif Aziz. All rights reserved.
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

#if !NO_ASYNC

namespace MoreLinq
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.ExceptionServices;
    using System.Threading;
    using System.Threading.Tasks;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Asynchronously projects each element of a sequence to its new form.
        /// </summary>

        public static IEnumerable<TResult> SelectAsync<T, TResult>(
            this IEnumerable<T> source, Func<T, Task<TResult>> selector)
        {
            return source.SelectAsync(int.MaxValue, null, selector);
        }

        /// <summary>
        /// Asynchronously projects each element of a sequence to its new form.
        /// The projection function receives a <see cref="CancellationToken"/>
        /// as an additional argument that can be used to abort any
        /// asynchronous operations in flight.
        /// </summary>

        public static IEnumerable<TResult> SelectAsync<T, TResult>(
            this IEnumerable<T> source, Func<T, CancellationToken, Task<TResult>> selector)
        {
            return source.SelectAsync(int.MaxValue, null, selector);
        }

        /// <summary>
        /// Asynchronously projects each element of a sequence to its new form
        /// with a given concurrency.
        /// </summary>
        /// <remarks>
        /// The <paramref name="selector"/> function should be designed to be
        /// thread-agnostic.
        /// </remarks>

        public static IEnumerable<TResult> SelectAsync<T, TResult>(
            this IEnumerable<T> source,
            int maxConcurrency,
            Func<T, Task<TResult>> selector)
        {
            return source.SelectAsync(maxConcurrency, null, selector);
        }

        /// <summary>
        /// Asynchronously projects each element of a sequence to its new form
        /// with a given concurrency. The projection function receives a
        /// <see cref="CancellationToken"/> as an additional argument that can
        /// be used to abort any asynchronous operations in flight.
        /// </summary>
        /// <remarks>
        /// The <paramref name="selector"/> function should be designed to be
        /// thread-agnostic.
        /// </remarks>

        public static IEnumerable<TResult> SelectAsync<T, TResult>(
            this IEnumerable<T> source,
            int maxConcurrency,
            Func<T, CancellationToken, Task<TResult>> selector)
        {
            return source.SelectAsync(maxConcurrency, null, selector);
        }

        /// <summary>
        /// Asynchronously projects each element of a sequence to its new form
        /// with a given concurrency. An additional parameter specifies the
        /// <see cref="TaskScheduler"/> to use to await for tasks to complete.
        /// </summary>
        /// <remarks>
        /// The <paramref name="selector"/> function should be designed to be
        /// thread-agnostic.
        /// </remarks>

        public static IEnumerable<TResult> SelectAsync<T, TResult>(
            this IEnumerable<T> source,
            int maxConcurrency,
            TaskScheduler scheduler,
            Func<T, Task<TResult>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return source.SelectAsync(maxConcurrency, scheduler, (e, _) => selector(e));
        }

        /// <summary>
        /// Asynchronously projects each element of a sequence to its new form
        /// with a given concurrency. An additional parameter specifies the
        /// <see cref="TaskScheduler"/> to use to await for tasks to complete.
        /// </summary>
        /// <remarks>
        /// The <paramref name="selector"/> function should be designed to be
        /// thread-agnostic.
        /// </remarks>

        public static IEnumerable<TResult> SelectAsync<T, TResult>(
            this IEnumerable<T> source,
            int maxConcurrency,
            TaskScheduler scheduler,
            Func<T, CancellationToken, Task<TResult>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (maxConcurrency <= 0) throw new ArgumentOutOfRangeException(nameof(maxConcurrency));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return _(); IEnumerable<TResult> _()
            {
                var queue = new BlockingCollection<object>();
                var cancellationTokenSource = new CancellationTokenSource();
                var completed = false;

                var item = source.GetEnumerator();
                IDisposable disposable = item; // disables AccessToDisposedClosure warnings
                try
                {
                    Task.Factory.StartNew(async () =>
                        {
                            var cancellationToken = cancellationTokenSource.Token;
                            var cancellationTaskSource = new TaskCompletionSource<bool>();
                            cancellationToken.Register(() => cancellationTaskSource.TrySetResult(true));

                            var tasks = new List<Task<TResult>>();

                            var more = false;
                            for (var i = 0; i < maxConcurrency && (more = item.MoveNext()); i++)
                                tasks.Add(selector(item.Current, cancellationToken));

                            if (!more)
                                item.Dispose();

                            try
                            {
                                while (tasks.Count > 0)
                                {
                                    // Task.WaitAny is synchronous and blocking
                                    // but allows the waiting to be cancelled
                                    // via a CancellationToken. Task.WhenAny can
                                    // be awaited so it is better since the
                                    // tread won't be blocked and can return to
                                    // the pool. However, it doesn't support
                                    // cancellation so instead a task is built
                                    // on top of the CancellationToken that
                                    // completes when the CancellationToken
                                    // trips.

                                    var task = await Task.WhenAny(tasks.Cast<Task>().Concat(cancellationTaskSource.Task));

                                    if (task == cancellationTaskSource.Task)
                                    {
                                        // Cancellation during the wait means
                                        // the enumeration has been stopped by
                                        // the user so the results of the
                                        // remaining tasks are no longer needed.
                                        // Those tasks should cancel as a result
                                        // of sharing the same cancellation
                                        // token and provided that they passed
                                        // it on to any downstream asynchronous
                                        // operations. Either way, this loop
                                        // is done so exit hard here.

                                        return;
                                    }

                                    tasks.Remove((Task<TResult>)task);
                                    queue.Add(task);

                                    if (more && (more = item.MoveNext()))
                                        tasks.Add(selector(item.Current, cancellationToken));
                                }
                                queue.Add(null);
                            }
                            catch (Exception e)
                            {
                                cancellationTokenSource.Cancel();
                                queue.Add(ExceptionDispatchInfo.Capture(e));
                            }
                            queue.CompleteAdding();
                        },
                        CancellationToken.None,
                        TaskCreationOptions.DenyChildAttach,
                        scheduler ?? TaskScheduler.Default);

                    foreach (var e in queue.GetConsumingEnumerable())
                    {
                        (e as ExceptionDispatchInfo)?.Throw();
                        if (e == null)
                            break;
                        yield return ((Task<TResult>) e).Result;
                    }

                    completed = true;
                }
                finally
                {
                    // The cancellation token is signaled here for the case where
                    // tasks may be in flight but the user stopped the enumeration
                    // partway (e.g. SelectAsync was combined with a Take or
                    // TakeWhile). The in-flight tasks need to be aborted as well
                    // as the awaiter loop.

                    if (!completed)
                        cancellationTokenSource.Cancel();
                    disposable.Dispose();
                }
            }
        }
    }
}

#endif // !NO_ASYNC