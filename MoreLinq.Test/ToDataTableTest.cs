using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Data;

namespace MoreLinq.Test
{
	[TestFixture, SetUpFixture]
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
			for (int i = 0; i < 3; i++)
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
			DataTable dt = new DataTable();
			dt.Columns.Add("Test");

			m_TestObjects.ToDataTable(dt);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ToDataTableTableWithWrongColumnDataType()
		{
			DataTable dt = new DataTable();
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
			DataTable dt = m_TestObjects.ToDataTable();

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
			DataTable dt = m_TestObjects.ToDataTable();
			Assert.AreEqual(m_TestObjects.Count, dt.Rows.Count);
		}
		
		
		[Test]
		public void ToDataTableWithExpression()
		{
			DataTable dt = m_TestObjects.ToDataTable(t => t.AString);

			Assert.AreEqual("AString", dt.Columns[0].Caption);
			Assert.AreEqual(typeof(string), dt.Columns[0].DataType);

			Assert.AreEqual(1, dt.Columns.Count);
		}
	}
}
