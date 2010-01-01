using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using MoreLinq.ModelShredder;

namespace MoreLinq.Test.ModelShredder
{	
	[TestFixture]
	public class DefaultShredderOptionsProviderTest
    {
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ProvideShredderOptionsWithTypeNull()
		{
			DefaultShredderOptionsProvider.ProvideShredderOptions(null);
		}

		[Test]
		public void ProvideShredderOptionsContainsAllFieldsAndPropertiesExceptIndexers()
		{
			ShredderOptions options = DefaultShredderOptionsProvider.ProvideShredderOptions(typeof(ShredderTestObject));

			Assert.AreEqual("KeyField", options.Members[0].Name);
			Assert.AreEqual("ANullableGuidField", options.Members[1].Name);
			Assert.AreEqual("AString", options.Members[2].Name);
			Assert.AreEqual("ANullableDecimal", options.Members[3].Name);
			Assert.AreEqual(4, options.Members.Count);
		}
    }
}
