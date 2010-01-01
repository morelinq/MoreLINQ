using System;
using System.Collections.Generic;
using System.Text;

namespace MoreLinq.ModelShredder
{
    /// <summary>
    /// Shredder method for an object.
    /// </summary>
    /// <param name="obj">The object to Shred.</param>
    /// <returns>An object Array, representing the shredded object.</returns>
    public delegate object[] ShredderDelegate(object obj);
}
