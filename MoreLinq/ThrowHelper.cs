using System;

namespace MoreLinq
{
    /// <summary>
    /// Helper methods to make it easier to throw exceptions.
    /// </summary>
    internal static class ThrowHelper
    {
        internal static void ThrowIfNull<T>(this T argument, string name) where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
