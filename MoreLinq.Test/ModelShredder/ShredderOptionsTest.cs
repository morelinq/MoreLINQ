using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MoreLinq.ModelShredder;
using System.Reflection;

namespace MoreLinq.Test.ModelShredder
{
	[TestFixture]
	public class ShredderOptionsTest
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void OptionsWithNullType()
		{
			ShredderOptions options = new ShredderOptions(null, new List<MemberInfo>());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void OptionsWithNullMembers()
		{
			ShredderOptions options = new ShredderOptions(typeof(Type), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void OptionsWithMembersNotPropertyOrFieldInfo()
		{
			IList<MemberInfo> members = new List<MemberInfo>();
			members.Add(typeof(Type).GetMethods().First());

			ShredderOptions options = new ShredderOptions(typeof(Type), members);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void OptionsWithMembersContainingIndexerProperty()
		{
			IList<MemberInfo> members = new List<MemberInfo>();
			members.Add(typeof(string).GetProperties().Where(p=>p.GetIndexParameters().Length > 0).First());

			ShredderOptions options = new ShredderOptions(typeof(string), members);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void OptionsWithMembersOfOtherType()
		{
			IList<MemberInfo> members = new List<MemberInfo>(typeof(Type).GetProperties());

			ShredderOptions options = new ShredderOptions(typeof(string), members);
		}
	}
}
