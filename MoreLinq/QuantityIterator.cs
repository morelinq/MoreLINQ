using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreLinq
{
    static partial class MoreEnumerable
    {
        private static bool QuantityIterator<T>(IEnumerable<T> source, int limit, Func<int, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (source is ICollection<T>) return predicate(((ICollection<T>)source).Count);

            var count = 0;

            using (var e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (++count == limit)
                    {
                        break;
                    }
                }
            }

            return predicate(count);
        }
    }
}
