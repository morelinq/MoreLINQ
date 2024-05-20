#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2024 Atif Aziz. All rights reserved.
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

namespace MoreLinq.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;

    [TestClass]
    public class ToDataTableTest
    {
        sealed class TestObject(int key)
        {
            public int KeyField = key;
            public Guid? ANullableGuidField = Guid.NewGuid();

            public string AString { get; } = "ABCDEFGHIKKLMNOPQRSTUVWXYSZ";
            public decimal? ANullableDecimal { get; } = key / 3;
            public object Unreadable { set => throw new NotImplementedException(); }

            public object this[int index] { get => new(); set { } }

            public override string ToString() => nameof(TestObject);
        }

        readonly IReadOnlyCollection<TestObject> testObjects;

        public ToDataTableTest() =>
            this.testObjects = Enumerable.Range(0, 3)
                                         .Select(i => new TestObject(i))
                                         .ToArray();

        [TestMethod]
        public void ToDataTableNullMemberExpressionMethod()
        {
            Expression<Func<TestObject, object?>>? expression = null;

            [UnconditionalSuppressMessage("Aot", "IL2026")]
            void Act() => _ = this.testObjects.ToDataTable(expression!);

            var e = Assert.ThrowsException<ArgumentException>(Act);
            Assert.AreEqual("expressions", e.ParamName);
        }

        [TestMethod]
        public void ToDataTableTableWithWrongColumnNames()
        {
            using var dt = new DataTable();
            _ = dt.Columns.Add("Test");

            [UnconditionalSuppressMessage("Aot", "IL2026")]
            void Act() => _ = this.testObjects.ToDataTable(dt);

            var e = Assert.ThrowsException<ArgumentException>(Act);
            Assert.AreEqual("table", e.ParamName);
        }

        [TestMethod]
        public void ToDataTableTableWithWrongColumnDataType()
        {
            using var dt = new DataTable();
            _ = dt.Columns.Add("AString", typeof(int));

            [UnconditionalSuppressMessage("Aot", "IL2026")]
            void Act() => _ = this.testObjects.ToDataTable(dt, t => t.AString);

            var e = Assert.ThrowsException<ArgumentException>(Act);
            Assert.AreEqual("table", e.ParamName);
        }

        void TestDataTableMemberExpression(Expression<Func<TestObject, object?>> expression)
        {
            [UnconditionalSuppressMessage("Aot", "IL2026")]
            void Act() => _ = this.testObjects.ToDataTable(expression);

            var e = Assert.ThrowsException<ArgumentException>(Act);
            Assert.AreEqual("expressions", e.ParamName);
            var innerException = e.InnerException;
            Assert.IsNotNull(innerException);
            Assert.IsInstanceOfType<ArgumentException>(innerException);
            Assert.AreEqual("lambda", ((ArgumentException)innerException).ParamName);
        }

        [TestMethod]
        public void ToDataTableMemberExpressionMethod()
        {
            TestDataTableMemberExpression(t => t.ToString());
        }

        [TestMethod]
        public void ToDataTableMemberExpressionNonMember()
        {
            TestDataTableMemberExpression(t => t.ToString().Length);
        }

        [TestMethod]
        public void ToDataTableMemberExpressionIndexer()
        {
            TestDataTableMemberExpression(t => t[0]);
        }

        [TestMethod]
        public void ToDataTableMemberExpressionStatic()
        {
            TestDataTableMemberExpression(_ => DateTime.Now);
        }

        [TestMethod]
        public void ToDataTableSchemaInDeclarationOrder()
        {
            [UnconditionalSuppressMessage("Aot", "IL2026")]
            DataTable Act() => this.testObjects.ToDataTable();

            var dt = Act();

            // Assert properties first, then fields, then in declaration order

            Assert.AreEqual("KeyField", dt.Columns[2].Caption);
            Assert.AreEqual(typeof(int), dt.Columns[2].DataType);

            Assert.AreEqual("ANullableGuidField", dt.Columns[3].Caption);
            Assert.AreEqual(typeof(Guid), dt.Columns[3].DataType);
            Assert.IsTrue(dt.Columns[3].AllowDBNull);

            Assert.AreEqual("AString", dt.Columns[0].Caption);
            Assert.AreEqual(typeof(string), dt.Columns[0].DataType);

            Assert.AreEqual("ANullableDecimal", dt.Columns[1].Caption);
            Assert.AreEqual(typeof(decimal), dt.Columns[1].DataType);

            Assert.AreEqual(4, dt.Columns.Count);
        }

        [TestMethod]
        public void ToDataTableContainsAllElements()
        {
            [UnconditionalSuppressMessage("Aot", "IL2026")]
            DataTable Act() => this.testObjects.ToDataTable();

            var dt = Act();

            Assert.AreEqual(this.testObjects.Count, dt.Rows.Count);
        }

        [TestMethod]
        public void ToDataTableWithExpression()
        {
            [UnconditionalSuppressMessage("Aot", "IL2026")]
            DataTable Act() => this.testObjects.ToDataTable(t => t.AString);

            var dt = Act();

            Assert.AreEqual("AString", dt.Columns[0].Caption);
            Assert.AreEqual(typeof(string), dt.Columns[0].DataType);

            Assert.AreEqual(1, dt.Columns.Count);
        }

        [TestMethod]
        public void ToDataTableWithSchema()
        {
            using var dt = new DataTable();
            var columns = dt.Columns;
            _ = columns.Add("Column1", typeof(int));
            _ = columns.Add("Value", typeof(string));
            _ = columns.Add("Column3", typeof(int));
            _ = columns.Add("Name", typeof(string));

            var vars = Environment.GetEnvironmentVariables()
                                  .Cast<DictionaryEntry>()
                                  .ToArray();

            [UnconditionalSuppressMessage("Aot", "IL2026")]
            void Act() => _ = vars.Select(e => new { Name = e.Key.ToString(), Value = e.Value!.ToString() })
                                  .ToDataTable(dt, e => e.Name, e => e.Value);

            Act();

            var rows = dt.Rows.Cast<DataRow>().ToArray();
            Assert.AreEqual(vars.Length, rows.Length);
            CollectionAssert.AreEqual(vars.Select(e => e.Key).ToArray(), rows.Select(r => r["Name"]).ToArray());
            CollectionAssert.AreEqual(vars.Select(e => e.Value).ToArray(), rows.Select(r => r["Value"]).ToArray());
        }

        readonly struct Point(int x, int y)
        {
#pragma warning disable CA1805 // Do not initialize unnecessarily (avoids CS0649)
            public static Point Empty = new();
#pragma warning restore CA1805 // Do not initialize unnecessarily
            public bool IsEmpty => X == 0 && Y == 0;
            public int X { get; } = x;
            public int Y { get; } = y;
        }

        [TestMethod]
        public void ToDataTableIgnoresStaticMembers()
        {
            [UnconditionalSuppressMessage("Aot", "IL2026")]
            static DataTable Act() => new[] { new Point(12, 34) }.ToDataTable();

            var points = Act();

            Assert.AreEqual(3, points.Columns.Count);
            var x = points.Columns["X"];
            var y = points.Columns["Y"];
            var empty = points.Columns["IsEmpty"];
            Assert.IsNotNull(x);
            Assert.IsNotNull(y);
            Assert.IsNotNull(empty);
            var row = points.Rows.Cast<DataRow>().Single();
            Assert.AreEqual(12, row[x]);
            Assert.AreEqual(34, row[y]);
            Assert.AreEqual(row[empty], false);
        }
    }
}
