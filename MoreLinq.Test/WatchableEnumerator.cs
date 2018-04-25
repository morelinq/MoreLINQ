#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
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

namespace MoreLinq.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    partial class TestExtensions
    {
        public static WatchableEnumerator<T> AsWatchtable<T>(this IEnumerator<T> source) =>
            new WatchableEnumerator<T>(source);
    }

    sealed class WatchableEnumerator<T> : IEnumerator<T>
    {
        readonly IEnumerator<T> _source;

        public event EventHandler Disposed;
        public event EventHandler MoveNextCalled;

        public WatchableEnumerator(IEnumerator<T> source) =>
            _source = source ?? throw new ArgumentNullException(nameof(source));

        public T Current => _source.Current;
        object IEnumerator.Current => Current;
        public void Reset() => _source.Reset();

        public bool MoveNext()
        {
            MoveNextCalled?.Invoke(this, EventArgs.Empty);
            return _source.MoveNext();
        }

        public void Dispose()
        {
            _source.Dispose();
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
