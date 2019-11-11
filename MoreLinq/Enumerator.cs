#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Pierre Lando. All rights reserved.
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
    using System.Collections.Generic;

    static class Enumerator
    {
        /// <summary>
        /// <para>
        /// Move the <paramref name="enumerator"/> to the next position and put the
        /// new current value into <paramref name="item"/>.</para>
        /// <para>
        /// If the <paramref name="enumerator"/> has no more element it's disposed and
        /// set to <c>null</c>, and <paramref name="item"/> is set to <c>default</c>.</para>
        /// <para>
        /// If the <paramref name="enumerator"/> is <c>null</c> the method return immediately
        /// and <paramref name="item"/> is set to <c>null</c>.</para>
        /// </summary>
        /// <typeparam name="T">The type of element that are enumerated.</typeparam>
        /// <param name="enumerator">The enumerator to iterate or dispose.</param>
        /// <param name="item">The new current value of <paramref name="enumerator"/> or
        /// <c>default</c> if <paramref name="enumerator"/> has no more element.
        /// </param>
        /// <remarks>
        /// Because <paramref name="enumerator"/> and <paramref name="item"/> may both be modified
        /// they are both passed by reference.
        /// </remarks>
        /// <returns>A <c>bool</c> value indicating if the enumerator has moved to the next element.</returns>

        public static bool TryRead<T>(ref IEnumerator<T> enumerator, out T item)
        {
            if (enumerator == null)
            {
                item = default;
                return false;
            }

            if (enumerator.MoveNext())
            {
                item = enumerator.Current;
                return true;
            }

            enumerator.Dispose();
            enumerator = null;
            item = default;
            return false;
        }
    }
}
