using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MoreLinq.ModelShredder
{	
	/// <summary>
	/// Provides <see cref="ShredderOptions"/> for fields first and then properties according to the order of their definition. 
	/// </summary>
	public static class DefaultShredderOptionsProvider
    {
        /// <summary>
		/// Provides <see cref="ShredderOptions"/> for fields first and then properties according to the order of their definition. 
        /// </summary>
        /// <param name="t">The type for which to provide <see cref="ShredderOptions"/>.</param>
        /// <returns>A <see cref="ShredderOptions"/> instance containing information that controls how the type should be shredderd.</returns>
		public static ShredderOptions ProvideShredderOptions(Type t)
		{
			if (t == null)
				throw new ArgumentNullException("t", "t is null.");
			
			var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetIndexParameters().Length == 0);
			var fields = t.GetFields(BindingFlags.Public | BindingFlags.Instance);

			var members = fields.ToList().Cast<MemberInfo>().Concat(props.Cast<MemberInfo>());

			return new ShredderOptions(t, members.ToList());
		}
    }
}
