#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Felipe Sateler. All rights reserved.
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

    partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a single-element sequence containing the result of invoking the function.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results.
        /// If the resulting sequence is enumerated multiple times, the function will be
        /// invoked multiple times too.
        /// </remarks>
        /// <typeparam name="T">The type of the object returned by the function.</typeparam>
        /// <param name="function">The function to evaluate.</param>
        /// <returns>A sequence with the value resulting from invoking <paramref name="function"/>.</returns>

        public static IEnumerable<T> From<T>(Func<T> function)
        {
            return _(); IEnumerable<T> _()
            {
#pragma warning disable CA1062 // Validate arguments of public methods
                yield return function();
#pragma warning restore CA1062 // Validate arguments of public methods
            }
        }

        /// <summary>
        /// Returns a sequence containing the result of invoking each parameter function in order.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results.
        /// If the resulting sequence is enumerated multiple times, the functions will be
        /// invoked multiple times too.
        /// </remarks>
        /// <typeparam name="T">The type of the object returned by the functions.</typeparam>
        /// <param name="function1">The first function to evaluate.</param>
        /// <param name="function2">The second function to evaluate.</param>
        /// <returns>A sequence with the values resulting from invoking <paramref name="function1"/> and <paramref name="function2"/>.</returns>

        public static IEnumerable<T> From<T>(Func<T> function1, Func<T> function2)
        {
            return _(); IEnumerable<T> _()
            {
#pragma warning disable CA1062 // Validate arguments of public methods
                yield return function1();
                yield return function2();
#pragma warning restore CA1062 // Validate arguments of public methods
            }
        }

        /// <summary>
        /// Returns a sequence containing the result of invoking each parameter function in order.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results.
        /// If the resulting sequence is enumerated multiple times, the functions will be
        /// invoked multiple times too.
        /// </remarks>
        /// <typeparam name="T">The type of the object returned by the functions.</typeparam>
        /// <param name="function1">The first function to evaluate.</param>
        /// <param name="function2">The second function to evaluate.</param>
        /// <param name="function3">The third function to evaluate.</param>
        /// <returns>A sequence with the values resulting from invoking <paramref name="function1"/>, <paramref name="function2"/> and <paramref name="function3"/>.</returns>

        public static IEnumerable<T> From<T>(Func<T> function1, Func<T> function2, Func<T> function3)
        {
            return _(); IEnumerable<T> _()
            {
#pragma warning disable CA1062 // Validate arguments of public methods
                yield return function1();
                yield return function2();
                yield return function3();
#pragma warning restore CA1062 // Validate arguments of public methods
            }
        }

        /// <summary>
        /// Returns a sequence containing the values resulting from invoking (in order) each function in the source sequence of functions.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results.
        /// If the resulting sequence is enumerated multiple times, the functions will be
        /// invoked multiple times too.
        /// </remarks>
        /// <typeparam name="T">The type of the object returned by the functions.</typeparam>
        /// <param name="functions">The functions to evaluate.</param>
        /// <returns>A sequence with the values resulting from invoking all of the <paramref name="functions"/>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="functions"/> is <c>null</c>.</exception>

        public static IEnumerable<T> From<T>(params Func<T>[] functions)
        {
            if (functions == null) throw new ArgumentNullException(nameof(functions));
            return Evaluate(functions);
        }
    }
}
