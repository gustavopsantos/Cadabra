using System;
using System.Linq;
using System.Reflection;

namespace Cadabra.CodeGen.Extensions
{
    internal static class TypeExtensions
    {
        public static MethodInfo FindMethod(this Type type, string name, params Type[] parameters)
        {
            var overloads = type.GetMethods().Where(m => m.Name == name).ToArray();

            foreach (var overload in overloads)
            {
                var overloadArguments = overload.GetParameters().Select(p => p.ParameterType);

                if (overloadArguments.Where((t, i) => t == parameters[i]).Count() == parameters.Length)
                {
                    return overload;
                }
            }

            throw new Exception("MethodInfo not found");
        }
    }
}