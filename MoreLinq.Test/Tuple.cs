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

using System;
using System.Collections.Generic;

namespace MoreLinq.Test
{
    internal sealed class Tuple<TFirst, TSecond>
    {
        public Tuple(TFirst first, TSecond second) { First = first; Second = second; }

        public TFirst First { get; private set; }
        public TSecond Second { get; private set; }

        public override bool Equals(object value)
        {
            return value != null && Equals(value as Tuple<TFirst, TSecond>);
        }

        private bool Equals(Tuple<TFirst, TSecond> type)
        {
            return type != null
                   && EqualityComparer<TFirst>.Default.Equals(type.First, First) 
                   && EqualityComparer<TSecond>.Default.Equals(type.Second, Second);
        }

        public override int GetHashCode() { throw new NotImplementedException(); }
        
        public override string ToString()
        {
            return "{ First = " + First + ", Second = " + Second + " }";
        }
    }
}