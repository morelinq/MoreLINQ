using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Converts a sequence to a <see cref="DataTable"/> object.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>
        /// A <see cref="DataTable"/> representing the source.
        /// </returns>
        
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            return ToDataTable(source, new DataTable());
        }

        /// <summary>
        /// Appends elements in the sequence as rows of a given <see cref="DataTable"/> object.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="table"></param>
        /// <returns>
        /// A <see cref="DataTable"/> or subclass representing the source.
        /// </returns>
        
        public static TTable ToDataTable<T, TTable>(this IEnumerable<T> source, TTable table)
            where TTable : DataTable
        {
            return ToDataTable(source, table, null);
        }

        /// <summary>
        /// Appends elements in the sequence as rows of a given <see cref="DataTable"/> 
        /// object with a set of lambda expressions specifying which members (property
        /// or field) of each element in the sequence will supply the column values.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="expressions">Expressions providing access to element members.</param>
        /// <returns>
        /// A <see cref="DataTable"/> representing the source.
        /// </returns>
        
        public static DataTable ToDataTable<T>(this IEnumerable<T> source, params Expression<Func<T, object>>[] expressions)
        {
            return ToDataTable(source, new DataTable(), expressions);
        }

        /// <summary>
        /// Appends elements in the sequence as rows of a given <see cref="DataTable"/> 
        /// object with a set of lambda expressions specifying which members (property
        /// or field) of each element in the sequence will supply the column values.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TTable">The type of the input and resulting <see cref="DataTable"/> object.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="table">The <see cref="DataTable"/> type of object where to add rows</param>
        /// <param name="expressions">Expressions providing access to element members.</param>
        /// <returns>
        /// A <see cref="DataTable"/> or subclass representing the source.
        /// </returns>
        
        public static TTable ToDataTable<T, TTable>(this IEnumerable<T> source, TTable table, params Expression<Func<T, object>>[] expressions)
            where TTable : DataTable
        {
            if (source == null) throw new ArgumentNullException("source");
            if (table == null) throw new ArgumentNullException("table");

            //
            // If no lambda expressions supplied then reflect them off
            // the source element type.
            //

            if (expressions == null || expressions.Length == 0)
            {
                var parameter = Expression.Parameter(typeof(T), "e");
                var expressionz = from m in typeof(T).GetMembers()
                                  where m.MemberType == MemberTypes.Field
                                     || m.MemberType == MemberTypes.Property
                                  select CreateMemberAccessorLambda<T>(parameter, m);

                expressions = expressionz.ToArray();
            }
            else
            {
                //
                // Ensure none of the expressions is null.
                //

                if (expressions.Any(e => e == null))
                    throw new ArgumentException(null, "expressions");
            }

            //
            // Compile the expressions while also digging out the member 
            // information that may be needed to build the table schema.
            //

            var memberz = from e in expressions
                          let member = GetMemberExpression(e)
                          let type = member.Type
                          select new
                          {
                              member.Member.Name,
                              Type = type.IsGenericType 
                                     && typeof(Nullable<>) == type.GetGenericTypeDefinition() 
                                   ? type.GetGenericArguments()[0] 
                                   : type,
                              GetValue = e.Compile(),
                         };

            var members = memberz.ToArray();

            //
            // If the table has no columns then build the schema.
            // If it has columns, then bind to each based on the
            // member name pulled out of the expressions.
            //

            DataColumn[] columns;
            if (table.Columns.Count == 0)
            {
                columns = members.Select(m => new DataColumn(m.Name, m.Type)).ToArray();
                table.Columns.AddRange(columns);
            }
            else
            {
                columns = members.Select(m => table.Columns[m.Name])
                                 .ToArray();

                for (var i = 0; i < members.Length; i++)
                {
                    if (columns[i] == null)
                        throw new Exception(string.Format("Column named '{0}' is missing.", members[i].Name));
                }
            }

            //
            // Builds rows out of elements in the sequence and
            // add them to the table.
            //

            table.BeginLoadData();

            try
            {
                foreach (var element in source)
                {
                    var row = table.NewRow();
                    for (var i = 0; i < members.Length; i++)
                        row[columns[i]] = members[i].GetValue(element) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
            }
            finally
            {
                table.EndLoadData();
            }

            return table;
        }

        private static UnaryExpression CreateMemberAccessor(Expression parameter, MemberInfo member)
        {
            var access = Expression.MakeMemberAccess(parameter, member);
            return Expression.Convert(access, typeof(object));
        }

        private static Expression<Func<T, object>> CreateMemberAccessorLambda<T>(ParameterExpression parameter, MemberInfo member)
        {
            var accessor = CreateMemberAccessor(parameter, member);
            return Expression.Lambda<Func<T, object>>(accessor, parameter);
        }

        private static MemberExpression GetMemberExpression<T>(Expression<Func<T, object>> e)
        {
            var unary = e.Body as UnaryExpression;
            return (MemberExpression) (unary != null ? unary.Operand : e.Body);
        }
    }
}
