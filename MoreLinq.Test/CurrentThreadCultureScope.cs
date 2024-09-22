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

namespace MoreLinq.Test
{
    using System.Globalization;

    sealed class CurrentThreadCultureScope : Scope<CultureInfo>
    {
        public CurrentThreadCultureScope(CultureInfo @new) :
            base(CultureInfo.CurrentCulture) => Install(@new);

        protected override void Restore(CultureInfo old) => Install(old);
        static void Install(CultureInfo value) => CultureInfo.CurrentCulture = value;
    }
}
