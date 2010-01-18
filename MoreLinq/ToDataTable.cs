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
        private static MemberInfo GetAccessedMember(LambdaExpression lambda)
        {
            Expression body = lambda.Body;

            // If it's a field access, boxing was used, we need the field
            if ((body.NodeType == ExpressionType.Convert) || (body.NodeType == ExpressionType.ConvertChecked))
            {
                body = ((UnaryExpression)body).Operand;
            }

            // Check if the MemberExpression is valid and is a "first level" member access e.g. not a.b.c 
            MemberExpression memberExpression = body as MemberExpression;
            if ((memberExpression == null) || (memberExpression.Expression.NodeType != ExpressionType.Parameter))
            {
                return null;
            }

            return memberExpression.Member;
        }

        private static MemberInfo[] PrepareMemberInfos<T>(Expression<Func<T, object>>[] expressions)
        {
            // If no lambda expressions supplied then reflect them off the source element type.

            MemberInfo[] members = null;

            if (expressions == null || expressions.Length == 0)
            {
                var memberz = from m in typeof(T).GetMembers()
                              where m.MemberType == MemberTypes.Field
                                 || (m.MemberType == MemberTypes.Property && ((PropertyInfo) m).GetIndexParameters().Length == 0)
                              select m;

                members = memberz.ToArray();
            }
            else
            {
                //
                // Ensure none of the expressions is null, a non-MemberExpression or accesses an invalid Member
                //

                if (expressions.Any(e => e == null))
                    throw new ArgumentException("contains null expression", "expressions");

                members = expressions.Select(x => GetAccessedMember(x)).ToArray();

                if (members.Any(m => m == null))
                    throw new ArgumentException("contains an invalid member expression", "expressions");
            }
            return members;
        }

        private static void PrepareDataTable(DataTable table, MemberInfo[] members)
        {
            //
            // retrieve member information
            // needed to build or validate the table schema.
            //
            var columnInfos = from m in members
                              let type = m.MemberType == MemberTypes.Property ? ((PropertyInfo)m).PropertyType : ((FieldInfo)m).FieldType
                              select new
                              {
                                  m.Name,
                                  Type = type.IsGenericType
                                         && typeof(Nullable<>) == type.GetGenericTypeDefinition()
                                       ? type.GetGenericArguments()[0]
                                       : type,
                              };


            //
            // If the table has no columns then build the schema.
            // If it has columns, validate they are correctly ordered
            // and of correct datatype. The table can have columns left at the end.
            //

            if (table.Columns.Count == 0)
            {
                var columns = columnInfos.Select(m => new DataColumn(m.Name, m.Type)).ToArray();
                table.Columns.AddRange(columns);
            }
            else
            {
                var expectedColumns = columnInfos.ToArray();

                for (var i = 0; i < expectedColumns.Length; i++)
                {
                    if (table.Columns[i].ColumnName != expectedColumns[i].Name)
                        throw new ArgumentException(string.Format("Column named '{0}' is missing or the table has unexpected column ordering.", members[i].Name), "table");

                    if (table.Columns[i].DataType != expectedColumns[i].Type)
                        throw new ArgumentException(string.Format("Column named '{0}' has wrong datatype.", members[i].Name), "table");

                    // can have more columns at the end
                }
            }
        }

        private static UnaryExpression CreateMemberAccessor(Expression parameter, MemberInfo member)
        {
            var access = Expression.MakeMemberAccess(parameter, member);
            return Expression.Convert(access, typeof(object));
        }

        private static Func<T, object[]> CreateShredder<T>(IEnumerable<MemberInfo> members)
        {
            var parameter = Expression.Parameter(typeof(T), "e");

            var initializers = members.Select(m => CreateMemberAccessor(parameter, m))
                                      .Cast<Expression>();

            var array = Expression.NewArrayInit(typeof(object), initializers);

            var lambda = Expression.Lambda<Func<T, object[]>>(array, parameter);

            return lambda.Compile();
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



            MemberInfo[] members = PrepareMemberInfos<T>(expressions);

            PrepareDataTable(table, members);

            var shredder = CreateShredder<T>(members);

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
                    row.ItemArray = shredder(element);
                    table.Rows.Add(row);
                }
            }
            finally
            {
                table.EndLoadData();
            }

            return table;
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
    }
}
