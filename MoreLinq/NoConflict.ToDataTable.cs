namespace MoreLinq.NoConflict
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;

    /// <summary><c>ToDataTable</c> extension.</summary>
    public static partial class ToDataTableExtension
    {
        /// <summary>
        /// Appends elements in the sequence as rows of a given <see cref = "DataTable"/> 
        /// object with a set of lambda expressions specifying which members (property
        /// or field) of each element in the sequence will supply the column values.
        /// </summary>
        /// <typeparam name = "T">The type of the elements of <paramref name = "source"/>.</typeparam>
        /// <typeparam name = "TTable">The type of the input and resulting <see cref = "DataTable"/> object.</typeparam>
        /// <param name = "source">The source.</param>
        /// <param name = "table">The <see cref = "DataTable"/> type of object where to add rows</param>
        /// <param name = "expressions">Expressions providing access to element members.</param>
        /// <returns>
        /// A <see cref = "DataTable"/> or subclass representing the source.
        /// </returns>
        /// <remarks>This operator uses immediate execution.</remarks>
        public static TTable ToDataTable<T, TTable>(this IEnumerable<T> source, TTable table, params Expression<Func<T, object>>[] expressions)where TTable : DataTable
        {
            return MoreEnumerable.ToDataTable(source, table, expressions);
        }
    }

    /// <summary><c>ToDataTable</c> extension.</summary>
    public static partial class ToDataTableExtension
    {
        /// <summary>
        /// Appends elements in the sequence as rows of a given <see cref = "DataTable"/> object.
        /// </summary>
        /// <typeparam name = "T">The type of the elements of <paramref name = "source"/>.</typeparam>
        /// <typeparam name = "TTable"></typeparam>
        /// <param name = "source">The source.</param>
        /// <param name = "table"></param>
        /// <returns>
        /// A <see cref = "DataTable"/> or subclass representing the source.
        /// </returns>
        /// <remarks>This operator uses immediate execution.</remarks>
        public static TTable ToDataTable<T, TTable>(this IEnumerable<T> source, TTable table)where TTable : DataTable
        {
            return MoreEnumerable.ToDataTable(source, table);
        }
    }

    /// <summary><c>ToDataTable</c> extension.</summary>
    public static partial class ToDataTableExtension
    {
        /// <summary>
        /// Appends elements in the sequence as rows of a given <see cref = "DataTable"/> 
        /// object with a set of lambda expressions specifying which members (property
        /// or field) of each element in the sequence will supply the column values.
        /// </summary>
        /// <typeparam name = "T">The type of the elements of <paramref name = "source"/>.</typeparam>
        /// <param name = "source">The source.</param>
        /// <param name = "expressions">Expressions providing access to element members.</param>
        /// <returns>
        /// A <see cref = "DataTable"/> representing the source.
        /// </returns>
        /// <remarks>This operator uses immediate execution.</remarks>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source, params Expression<Func<T, object>>[] expressions)
        {
            return MoreEnumerable.ToDataTable(source, expressions);
        }
    }

    /// <summary><c>ToDataTable</c> extension.</summary>
    public static partial class ToDataTableExtension
    {
        /// <summary>
        /// Converts a sequence to a <see cref = "DataTable"/> object.
        /// </summary>
        /// <typeparam name = "T">The type of the elements of <paramref name = "source"/>.</typeparam>
        /// <param name = "source">The source.</param>
        /// <returns>
        /// A <see cref = "DataTable"/> representing the source.
        /// </returns>
        /// <remarks>This operator uses immediate execution.</remarks>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            return MoreEnumerable.ToDataTable(source);
        }
    }
}
