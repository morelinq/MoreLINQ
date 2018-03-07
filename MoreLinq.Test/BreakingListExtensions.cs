namespace MoreLinq.Test
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	static class BreakingListExtensions
    {
        internal static IEnumerable<T> ToBreakingList<T>(this IEnumerable<T> enumerable, bool readOnly)
		{
			return readOnly
	            ? (IEnumerable<T>)new BreakingReadOnlyList<T>(enumerable)
	            : (IEnumerable<T>)new BreakingList<T>(enumerable);
		}

	}
}
