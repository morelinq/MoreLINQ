#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2010 Leopold Bushkin. All rights reserved.
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

#pragma warning disable CA5394 // Do not use insecure randomness

namespace MoreLinq
{
    using System;
#if !NET6_0_OR_GREATER
    using System.Threading;
#endif

    public static partial class MoreEnumerable
    {
        /// <remarks>
        /// <see cref="System.Random"/> is not thread-safe so the following
        /// implementation uses thread-local <see cref="System.Random"/>
        /// instances to create the illusion of a global
        /// <see cref="System.Random"/> implementation. For some background,
        /// see <a href="https://blogs.msdn.microsoft.com/pfxteam/2009/02/19/getting-random-numbers-in-a-thread-safe-way/">Getting
        /// random numbers in a thread-safe way</a>.
        /// On .NET 6+, delegates to <c>Random.Shared</c>.
        /// </remarks>

        partial class GlobalRandom { }

#if NET6_0_OR_GREATER
        static partial class GlobalRandom
        {
            public static Random Instance => System.Random.Shared;
        }
#else // NET6_0_OR_GREATER
        sealed partial class GlobalRandom : Random
        {
            public static Random Instance { get; } = new GlobalRandom();

            static int _seed = Environment.TickCount;
            [ThreadStatic] static Random? _threadRandom;
            static Random ThreadRandom => _threadRandom ??= new Random(Interlocked.Increment(ref _seed));

            GlobalRandom() { }

            public override int Next() => ThreadRandom.Next();
            public override int Next(int minValue, int maxValue) => ThreadRandom.Next(minValue, maxValue);
            public override int Next(int maxValue) => ThreadRandom.Next(maxValue);
            public override double NextDouble() => ThreadRandom.NextDouble();
            public override void NextBytes(byte[] buffer) => ThreadRandom.NextBytes(buffer);

            protected override double Sample()
            {
                // All the NextXXX calls are hijacked above to use the Random
                // instance allocated for the thread so no call from the base
                // class should ever end up here. If Random introduces new
                // virtual members in the future that call into Sample and
                // which end up getting used in the implementation of a
                // randomizing operator from the outer class then they will
                // need to be overriden.

                throw new NotImplementedException();
            }
        }
#endif // NET6_0_OR_GREATER
    }
}
