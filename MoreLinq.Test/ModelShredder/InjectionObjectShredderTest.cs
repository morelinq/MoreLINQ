using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MoreLinq.ModelShredder;

namespace MoreLinq.Test.ModelShredder
{
	[TestFixture]
	public class InjectionObjectShredderTest
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetShredderMethodWithNullOptions()
		{
			InjectionObjectShredder objectShredder = new InjectionObjectShredder();
			objectShredder.GetShredderMethod(null);
		}

		[Test]
		public void GetShredderMethodInjectsValidMSIL()
		{
			ShredderOptions options = DefaultShredderOptionsProvider.ProvideShredderOptions(typeof(ShredderTestObject));

			InjectionObjectShredder objectShredder = new InjectionObjectShredder();
			ShredderDelegate shredderMethod = objectShredder.GetShredderMethod(options);

			ShredderTestObject tobj = new ShredderTestObject(1);

			try
			{
				shredderMethod.Invoke(tobj);
			}
			catch (InvalidProgramException)
			{
				Assert.Fail("Invalid MSIL was generated!");
			}
		}

		[Test]
		public void GetShredderMethodReturnedDelegateShredsObjects()
		{
			ShredderOptions options = DefaultShredderOptionsProvider.ProvideShredderOptions(typeof(ShredderTestObject));

			InjectionObjectShredder objectShredder = new InjectionObjectShredder();
			ShredderDelegate shredderMethod = objectShredder.GetShredderMethod(options);

			ShredderTestObject tobj = new ShredderTestObject(1);

			object[] shredderd = shredderMethod.Invoke(tobj);

			Assert.AreEqual(tobj.KeyField, shredderd[0]);
			Assert.AreEqual(tobj.ANullableGuidField, shredderd[1]);
			Assert.AreEqual(tobj.AString, shredderd[2]);
			Assert.AreEqual(tobj.ANullableDecimal, shredderd[3]);
		}
	}
}
