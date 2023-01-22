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
    using System.Collections.Generic;

    static class SequenceReader
    {
        public static SequenceReader<T> Read<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new SequenceReader<T>(source);
        }
    }

    /// <summary>
    /// Adds reader semantics to a sequence where <see cref="IEnumerator{T}.MoveNext"/>
    /// and <see cref="IEnumerator{T}.Current"/> are rolled into a single
    /// "read" operation.
    /// </summary>
    /// <typeparam name="T">Type of elements to read.</typeparam>
    sealed class SequenceReader<T> : IDisposable
    {
        IEnumerator<T>? _enumerator;

        /// <summary>
        /// Initializes a <see cref="SequenceReader{T}" /> instance
        /// from an enumerable sequence.
        /// </summary>
        /// <param name="source">Source sequence.</param>

        public SequenceReader(IEnumerable<T> source) :
            this(GetEnumerator(source)) { }

        /// <summary>
        /// Initializes a <see cref="SequenceReader{T}" /> instance
        /// from an enumerator.
        /// </summary>
        /// <param name="enumerator">Source enumerator.</param>

        public SequenceReader(IEnumerator<T> enumerator) =>
            _enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));

        static IEnumerator<T> GetEnumerator(IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.GetEnumerator();
        }

        IEnumerator<T> Enumerator =>
            _enumerator ?? throw new ObjectDisposedException(GetType().FullName);

        /// <summary>
        /// Reads a value otherwise throws <see cref="InvalidOperationException"/>
        /// if no more values are available.
        /// </summary>
        /// <returns>
        /// Returns the read value;
        /// </returns>

        public T Read()
        {
            var e = Enumerator;

            if (!e.MoveNext())
                throw new InvalidOperationException();

            return e.Current;
        }

        /// <summary>
        /// Reads the end. If the end has not been reached then it
        /// throws <see cref="InvalidOperationException"/>.
        /// </summary>

        public void ReadEnd()
        {
            var enumerator = Enumerator;

            if (enumerator.MoveNext())
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Disposes this object and enumerator with which is was
        /// initialized.
        /// </summary>

        public void Dispose()
        {
            var e = _enumerator;
            if (e == null) return;
            _enumerator = null;
            e.Dispose();
        }
    }
}
