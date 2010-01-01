using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace MoreLinq.ModelShredder
{
	/// <summary>
	/// Provides information about how a type should be translated to a <see cref="System.Data.DataTable"/>.
	/// </summary>
    public sealed class ShredderOptions
    {
		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
        public Type Type { get; private set; }
		/// <summary>
		/// Gets the members to include during translation.
		/// </summary>
		/// <value>The members.</value>
        public ReadOnlyCollection<MemberInfo> Members { get; private set; }


		/// <summary>
		/// Initializes a new instance of the <see cref="ShredderOptions"/> class.
		/// </summary>
		/// <param name="type">The type to provide options for.</param>
		/// <param name="members">The members the members to include during translation.</param>
        public ShredderOptions(Type type, IList<MemberInfo> members)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (members == null) throw new ArgumentNullException("members");
            
            foreach (MemberInfo member in members)
            {
				if (member.ReflectedType != type)
					throw new ArgumentException("Members do not match type.", "members");

                if (member.MemberType != MemberTypes.Property && member.MemberType != MemberTypes.Field)
                    throw new ArgumentException("May only contan Field and Property Infos.", "members");

				PropertyInfo pi = member as PropertyInfo;
				if (pi != null)
				{
					if (pi.GetIndexParameters().Length > 0 )
						throw new ArgumentException("May not contain indexer properties.", "members");
				}
            }

            Type = type;
            Members = new ReadOnlyCollection<MemberInfo>(members);
        }
    }
}