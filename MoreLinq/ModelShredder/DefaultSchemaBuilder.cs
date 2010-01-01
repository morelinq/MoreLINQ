using System;
using System.Data;
using System.Reflection;

namespace MoreLinq.ModelShredder
{
	/// <summary>
	/// Builds a flat table schema.
	/// </summary>
    public sealed class DefaultSchemaBuilder : ISchemaBuilder
    {
		private static bool IsNullable(Type t)
		{
			return (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
		}

        /// <summary>
        /// Adds columns to an empty DataTable that map to properties and fields on a Type.
        /// </summary>
        /// <param name="table">The empty table to which to add columns.</param>
        /// <param name="options">Controls the visible members and their order in the schema.</param>
        /// <returns><see cref="ShredderOptions"/> which indicate which Fields and Properties were mapped (in order).</returns>
		public void BuildTableSchema(DataTable table, ShredderOptions options)
		{
			if (table == null)
				throw new ArgumentNullException("table", "table is null.");
			if (options == null)
				throw new ArgumentNullException("options", "options is null.");
			if (table.Columns.Count > 0)
				throw new ArgumentException("table", "table schema is not empty.");

			foreach (MemberInfo member in options.Members)
			{
				DataColumn dc = new DataColumn();
				dc.ColumnName = member.Name;

				// Member info is either PropertyInfo or FieldInfo, PropertyInfo is more likely.
				PropertyInfo pi = member as PropertyInfo;
				if (pi != null)
				{
					if (IsNullable(pi.PropertyType))
					{
						dc.DataType = pi.PropertyType.GetGenericArguments()[0];
						dc.AllowDBNull = true;
					}
					else
					{
						dc.DataType = pi.PropertyType;
					}
				}
				else
				{
					// Explicit casting makes sure exception is thrown when cast is invalid.
					// We tried PropertyInfo before. Should never happen.
					FieldInfo fi = (FieldInfo)member;
					if (IsNullable(fi.FieldType))
					{
						dc.DataType = fi.FieldType.GetGenericArguments()[0];
						dc.AllowDBNull = true;
					}
					else
					{
						dc.DataType = fi.FieldType;
					}
				}

				// Add column to table
				table.Columns.Add(dc);
			}
		}
    }
}