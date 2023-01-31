using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Cadabra.CodeGen.Extensions
{
    internal static class AssemblyDefinitionExtensions
    {
        public static void Write(this AssemblyDefinition assemblyDefinition, out byte[] pe, out byte[] pdb)
        {
            using var peStream = new MemoryStream();
            using var pdbStream = new MemoryStream();

            var writerParameters = new WriterParameters
            {
                SymbolWriterProvider = new PortablePdbWriterProvider(),
                SymbolStream = pdbStream,
                WriteSymbols = true,
            };

            assemblyDefinition.Write(peStream, writerParameters);

            pe = peStream.ToArray();
            pdb = pdbStream.ToArray();
        }
    }
}