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
    /// Represents options for a query whose results evaluate asynchronously.
    /// </summary>

    public sealed class AwaitQueryOptions
    {
        /// <summary>
        /// The default options used for a query whose results evaluate
        /// asynchronously.
        /// </summary>

        public static readonly AwaitQueryOptions Default =
            new AwaitQueryOptions(null /* = unbounded concurrency */,
                                  TaskScheduler.Default,
                                  preserveOrder: false);

        /// <summary>
        /// Gets a positive (non-zero) integer that specifies the maximum
        /// number of asynchronous operations to have in-flight concurrently
        /// or <c>null</c> to mean unlimited concurrency.
        /// </summary>

        public int? MaxConcurrency { get; }

        /// <summary>
        /// Get the scheduler to be used for any workhorse task.
        /// </summary>

        public TaskScheduler Scheduler { get; }

        /// <summary>
        /// Get a Boolean that determines whether results should be ordered
        /// the same as the source.
        /// </summary>

        public bool PreserveOrder { get; }

        AwaitQueryOptions(int? maxConcurrency, TaskScheduler scheduler, bool preserveOrder)
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
        /// <param name="value">
        /// The maximum concurrent asynchronous operation to keep in flight.
        /// Use <c>null</c> to mean unbounded concurrency.</param>
        /// <returns>Options with the new setting.</returns>

        public AwaitQueryOptions WithMaxConcurrency(int? value) =>
            value == MaxConcurrency ? this : new AwaitQueryOptions(value, Scheduler, PreserveOrder);

        /// <summary>
        /// Returns new options with the given scheduler.
        /// </summary>
        /// <param name="value">
        /// The scheduler to use to for the workhorse task.</param>
        /// <returns>Options with the new setting.</returns>

        public AwaitQueryOptions WithScheduler(TaskScheduler value) =>
            value == Scheduler ? this : new AwaitQueryOptions(MaxConcurrency, value, PreserveOrder);

        /// <summary>
        /// Returns new options with the given Boolean indicating whether or
        /// not the results should be returned in the order of the source.
        /// </summary>
        /// <param name="value">
        /// A Boolean where <c>true</c> means results are in source order and
        /// <c>false</c> means that results can be delivered in order of
        /// efficiency.</param>
        /// <returns>Options with the new setting.</returns>

        public AwaitQueryOptions WithPreserveOrder(bool value) =>
            value == PreserveOrder ? this : new AwaitQueryOptions(MaxConcurrency, Scheduler, value);
    }

    /// <summary>
    /// Represents a sequence whose elements or results evaluate asynchronously.
    /// </summary>
    /// <inheritdoc />
    /// <typeparam name="T">The type of the source elements.</typeparam>

    public interface IAwaitQuery<out T> : IEnumerable<T>
    {
        /// <summary>
        /// The options that determine how the sequence evaluation behaves when
        /// it is iterated.
        /// </summary>

        AwaitQueryOptions Options { get; }

        /// <summary>
        /// Returns a new query that will use the given options.
        /// </summary>
        /// <param name="options">The new options to use.</param>
        /// <returns>
        /// Returns a new query using the supplied options.
        /// </returns>

        IAwaitQuery<T> WithOptions(AwaitQueryOptions options);
    }

    static partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Converts a query whose results evaluate asynchronously to use
        /// sequential instead of concurrent evaluation.
        /// </summary>
        /// <typeparam name="T">The type of the source elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>The converted sequence.</returns>

        public static IEnumerable<T> AsSequential<T>(this IAwaitQuery<T> source) =>
            source.MaxConcurrency(1);

        /// <summary>
        /// Returns a query whose results evaluate asynchronously to use a
        /// concurrency limit.
        /// </summary>
        /// <typeparam name="T">The type of the source elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="value"></param>
        /// <returns>
        /// A query whose results evaluate asynchronously using the given
        /// concurrency limit.</returns>

        public static IAwaitQuery<T> MaxConcurrency<T>(this IAwaitQuery<T> source, int value) =>
            source.WithOptions(source.Options.WithMaxConcurrency(value));

        /// <summary>
        /// Returns a query whose results evaluate asynchronously and
        /// concurrently with no defined limitation on concurrency.
        /// </summary>
        /// <typeparam name="T">The type of the source elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>
        /// A query whose results evaluate asynchronously using no defined
        /// limitation on concurrency.</returns>

        public static IAwaitQuery<T> UnboundedConcurrency<T>(this IAwaitQuery<T> source) =>
            source.WithOptions(source.Options.WithMaxConcurrency(null));

        /// <summary>
        /// Returns a query whose results evaluate asynchronously and uses the
        /// given scheduler for the workhorse task.
        /// </summary>
        /// <typeparam name="T">The type of the source elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="value">The scheduler to use.</param>
        /// <returns>
        /// A query whose results evaluate asynchronously and uses the
        /// given scheduler for the workhorse task.</returns>

        public static IAwaitQuery<T> Scheduler<T>(this IAwaitQuery<T> source, TaskScheduler value)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (value == null) throw new ArgumentNullException(nameof(value));
            return source.WithOptions(source.Options.WithScheduler(value));
        }

        /// <summary>
        /// Returns a query whose results evaluate asynchronously but which
        /// are returned in the order of the source.
        /// </summary>
        /// <typeparam name="T">The type of the source elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>
        /// A query whose results evaluate asynchronously but which
        /// are returned in the order of the source.</returns>
        /// <remarks>
        /// Internally, the asynchronous operations will be done concurrently
        /// but the results will be yielded in order.
        /// </remarks>

        public static IAwaitQuery<T> AsOrdered<T>(this IAwaitQuery<T> source) =>
            PreserveOrder(source, true);

        /// <summary>
        /// Returns a query whose results evaluate asynchronously but which
        /// are returned without guarantee of the source order.
        /// </summary>
        /// <typeparam name="T">The type of the source elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>
        /// A query whose results evaluate asynchronously but which
        /// are returned without guarantee of the source order.</returns>

        public static IAwaitQuery<T> AsUnordered<T>(this IAwaitQuery<T> source) =>
            PreserveOrder(source, false);

        /// <summary>
        /// Returns a query whose results evaluate asynchronously and a Boolean
        /// argument indicating whether the source order of the results is
        /// preserved.
        /// </summary>
        /// <typeparam name="T">The type of the source elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="value">
        /// A Boolean where <c>true</c> means results are in source order and
        /// <c>false</c> means that results can be delivered in order of
        /// efficiency.</param>
        /// <returns>
        /// A query whose results evaluate asynchronously and returns the
        /// results ordered or unordered based on <paramref name="value"/>.
        /// </returns>

        public static IAwaitQuery<T> PreserveOrder<T>(this IAwaitQuery<T> source, bool value) =>
            source.WithOptions(source.Options.WithPreserveOrder(value));

        /// <summary>
        /// Creates a sequence query that streams the result of each task in
        /// the source sequence as it completes asynchronously.
        /// </summary>
        /// <typeparam name="T">
        /// The type of each task's result as well as the type of the elements
        /// of the resulting sequence.</typeparam>
        /// <param name="source">The source sequence of tasks.</param>
        /// <returns>
        /// A sequence query that streams the result of each task in
        /// <paramref name="source"/> as it completes asynchronously.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method uses deferred execution semantics. The results are
        /// yielded as each asynchronous task completes and, by default,
        /// not guaranteed to be based on the source sequence order. If order
        /// is important, compose further with
        /// <see cref="AsOrdered{T}"/>.</para>
        /// <para>
        /// This method starts a new task where the tasks are awaited. If the
        /// resulting sequence is partially consumed then there's a good chance
        /// that some tasks will be wasted, those that are in flight.</para>
        /// <para>
        /// The tasks in <paramref name="source"/> are already assumed to be in
        /// flight therefore changing concurrency options via
        /// <see cref="AsSequential{T}"/>, <see cref="MaxConcurrency{T}"/> or
        /// <see cref="UnboundedConcurrency{T}"/> will only change how many
        /// tasks are awaited at any given moment, not how many will be
        /// kept in flight. For the latter effect, use the other overload.
        /// </para>
        /// </remarks>

        public static IAwaitQuery<T> Await<T>(
            this IEnumerable<Task<T>> source)
        {
            return source.Await((e, _) => e);
        }

        /// <summary>
        /// Creates a sequence query that streams the result of each task in
        /// the source sequence as it completes asynchronously. A
        /// <see cref="CancellationToken"/> is passed for each asynchronous
        /// evaluation to abort any asynchronous operations in flight if the
        /// sequence is not full iterated.
        /// </summary>
        /// <typeparam name="T">The type of the source elements.</typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="evaluator">A function to begin the asynchronous
        /// evaluation of each element, the second parameter of which is a
        /// <see cref="CancellationToken"/> that can be used to abort
        /// asynchronous operations.</param>
        /// <returns>
        /// A sequence query that stream its results as they are
        /// evaluated asynchronously.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method uses deferred execution semantics. The results are
        /// yielded as each asynchronous evaluation completes and, by default,
        /// not guaranteed to be based on the source sequence order. If order
        /// is important, compose further with
        /// <see cref="AsOrdered{T}"/>.</para>
        /// <para>
        /// This method starts a new task where the asynchronous evaluations
        /// take place and awaited. If the resulting sequence is partially
        /// consumed then there's a good chance that some projection work will
        /// be wasted and a cooperative effort is done that depends on the
        /// <paramref name="evaluator"/> function (via a
        /// <see cref="CancellationToken"/> as its second argument) to cancel
        /// those in flight.</para>
        /// <para>
        /// The <paramref name="evaluator"/> function should be designed to be
        /// thread-agnostic.</para>
        /// <para>
        /// The task returned by <paramref name="evaluator"/> should be started
        /// when the function is called (and not just a mere projection)
        /// otherwise changing concurrency options via
        /// <see cref="AsSequential{T}"/>, <see cref="MaxConcurrency{T}"/> or
        /// <see cref="UnboundedConcurrency{T}"/> will only change how many
        /// tasks are awaited at any given moment, not how many will be
        /// kept in flight.
        /// </para>
        /// </remarks>

        public static IAwaitQuery<TResult> Await<T, TResult>(
            this IEnumerable<T> source, Func<T, CancellationToken, Task<TResult>> evaluator)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (evaluator == null) throw new ArgumentNullException(nameof(evaluator));

            return
                AwaitQuery.Create(
                    options => _(options.MaxConcurrency ?? int.MaxValue,
                                 options.Scheduler ?? TaskScheduler.Default,
                                 options.PreserveOrder));

            IEnumerable<TResult> _(int maxConcurrency, TaskScheduler scheduler, bool ordered)
            {
                var notices = new BlockingCollection<(Notice, (int, TResult), ExceptionDispatchInfo)>();
                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;
                var completed = false;

                var enumerator =
                    source.Index()
                          .Select(e => (e.Key, Task: evaluator(e.Value, cancellationToken)))
                          .GetEnumerator();

                IDisposable disposable = enumerator; // disables AccessToDisposedClosure warnings

                try
                {
                    Task.Factory.StartNew(
                        () =>
                            CollectToAsync(
                                enumerator,
                                e => e.Task,
                                notices,
                                (e, r) => (Notice.Result, (e.Key, r), default),
                                ex => (Notice.Error, default, ExceptionDispatchInfo.Capture(ex)),
                                (Notice.End, default, default),
                                maxConcurrency, cancellationTokenSource),
                        CancellationToken.None,
                        TaskCreationOptions.DenyChildAttach,
                        scheduler);

                    var nextKey = 0;
                    var holds = ordered ? new List<(int, TResult)>() : null;

                    foreach (var (kind, result, error) in notices.GetConsumingEnumerable())
                    {
                        if (kind == Notice.Error)
                            error.Throw();

                        if (kind == Notice.End)
                            break;

                        Debug.Assert(kind == Notice.Result);

                        var (key, value) = result;
                        if (holds == null || key == nextKey)
                        {
                            // If order does not need to be preserved or the key
                            // is the next that should be yielded then yield
                            // the result.

                            yield return value;

                            if (holds != null) // preserve order?
                            {
                                // Release withheld results consecutive in key
                                // order to the one just yielded...

                                var releaseCount = 0;

                                for (nextKey++; holds.Count > 0; nextKey++)
                                {
                                    var (candidateKey, candidate) = holds[0];
                                    if (candidateKey == nextKey)
                                    {
                                        releaseCount++;
                                        yield return candidate;
                                    }
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

                            var i = holds.BinarySearch(result, TupleComparer<int, TResult>.Item1);
                            Debug.Assert(i < 0);
                            holds.Insert(~i, result);
                        }
                    }

                    if (holds?.Count > 0) // yield any withheld, which should be in order...
                    {
                        foreach (var (key, value) in holds)
                        {
                            Debug.Assert(nextKey++ == key); //...assert so!
                            yield return value;
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

        enum Notice { Result, Error, End }

        static async Task<TResult> Select<T, TResult>(this Task<T> task, Func<T, TResult> selector) =>
            selector(await task.ConfigureAwait(continueOnCapturedContext: false));

        static async Task CollectToAsync<T, TResult, TNotice>(
            this IEnumerator<T> e,
            Func<T, Task<TResult>> taskSelector,
            BlockingCollection<TNotice> collection,
            Func<T, TResult, TNotice> resultNoticeSelector,
            Func<Exception, TNotice> errorNoticeSelector,
            TNotice endNotice,
            int maxConcurrency,
            CancellationTokenSource cancellationTokenSource)
        {
            using (e)
            {
                var cancellationToken = cancellationTokenSource.Token;
                var cancellationTaskSource = new TaskCompletionSource<bool>();
                cancellationToken.Register(() => cancellationTaskSource.TrySetResult(true));

                var tasks = new List<Task<(T, TResult)>>();

                var more = false;
                for (var i = 0; i < maxConcurrency && (more = e.MoveNext()); i++)
                    tasks.Add(taskSelector(e.Current).Select(r => (e.Current, r)));

                if (!more)
                    e.Dispose();

                try
                {
                    while (tasks.Count > 0)
                    {
                        // Task.WaitAny is synchronous and blocking but allows the
                        // waiting to be cancelled via a CancellationToken.
                        // Task.WhenAny can be awaited so it is better since the
                        // thread won't be blocked and can return to the pool.
                        // However, it doesn't support cancellation so instead a
                        // task is built on top of the CancellationToken that
                        // completes when the CancellationToken trips.

                        var completedTask = await
                            Task.WhenAny(tasks.Cast<Task>().Concat(cancellationTaskSource.Task))
                                .ConfigureAwait(continueOnCapturedContext: false);

                        if (completedTask == cancellationTaskSource.Task)
                        {
                            // Cancellation during the wait means the enumeration
                            // has been stopped by the user so the results of the
                            // remaining tasks are no longer needed. Those tasks
                            // should cancel as a result of sharing the same
                            // cancellation token and provided that they passed it
                            // on to any downstream asynchronous operations. Either
                            // way, this loop is done so exit hard here.

                            return;
                        }

                        var task = (Task<(T Input, TResult Result)>) completedTask;
                        tasks.Remove(task);
                        collection.Add(resultNoticeSelector(task.Result.Input, task.Result.Result));

                        if (more)
                        {
                            if (more = e.MoveNext())
                                tasks.Add(taskSelector(e.Current).Select(r => (e.Current, r)));
                            else
                                e.Dispose();
                        }
                    }

                    collection.Add(endNotice);
                }
                catch (Exception ex)
                {
                    cancellationTokenSource.Cancel();
                    collection.Add(errorNoticeSelector(ex));
                }

                collection.CompleteAdding();
            }
        }

        static class AwaitQuery
        {
            public static IAwaitQuery<T>
                Create<T>(
                    Func<AwaitQueryOptions, IEnumerable<T>> impl,
                    AwaitQueryOptions options = null) =>
                new AwaitQuery<T>(impl, options);
        }

        sealed class AwaitQuery<T> : IAwaitQuery<T>
        {
            readonly Func<AwaitQueryOptions, IEnumerable<T>> _impl;

            public AwaitQuery(Func<AwaitQueryOptions, IEnumerable<T>> impl,
                AwaitQueryOptions options = null)
            {
                _impl = impl;
                Options = options ?? AwaitQueryOptions.Default;
            }

            public AwaitQueryOptions Options { get; }

            public IAwaitQuery<T> WithOptions(AwaitQueryOptions options)
            {
                if (options == null) throw new ArgumentNullException(nameof(options));
                return Options == options ? this : new AwaitQuery<T>(_impl, options);
            }

            public IEnumerator<T> GetEnumerator() => _impl(Options).GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        static class TupleComparer<T1, T2>
        {
            public static readonly IComparer<(T1, T2)> Item1 =
                Comparer<(T1, T2)>.Create((x, y) => Comparer<T1>.Default.Compare(x.Item1, y.Item1));

            public static readonly IComparer<(T1, T2)> Item2 =
                Comparer<(T1, T2)>.Create((x, y) => Comparer<T2>.Default.Compare(x.Item2, y.Item2));
        }
    }
}

#endif // !NO_ASYNC
