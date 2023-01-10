using System.Linq;
using Mono.Cecil;

namespace ILWeaver.Editor.Extensions
{
    public static class TypeDefinitionExtensions
    {
        public static PropertyDefinition GetProperty(this TypeDefinition typeDefinition, string name)
        {
            return typeDefinition.Properties.FirstOrDefault(property => property.Name == name && !IsIndexer(property));
        }

        private static bool IsIndexer(PropertyDefinition propertyDefinition)
        {
            return propertyDefinition.HasParameters;
        }
    }
}