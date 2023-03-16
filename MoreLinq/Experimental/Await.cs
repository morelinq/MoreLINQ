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

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
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

        public static readonly AwaitQueryOptions Default = new(null /* = unbounded concurrency */,
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
            MaxConcurrency = maxConcurrency is null or > 0
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

        public static IEnumerable<T> AsSequential<T>(this IAwaitQuery<T> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return MaxConcurrency(source, 1);
        }

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

        public static IAwaitQuery<T> MaxConcurrency<T>(this IAwaitQuery<T> source, int value)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return source.WithOptions(source.Options.WithMaxConcurrency(value));
        }

        /// <summary>
        /// Returns a query whose results evaluate asynchronously and
        /// concurrently with no defined limitation on concurrency.
        /// </summary>
        /// <typeparam name="T">The type of the source elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>
        /// A query whose results evaluate asynchronously using no defined
        /// limitation on concurrency.</returns>

        public static IAwaitQuery<T> UnboundedConcurrency<T>(this IAwaitQuery<T> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return source.WithOptions(source.Options.WithMaxConcurrency(null));
        }

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

        public static IAwaitQuery<T> PreserveOrder<T>(this IAwaitQuery<T> source, bool value)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return source.WithOptions(source.Options.WithPreserveOrder(value));
        }

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

        public static IAwaitQuery<T> Await<T>(this IEnumerable<Task<T>> source) =>
            source.Await((e, _) => e);

        /// <summary>
        /// Creates a sequence query that streams the result of each task in
        /// the source sequence as it completes asynchronously. A
        /// <see cref="CancellationToken"/> is passed for each asynchronous
        /// evaluation to abort any asynchronous operations in flight if the
        /// sequence is not fully iterated.
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
            this IEnumerable<T> source, Func<T, CancellationToken, Task<TResult>> evaluator) =>
            AwaitQuery.Create(options =>
                from t in source.AwaitCompletion(evaluator, (_, t) => t)
                                .WithOptions(options)
                select t.GetAwaiter().GetResult());

        /*
        /// <summary>
        /// Awaits completion of all asynchronous evaluations.
        /// </summary>

        public static IAwaitQuery<TResult> AwaitCompletion<T, TT, TResult>(
            this IEnumerable<T> source,
            Func<T, CancellationToken, Task<TT>> evaluator,
            Func<T, TT, TResult> resultSelector,
            Func<T, Exception, TResult> errorSelector,
            Func<T, TResult> cancellationSelector) =>
            AwaitQuery.Create(options =>
                from e in source.AwaitCompletion(evaluator, (item, task) => (Item: item, Task: task))
                                .WithOptions(options)
                select e.Task.IsFaulted
                     ? errorSelector(e.Item, e.Task.Exception)
                     : e.Task.IsCanceled
                     ? cancellationSelector(e.Item)
                     : resultSelector(e.Item, e.Task.Result));
        */

        /// <summary>
        /// Awaits completion of all asynchronous evaluations irrespective of
        /// whether they succeed or fail. An additional argument specifies a
        /// function that projects the final result given the source item and
        /// completed task.
        /// </summary>
        /// <typeparam name="T">The type of the source elements.</typeparam>
        /// <typeparam name="TTaskResult"> The type of the task's result.</typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="evaluator">A function to begin the asynchronous
        /// evaluation of each element, the second parameter of which is a
        /// <see cref="CancellationToken"/> that can be used to abort
        /// asynchronous operations.</param>
        /// <param name="resultSelector">A function that projects the final
        /// result given the source item and its asynchronous completion
        /// result.</param>
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

        public static IAwaitQuery<TResult> AwaitCompletion<T, TTaskResult, TResult>(
            this IEnumerable<T> source,
            Func<T, CancellationToken, Task<TTaskResult>> evaluator,
            Func<T, Task<TTaskResult>, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (evaluator == null) throw new ArgumentNullException(nameof(evaluator));

            return
                AwaitQuery.Create(
                    options => Impl(options.MaxConcurrency,
                                    options.Scheduler ?? TaskScheduler.Default,
                                    options.PreserveOrder));

            IEnumerable<TResult> Impl(int? maxConcurrency, TaskScheduler scheduler, bool ordered)
            {
                // A separate task will enumerate the source and launch tasks.
                // It will post all progress as notices to the collection below.
                // A notice is essentially a discriminated union like:
                //
                // type Notice<'a, 'b> =
                // | End
                // | Result of (int * 'a * Task<'b>)
                // | Error of ExceptionDispatchInfo
                //
                // Note that BlockingCollection.CompleteAdding is never used to
                // to mark the end (which its own notice above) because
                // BlockingCollection.Add throws if called after CompleteAdding
                // and we want to deliberately tolerate the race condition.

                var notices = new BlockingCollection<(Notice, (int, T, Task<TTaskResult>), ExceptionDispatchInfo?)>();

                var consumerCancellationTokenSource = new CancellationTokenSource();
                (Exception?, Exception?) lastCriticalErrors = default;

                var completed = false;
                var cancellationTokenSource = new CancellationTokenSource();

                var enumerator = source.Index().GetEnumerator();
                IDisposable disposable = enumerator; // disables AccessToDisposedClosure warnings

                try
                {
                    var cancellationToken = cancellationTokenSource.Token;

                    // Fire-up a parallel loop to iterate through the source and
                    // launch tasks, posting a result-notice as each task
                    // completes and another, an end-notice, when all tasks have
                    // completed.

                    _ = Task.Factory.StartNew(
                        async () =>
                        {
                            try
                            {
                                await enumerator.StartAsync(e => evaluator(e.Value, cancellationToken),
                                                            (e, r) => PostNotice(Notice.Result, (e.Key, e.Value, r), default),
                                                            () => PostNotice(Notice.End, default, default),
                                                            maxConcurrency, cancellationToken)
                                                .ConfigureAwait(false);
                            }
#pragma warning disable CA1031 // Do not catch general exception types
                            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                            {
                                PostNotice(Notice.Error, default, e);
                            }

                            void PostNotice(Notice notice,
                                            (int, T, Task<TTaskResult>) item,
                                            Exception? error)
                            {
                                // If a notice fails to post then assume critical error
                                // conditions (like low memory), capture the error without
                                // further allocation of resources and trip the cancellation
                                // token source used by the main loop waiting on notices.
                                // Note that only the "last" critical error is reported
                                // as maintaining a list would incur allocations. The idea
                                // here is to make a best effort attempt to report any of
                                // the error conditions that may be occurring, which is still
                                // better than nothing.

                                try
                                {
                                    var edi = error != null
                                            ? ExceptionDispatchInfo.Capture(error)
                                            : null;
                                    notices.Add((notice, item, edi));
                                }
                                catch (Exception e)
                                {
                                    // Don't use ExceptionDispatchInfo.Capture here to avoid
                                    // inducing allocations if already under low memory
                                    // conditions.

                                    lastCriticalErrors = (e, error);
                                    consumerCancellationTokenSource.Cancel();
                                    throw;
                                }
                            }
                        },
                        CancellationToken.None,
                        TaskCreationOptions.DenyChildAttach,
                        scheduler);

                    // Remainder here is the main loop that waits for and
                    // processes notices.

                    var nextKey = 0;
                    var holds = ordered ? new List<(int, T, Task<TTaskResult>)>() : null;

#pragma warning disable IDE0011 // Add braces
                    using (var notice = notices.GetConsumingEnumerable(consumerCancellationTokenSource.Token)
                                               .GetEnumerator())
#pragma warning restore IDE0011 // Add braces
                    while (true)
                    {
                        try
                        {
                            if (!notice.MoveNext())
                                break;
                        }
                        catch (OperationCanceledException e) when (e.CancellationToken == consumerCancellationTokenSource.Token)
                        {
                            var (error1, error2) = lastCriticalErrors;
#pragma warning disable CA2201 // Do not raise reserved exception types
                            throw new Exception("One or more critical errors have occurred.",
                                error2 != null ? new AggregateException(Assume.NotNull(error1), error2)
                                               : new AggregateException(Assume.NotNull(error1)));
#pragma warning restore CA2201 // Do not raise reserved exception types
                        }

                        var (kind, result, error) = notice.Current;

                        if (kind == Notice.Error)
                            Assume.NotNull(error).Throw();

                        if (kind == Notice.End)
                            break;

                        Debug.Assert(kind == Notice.Result);

                        var (key, inp, value) = result;
                        if (holds == null || key == nextKey)
                        {
                            // If order does not need to be preserved or the key
                            // is the next that should be yielded then yield
                            // the result.

                            yield return resultSelector(inp, value);

                            if (holds != null) // preserve order?
                            {
                                // Release withheld results consecutive in key
                                // order to the one just yielded...

                                var releaseCount = 0;

                                for (nextKey++; holds.Count > 0; nextKey++)
                                {
                                    var (candidateKey, ic, candidate) = holds[0];
                                    if (candidateKey != nextKey)
                                        break;

                                    releaseCount++;
                                    yield return resultSelector(ic, candidate);
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

                            var i = holds.BinarySearch(result, TupleComparer<int, T, Task<TTaskResult>>.Item1);
                            Debug.Assert(i < 0);
                            holds.Insert(~i, result);
                        }
                    }

                    if (holds?.Count > 0) // yield any withheld, which should be in order...
                    {
                        foreach (var (key, x, value) in holds)
                        {
                            Debug.Assert(nextKey++ == key); //...assert so!
                            yield return resultSelector(x, value);
                        }
                    }

                    completed = true;
                }
                finally
                {
                    // The cancellation token is signaled here for the case where
                    // tasks may be in flight but the user stopped the enumeration
                    // partway (e.g. Await was combined with a Take or TakeWhile).
                    // The in-flight tasks need to be aborted as well as the
                    // awaiter loop.

                    if (!completed)
                        cancellationTokenSource.Cancel();
                    disposable.Dispose();
                }
            }
        }

        enum Notice { End, Result, Error }

        static async Task StartAsync<T, TResult>(
            this IEnumerator<T> enumerator,
            Func<T, Task<TResult>> starter,
            Action<T, Task<TResult>> onTaskCompletion,
            Action onEnd,
            int? maxConcurrency,
            CancellationToken cancellationToken)
        {
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));
            if (starter == null) throw new ArgumentNullException(nameof(starter));
            if (onTaskCompletion == null) throw new ArgumentNullException(nameof(onTaskCompletion));
            if (onEnd == null) throw new ArgumentNullException(nameof(onEnd));
            if (maxConcurrency < 1) throw new ArgumentOutOfRangeException(nameof(maxConcurrency));

            using (enumerator)
            {
                var pendingCount = 1; // terminator

                void OnPendingCompleted()
                {
                    if (Interlocked.Decrement(ref pendingCount) == 0)
                        onEnd();
                }

                var concurrencyGate = maxConcurrency is { } count
                                    ? new ConcurrencyGate(count)
                                    : ConcurrencyGate.Unbounded;

                while (enumerator.MoveNext())
                {
                    try
                    {
                        await concurrencyGate.EnterAsync(cancellationToken)
                                             .ConfigureAwait(false);
                    }
                    catch (OperationCanceledException e) when (e.CancellationToken == cancellationToken)
                    {
                        return;
                    }

                    _ = Interlocked.Increment(ref pendingCount);

                    var item = enumerator.Current;
                    var task = starter(item);

                    // Add a continuation that notifies completion of the task,
                    // along with the necessary housekeeping, in case it
                    // completes before maximum concurrency is reached.

                    _ = task.ContinueWith(cancellationToken: cancellationToken,
                        continuationOptions: TaskContinuationOptions.ExecuteSynchronously,
                        scheduler: TaskScheduler.Current,
                        continuationAction: t =>
                        {
                            concurrencyGate.Exit();

                            if (cancellationToken.IsCancellationRequested)
                                return;

                            onTaskCompletion(item, t);
                            OnPendingCompleted();
                        });
                }

                OnPendingCompleted();
            }
        }

        static class AwaitQuery
        {
            public static IAwaitQuery<T>
                Create<T>(
                    Func<AwaitQueryOptions, IEnumerable<T>> impl,
                    AwaitQueryOptions? options = null) =>
                new AwaitQuery<T>(impl, options);
        }

        sealed class AwaitQuery<T> : IAwaitQuery<T>
        {
            readonly Func<AwaitQueryOptions, IEnumerable<T>> _impl;

            public AwaitQuery(Func<AwaitQueryOptions, IEnumerable<T>> impl,
                AwaitQueryOptions? options = null)
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

        static class TupleComparer<T1, T2, T3>
        {
            public static readonly IComparer<(T1, T2, T3)> Item1 =
                Comparer<(T1, T2, T3)>.Create((x, y) => Comparer<T1>.Default.Compare(x.Item1, y.Item1));

            public static readonly IComparer<(T1, T2, T3)> Item2 =
                Comparer<(T1, T2, T3)>.Create((x, y) => Comparer<T2>.Default.Compare(x.Item2, y.Item2));

            public static readonly IComparer<(T1, T2, T3)> Item3 =
                Comparer<(T1, T2, T3)>.Create((x, y) => Comparer<T3>.Default.Compare(x.Item3, y.Item3));
        }

        static class CompletedTask
        {
#if NETSTANDARD1_0

            public static readonly Task Instance = CreateCompletedTask();

            static Task CreateCompletedTask()
            {
                var tcs = new TaskCompletionSource<System.ValueTuple>();
                tcs.SetResult(default);
                return tcs.Task;
            }

#else

            public static readonly Task Instance = Task.CompletedTask;

#endif
        }

        sealed class ConcurrencyGate
        {
            public static readonly ConcurrencyGate Unbounded = new();

            readonly SemaphoreSlim? _semaphore;

            ConcurrencyGate(SemaphoreSlim? semaphore = null) =>
                _semaphore = semaphore;

            public ConcurrencyGate(int max) :
                this(new SemaphoreSlim(max, max)) { }

            public Task EnterAsync(CancellationToken token)
            {
                if (_semaphore == null)
                {
                    token.ThrowIfCancellationRequested();
                    return CompletedTask.Instance;
                }

                return _semaphore.WaitAsync(token);
            }

            public void Exit() =>
                _semaphore?.Release();
        }
    }
}

