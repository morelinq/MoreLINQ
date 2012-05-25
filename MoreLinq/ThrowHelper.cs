#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008-2011 Jonathan Skeet. All rights reserved.
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

using System;

namespace MoreLinq
{
    /// <summary>
    /// Helper methods to make it easier to throw exceptions.
    /// </summary>
    internal static class ThrowHelper
    {
        internal static void ThrowIfNull<T>(this T argument, string name) where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        internal static void ThrowIfNegative(this int argument, string name)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        internal static void ThrowIfNonPositive(this int argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }
    }
}
