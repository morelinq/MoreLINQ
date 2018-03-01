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

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.ExceptionServices;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents options for an asynchronous projection operation.
    /// </summary>

    public sealed class SelectAsyncOptions
    {
        /// <summary>
        /// The default options an asynchronous projection operation.
        /// </summary>

        public static readonly SelectAsyncOptions Default =
            new SelectAsyncOptions(null /* = unbounded concurrency */,
                                   TaskScheduler.Default,
                                   preserveOrder: false);

        /// <summary>
        /// Gets a positive (non-zero) integer that specifies the maximum
        /// projections to run concurrenctly or <c>null</c> to mean unlimited
        /// concurrency.
        /// </summary>

        public int? MaxConcurrency { get; }

        /// <summary>
        /// Get the scheduler to be used for any workhorse task.
        /// </summary>

        public TaskScheduler Scheduler { get; }

        /// <summary>
        /// Get a Boolean that determines whether results should be ordered
        /// the same as the projection source.
        /// </summary>

        public bool PreserveOrder { get; }

        SelectAsyncOptions(int? maxConcurrency, TaskScheduler scheduler, bool preserveOrder)
        {
            MaxConcurrency = maxConcurrency == null || maxConcurrency > 0
                           ? maxConcurrency
                           : throw new ArgumentOutOfRangeException(
                                 nameof(maxConcurrency), maxConcurrency,
                                 "Maximum concurrency must be 1 or greater.");
            Scheduler      = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            PreserveOrder  = preserveOrder;
        }

        /// <summary>
        /// Returns new options with the given concurrency limit.
        /// </summary>

        public SelectAsyncOptions WithMaxConcurrency(int? value) =>
            value == MaxConcurrency ? this : new SelectAsyncOptions(value, Scheduler, PreserveOrder);

        /// <summary>
        /// Returns new options with the given scheduler.
        /// </summary>

        public SelectAsyncOptions WithScheduler(TaskScheduler value) =>
            value == Scheduler ? this : new SelectAsyncOptions(MaxConcurrency, value, PreserveOrder);

        /// <summary>
        /// Returns new options with the given Boolean indicating whether or
        /// not the projections should be returned in the order of the
        /// projection source.
        /// </summary>

        public SelectAsyncOptions WithPreserveOrder(bool value) =>
            value == PreserveOrder ? this : new SelectAsyncOptions(MaxConcurrency, Scheduler, value);
    }

    /// <summary>
    /// An <see cref="IEnumerable{T}"/> representing an asynchronous projection.
    /// </summary>
    /// <inheritdoc />

    public interface ISelectAsyncEnumerable<out T> : IEnumerable<T>
    {
        /// <summary>
        /// The options to apply to this asynchronous projection operation.
        /// </summary>

        SelectAsyncOptions Options { get; }

        /// <summary>
        /// Returns a new asynchronous projection operation that will use the
        /// given options.
        /// </summary>

        ISelectAsyncEnumerable<T> WithOptions(SelectAsyncOptions options);
    }

    static partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Returns a new asynchronous projection operation with the given
        /// concurrency limit.
        /// </summary>

        public static ISelectAsyncEnumerable<T> MaxConcurrency<T>(this ISelectAsyncEnumerable<T> source, int? value)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.WithOptions(source.Options.WithMaxConcurrency(value));
        }

        /// <summary>
        /// Returns a new asynchronous projection operation with the given
        /// scheduler.
        /// </summary>

        public static ISelectAsyncEnumerable<T> Scheduler<T>(this ISelectAsyncEnumerable<T> source, TaskScheduler value)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (value == null) throw new ArgumentNullException(nameof(value));
            return source.WithOptions(source.Options.WithScheduler(value));
        }

        /// <summary>
        /// Returns a new asynchronous projection operation for which the
        /// results will be returned in the order of the source sequence.
        /// </summary>
        /// <remarks>
        /// Internally, the projections will be done concurrently but the
        /// results will be yielded in order.
        /// </remarks>

        public static ISelectAsyncEnumerable<T> AsOrdered<T>(this ISelectAsyncEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return PreserveOrder(source, true);
        }

        /// <summary>
        /// Returns a new asynchronous projection operation for which the
        /// results are no longer guaranteed to be in the order of the source
        /// sequence.
        /// </summary>

        public static ISelectAsyncEnumerable<T> AsUnordered<T>(this ISelectAsyncEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return PreserveOrder(source, false);
        }

        /// <summary>
        /// Returns a new asynchronous projection operation with the given
        /// Boolean indicating whether or not the projections should be
        /// returned in the order of the projection source.
        /// </summary>

        public static ISelectAsyncEnumerable<T> PreserveOrder<T>(this ISelectAsyncEnumerable<T> source, bool value)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.WithOptions(source.Options.WithPreserveOrder(value));
        }

        /// <summary>
        /// Asynchronously projects each element of a sequence to its new form.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method uses deferred execution semantics. The results are
        /// yielded as each asynchronous projection completes and, by default,
        /// not guaranteed to be based on the source sequence order. If order
        /// is important, compose further with
        /// <see cref="AsOrdered{T}"/>.</para>
        /// <para>
        /// This method starts a new task where the asynchronous projections
        /// are started and awaited.</para>
        /// <para>
        /// The <paramref name="selector"/> function should be designed to be
        /// thread-agnostic.</para>
        /// </remarks>

        public static ISelectAsyncEnumerable<TResult> SelectAsync<T, TResult>(
            this IEnumerable<T> source, Func<T, Task<TResult>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return source.SelectAsync((e, _) => selector(e));
        }

        /// <summary>
        /// Asynchronously projects each element of a sequence to its new form.
        /// The projection function receives a <see cref="CancellationToken"/>
        /// as an additional argument that can be used to abort any asynchronous
        /// operations in flight.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method uses deferred execution semantics. The results are
        /// yielded as each asynchronous projection completes and, by default,
        /// not guaranteed to be based on the source sequence order. If order
        /// is important, compose further using
        /// <see cref="AsOrdered{T}"/>
        /// and a Boolean value of <c>true</c>.</para>
        /// <para>
        /// This method starts a new task where the asynchronous projections
        /// are started and awaited.</para>
        /// <para>
        /// The <paramref name="selector"/> function should be designed to be
        /// thread-agnostic.</para>
        /// </remarks>

        public static ISelectAsyncEnumerable<TResult> SelectAsync<T, TResult>(
            this IEnumerable<T> source, Func<T, CancellationToken, Task<TResult>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return SelectAsyncEnumerable.Create(
                options => _(options.MaxConcurrency ?? int.MaxValue,
                             options.Scheduler ?? TaskScheduler.Default,
                             options.PreserveOrder));

            IEnumerable<TResult> _(int maxConcurrency, TaskScheduler scheduler, bool ordered)
            {
                var queue = new BlockingCollection<object>();
                var cancellationTokenSource = new CancellationTokenSource();
                var completed = false;

                async Task<KeyValuePair<int, TResult>> Select(KeyValuePair<int, T> input, CancellationToken cancellationToken) =>
                    new KeyValuePair<int, TResult>(input.Key, await selector(input.Value, cancellationToken).ConfigureAwait(false));

                var item = source.Index().GetEnumerator();
                IDisposable disposable = item; // disables AccessToDisposedClosure warnings
                try
                {
                    Task.Factory.StartNew(async () =>
                        {
                            var cancellationToken = cancellationTokenSource.Token;
                            var cancellationTaskSource = new TaskCompletionSource<bool>();
                            cancellationToken.Register(() => cancellationTaskSource.TrySetResult(true));

                            var tasks = new List<Task<KeyValuePair<int, TResult>>>();

                            var more = false;
                            for (var i = 0; i < maxConcurrency && (more = item.MoveNext()); i++)
                                tasks.Add(Select(item.Current, cancellationToken));

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

                                    tasks.Remove((Task<KeyValuePair<int, TResult>>)task);
                                    queue.Add(task);

                                    if (more && (more = item.MoveNext()))
                                        tasks.Add(Select(item.Current, cancellationToken));
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
                        scheduler);

                    var nextKey = 0;
                    var holds = ordered ? new List<KeyValuePair<int, TResult>>() : null;

                    foreach (var e in queue.GetConsumingEnumerable())
                    {
                        (e as ExceptionDispatchInfo)?.Throw();
                        if (e == null)
                            break;

                        var r = ((Task<KeyValuePair<int, TResult>>) e).Result;

                        if (holds == null || r.Key == nextKey)
                        {
                            // If order does not need to be preserved or the key
                            // is the next that should be yielded then yield
                            // the result.

                            yield return r.Value;

                            if (holds != null) // preserve order?
                            {
                                // Release withheld results consecutive in key
                                // order to the one just yielded...

                                var releaseCount = 0;

                                for (nextKey++;
                                     holds.Count > 0 && holds[0] is KeyValuePair<int, TResult> n
                                                     && n.Key == nextKey;
                                     nextKey++)
                                {
                                    releaseCount++;
                                    yield return n.Value;
                                }

                                holds.RemoveRange(0, releaseCount);
                            }
                        }
                        else
                        {
                            // Received a result out of order when order must be
                            // preserved, so withhold the result by finding out
                            // where it belongs in the order of results withheld
                            // so far and insert it in the list.

                            var i = holds.BinarySearch(r, KeyValueComparer<int, TResult>.Default);
                            Debug.Assert(i < 0);
                            holds.Insert(~i, r);
                        }
                    }

                    if (holds?.Count > 0) // yield any withheld, which should be in order...
                    {
                        foreach (var hold in holds)
                        {
                            Debug.Assert(nextKey++ == hold.Key); //...assert so!
                            yield return hold.Value;
                        }
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

        static class SelectAsyncEnumerable
        {
            public static ISelectAsyncEnumerable<T>
                Create<T>(
                    Func<SelectAsyncOptions, IEnumerable<T>> impl,
                    SelectAsyncOptions options = null) =>
                new SelectAsyncEnumerable<T>(impl, options);

        }

        sealed class SelectAsyncEnumerable<T> : ISelectAsyncEnumerable<T>
        {
            readonly Func<SelectAsyncOptions, IEnumerable<T>> _impl;

            public SelectAsyncEnumerable(Func<SelectAsyncOptions, IEnumerable<T>> impl,
                SelectAsyncOptions options = null)
            {
                _impl = impl;
                Options = options ?? SelectAsyncOptions.Default;
            }

            public SelectAsyncOptions Options { get; }

            public ISelectAsyncEnumerable<T> WithOptions(SelectAsyncOptions options)
            {
                if (options == null) throw new ArgumentNullException(nameof(options));
                return Options == options ? this : new SelectAsyncEnumerable<T>(_impl, options);
            }

            public IEnumerator<T> GetEnumerator() => _impl(Options).GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        sealed class DelegatingComparer<T> : IComparer<T>
        {
            readonly Func<T, T, int> _comparer;
            public DelegatingComparer(Func<T, T, int> comparer) => _comparer = comparer;
            public int Compare(T x, T y) => _comparer(x, y);
        }

        static class KeyValueComparer<TKey, TValue>
        {
            public static readonly IComparer<KeyValuePair<TKey, TValue>> Default =
                new DelegatingComparer<KeyValuePair<TKey, TValue>>((x, y) => Comparer<TKey>.Default.Compare(x.Key, y.Key));
        }
    }
}

#endif // !NO_ASYNC
