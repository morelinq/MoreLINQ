#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2012 Atif Aziz. All rights reserved.
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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Ensures that a source sequence of <see cref="IDisposable"/>
        /// objects are all acquired successfully. If the acquisition of any
        /// one <see cref="IDisposable"/> fails then those successfully
        /// acquired till that point are disposed.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">Source sequence of <see cref="IDisposable"/> objects.</param>
        /// <returns>
        /// Returns an array of all the acquired <see cref="IDisposable"/>
        /// objects in source order.
        /// </returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TSource[] Acquire<TSource>(this IEnumerable<TSource> source)
            where TSource : IDisposable
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var disposables = new List<TSource>();

            return source.Acquire(x => x,
                                  _ => default,
                                  ex =>
                                  {
                                      foreach (var disposable in disposables)
                                          disposable.Dispose();
                                      return true;
                                  })
                         .Pipe(x => disposables.Add(x))
                         .ToArray();
        }

        static IEnumerable<TResult> Acquire<TSource, TResult>(this
            IEnumerable<TSource> source,
            Func<TSource, TResult> elementSelector,
            Func<Exception, TResult> exceptionSelector,
            Func<Exception, bool> errorPredicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));
            if (exceptionSelector == null) throw new ArgumentNullException(nameof(exceptionSelector));
            if (errorPredicate == null) throw new ArgumentNullException(nameof(errorPredicate));

            return _(); IEnumerable<TResult> _()
            {
                using (var e = source.GetEnumerator())
                {
                    while (true)
                    {
                        Exception error = null;

                        try
                        {
                            if (!e.MoveNext())
                                break;
                        }
                        catch (Exception ex)
                        {
                            if (errorPredicate(ex))
                                throw;

                            error = ex;
                        }

                        yield return error == null
                                     ? elementSelector(e.Current)
                                     : exceptionSelector(error);
                    }
                }
            }
        }
    }
}
