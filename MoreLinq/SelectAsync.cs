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
            return SelectAsync(source, null, selector);
        }

        /// <summary>
        /// Asynchronously projects each element of a sequence to its new form.
        /// An additional parameter specifies the <see cref="TaskScheduler"/>
        /// to use to await for tasks to complete.
        /// </summary>

        public static IEnumerable<TResult> SelectAsync<T, TResult>(
            this IEnumerable<T> source,
            TaskScheduler scheduler,
            Func<T, Task<TResult>> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            var queue = new BlockingCollection<object>();
            var tasks = source.Select(selector).ToList(); // TODO max concurrency

            Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        while (tasks.Count > 0)
                        {
                            var task = await Task.WhenAny(tasks);
                            tasks.Remove(task);
                            queue.Add(task);
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

#endif // !NO_ASYNC