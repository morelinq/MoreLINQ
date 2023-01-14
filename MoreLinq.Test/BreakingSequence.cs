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

    /// <summary>
    /// Enumerable sequence which throws InvalidOperationException as soon as its
    /// enumerator is requested. Used to check lazy evaluation.
    /// </summary>
    class BreakingSequence<T> : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator() => throw new BreakException();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    sealed class BreakException : Exception
    {
        public BreakException() { }
        public BreakException(string message) : base(message) { }
        public BreakException(string message, Exception inner) : base(message, inner) { }
    }
}
