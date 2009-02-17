using System;

namespace MoreLinq.Test.Pull
{
    internal abstract class Scope<T> : IDisposable
    {
        private readonly T old;

        protected Scope(T current)
        {
            old = current;
        }

        public virtual void Dispose()
        {
            Restore(old);
        }

        protected abstract void Restore(T old);
    }
}