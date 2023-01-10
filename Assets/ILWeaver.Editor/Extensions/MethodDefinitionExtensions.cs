using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

namespace ILWeaver.Editor.Extensions
{
    public static class MethodDefinitionExtensions
    {
        public static bool DefinesAttribute<T>(this MethodDefinition methodDefinition)
        {
            var attributes = methodDefinition.CustomAttributes;
            var attribute = attributes.FirstOrDefault(t => t.AttributeType.FullName.Equals(typeof(T).FullName, StringComparison.Ordinal));
            return attribute != null;
        }

        public static bool AcceptsArguments(this MethodDefinition methodDefinition, Type[] arguments)
        {
            static Type ToType(ParameterDefinition parameterDefinition)
            {
                var typeName = parameterDefinition.ParameterType.FullName;
                var moduleName = Path.GetFileNameWithoutExtension(parameterDefinition.ParameterType.Scope.Name);
                return Type.GetType($"{typeName},{moduleName}");
            }

            if (methodDefinition.Parameters.Count != arguments.Length)
            {
                return false;
            }

            var methodArguments = methodDefinition.Parameters.Select(ToType).ToArray();
            return methodArguments.Where((t, i) => t == arguments[i]).Count() == arguments.Length;
        }
    }
}