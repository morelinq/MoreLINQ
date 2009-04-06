using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace MoreLinq.Test
{
    /// <summary>
    /// Data and functions to use throughout tests.
    /// </summary>
    internal static class SampleData
    {
        internal static readonly ReadOnlyCollection<string> Strings = new ReadOnlyCollection<string>(
            new[] { "ax", "hello", "world", "aa", "ab", "ay", "az" });

        internal static readonly ReadOnlyCollection<int> Values =
            new ReadOnlyCollection<int>(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

        internal static readonly Func<int, int, int> Plus = (a, b) => a + b;
        internal static readonly Func<int, int, int> Mul = (a, b) => a * b;

        internal static readonly IComparer<char> ReverseCharComparer = new ReverseCharComparerImpl();

        private class ReverseCharComparerImpl : IComparer<char>
        {
            public int Compare(char x, char y)
            {
                return y.CompareTo(x);
            }
        }
    }
}
