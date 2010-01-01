using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MoreLinq.ModelShredder;
using System.Reflection;
using System.Data;

namespace MoreLinq.Test.ModelShredder
{
	[TestFixture]
	public class DefaultSchemaBuilderTest
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void BuildTableSchemaWithNullTable()
		{
			DefaultSchemaBuilder builder = new DefaultSchemaBuilder();
			builder.BuildTableSchema(null, new ShredderOptions(typeof(Type), new List<MemberInfo>()));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void BuildTableSchemaWithNullOptions()
		{
			DefaultSchemaBuilder builder = new DefaultSchemaBuilder();
			builder.BuildTableSchema(new DataTable(), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void BuildTableSchemaWithNonEmptyTable()
		{
			DefaultSchemaBuilder builder = new DefaultSchemaBuilder();
			
			DataTable dt = new DataTable();
			dt.Columns.Add("blabla");

			builder.BuildTableSchema(dt, new ShredderOptions(typeof(Type), new List<MemberInfo>()));
		}

		[Test]
		public void BuildTableSchemaGeneratesCorrectColumns()
		{
			DefaultSchemaBuilder builder = new DefaultSchemaBuilder();

			ShredderOptions options = DefaultShredderOptionsProvider.ProvideShredderOptions(typeof(ShredderTestObject));

			DataTable dt = new DataTable();
			builder.BuildTableSchema(dt, options);

			Assert.AreEqual("KeyField", dt.Columns[0].Caption);
			Assert.AreEqual(typeof(int), dt.Columns[0].DataType);
			
			Assert.AreEqual("ANullableGuidField", dt.Columns[1].Caption);
			Assert.AreEqual(typeof(Guid), dt.Columns[1].DataType);
			Assert.IsTrue(dt.Columns[1].AllowDBNull);

			Assert.AreEqual("AString", dt.Columns[2].Caption);
			Assert.AreEqual(typeof(string), dt.Columns[2].DataType);

			Assert.AreEqual("ANullableDecimal", dt.Columns[3].Caption);
			Assert.AreEqual(typeof(decimal), dt.Columns[3].DataType);
			Assert.IsTrue(dt.Columns[3].AllowDBNull);
			
			Assert.AreEqual(4, dt.Columns.Count);
		}

	}
}
