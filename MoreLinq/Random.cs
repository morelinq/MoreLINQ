using System;
using System.Collections.Generic;

namespace MoreLinq
{
    /// <summary>
    /// Extension methods for generating random sequences.
    /// </summary>
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns an infinite sequence of random integers using the standard 
        /// .NET random number generator.
        /// </summary>
        /// <returns>An infinite sequence of random integers</returns>
        public static IEnumerable<int> Random()
        {
            return Random(new Random());
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
            rand.ThrowIfNull("rand");

            return RandomImpl(rand, r => r.Next());
        }

        /// <summary>
        /// Returns an infinite sequence of random integers between 0 and <paramref name="maxValue"/>/>.
        /// </summary>
        /// <param name="maxValue">exclusive upper bound for the random values returned</param>
        /// <returns>An infinite sequence of random integers</returns>
        public static IEnumerable<int> Random(int maxValue)
        {
            maxValue.ThrowIfNegative("maxValue");
            
            return Random(new Random(), maxValue);
        }

        /// <summary>
        /// Returns an infinite sequence of random integers between 0 and <paramref name="maxValue"/>/>
        /// using the supplied random number generator.
        /// </summary>
        /// <param name="rand">Random generator used to produce values</param>
        /// <param name="maxValue">Exclusive upper bound for random values returned</param>
        /// <returns>An infinite sequence of random integers</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rand"/> is <see langword="null"/>.</exception>
        public static IEnumerable<int> Random(Random rand, int maxValue)
        {
            rand.ThrowIfNull("rand");
            maxValue.ThrowIfNegative("maxValue");

            return RandomImpl(rand, r => r.Next(maxValue));
        }

        /// <summary>
        /// Returns an infinite sequence of random integers between <paramref name="minValue"/> and
        /// <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">Inclusive lower bound of the values returned</param>
        /// <param name="maxValue">Exclusive upper bound of the values returned</param>
        /// <returns>An infinite sequence of random integers</returns>
        public static IEnumerable<int> Random(int minValue, int maxValue)
        {
            return Random(new Random(), minValue, maxValue);
        }

        /// <summary>
        /// Returns an infinite sequence of random integers between <paramref name="minValue"/> and
        /// <paramref name="maxValue"/> using the supplied random number generator.
        /// </summary>
        /// <param name="rand">Generator used to produce random numbers</param>
        /// <param name="minValue">Inclusive lower bound of the values returned</param>
        /// <param name="maxValue">Exclusive upper bound of the values returned</param>
        /// <returns>An infinite sequence of random integers</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rand"/> is <see langword="null"/>.</exception>
        public static IEnumerable<int> Random(Random rand, int minValue, int maxValue)
        {
            rand.ThrowIfNull("rand");
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException( "minValue", 
                    string.Format("The argument minValue ({0}) is greater than maxValue ({1})", minValue, maxValue) );

            return RandomImpl(rand, r => r.Next(minValue, maxValue));
        }

        /// <summary>
        /// Returns an infinite sequence of random double values between 0.0 and 1.0
        /// </summary>
        /// <returns>An infinite sequence of random doubles</returns>
        public static IEnumerable<double> RandomDouble()
        {
            return RandomDouble(new Random());
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
            rand.ThrowIfNull("rand");

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
        private static IEnumerable<T> RandomImpl<T>(Random rand, Func<Random, T> nextValue)
        {
            while (true)
                yield return nextValue(rand);
        }
    }
}