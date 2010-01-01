using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq.ModelShredder;
using System.Data;

namespace MoreLinq
{
	public static partial class MoreEnumerable
	{
		/// <summary>
		/// Converts a sequence to a data table.
		/// </summary>
		/// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <param name="source">The source.</param>
		/// <returns>A <see cref="DataTable"/> representing the source.</returns>
		public static DataTable ToDataTable<T>(this IEnumerable<T> source)
		{
			return source.ToDataTable(DefaultShredderOptionsProvider.ProvideShredderOptions(typeof(T)));
		}
		/// <summary>
		/// Converts a sequence to a data table according to <see cref="ShredderOptions"/>
		/// </summary>
		/// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="options">The options to respect.</param>
		/// <returns>
		/// A <see cref="DataTable"/> representing the source.
		/// </returns>
		public static DataTable ToDataTable<T>(this IEnumerable<T> source, ShredderOptions options)
		{
			Shredder<T> ms = new Shredder<T>(options, new InjectionObjectShredder(), new DefaultSchemaBuilder());

			return ms.Shred(source);
		}

	}
}
