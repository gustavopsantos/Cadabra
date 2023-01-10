using Mono.Cecil;

namespace ILWeaver.Editor.Core
{
    public interface IWeaver
    {
        void Weave(ModuleDefinition module);
    }
}