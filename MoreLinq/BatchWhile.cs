#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2020 Kir_Antipov. All rights reserved.
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
    using System.Collections.Generic;
    using System.Linq;

    public static partial class MoreEnumerable
    {
        /// <inheritdoc cref="BatchWhile{TSource, TElement, TResult}(IEnumerable{TSource}, Func{TSource, TElement}, Func{TElement, IBucket{TElement}, bool}, Func{IBucket{TElement}, TResult})"/>

        public static IEnumerable<IBucket<TSource>> BatchWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, IBucket<TSource>, bool> predicate) =>
            BatchWhile(source, x => x, predicate, x => x);

        /// <inheritdoc cref="BatchWhile{TSource, TElement, TResult}(IEnumerable{TSource}, Func{TSource, TElement}, Func{TElement, IBucket{TElement}, bool}, Func{IBucket{TElement}, TResult})"/>

        public static IEnumerable<IBucket<TElement>> BatchWhile<TSource, TElement>(this IEnumerable<TSource> source, Func<TSource, TElement> elementSelector, Func<TElement, IBucket<TElement>, bool> predicate) =>
            BatchWhile(source, elementSelector, predicate, x => x);

        /// <inheritdoc cref="BatchWhile{TSource, TElement, TResult}(IEnumerable{TSource}, Func{TSource, TElement}, Func{TElement, IBucket{TElement}, bool}, Func{IBucket{TElement}, TResult})"/>

        public static IEnumerable<TResult> BatchWhile<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IBucket<TSource>, bool> predicate, Func<IBucket<TSource>, TResult> resultSelector) =>
            BatchWhile(source, x => x, predicate, resultSelector);

        /// <summary>
        /// Batches the <paramref name="source"/> sequence into independent buckets according to a user predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TElement">The type of the elements in each <see cref="IBucket{TElement}"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector"/>.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an <see cref="IBucket{TElement}"/>.</param>
        /// <param name="predicate">A function to test each element for a condition. If the function returns <see langword="false"/>, a new subsequence begins.</param>
        /// <param name="resultSelector">A function to create a result value from each bucket.</param>
        /// <returns>A sequence of independent buckets containing elements of the source collection.</returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results
        /// (buckets are streamed but their content buffered).
        /// </para>
        /// <para>
        /// All buckets are guaranteed to have at least <see langword="1" /> element.
        /// </para>
        /// </remarks>

        public static IEnumerable<TResult> BatchWhile<TSource, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TElement> elementSelector, Func<TElement, IBucket<TElement>, bool> predicate, Func<IBucket<TElement>, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            switch (source)
            {
                case IList<TSource> list when list.Count == 0:
                {
                    return Enumerable.Empty<TResult>();
                }
                case IList<TSource> list:
                {
                    return _(); IEnumerable<TResult> _()
                    {
                        var current = 1;
                        var size = list.Count;
                        var bucket = new Bucket<TElement> { elementSelector(list[0]) }; // list has at least 1 element here

                        while (current < size)
                        {
                            var element = elementSelector(list[current]);
                            if (predicate(element, bucket))
                            {
                                bucket.Add(element);
                            }
                            else
                            {
                                yield return resultSelector(bucket);
                                bucket = new Bucket<TElement> { element };
                            }
                            ++current;
                        }
                        yield return resultSelector(bucket);
                    }
                }

                case IReadOnlyList<TSource> list when list.Count == 0:
                {
                    return Enumerable.Empty<TResult>();
                }
                case IReadOnlyList<TSource> list:
                {
                    return _(); IEnumerable<TResult> _()
                    {
                        var current = 1;
                        var size = list.Count;
                        var bucket = new Bucket<TElement> { elementSelector(list[0]) }; // list has at least 1 element here

                        while (current < size)
                        {
                            var element = elementSelector(list[current]);
                            if (predicate(element, bucket))
                            {
                                bucket.Add(element);
                            }
                            else
                            {
                                yield return resultSelector(bucket);
                                bucket = new Bucket<TElement> { element };
                            }
                            ++current;
                        }
                        yield return resultSelector(bucket);
                    }
                }

                case ICollection<TSource> collection when collection.Count == 0:
                case IReadOnlyCollection<TSource> readOnlyCollection when readOnlyCollection.Count == 0:
                {
                    return Enumerable.Empty<TResult>();
                }

                default:
                {
                    return _(); IEnumerable<TResult> _()
                    {
                        var enumerator = source.GetEnumerator();
                        var active = enumerator.MoveNext();

                        if (!active)
                            yield break;

                        var bucket = new Bucket<TElement> { elementSelector(enumerator.Current) };
                        active = enumerator.MoveNext();

                        while (active)
                        {
                            var element = elementSelector(enumerator.Current);
                            if (predicate(element, bucket))
                            {
                                bucket.Add(element);
                            }
                            else
                            {
                                yield return resultSelector(bucket);
                                bucket = new Bucket<TElement> { element };
                            }
                            active = enumerator.MoveNext();
                        }
                        yield return resultSelector(bucket);
                    }
                }
            }
        }
    }
}
