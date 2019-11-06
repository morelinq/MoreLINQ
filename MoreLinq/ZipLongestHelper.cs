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

    static class ZipLongestHelper
    {
        public static bool MoveNextOrDefault<T>(ref IEnumerator<T> enumerator, ref T value)
        {
            if (enumerator == null)
            {
                return false;
            }

            if (enumerator.MoveNext())
            {
                value = enumerator.Current;
                return true;
            }

            enumerator.Dispose();
            enumerator = null;
            value = default;
            return false;
        }
    }
}
