using Mono.Cecil;
using System.Linq;
using Cadabra.CodeGen.Emitters;
using Cadabra.CodeGen.Extensions;
using System.Collections.Generic;
using Unity.CompilationPipeline.Common.Diagnostics;
using Unity.CompilationPipeline.Common.ILPostProcessing;

namespace Cadabra.CodeGen
{
    internal sealed class CadabraILPostProcessor : ILPostProcessor
    {
        public override ILPostProcessor GetInstance()
        {
            return this;
        }

        public override bool WillProcess(ICompiledAssembly compiledAssembly)
        {
            return true;
        }

        public override ILPostProcessResult Process(ICompiledAssembly compiledAssembly)
        {
            var diagnostics = new List<DiagnosticMessage>();
            var assemblyDefinition = compiledAssembly.Read();

            foreach (var module in assemblyDefinition.Modules)
            {
                BenchmarkILEmitter.Weave(module);
            }

            RemoveSelfReferenceIfNeeded(assemblyDefinition);
            assemblyDefinition.Write(out var pe, out var pdb);

            return new ILPostProcessResult(new InMemoryAssembly(pe, pdb), diagnostics);
        }

        private static void RemoveSelfReferenceIfNeeded(AssemblyDefinition assemblyDefinition)
        {
            var (selfReference, selfReferenceIndex) = assemblyDefinition.MainModule.AssemblyReferences
                .Select((x, i) => (x, i))
                .FirstOrDefault(e => e.x.Name == assemblyDefinition.Name.Name);

            if (selfReference != null)
            {
                assemblyDefinition.MainModule.AssemblyReferences.RemoveAt(selfReferenceIndex);
            }
        }
    }
}