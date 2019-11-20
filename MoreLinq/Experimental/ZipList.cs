#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Atif Aziz. All rights reserved.
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

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// TODO
    /// </summary>
    /// <typeparam name="T">TODO</typeparam>

    public interface IZipList<out T> : IEnumerable<T>
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="second"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>

        IZipList<TResult>
            Join<TInner, TKey, TResult>(
                IEnumerable<TInner> second,
                Func<T, TKey> outerKeySelector,          // unused!
                Func<TInner, TKey> innerKeySelector,     // unused!
                Func<T, TInner, TResult> resultSelector);
    }

    /// <summary>
    /// TODO
    /// </summary>

    public static class ZipList
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        public static IZipList<T> ToZipList<T>(this IEnumerable<T> source) =>
            new ZipList<T>(source);
    }

    sealed class ZipList<T> : IZipList<T>
    {
        readonly IEnumerable<T> _source;

        public ZipList(IEnumerable<T> source) =>
            _source = source ?? throw new ArgumentNullException(nameof(source));

        public IEnumerator<T> GetEnumerator() => _source.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IZipList<TResult>
            Join<TInner, TKey, TResult>(
                IEnumerable<TInner> second,
                Func<T, TKey> outerKeySelector, // unused!
                Func<TInner, TKey> innerKeySelector, // unused!
                Func<T, TInner, TResult> resultSelector) =>
            _source.Zip(second, resultSelector).ToZipList();
    }
}
