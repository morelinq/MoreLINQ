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

namespace MoreLinq.Test
{
    /// <summary>
    /// Functions which throw NotImplementedException if they're ever called.
    /// </summary>
    internal static class BreakingFunc
    {
        internal static Func<TResult> Of<TResult>()
        {
            return () => { throw new NotImplementedException(); };
        }

        internal static Func<T, TResult> Of<T, TResult>()
        {
            return t => { throw new NotImplementedException(); };
        }

        internal static Func<T1, T2, TResult> Of<T1, T2, TResult>()
        {
            return (t1, t2) => { throw new NotImplementedException(); };
        }

        internal static Func<T1, T2, T3, TResult> Of<T1, T2, T3, TResult>()
        {
            return (t1, t2, t3) => { throw new NotImplementedException(); };
        }

        internal static Func<T1, T2, T3, T4, TResult> Of<T1, T2, T3, T4, TResult>()
        {
            return (t1, t2, t3, t4) => { throw new NotImplementedException(); };
        }
    }
}
