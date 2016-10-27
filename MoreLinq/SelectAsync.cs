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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Asynchronously pairs each element of a sequence with its projection.
        /// </summary>

        public static Task<ICollection<KeyValuePair<T, TAsync>>> SelectAsync<T, TAsync>(
            this IEnumerable<T> sources, Func<T, Task<TAsync>> taskSelector)
        {
            return SelectAsync(sources, taskSelector, (k, v) => new KeyValuePair<T, TAsync>(k, v));
        }

        /// <summary>
        /// Asynchronously projects each element of a sequence and then uses
        /// a function to create the resulting value from the two.
        /// </summary>

        public static async Task<ICollection<TResult>> SelectAsync<T, TAsync, TResult>(
            this IEnumerable<T> sources, Func<T, Task<TAsync>> taskSelector,
            Func<T, TAsync, TResult> resultSelector)
        {
            if (sources == null) throw new ArgumentNullException("sources");
            if (taskSelector == null) throw new ArgumentNullException("taskSelector");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            var results = await
                Task.WhenAll(sources.Select(async e => new KeyValuePair<T, TAsync>(e, await taskSelector(e).ConfigureAwait(continueOnCapturedContext: false))));
            return results.Select(e => resultSelector(e.Key, e.Value)).ToList();
        }
    }
}

#endif // !NO_ASYNC