#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2021 Atif Aziz. All rights reserved.
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
    using System.Threading.Tasks;

    partial class TestExtensions
    {
        public static WatchableEnumerator<T> AsWatchable<T>(this IAsyncEnumerator<T> source) => new(source);
    }

    sealed class WatchableEnumerator<T> : IAsyncEnumerator<T>
    {
        readonly IAsyncEnumerator<T> _source;

        public event EventHandler? Disposed;
        public event EventHandler<bool>? MoveNextCalled;

        public WatchableEnumerator(IAsyncEnumerator<T> source) =>
            _source = source ?? throw new ArgumentNullException(nameof(source));

        public T Current => _source.Current;

        public async ValueTask<bool> MoveNextAsync()
        {
            var moved = await _source.MoveNextAsync();
            MoveNextCalled?.Invoke(this, moved);
            return moved;
        }

        public async ValueTask DisposeAsync()
        {
            await _source.DisposeAsync();
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
