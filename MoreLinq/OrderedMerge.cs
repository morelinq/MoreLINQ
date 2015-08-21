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

namespace MoreLinq
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    #endregion

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Merges two ordered sequences into one. Where the elements equal
        /// in both sequences, the element from the first sequence is 
        /// returned in the resulting sequence.
        /// </summary>
        /// <remarks>
        /// This method uses deferred execution. The behavior is undefined 
        /// if the sequences are unordered as inputs.
        /// </remarks>

        public static IEnumerable<T> OrderedMerge<T>(
            this IEnumerable<T> first,
            IEnumerable<T> second)
        {
            return OrderedMerge(first, second, null);
        }

        /// <summary>
        /// Merges two ordered sequences into one with an additional 
        /// parameter specifying how to compare the elements of the 
        /// sequences. Where the elements equal in both sequences, the 
        /// element from the first sequence is returned in the resulting 
        /// sequence.
        /// </summary>
        /// <remarks>
        /// This method uses deferred execution. The behavior is undefined 
        /// if the sequences are unordered as inputs.
        /// </remarks>

        public static IEnumerable<T> OrderedMerge<T>(
            this IEnumerable<T> first,
            IEnumerable<T> second,
            IComparer<T> comparer)
        {
            return OrderedMerge(first, second, e => e, f => f, s => s, (a, _) => a, comparer);
        }

        /// <summary>
        /// Merges two ordered sequences into one with an additional 
        /// parameter specifying the element key by which the sequences are 
        /// ordered. Where the keys equal in both sequences, the 
        /// element from the first sequence is returned in the resulting 
        /// sequence.
        /// </summary>
        /// <remarks>
        /// This method uses deferred execution. The behavior is undefined 
        /// if the sequences are unordered (by key) as inputs.
        /// </remarks>

        public static IEnumerable<T> OrderedMerge<T, TKey>(
            this IEnumerable<T> first,
            IEnumerable<T> second,
            Func<T, TKey> keySelector)
        {
            return OrderedMerge(first, second, keySelector, a => a, b => b, (a, _) => a, null);
        }

        /// <summary>
        /// Merges two ordered sequences into one. Additional parameters
        /// specify the element key by which the sequences are ordered, 
        /// the result when element is found in first sequence but not in 
        /// the second, the result when element is found in second sequence 
        /// but not in the first and the result when elements are found in 
        /// both sequences.
        /// </summary>
        /// <remarks>
        /// This method uses deferred execution. The behavior is undefined 
        /// if the sequences are unordered (by key) as inputs.
        /// </remarks>

        public static IEnumerable<TResult> OrderedMerge<T, TKey, TResult>(
            this IEnumerable<T> first,
            IEnumerable<T> second,
            Func<T, TKey> keySelector,
            Func<T, TResult> firstSelector,
            Func<T, TResult> secondSelector,
            Func<T, T, TResult> bothSelector)
        {
            return OrderedMerge(first, second, keySelector, firstSelector, secondSelector, bothSelector, null);
        }

        /// <summary>
        /// Merges two ordered sequences into one. Additional parameters
        /// specify the element key by which the sequences are ordered, 
        /// the result when element is found in first sequence but not in 
        /// the second, the result when element is found in second sequence 
        /// but not in the first, the result when elements are found in 
        /// both sequences and a method for comparing keys.
        /// </summary>
        /// <remarks>
        /// This method uses deferred execution. The behavior is undefined 
        /// if the sequences are unordered (by key) as inputs.
        /// </remarks>

        public static IEnumerable<TResult> OrderedMerge<T, TKey, TResult>(
            this IEnumerable<T> first,
            IEnumerable<T> second,
            Func<T, TKey> keySelector,
            Func<T, TResult> firstSelector,
            Func<T, TResult> secondSelector,
            Func<T, T, TResult> bothSelector,
            IComparer<TKey> comparer)
        {
            return OrderedMerge(first, second, keySelector, keySelector, firstSelector, secondSelector, bothSelector, comparer);
        }

        /// <summary>
        /// Merges two heterogeneous sequences ordered by a common key type 
        /// into a homogeneous one. Additional parameters specify the 
        /// element key by which the sequences are ordered, the result when 
        /// element is found in first sequence but not in the second and  
        /// the result when element is found in second sequence but not in 
        /// the first, the result when elements are found in both sequences.
        /// </summary>
        /// <remarks>
        /// This method uses deferred execution. The behavior is undefined 
        /// if the sequences are unordered (by key) as inputs.
        /// </remarks>

        public static IEnumerable<TResult> OrderedMerge<TFirst, TSecond, TKey, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst, TResult> firstSelector,
            Func<TSecond, TResult> secondSelector,
            Func<TFirst, TSecond, TResult> bothSelector)
        {
            return OrderedMerge(first, second, firstKeySelector, secondKeySelector, firstSelector, secondSelector, bothSelector, null);
        }

        /// <summary>
        /// Merges two heterogeneous sequences ordered by a common key type 
        /// into a homogeneous one. Additional parameters specify the 
        /// element key by which the sequences are ordered, the result when 
        /// element is found in first sequence but not in the second, 
        /// the result when element is found in second sequence but not in 
        /// the first, the result when elements are found in both sequences 
        /// and a method for comparing keys.
        /// </summary>
        /// <remarks>
        /// This method uses deferred execution. The behavior is undefined 
        /// if the sequences are unordered (by key) as inputs.
        /// </remarks>

        public static IEnumerable<TResult> OrderedMerge<TFirst, TSecond, TKey, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst, TResult> firstSelector,
            Func<TSecond, TResult> secondSelector,
            Func<TFirst, TSecond, TResult> bothSelector,
            IComparer<TKey> comparer)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (firstKeySelector == null) throw new ArgumentNullException("firstKeySelector");
            if (secondKeySelector == null) throw new ArgumentNullException("secondKeySelector");
            if (firstSelector == null) throw new ArgumentNullException("firstSelector");
            if (bothSelector == null) throw new ArgumentNullException("bothSelector");
            if (secondSelector == null) throw new ArgumentNullException("secondSelector");

            return OrderedMergeImpl(first, second, 
                                    firstKeySelector, secondKeySelector, 
                                    firstSelector, secondSelector, bothSelector, 
                                    comparer ?? Comparer<TKey>.Default);
        }

        static IEnumerable<TResult> OrderedMergeImpl<TFirst, TSecond, TKey, TResult>(
            IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst, TResult> firstSelector,
            Func<TSecond, TResult> secondSelector,
            Func<TFirst, TSecond, TResult> bothSelector,
            IComparer<TKey> comparer)
        {
            Debug.Assert(first != null);
            Debug.Assert(second != null);
            Debug.Assert(firstKeySelector != null);
            Debug.Assert(secondKeySelector != null);
            Debug.Assert(firstSelector != null);
            Debug.Assert(secondSelector != null);
            Debug.Assert(bothSelector != null);
            Debug.Assert(comparer != null);

            using (var e1 = first.GetEnumerator())
            using (var e2 = second.GetEnumerator())
            {
                var gotFirst = e1.MoveNext();
                var gotSecond = e2.MoveNext();

                while (gotFirst || gotSecond)
                {
                    if (gotFirst && gotSecond)
                    {
                        var element1 = e1.Current;
                        var key1 = firstKeySelector(element1);
                        var element2 = e2.Current;
                        var key2 = secondKeySelector(element2);
                        var comparison = comparer.Compare(key1, key2);

                        if (comparison < 0)
                        {
                            yield return firstSelector(element1);
                            gotFirst = e1.MoveNext();
                        }
                        else if (comparison > 0)
                        {
                            yield return secondSelector(element2);
                            gotSecond = e2.MoveNext();
                        }
                        else
                        {
                            yield return bothSelector(element1, element2);
                            gotFirst = e1.MoveNext();
                            gotSecond = e2.MoveNext();
                        }
                    }
                    else if (gotSecond)
                    {
                        yield return secondSelector(e2.Current);
                        gotSecond = e2.MoveNext();
                    }
                    else // (gotFirst)
                    {
                        yield return firstSelector(e1.Current);
                        gotFirst = e1.MoveNext();
                    }
                }
            }
        }
    }
}