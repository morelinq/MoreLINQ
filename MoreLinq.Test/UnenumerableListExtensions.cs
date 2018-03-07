namespace MoreLinq.Test
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	static class UnenumerableListExtensions
    {
        internal static IEnumerable<T> ToUnenumerableList<T>(this IEnumerable<T> enumerable, bool readOnly)
		{
			return readOnly
	            ? (IEnumerable<T>)new UnenumerableReadOnlyList<T>(enumerable)
	            : (IEnumerable<T>)new UnenumerableList<T>(enumerable);
		}

	}
}
