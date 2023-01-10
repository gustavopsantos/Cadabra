using System;
using System.IO;
using System.Linq;
using ILWeaver.Editor.Extensions;
using Mono.Cecil;

namespace ILWeaver.Editor.Utilities
{
    public static class CecilUtilities
    {
        public static TypeDefinition GetTypeDefinition<T>()
        {
            var type = typeof(T);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var module = assemblies.Single(a => a.FullName == type.Module.Assembly.FullName);
            using var stream = new FileStream(module.Location, FileMode.Open, FileAccess.Read);
            var moduleDefinition = ModuleDefinition.ReadModule(stream);
            return moduleDefinition.GetTypes().Single(t => t.FullName == type.FullName);
        }

        public static PropertyDefinition GetPropertyDefinition<T>(string name)
        {
            var typeDef = GetTypeDefinition<T>();
            return typeDef.GetProperty(name);
        }

        public static MethodDefinition GetMethodDefinition<T>(string name, Type[] arguments)
        {
            var typeDef = GetTypeDefinition<T>();
            var methods = typeDef.Methods.Where(m => m.Name == name);
            return methods.Single(m => m.AcceptsArguments(arguments));
        }
    }
}