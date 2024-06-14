using System;

namespace Quik.Stb
{
    [AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    internal sealed class NativeTypeNameAttribute : System.Attribute
    {
        public NativeTypeNameAttribute(string typename)
        {
        }
    }
}
