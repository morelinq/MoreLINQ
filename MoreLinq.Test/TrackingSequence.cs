using System.Collections;
using System.Collections.Generic;

namespace MoreLinq.Test
{
    sealed class TrackingTestEnumerable<T> : IEnumerable<T>
    {
        public TrackingTestEnumerable(params T[] values)
        {
            Enumerator = new TrackingEnumerator<T>(values);
        }

        public TrackingEnumerator<T> Enumerator { get; }

        public IEnumerator<T> GetEnumerator()
        {
            Enumerator.IncreaseEnumerationCount();
            Enumerator.Reset();
            return Enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    sealed class TrackingTestCollection<T> : ICollection<T>
    {
        private readonly T[] values;

        public TrackingTestCollection(params T[] values)
        {
            this.values = values;
            Enumerator = new TrackingEnumerator<T>(this.values);
        }

        public TrackingEnumerator<T> Enumerator { get; }

        public int Count => ((ICollection<T>)values).Count;

        public bool IsReadOnly => ((ICollection<T>)values).IsReadOnly;

        public void Add(T item) => ((ICollection<T>)values).Add(item);

        public void Clear() => ((ICollection<T>)values).Clear();

        public bool Contains(T item) => ((ICollection<T>)values).Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => ((ICollection<T>)values).CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator()
        {
            Enumerator.IncreaseEnumerationCount();
            Enumerator.Reset();
            return Enumerator;
        }

        public bool Remove(T item) => ((ICollection<T>)values).Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    sealed class TrackingEnumerator<T> : IEnumerator<T>
    {
        private readonly T[] values;
        private int loopCount;
        private int moveNextCount;
        private int index;
        private bool disposed;

        public TrackingEnumerator(T[] values)
        {
            index = -1;

            this.values = values;
        }

        public bool Disposed => disposed;

        public int LoopCount => loopCount;

        public int MoveNextCount => moveNextCount;

        public void IncreaseEnumerationCount() => loopCount++;

        public bool MoveNext()
        {
            index++;
            moveNextCount++;
            return index < values.Length;
        }

        public void Reset() => index = -1;

        public void Dispose() => disposed = true;

        public T Current => values[index];

        object IEnumerator.Current => Current;
    }
}
