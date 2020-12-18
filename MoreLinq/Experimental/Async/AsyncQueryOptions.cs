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

    /// <summary>
    /// Represents options for asynchronous queries that may have concurrent
    /// execution semantics.
    /// </summary>

    public sealed class AsyncQueryOptions
    {
        /// <summary>
        /// The default options used for a query whose results evaluate
        /// asynchronously.
        /// </summary>

        public static readonly AsyncQueryOptions Default =
            new AsyncQueryOptions(null /* = unbounded concurrency */);

        /// <summary>
        /// Gets a positive (non-zero) integer that specifies the maximum
        /// number of asynchronous operations to have in-flight concurrently
        /// or <c>null</c> to mean unlimited concurrency.
        /// </summary>

        public int? MaxConcurrency { get; }

        AsyncQueryOptions(int? maxConcurrency)
        {
            MaxConcurrency = maxConcurrency == null || maxConcurrency > 0
                ? maxConcurrency
                : throw new ArgumentOutOfRangeException(
                    nameof(maxConcurrency), maxConcurrency,
                    "Maximum concurrency must be 1 or greater.");
        }

        /// <summary>
        /// Returns new options with the given concurrency limit.
        /// </summary>
        /// <param name="value">
        /// The maximum concurrent asynchronous operation to keep in flight.
        /// Use <c>null</c> to mean unbounded concurrency.</param>
        /// <returns>Options with the new setting.</returns>

        public AsyncQueryOptions WithMaxConcurrency(int? value) =>
            value == MaxConcurrency ? this : new AsyncQueryOptions(value);
    }
}

#endif // !NO_ASYNC_STREAMS
