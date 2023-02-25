#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

// This code was generated by a tool. Any changes made manually will be lost
// the next time this code is regenerated.

#nullable enable // required for auto-generated sources (see below why)

// > Older code generation strategies may not be nullable aware. Setting the
// > project-level nullable context to "enable" could result in many
// > warnings that a user is unable to fix. To support this scenario any syntax
// > tree that is determined to be generated will have its nullable state
// > implicitly set to "disable", regardless of the overall project state.
//
// Source: https://github.com/dotnet/roslyn/blob/70e158ba6c2c99bd3c3fc0754af0dbf82a6d353d/docs/features/nullable-reference-types.md#generated-code

namespace MoreLinq.Extensions
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Data;
    using System.Linq.Expressions;

    /// <summary><c>ToDataTable</c> extension.</summary>

    [GeneratedCode("MoreLinq.ExtensionsGenerator", "1.0.0.0")]
    public static partial class ToDataTableExtension
    {

        /// <summary>
        /// Converts a sequence to a <see cref="DataTable"/> object.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>
        /// A <see cref="DataTable"/> representing the source.
        /// </returns>
        /// <remarks>This operator uses immediate execution.</remarks>

        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
            => MoreEnumerable.ToDataTable(source);

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
        /// <remarks>This operator uses immediate execution.</remarks>

        public static DataTable ToDataTable<T>(this IEnumerable<T> source, params Expression<Func<T, object?>>[] expressions)
            => MoreEnumerable.ToDataTable(source, expressions);
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
        /// <remarks>This operator uses immediate execution.</remarks>

        public static TTable ToDataTable<T, TTable>(this IEnumerable<T> source, TTable table)
            where TTable : DataTable
            => MoreEnumerable.ToDataTable(source, table);

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
        /// <remarks>This operator uses immediate execution.</remarks>

        public static TTable ToDataTable<T, TTable>(this IEnumerable<T> source, TTable table, params Expression<Func<T, object?>>[] expressions)
            where TTable : DataTable
            => MoreEnumerable.ToDataTable(source, table, expressions);

    }
}
