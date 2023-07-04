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

    sealed class ReverseComparer<T> : IComparer<T>
    {
        readonly IComparer<T> _underlying;

        public ReverseComparer(IComparer<T>? underlying) =>
            _underlying = underlying ?? Comparer<T>.Default;

        public int Compare
#if NETCOREAPP3_1_OR_GREATER
            (T? x, T? y)
#else
            (T x, T y)
#endif
        {
            var result = _underlying.Compare(x, y);
            return result < 0 ? 1 : result > 0 ? -1 : 0;
        }
    }
}
