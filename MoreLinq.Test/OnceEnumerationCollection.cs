#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Leandro F. Vieira (leandromoh). All rights reserved.
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

namespace MoreLinq.Test
{
    using System.Collections.Generic;

    /// <summary>
    /// Collection that removes each item after yield it
    /// </summary>
    sealed class OnceEnumerationCollection<T> : List<T>, IEnumerable<T>
    {
        public OnceEnumerationCollection(IEnumerable<T> source) : base(source) { }

        public new IEnumerator<T> GetEnumerator()
        {
            for(int i = 0, leng = Count; i < leng; i++)
            {
                var temp = this[0];
                this.RemoveAt(0);
                yield return temp;
            }
        }
    }
}
