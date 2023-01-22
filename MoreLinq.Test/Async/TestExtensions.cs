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

namespace MoreLinq.Test.Async
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    static partial class TestExtensions
    {
        public static IAsyncEnumerable<T> Yield<T>(this IAsyncEnumerable<T> source) =>
            Yield(source, _ => true);

        public static IAsyncEnumerable<T>
            Yield<T>(this IAsyncEnumerable<T> source,
                     Func<T, bool> predicate) =>
            Yield(source, shouldEndSynchronously: false, predicate);

        public static IAsyncEnumerable<T>
            Yield<T>(this IAsyncEnumerable<T> source,
                     bool shouldEndSynchronously,
                     Func<T, bool> predicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return Async();

            async IAsyncEnumerable<T> Async([EnumeratorCancellation]CancellationToken cancellationToken = default)
            {
                await foreach (var item in source.WithCancellation(cancellationToken))
                {
                    if (predicate(item))
                        await Task.Yield();
                    yield return item;
                }

                if (!shouldEndSynchronously)
                    await Task.Yield();
            }
        }
    }
}
