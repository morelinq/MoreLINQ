#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
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
    using System.Linq.Expressions;
    using NUnit.Framework;

    [TestFixture]
    public class ToDataTableTest
    {
        class TestObject
        {
            public int KeyField;
            public Guid? ANullableGuidField;

            public string AString { get; }
            public decimal? ANullableDecimal { get; }
            public object Unreadable { set => throw new NotImplementedException(); }

            public object this[int index]
            {
                get => new object();
                set { }
            }


            public TestObject(int key)
            {
                KeyField = key;
                ANullableGuidField = Guid.NewGuid();

                ANullableDecimal = key / 3;
                AString = "ABCDEFGHIKKLMNOPQRSTUVWXYSZ";
            }
        }


        readonly IReadOnlyCollection<TestObject> _testObjects;


        public ToDataTableTest()
        {
            _testObjects = Enumerable.Range(0, 3)
                                     .Select(i => new TestObject(i))
                                     .ToArray();
        }

        [Test]
        public void ToDataTableNullMemberExpressionMethod()
        {
            Expression<Func<TestObject, object>> expression = null;

            AssertThrowsArgument.Exception("expressions",() =>
                _testObjects.ToDataTable<TestObject>(expression));
        }

        [Test]
        public void ToDataTableTableWithWrongColumnNames()
        {
            var dt = new DataTable();
            dt.Columns.Add("Test");

            AssertThrowsArgument.Exception("table",() =>
                _testObjects.ToDataTable(dt));
        }

        [Test]
        public void ToDataTableTableWithWrongColumnDataType()
        {
            var dt = new DataTable();
            dt.Columns.Add("AString", typeof(int));

            AssertThrowsArgument.Exception("table",() =>
                _testObjects.ToDataTable(dt, t=>t.AString));
        }

        [Test]
        public void ToDataTableMemberExpressionMethod()
        {
            AssertThrowsArgument.Exception("lambda", () =>
                _testObjects.ToDataTable(t => t.ToString()));
        }


        [Test]
        public void ToDataTableMemberExpressionNonMember()
        {
            AssertThrowsArgument.Exception("lambda", () =>
                _testObjects.ToDataTable(t => t.ToString().Length));
        }

        [Test]
        public void ToDataTableMemberExpressionIndexer()
        {
            AssertThrowsArgument.Exception("lambda",() =>
                _testObjects.ToDataTable(t => t[0]));
        }

        [Test]
        public void ToDataTableSchemaInDeclarationOrder()
        {
            var dt = _testObjects.ToDataTable();

            // Assert properties first, then fields, then in declaration order

            Assert.AreEqual("AString", dt.Columns[0].Caption);
            Assert.AreEqual(typeof(string), dt.Columns[0].DataType);

            Assert.AreEqual("ANullableDecimal", dt.Columns[1].Caption);
            Assert.AreEqual(typeof(decimal), dt.Columns[1].DataType);

            Assert.AreEqual("KeyField", dt.Columns[2].Caption);
            Assert.AreEqual(typeof(int), dt.Columns[2].DataType);

            Assert.AreEqual("ANullableGuidField", dt.Columns[3].Caption);
            Assert.AreEqual(typeof(Guid), dt.Columns[3].DataType);
            Assert.IsTrue(dt.Columns[3].AllowDBNull);

            Assert.AreEqual(4, dt.Columns.Count);
        }

        [Test]
        public void ToDataTableContainsAllElements()
        {
            var dt = _testObjects.ToDataTable();
            Assert.AreEqual(_testObjects.Count, dt.Rows.Count);
        }

        [Test]
        public void ToDataTableWithExpression()
        {
            var dt = _testObjects.ToDataTable(t => t.AString);

            Assert.AreEqual("AString", dt.Columns[0].Caption);
            Assert.AreEqual(typeof(string), dt.Columns[0].DataType);

            Assert.AreEqual(1, dt.Columns.Count);
        }

        [Test]
        public void ToDataTableWithSchema()
        {
            var dt = new DataTable();
            var columns = dt.Columns;
            columns.Add("Column1", typeof(int));
            columns.Add("Value", typeof(string));
            columns.Add("Column3", typeof(int));
            columns.Add("Name", typeof(string));

            var vars = Environment.GetEnvironmentVariables()
                                  .Cast<DictionaryEntry>()
                                  .ToArray();

            vars.Select(e => new { Name = e.Key.ToString(), Value = e.Value.ToString() })
                .ToDataTable(dt, e => e.Name, e => e.Value);

            var rows = dt.Rows.Cast<DataRow>().ToArray();
            Assert.That(rows.Length, Is.EqualTo(vars.Length));
            Assert.That(rows.Select(r => r["Name"]).ToArray(), Is.EqualTo(vars.Select(e => e.Key).ToArray()));
            Assert.That(rows.Select(r => r["Value"]).ToArray(), Is.EqualTo(vars.Select(e => e.Value).ToArray()));
        }

        struct Point
        {
            public static Point Empty = new Point();
            public bool IsEmpty => X == 0 && Y == 0;
            public int X { get; }
            public int Y { get; }
            public Point(int x, int y) : this() { X = x; Y = y; }
        }

        [Test]
        public void ToDataTableIgnoresStaticMembers()
        {
            var points = new[] { new Point(12, 34) }.ToDataTable();

            Assert.AreEqual(3, points.Columns.Count);
            DataColumn x, y, empty;
            Assert.NotNull(x = points.Columns["X"]);
            Assert.NotNull(y = points.Columns["Y"]);
            Assert.NotNull(empty = points.Columns["IsEmpty"]);
            var row = points.Rows.Cast<DataRow>().Single();
            Assert.AreEqual(12, row[x]);
            Assert.AreEqual(34, row[y]);
            Assert.AreEqual(false, row[empty]);
        }
    }
}
