using System;

namespace ILWeaver.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BenchmarkAttribute : Attribute
    {
    }
}