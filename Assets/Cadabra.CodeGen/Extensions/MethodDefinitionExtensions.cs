using System;
using System.Linq;
using Mono.Cecil;

namespace Cadabra.CodeGen.Extensions
{
    public static class MethodDefinitionExtensions
    {
        public static bool DefinesAttribute<T>(this MethodDefinition methodDefinition)
        {
            var attributes = methodDefinition.CustomAttributes;
            var attribute = attributes.FirstOrDefault(t => t.AttributeType.FullName.Equals(typeof(T).FullName, StringComparison.Ordinal));
            return attribute != null;
        }
    }
}