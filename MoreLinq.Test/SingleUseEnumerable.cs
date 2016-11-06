using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreLinq.Test
{
    public sealed class SingleUseEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> src;
        private bool enumerated = false;

        public SingleUseEnumerable(IEnumerable<T> src)
        {
            this.src = src;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (enumerated) throw new InvalidOperationException("enumerated twice");

            foreach (var i in src)
                yield return i;

            enumerated = true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
