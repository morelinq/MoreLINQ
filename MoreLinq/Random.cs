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

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns an infinite sequence of random integers using the standard
        /// .NET random number generator.
        /// </summary>
        /// <returns>An infinite sequence of random integers</returns>
        /// <remarks>
        /// The implementation internally uses a shared, thread-local instance of
        /// <see cref="System.Random" /> to generate a random number on each
        /// iteration. The actual <see cref="System.Random" /> instance used
        /// therefore will depend on the thread on which a single iteration is
        /// taking place; that is the call to
        /// <see cref="System.Collections.IEnumerator.MoveNext()" />. If the
        /// overall iteration takes place on different threads (e.g.
        /// via asynchronous awaits completing on different threads) then various
        /// different <see cref="System.Random" /> instances will be involved
        /// in the generation of the sequence of random numbers. Because the
        /// <see cref="System.Random" /> instance is shared, if multiple sequences
        /// are generated on the same thread, the order of enumeration affects the
        /// resulting sequences.
        /// </remarks>

        public static IEnumerable<int> Random()
        {
            return Random(GlobalRandom.Instance);
        }

        /// <summary>
        /// Returns an infinite sequence of random integers using the supplied
        /// random number generator.
        /// </summary>
        /// <param name="rand">Random generator used to produce random numbers</param>
        /// <returns>An infinite sequence of random integers</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rand"/> is <see langword="null"/>.</exception>

        public static IEnumerable<int> Random(Random rand)
        {
            if (rand == null) throw new ArgumentNullException(nameof(rand));

            return RandomImpl(rand, r => r.Next());
        }

        /// <summary>
        /// Returns an infinite sequence of random integers between zero and
        /// a given maximum.
        /// </summary>
        /// <param name="maxValue">exclusive upper bound for the random values returned</param>
        /// <returns>An infinite sequence of random integers</returns>
        /// <remarks>
        /// The implementation internally uses a shared, thread-local instance of
        /// <see cref="System.Random" /> to generate a random number on each
        /// iteration. The actual <see cref="System.Random" /> instance used
        /// therefore will depend on the thread on which a single iteration is
        /// taking place; that is the call to
        /// <see cref="System.Collections.IEnumerator.MoveNext()" />. If the
        /// overall iteration takes place on different threads (e.g.
        /// via asynchronous awaits completing on different threads) then various
        /// different <see cref="System.Random" /> instances will be involved
        /// in the generation of the sequence of random numbers. Because the
        /// <see cref="System.Random" /> instance is shared, if multiple sequences
        /// are generated on the same thread, the order of enumeration affects the
        /// resulting sequences.
        /// </remarks>

        public static IEnumerable<int> Random(int maxValue)
        {
            if (maxValue < 0) throw new ArgumentOutOfRangeException(nameof(maxValue));

            return Random(GlobalRandom.Instance, maxValue);
        }

        /// <summary>
        /// Returns an infinite sequence of random integers between zero and a
        /// given maximum using the supplied random number generator.
        /// </summary>
        /// <param name="rand">Random generator used to produce values</param>
        /// <param name="maxValue">Exclusive upper bound for random values returned</param>
        /// <returns>An infinite sequence of random integers</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rand"/> is <see langword="null"/>.</exception>

        public static IEnumerable<int> Random(Random rand, int maxValue)
        {
            if (rand == null) throw new ArgumentNullException(nameof(rand));
            if (maxValue < 0) throw new ArgumentOutOfRangeException(nameof(maxValue));

            return RandomImpl(rand, r => r.Next(maxValue));
        }

        /// <summary>
        /// Returns an infinite sequence of random integers between a given
        /// minimum and a maximum.
        /// </summary>
        /// <param name="minValue">Inclusive lower bound of the values returned</param>
        /// <param name="maxValue">Exclusive upper bound of the values returned</param>
        /// <returns>An infinite sequence of random integers</returns>
        /// <remarks>
        /// The implementation internally uses a shared, thread-local instance of
        /// <see cref="System.Random" /> to generate a random number on each
        /// iteration. The actual <see cref="System.Random" /> instance used
        /// therefore will depend on the thread on which a single iteration is
        /// taking place; that is the call to
        /// <see cref="System.Collections.IEnumerator.MoveNext()" />. If the
        /// overall iteration takes place on different threads (e.g.
        /// via asynchronous awaits completing on different threads) then various
        /// different <see cref="System.Random" /> instances will be involved
        /// in the generation of the sequence of random numbers. Because the
        /// <see cref="System.Random" /> instance is shared, if multiple sequences
        /// are generated on the same thread, the order of enumeration affects the
        /// resulting sequences.
        /// </remarks>

        public static IEnumerable<int> Random(int minValue, int maxValue)
        {
            return Random(GlobalRandom.Instance, minValue, maxValue);
        }

        /// <summary>
        /// Returns an infinite sequence of random integers between a given
        /// minumum and a maximum using the supplied random number generator.
        /// </summary>
        /// <param name="rand">Generator used to produce random numbers</param>
        /// <param name="minValue">Inclusive lower bound of the values returned</param>
        /// <param name="maxValue">Exclusive upper bound of the values returned</param>
        /// <returns>An infinite sequence of random integers</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rand"/> is <see langword="null"/>.</exception>

        public static IEnumerable<int> Random(Random rand, int minValue, int maxValue)
        {
            if (rand == null) throw new ArgumentNullException(nameof(rand));
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException( nameof(minValue),
                    string.Format("The argument minValue ({0}) is greater than maxValue ({1})", minValue, maxValue) );

            return RandomImpl(rand, r => r.Next(minValue, maxValue));
        }

        /// <summary>
        /// Returns an infinite sequence of random double values between 0.0 and 1.0
        /// </summary>
        /// <returns>An infinite sequence of random doubles</returns>
        /// <remarks>
        /// The implementation internally uses a shared, thread-local instance of
        /// <see cref="System.Random" /> to generate a random number on each
        /// iteration. The actual <see cref="System.Random" /> instance used
        /// therefore will depend on the thread on which a single iteration is
        /// taking place; that is the call to
        /// <see cref="System.Collections.IEnumerator.MoveNext()" />. If the
        /// overall iteration takes place on different threads (e.g.
        /// via asynchronous awaits completing on different threads) then various
        /// different <see cref="System.Random" /> instances will be involved
        /// in the generation of the sequence of random numbers. Because the
        /// <see cref="System.Random" /> instance is shared, if multiple sequences
        /// are generated on the same thread, the order of enumeration affects the
        /// resulting sequences.
        /// </remarks>

        public static IEnumerable<double> RandomDouble()
        {
            return RandomDouble(GlobalRandom.Instance);
        }

        /// <summary>
        /// Returns an infinite sequence of random double values between 0.0 and 1.0
        /// using the supplied random number generator.
        /// </summary>
        /// <param name="rand">Generator used to produce random numbers</param>
        /// <returns>An infinite sequence of random doubles</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rand"/> is <see langword="null"/>.</exception>

        public static IEnumerable<double> RandomDouble(Random rand)
        {
            if (rand == null) throw new ArgumentNullException(nameof(rand));

            return RandomImpl(rand, r => r.NextDouble());
        }

        /// <summary>
        /// This is the underlying implementation that all random operators use to
        /// produce a sequence of random values.
        /// </summary>
        /// <typeparam name="T">The type of value returned (either Int32 or Double)</typeparam>
        /// <param name="rand">Random generators used to produce the sequence</param>
        /// <param name="nextValue">Generator function that actually produces the next value - specific to T</param>
        /// <returns>An infinite sequence of random numbers of type T</returns>

        static IEnumerable<T> RandomImpl<T>(Random rand, Func<Random, T> nextValue)
        {
            while (true)
                yield return nextValue(rand);
        }

        /// <remarks>
        /// <see cref="System.Random"/> is not thread-safe so the following
        /// implementation uses thread-local <see cref="System.Random"/>
        /// instances to create the illusion of a global
        /// <see cref="System.Random"/> implementation. For some background,
        /// see <a href="https://blogs.msdn.microsoft.com/pfxteam/2009/02/19/getting-random-numbers-in-a-thread-safe-way/">Getting
        /// random numbers in a thread-safe way</a>
        /// </remarks>

        sealed class GlobalRandom : Random
        {
            public static readonly Random Instance = new GlobalRandom();

            static int _seed = Environment.TickCount;
            [ThreadStatic] static Random _threadRandom;
            static Random ThreadRandom => _threadRandom ?? (_threadRandom = new Random(Interlocked.Increment(ref _seed)));

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
    }
}
