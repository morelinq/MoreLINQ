#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All rights reserved.
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
    using NUnit.Framework;

    static class TestingList
    {
        public static TestingList<T> Of<T>(params T[] elements) =>
            new TestingList<T>(elements);

        public static TestingList<T> AsTestingList<T>(this IList<T> source) =>
            source != null
                ? new TestingList<T>(source)
                : throw new ArgumentNullException(nameof(source));

    }

    /// <summary>
    /// List that asserts whether its iterator has been disposed
    /// when it is disposed itself and also whether GetEnumerator() is
    /// called exactly once or not.
    /// </summary>

    sealed class TestingList<T> : IList<T>, IDisposable
    {
        readonly IList<T> _list;
        bool? _disposed;

        public TestingList(IList<T> list) => _list = list;

        void IDisposable.Dispose()
        {
            if (_disposed == null)
                return;
            Assert.That(_disposed, Is.True, "Expected enumerator to be disposed.");
            _disposed = null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Assert.That(_disposed, Is.Null, "LINQ operators should not enumerate a sequence more than once.");
            _disposed = false;
            var e = new DisposeNotificationEnumerator<T>(_list.GetEnumerator());
            e.Disposed += delegate { _disposed = true; };
            return e;
        }

        public int Count => _list.Count;
        public T this[int index] { get => _list[index]; set => _list[index] = value; }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Contains(T item) => _list.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
        public bool IsReadOnly => _list.IsReadOnly;
        public int IndexOf(T item) => _list.IndexOf(item);

        public void Add(T item)               => throw UnexpectedBehaviorError();
        public void Clear()                   => throw UnexpectedBehaviorError();
        public bool Remove(T item)            => throw UnexpectedBehaviorError();
        public void Insert(int index, T item) => throw UnexpectedBehaviorError();
        public void RemoveAt(int index)       => throw UnexpectedBehaviorError();

        static Exception UnexpectedBehaviorError() => new Exception("LINQ operators should not modify the source.");
    }
}