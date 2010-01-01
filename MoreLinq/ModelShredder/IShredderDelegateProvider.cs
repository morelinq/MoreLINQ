using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MoreLinq.ModelShredder
{
	/// <summary>
	/// Represents a Provider for <see cref="ShredderDelegate"/>s.
	/// </summary>
    public interface IShredderDelegateProvider
    {
		/// <summary>
		/// Gets a <see cref="ShredderDelegate"/> for a set of <see cref="ShredderOptions"/>.
		/// </summary>
		/// <param name="options">The <see cref="ShredderOptions"/> to respect.</param>
		/// <returns>A <see cref="ShredderDelegate"/> respecting the <see cref="ShredderOptions"/></returns>
        ShredderDelegate GetShredderMethod(ShredderOptions options);
    }
}
