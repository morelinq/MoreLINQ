namespace MoreLinq
{
    static class EmptyArray<T>
    {
        public static readonly T[] Value =
#if NETSTANDARD1_6_OR_GREATER || NET6_0_OR_GREATER
            System.Array.Empty<T>();
#else
#pragma warning disable CA1825 // Avoid zero-length array allocations
            new T[0];
#pragma warning restore CA1825 // Avoid zero-length array allocations
#endif
    }
}
