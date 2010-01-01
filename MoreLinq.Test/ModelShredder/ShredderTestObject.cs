using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreLinq.Test.ModelShredder
{
    /// <summary>
    /// A simple demo Object.
    /// </summary>
    public class ShredderTestObject
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


		public ShredderTestObject(int key)
        {
            KeyField = key;
            ANullableGuidField = Guid.NewGuid();

			ANullableDecimal = key / 3;
            AString = "ABCDEFGHIKKLMNOPQRSTUVWXYSZ";
        }

    }

}
