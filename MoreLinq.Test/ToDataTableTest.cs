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
using NUnit.Framework.SyntaxHelpers;

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToDataTableNullSequence()
        {
            IEnumerable<TestObject> source = null;
            source.ToDataTable();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToDataTableNullTable()
        { 
            DataTable dt = null;
            m_TestObjects.ToDataTable(dt);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ToDataTableNullMemberExpressionMethod()
        {
            Expression<Func<TestObject, object>> expression = null;

            m_TestObjects.ToDataTable<TestObject>(expression);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ToDataTableTableWithWrongColumnNames()
        {
            var dt = new DataTable();
            dt.Columns.Add("Test");

            m_TestObjects.ToDataTable(dt);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ToDataTableTableWithWrongColumnDataType()
        {
            var dt = new DataTable();
            dt.Columns.Add("AString", typeof(int));

            m_TestObjects.ToDataTable(dt, t=>t.AString);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ToDataTableMemberExpressionMethod()
        {
            m_TestObjects.ToDataTable(t => t.ToString());
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ToDataTableMemberExpressionNonMember()
        {
            m_TestObjects.ToDataTable(t => t.ToString().Length);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ToDataTableMemberExpressionIndexer()
        {
            m_TestObjects.ToDataTable(t => t[0]);
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
    }
}
