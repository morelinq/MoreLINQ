namespace MoreLinq.Test
{
    using System.Collections.Generic;

    sealed class BreakingReadOnlyCollection<T> :
        BreakingSequence<T>,
        IReadOnlyCollection<T>
    {
        public BreakingReadOnlyCollection(int count) => Count = count;
        public int Count { get; }
    }
}
