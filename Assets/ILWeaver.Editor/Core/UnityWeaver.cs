using System.Collections.Generic;
using System.IO;
using ILWeaver.Editor.Weavers;
using Mono.Cecil;
using UnityEditor;
using UnityEditor.Compilation;

namespace ILWeaver.Editor.Core
{
    internal static class UnityWeaver
    {
        private static readonly List<IWeaver> _weavers = new()
        {
            new BenchmarkWeaver(),
        };
        
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            CompilationPipeline.assemblyCompilationFinished += Weave;
        }
        
        private static void Weave(string assemblyPath, CompilerMessage[] compilerMessages)
        {
            using var stream = new FileStream(assemblyPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            using var module = ModuleDefinition.ReadModule(stream);

            foreach (var weaver in _weavers)
            {
                weaver.Weave(module);
            }

            module.Write(stream);
        }
    }
}