using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace MoreLinq.ModelShredder
{
	/// <summary>
	/// Creates a ShredderDelegate dynamically using DynamicMethod.
	/// </summary>
    public sealed class InjectionObjectShredder : IShredderDelegateProvider
    {
        #region IObjectShredder Members

		/// <summary>
		/// Gets a <see cref="ShredderDelegate"/> for a set of <see cref="ShredderOptions"/>.
		/// </summary>
		/// <param name="options">The <see cref="ShredderOptions"/> to respect.</param>
		/// <returns>
		/// A <see cref="ShredderDelegate"/> respecting the <see cref="ShredderOptions"/>
		/// </returns>
		public ShredderDelegate GetShredderMethod(ShredderOptions options)
		{
			if (options == null)
				throw new ArgumentNullException("options", "options is null.");

			DynamicMethod toObjectArray = new DynamicMethod("toObjectArray" + options.Type.Name, typeof(object[]),
															new Type[] { typeof(object) }, options.Type, true);

			// Begin emitting MSIL code
			ILGenerator ilgen = toObjectArray.GetILGenerator();

			ilgen.DeclareLocal(typeof(object[])); // stored at index 0
			ilgen.Emit(OpCodes.Ldc_I4_S, options.Members.Count); // load count of props+fields on stack
			ilgen.Emit(OpCodes.Newarr, typeof(object)); // declare array
			ilgen.Emit(OpCodes.Stloc_0); // Store array in field 0


			for (int i = 0; i < options.Members.Count; i++)
			{
				// Member info is either PropertyInfo or FieldInfo, PropertyInfo is more likely.
				MemberInfo member = options.Members[i];

				PropertyInfo pi = member as PropertyInfo;
				if (pi != null)
				{
					ilgen.Emit(OpCodes.Ldloc_0); // Load array on evaluation stack
					ilgen.Emit(OpCodes.Ldc_I4_S, i); // Load array position on eval stack
					ilgen.Emit(OpCodes.Ldarg_0); // Load ourselves on the eval stack
					ilgen.Emit(OpCodes.Call, pi.GetGetMethod());

					// Check if we need to box a value type
					if (pi.PropertyType.IsValueType)
					{
						ilgen.Emit(OpCodes.Box, pi.PropertyType);
					}

					// Store value in array, this pops all fields from eval stack that were added this for loop
					ilgen.Emit(OpCodes.Stelem_Ref);
				}
				else
				{
					FieldInfo fi = (FieldInfo)member;
					// makes sure exception is thrown when cast is invalid. Should never happen.

					ilgen.Emit(OpCodes.Ldloc_0); // Load array on evaluation stack
					ilgen.Emit(OpCodes.Ldc_I4_S, i); // Load array position on eval stack
					ilgen.Emit(OpCodes.Ldarg_0); // Load ourselves on the eval stack
					ilgen.Emit(OpCodes.Ldfld, fi); // Load field on the stack               

					// Check if we need to box a value type
					if (fi.FieldType.IsValueType)
					{
						ilgen.Emit(OpCodes.Box, fi.FieldType);
					}

					// Store value in array, this pops all fields from stack that were added this for loop
					ilgen.Emit(OpCodes.Stelem_Ref);
				}
			}

			ilgen.Emit(OpCodes.Ldloc_0); // Load objArray on stack 
			ilgen.Emit(OpCodes.Ret); // return it

			return (ShredderDelegate)toObjectArray.CreateDelegate(typeof(ShredderDelegate));
		}

        #endregion
    }
}

