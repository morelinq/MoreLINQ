using System;

namespace MoreLinq
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    internal class CanBeNullAttribute : Attribute
    {
    }
}