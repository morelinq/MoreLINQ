using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MoreLinq.ModelShredder;
using System.Data;

namespace MoreLinq.Test.ModelShredder
{
	[TestFixture]
	public class ShredderTest
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShredderWithNullOptions()
		{
			Shredder<Type> shredder = new Shredder<Type>(null, new InjectionObjectShredder(), new DefaultSchemaBuilder());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShredderWithNullShredderMethodProvider()
		{
			var options = DefaultShredderOptionsProvider.ProvideShredderOptions(typeof(Type));

			Shredder<Type> shredder = new Shredder<Type>(options, null, new DefaultSchemaBuilder());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShredderWithNullSchemaBuilder()
		{
			Shredder<Type> shredder = new Shredder<Type>(DefaultShredderOptionsProvider.ProvideShredderOptions(typeof(Type)), new InjectionObjectShredder(), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShredWithNullSequence()
		{
			Shredder<Type> shredder = new Shredder<Type>(DefaultShredderOptionsProvider.ProvideShredderOptions(typeof(Type)), new InjectionObjectShredder(), new DefaultSchemaBuilder());

			shredder.Shred(null);
		}

		[Test]
		public void ShredReturnsFilledDataTable()
		{
			Shredder<ShredderTestObject> shredder = new Shredder<ShredderTestObject>(DefaultShredderOptionsProvider.ProvideShredderOptions(typeof(ShredderTestObject)), new InjectionObjectShredder(), new DefaultSchemaBuilder());

			List<ShredderTestObject> sequence = new List<ShredderTestObject>();
			sequence.Add(new ShredderTestObject(1));
			sequence.Add(new ShredderTestObject(2));

			DataTable dt = shredder.Shred(sequence);

			Assert.AreEqual(2, dt.Rows.Count);
		}
	}
}
