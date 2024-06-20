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
        /// <param name="inner"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>

        IZipList<TResult>
            Join<TInner, TKey, TResult>(
                 IEnumerable<TInner> inner,
                 Func<T, TKey> outerKeySelector,      // unused!
                 Func<TInner, TKey> innerKeySelector, // unused!
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

        public static ZipList<T> AsZipList<T>(this IEnumerable<T> source) =>
            new ZipList<T>(source);
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public readonly struct ZipList<T> : IZipList<T>
    {
        readonly IEnumerable<T> _source;

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="source"></param>

        public ZipList(IEnumerable<T> source) =>
            _source = source ?? throw new ArgumentNullException(nameof(source));

        /// <summary>
        ///
        /// </summary>

        public IEnumerator<T> GetEnumerator() =>
            _source switch
            {
                null => Enumerable.Empty<T>().GetEnumerator(),
                var source => source.GetEnumerator()
            };

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="inner"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>

        public ZipList<TResult>
            Join<TInner, TKey, TResult>(
                 IEnumerable<TInner> inner,
                 Func<T, TKey> outerKeySelector,      // unused!
                 Func<TInner, TKey> innerKeySelector, // unused!
                 Func<T, TInner, TResult> resultSelector)
        {
            if (inner == null) throw new ArgumentNullException(nameof(inner));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _source switch
            {
                null => default,
                var source => source.Zip(inner, resultSelector).AsZipList()
            };
        }

        IZipList<TResult>
            IZipList<T>.Join<TInner, TKey, TResult>(
                IEnumerable<TInner> inner,
                Func<T, TKey> outerKeySelector,      // unused!
                Func<TInner, TKey> innerKeySelector, // unused!
                Func<T, TInner, TResult> resultSelector) =>
            Join(inner, outerKeySelector, innerKeySelector, resultSelector);
    }
}
