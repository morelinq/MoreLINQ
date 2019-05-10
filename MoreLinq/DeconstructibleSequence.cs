#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Atif Aziz. All rights reserved.
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

namespace MoreLinq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Represents a source whose elements can be sequentially deconstructed
    /// into separate variables.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>

    public partial struct DeconstructibleSequence<T> : IEnumerable<T>
    {
        readonly IEnumerable<T> _source;

        /// <summary>
        /// Initializes a new instance of <see cref="DeconstructibleSequence{T}"/>
        /// with a sequence.
        /// </summary>
        /// <param name="source">
        /// The source sequence on which to enable deconstruction.</param>

        public DeconstructibleSequence(IEnumerable<T> source) =>
            _source = source ?? throw new ArgumentNullException(nameof(source));

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() =>
            _source != null ? _source.GetEnumerator()
                            : throw CreateTooShortError();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void Deconstruct(int count, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T item9, out T item10, out T item11, out T item12, out T item13, out T item14, out T item15, out T item16)
        {
            Debug.Assert(count >= 2);
            Debug.Assert(count <= 16);

            using (var e = GetEnumerator())
            {
                item1  = e.MoveNext() ? e.Current : throw CreateTooShortError();
                item2  = e.MoveNext() ? e.Current : throw CreateTooShortError();
                item3  = count >=  3 ? e.MoveNext() ? e.Current : throw CreateTooShortError() : default;
                item4  = count >=  4 ? e.MoveNext() ? e.Current : throw CreateTooShortError() : default;
                item5  = count >=  5 ? e.MoveNext() ? e.Current : throw CreateTooShortError() : default;
                item6  = count >=  6 ? e.MoveNext() ? e.Current : throw CreateTooShortError() : default;
                item7  = count >=  7 ? e.MoveNext() ? e.Current : throw CreateTooShortError() : default;
                item8  = count >=  8 ? e.MoveNext() ? e.Current : throw CreateTooShortError() : default;
                item9  = count >=  9 ? e.MoveNext() ? e.Current : throw CreateTooShortError() : default;
                item10 = count >= 10 ? e.MoveNext() ? e.Current : throw CreateTooShortError() : default;
                item11 = count >= 11 ? e.MoveNext() ? e.Current : throw CreateTooShortError() : default;
                item12 = count >= 12 ? e.MoveNext() ? e.Current : throw CreateTooShortError() : default;
                item13 = count >= 13 ? e.MoveNext() ? e.Current : throw CreateTooShortError() : default;
                item14 = count >= 14 ? e.MoveNext() ? e.Current : throw CreateTooShortError() : default;
                item15 = count >= 15 ? e.MoveNext() ? e.Current : throw CreateTooShortError() : default;
                item16 = count >= 16 ? e.MoveNext() ? e.Current : throw CreateTooShortError() : default;
                if (e.MoveNext())
                    throw CreateTooLongError();
            }
        }

        static InvalidOperationException CreateTooShortError() =>
            new InvalidOperationException("Sequence too short.");

        static InvalidOperationException CreateTooLongError() =>
            new InvalidOperationException("Sequence too long.");
    }
}
