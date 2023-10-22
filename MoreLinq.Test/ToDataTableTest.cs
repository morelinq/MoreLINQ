#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2010 Johannes Rudolph. All rights reserved.
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
        sealed class TestObject
        {
            public int KeyField;
            public Guid? ANullableGuidField;

            public string AString { get; }
            public decimal? ANullableDecimal { get; }
            public object Unreadable { set => throw new NotImplementedException(); }

            public object this[int index] { get => new(); set { } }

            public TestObject(int key)
            {
                KeyField = key;
                ANullableGuidField = Guid.NewGuid();

                ANullableDecimal = key / 3;
                AString = "ABCDEFGHIKKLMNOPQRSTUVWXYSZ";
            }

            public override string ToString() => nameof(TestObject);
        }


        readonly IReadOnlyCollection<TestObject> _testObjects;


        public ToDataTableTest() =>
            _testObjects = Enumerable.Range(0, 3)
                                     .Select(i => new TestObject(i))
                                     .ToArray();

        [Test]
        public void ToDataTableNullMemberExpressionMethod()
        {
            Expression<Func<TestObject, object?>>? expression = null;

            Assert.That(() => _testObjects.ToDataTable(expression!),
                        Throws.ArgumentException("expressions"));
        }

        [Test]
        public void ToDataTableTableWithWrongColumnNames()
        {
            using var dt = new DataTable();
            _ = dt.Columns.Add("Test");

            Assert.That(() => _testObjects.ToDataTable(dt),
                        Throws.ArgumentException("table"));
        }

        [Test]
        public void ToDataTableTableWithWrongColumnDataType()
        {
            using var dt = new DataTable();
            _ = dt.Columns.Add("AString", typeof(int));

            Assert.That(() => _testObjects.ToDataTable(dt, t => t.AString),
                        Throws.ArgumentException("table"));
        }

        [Test]
        public void ToDataTableMemberExpressionMethod()
        {
            Assert.That(() => _testObjects.ToDataTable(t => t.ToString()),
                        Throws.ArgumentException("lambda"));
        }


        [Test]
        public void ToDataTableMemberExpressionNonMember()
        {
            Assert.That(() => _testObjects.ToDataTable(t => t.ToString().Length),
                        Throws.ArgumentException("lambda"));
        }

        [Test]
        public void ToDataTableMemberExpressionIndexer()
        {
            Assert.That(() => _testObjects.ToDataTable(t => t[0]),
                        Throws.ArgumentException("lambda"));
        }

        [Test]
        public void ToDataTableMemberExpressionStatic()
        {
            Assert.That(() => _ = _testObjects.ToDataTable(_ => DateTime.Now),
                        Throws.ArgumentException("lambda"));
        }

        [Test]
        public void ToDataTableSchemaInDeclarationOrder()
        {
            var dt = _testObjects.ToDataTable();

            // Assert properties first, then fields, then in declaration order

            Assert.That(dt.Columns[0].Caption, Is.EqualTo("AString"));
            Assert.That(dt.Columns[0].DataType, Is.EqualTo(typeof(string)));

            Assert.That(dt.Columns[1].Caption, Is.EqualTo("ANullableDecimal"));
            Assert.That(dt.Columns[1].DataType, Is.EqualTo(typeof(decimal)));

            Assert.That(dt.Columns[2].Caption, Is.EqualTo("KeyField"));
            Assert.That(dt.Columns[2].DataType, Is.EqualTo(typeof(int)));

            Assert.That(dt.Columns[3].Caption, Is.EqualTo("ANullableGuidField"));
            Assert.That(dt.Columns[3].DataType, Is.EqualTo(typeof(Guid)));
            Assert.That(dt.Columns[3].AllowDBNull, Is.True);

            Assert.That(dt.Columns.Count, Is.EqualTo(4));
        }

        [Test]
        public void ToDataTableContainsAllElements()
        {
            var dt = _testObjects.ToDataTable();
            Assert.That(dt.Rows.Count, Is.EqualTo(_testObjects.Count));
        }

        [Test]
        public void ToDataTableWithExpression()
        {
            var dt = _testObjects.ToDataTable(t => t.AString);

            Assert.That(dt.Columns[0].Caption, Is.EqualTo("AString"));
            Assert.That(dt.Columns[0].DataType, Is.EqualTo(typeof(string)));

            Assert.That(dt.Columns.Count, Is.EqualTo(1));
        }

        [Test]
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

            _ = vars.Select(e => new { Name = e.Key.ToString(), Value = e.Value!.ToString() })
                    .ToDataTable(dt, e => e.Name, e => e.Value);

            var rows = dt.Rows.Cast<DataRow>().ToArray();
            Assert.That(rows.Length, Is.EqualTo(vars.Length));
            Assert.That(rows.Select(r => r["Name"]).ToArray(), Is.EqualTo(vars.Select(e => e.Key).ToArray()));
            Assert.That(rows.Select(r => r["Value"]).ToArray(), Is.EqualTo(vars.Select(e => e.Value).ToArray()));
        }

        readonly struct Point
        {

#pragma warning disable CA1805 // Do not initialize unnecessarily (avoids CS0649)
            public static Point Empty = new();
#pragma warning restore CA1805 // Do not initialize unnecessarily
            public bool IsEmpty => X == 0 && Y == 0;
            public int X { get; }
            public int Y { get; }
            public Point(int x, int y) : this() { X = x; Y = y; }
        }

        [Test]
        public void ToDataTableIgnoresStaticMembers()
        {
            var points = new[] { new Point(12, 34) }.ToDataTable();

            Assert.That(points.Columns.Count, Is.EqualTo(3));
            var x = points.Columns["X"];
            var y = points.Columns["Y"];
            var empty = points.Columns["IsEmpty"];
            Assert.That(x, Is.Not.Null);
            Assert.That(y, Is.Not.Null);
            Assert.That(empty, Is.Not.Null);
            var row = points.Rows.Cast<DataRow>().Single();
            Assert.That(row[x], Is.EqualTo(12));
            Assert.That(row[y], Is.EqualTo(34));
            Assert.That(row[empty], Is.False);
        }
    }
}
