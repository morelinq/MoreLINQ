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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class ToDataTableTest
    {
        private class TestObject
        {
            public int KeyField;
            public Nullable<Guid> ANullableGuidField;

            public string AString { get; set; }
            public Nullable<decimal> ANullableDecimal { get; set; }

            public object this[int index]
            {
                get
                {
                    return new object();
                }
                set
                { }
            }


            public TestObject(int key)
            {
                KeyField = key;
                ANullableGuidField = Guid.NewGuid();

                ANullableDecimal = key / 3;
                AString = "ABCDEFGHIKKLMNOPQRSTUVWXYSZ";
            }
        }


        private IList<TestObject> m_TestObjects;


        public ToDataTableTest()
        {
            m_TestObjects = new List<TestObject>();
            for (var i = 0; i < 3; i++)
            {
                m_TestObjects.Add(new TestObject(i));
            }
        }


        [Test]
        public void ToDataTableNullSequence()
        {
            IEnumerable<TestObject> source = null;

            Assert.ThrowsArgumentNullException("source",() =>
                source.ToDataTable());
        }

        [Test]
        public void ToDataTableNullTable()
        {
            DataTable dt = null;

            Assert.ThrowsArgumentNullException("table",() =>
                m_TestObjects.ToDataTable(dt));
        }

        [Test]
        public void ToDataTableNullMemberExpressionMethod()
        {
            Expression<Func<TestObject, object>> expression = null;

            Assert.ThrowsArgumentException("expressions",() =>
                m_TestObjects.ToDataTable<TestObject>(expression));
        }

        [Test]
        public void ToDataTableTableWithWrongColumnNames()
        {
            var dt = new DataTable();
            dt.Columns.Add("Test");

            Assert.ThrowsArgumentException("table",() =>
                m_TestObjects.ToDataTable(dt));
        }

        [Test]
        public void ToDataTableTableWithWrongColumnDataType()
        {
            var dt = new DataTable();
            dt.Columns.Add("AString", typeof(int));

            Assert.ThrowsArgumentException("table",() =>
                m_TestObjects.ToDataTable(dt, t=>t.AString));
        }

        [Test]
        public void ToDataTableMemberExpressionMethod()
        {
            Assert.ThrowsArgumentException("lambda", () =>
                m_TestObjects.ToDataTable(t => t.ToString()));
        }


        [Test]
        public void ToDataTableMemberExpressionNonMember()
        {
            Assert.ThrowsArgumentException("lambda", () =>
                m_TestObjects.ToDataTable(t => t.ToString().Length));
        }

        [Test]
        public void ToDataTableMemberExpressionIndexer()
        {
            Assert.ThrowsArgumentException("lambda",() =>
                m_TestObjects.ToDataTable(t => t[0]));
        }

        [Test]
        public void ToDataTableSchemaInDeclarationOrder()
        {
            var dt = m_TestObjects.ToDataTable();

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
            var dt = m_TestObjects.ToDataTable();
            Assert.AreEqual(m_TestObjects.Count, dt.Rows.Count);
        }

        [Test]
        public void ToDataTableWithExpression()
        {
            var dt = m_TestObjects.ToDataTable(t => t.AString);

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

            var rows = dt.AsEnumerable().ToArray();
            Assert.That(rows.Length, Is.EqualTo(vars.Length));
            Assert.That(rows.Select(r => r["Name"]).ToArray(), Is.EqualTo(vars.Select(e => e.Key).ToArray()));
            Assert.That(rows.Select(r => r["Value"]).ToArray(), Is.EqualTo(vars.Select(e => e.Value).ToArray()));
        }

        struct Point
        {
            public static Point Empty = new Point();
            public bool IsEmpty { get { return X == 0 && Y == 0; } }
            public int X { get; set; }
            public int Y { get; set; }
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
