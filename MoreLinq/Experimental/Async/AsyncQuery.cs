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
    using System.Threading;

    /// <summary>
    /// Represents an asynchronous stream that may internally have concurrent
    /// yielding semantics.
    /// </summary>
    /// <inheritdoc />
    /// <typeparam name="T">The type of the source elements.</typeparam>

    public interface IAsyncQuery<out T> : IAsyncEnumerable<T>
    {
        /// <summary>
        /// The options that determine how the sequence evaluation behaves when
        /// it is iterated.
        /// </summary>

        AsyncQueryOptions Options { get; }

        /// <summary>
        /// Returns a new query that will use the given options.
        /// </summary>
        /// <param name="options">The new options to use.</param>
        /// <returns>
        /// Returns a new query using the supplied options.
        /// </returns>

        IAsyncQuery<T> WithOptions(AsyncQueryOptions options);
    }

    static class AsyncQuery
    {
        public static IAsyncQuery<T>
            Create<T>(Func<AsyncQueryOptions, IAsyncEnumerable<T>> impl,
                      AsyncQueryOptions? options = null) =>
            new DelegatingAsyncQuery<T>(impl, options);

        sealed class DelegatingAsyncQuery<T> : IAsyncQuery<T>
        {
            readonly Func<AsyncQueryOptions, IAsyncEnumerable<T>> _impl;

            public DelegatingAsyncQuery(Func<AsyncQueryOptions, IAsyncEnumerable<T>> impl,
                                        AsyncQueryOptions? options = null)
            {
                _impl = impl;
                Options = options ?? AsyncQueryOptions.Default;
            }

            public AsyncQueryOptions Options { get; }

            public IAsyncQuery<T> WithOptions(AsyncQueryOptions options)
            {
                if (options == null) throw new ArgumentNullException(nameof(options));
                return Options == options ? this : new DelegatingAsyncQuery<T>(_impl, options);
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken) =>
                _impl(Options).GetAsyncEnumerator(cancellationToken);
        }
    }
}

#endif // !NO_ASYNC_STREAMS
