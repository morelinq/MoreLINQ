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
            if (source == null) throw new ArgumentNullException("source");
            if (maxConcurrency <= 0) throw new ArgumentOutOfRangeException(nameof(maxConcurrency));
            if (selector == null) throw new ArgumentNullException("selector");

            var queue = new BlockingCollection<object>();
            using (var _ = source.GetEnumerator())
            {
                var item = _;

                Task.Factory.StartNew(async () =>
                    {
                        var tasks = new List<Task<TResult>>();

                        var more = false;
                        for (var i = 0; i < maxConcurrency && (more = item.MoveNext()); i++)
                            tasks.Add(selector(item.Current));

                        if (!more)
                            item.Dispose();

                        try
                        {
                            while (tasks.Count > 0)
                            {
                                var task = await Task.WhenAny(tasks);
                                tasks.Remove(task);
                                queue.Add(task);

                                if (more && (more = item.MoveNext()))
                                    tasks.Add(selector(item.Current));
                            }
                            queue.Add(null);
                        }
                        catch (Exception e)
                        {
                            queue.Add(ExceptionDispatchInfo.Capture(e));
                        }
                        queue.CompleteAdding();
                    },
                    CancellationToken.None,
                    TaskCreationOptions.DenyChildAttach,
                    scheduler ?? TaskScheduler.Default);

                // TODO Consider the impact of throwing partway while other tasks are in flight!

                foreach (var e in queue.GetConsumingEnumerable())
                {
                    (e as ExceptionDispatchInfo)?.Throw();
                    if (e == null)
                        yield break;
                    yield return ((Task<TResult>) e).Result;
                }
            }
        }
    }
}

#endif // !NO_ASYNC