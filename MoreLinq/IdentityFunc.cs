using System;

namespace MoreLinq
{
    internal static class IdentityFunc<T>
    {
        public static readonly Func<T, T> Value = x => x;
    }
}
