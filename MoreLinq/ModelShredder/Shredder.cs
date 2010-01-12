using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MoreLinq.ModelShredder
{
	/// <summary>
	/// Coordinates translation of a sequence to a DataTable.
	/// </summary>
	/// <typeparam name="T">The type of elements contained in the sequence.</typeparam>
	public sealed class Shredder<T>
	{
		private readonly IShredderDelegateProvider m_ShredderProvider;
		private readonly ISchemaBuilder m_SchemaBuilder;

		private ShredderDelegate m_ShredderMethod;
		private ShredderOptions m_ShredderOptions;


        /// <summary>
		/// Initializes a new instance of the <see cref="Shredder{T}"/> class.
		/// </summary>
		/// <param name="options">The <see cref="ShredderOptions"/> to use.</param>
		/// <param name="shredderProvider">The shredder.</param>
		/// <param name="schemaBuilder">The table schema builder.</param>
		public Shredder(ShredderOptions options, IShredderDelegateProvider shredderProvider, ISchemaBuilder schemaBuilder)
		{
			if (options == null) throw new ArgumentNullException("options", "options is null.");
			if (shredderProvider == null) throw new ArgumentNullException("shredderProvider");
			if (schemaBuilder == null) throw new ArgumentNullException("schemaBuilder");

			m_ShredderOptions = options;
			m_ShredderProvider = shredderProvider;
			m_SchemaBuilder = schemaBuilder;

			m_ShredderMethod = m_ShredderProvider.GetShredderMethod(m_ShredderOptions);
		}

		/// <summary>
		/// Shreds the specified source objects into a <see cref="System.Data.DataTable"/>.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>A <see cref="System.Data.DataTable"/> containing a representation of the souce objects.</returns>
		public DataTable Shred(IEnumerable<T> source)
		{
			if (source == null) throw new ArgumentNullException("source", "source is null.");

			DataTable table = new DataTable();

			// Initialize table
			m_SchemaBuilder.BuildTableSchema(table, m_ShredderOptions);

			table.BeginLoadData();
			foreach (T element in source)
			{
				table.Rows.Add(m_ShredderMethod.Invoke(element));
			}
			table.EndLoadData();

			return table;
		}
    }
}
