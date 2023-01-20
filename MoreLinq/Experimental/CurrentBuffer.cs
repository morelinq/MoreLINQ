#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2022 Atif Aziz. All rights reserved.
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

#if !NO_BUFFERS

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a current buffered view of a larger result and which
    /// is updated in-place (thus current) as it is moved through the overall
    /// result.
    /// </summary>
    /// <typeparam name="T">Type of elements in the list.</typeparam>

    public interface ICurrentBuffer<T> : IList<T> { }

    /// <summary>
    /// A provider of current buffer that updates it in-place.
    /// </summary>
    /// <typeparam name="T">Type of elements in the list.</typeparam>

    interface ICurrentBufferProvider<T> : IDisposable
    {
        /// <summary>
        /// Gets the current items of the list.
        /// </summary>
        /// <remarks>
        /// The returned list is updated in-place when <see cref="UpdateWithNext"/>
        /// is called.
        /// </remarks>

        ICurrentBuffer<T> CurrentBuffer { get; }

        /// <summary>
        /// Update this instance with the next set of elements from the source.
        /// </summary>
        /// <returns>
        /// A Boolean that is <c>true</c> if this instance was updated with
        /// new elements; otherwise <c>false</c> to indicate that the end of
        /// the bucket source has been reached.
        /// </returns>

        bool UpdateWithNext();
    }

    abstract class CurrentBuffer<T> : ICurrentBuffer<T>
    {
        public abstract int Count { get; }
        public abstract T this[int index] { get; set; }

        public virtual bool IsReadOnly => false;

        public virtual int IndexOf(T item)
        {
            var comparer = EqualityComparer<T>.Default;

            for (var i = 0; i < Count; i++)
            {
                if (comparer.Equals(this[i], item))
                    return i;
            }

            return -1;
        }

        public virtual bool Contains(T item) => IndexOf(item) >= 0;

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex), arrayIndex, null);
            if (arrayIndex + Count > array.Length) throw new ArgumentException(null, nameof(arrayIndex));

            for (int i = 0, j = arrayIndex; i < Count; i++, j++)
                array[j] = this[i];
        }

        public virtual IEnumerator<T> GetEnumerator() => this.Take(Count).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
        void IList<T>.RemoveAt(int index) => throw new NotSupportedException();
        void ICollection<T>.Add(T item) => throw new NotSupportedException();
        void ICollection<T>.Clear() => throw new NotSupportedException();
        bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
    }
}

#endif // !NO_BUFFERS
