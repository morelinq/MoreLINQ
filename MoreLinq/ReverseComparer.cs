#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Felipe Sateler. All rights reserved.
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

    /// <summary>
    /// A <see cref="IComparer{T}"/> that compares in reverse order than the specified <see cref="IComparer{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared</typeparam>

    class ReverseComparer<T> : IComparer<T>
    {
        readonly IComparer<T> _underlying;

        public ReverseComparer(IComparer<T> underlying)
        {
            if (underlying == null) underlying = Comparer<T>.Default;
            _underlying = underlying;
        }

        public int Compare(T x, T y)
        {
            int res = _underlying.Compare(x, y);
            if (res > 0) {
                return -1;
            }
            else if (res < 0) {
                return 1;
            }
            else {
                return 0;
            }
        }
    }
}
