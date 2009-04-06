using System;
using System.Collections.Generic;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a sequence of values based on indexes.
        /// </summary>
        /// <remarks>
        /// The sequence is (practically) infinite
        /// - the index ranges from 0 to <c>int.MaxValue</c> inclusive. This function defers
        /// execution and streams the results.
        /// </remarks>
        /// <typeparam name="TResult">Type of result to generate</typeparam>
        /// <param name="generator">Generation function to apply to each index</param>
        /// <returns>A sequence </returns>

        public static IEnumerable<TResult> GenerateByIndex<TResult>(Func<int, TResult> generator)
        {
            // Would just use Enumerable.Range(0, int.MaxValue).Select(generator) but that doesn't
            // include int.MaxValue. Picky, I know...
            generator.ThrowIfNull("generator");
            return GenerateByIndexImpl(generator);
        }

        private static IEnumerable<TResult> GenerateByIndexImpl<TResult>(Func<int, TResult> generator)
        {
            // Looping over 0...int.MaxValue inclusive is a pain. Simplest is to go exclusive,
            // then go again for int.MaxValue.
            for (int i = 0; i < int.MaxValue; i++)
            {
                yield return generator(i);
            }
            yield return generator(int.MaxValue);
        }
    }
}
