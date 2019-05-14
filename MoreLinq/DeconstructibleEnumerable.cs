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

    /// <summary>
    /// Represents a source whose elements can be sequentially deconstructed
    /// into separate variables.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>

    public partial struct DeconstructibleEnumerable<T> : IEnumerable<T>
    {
        readonly IEnumerable<T> _source;

        /// <summary>
        /// Initializes a new instance of <see cref="DeconstructibleEnumerable{T}"/>
        /// with a sequence.
        /// </summary>
        /// <param name="source">
        /// The source sequence on which to enable deconstruction.</param>

        public DeconstructibleEnumerable(IEnumerable<T> source) =>
            _source = source ?? throw new ArgumentNullException(nameof(source));

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() =>
            // can be null via default construction
            _source != null ? _source.GetEnumerator()
                            : throw new InvalidOperationException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        static InvalidOperationException CreateTooShortError(int expectedCount) =>
            CreateLengthMismatchError(expectedCount, "few");

        static InvalidOperationException CreateTooLongError(int expectedCount) =>
            CreateLengthMismatchError(expectedCount, "many");

        static InvalidOperationException CreateLengthMismatchError(int expectedCount, string actualQuantifier) =>
            new InvalidOperationException($"Sequence contains too {actualQuantifier} elements when exactly {expectedCount} {(expectedCount == 1 ? "was" : "were")} expected.");
    }
}
