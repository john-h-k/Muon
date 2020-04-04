using LLVMSharp.Interop;
using Ultz.Muon.Utils;

namespace Ultz.Muon
{
    public class CompilationUnit
    {
        public unsafe CompilationUnit(CompilerConfiguration compilerConfiguration, string name)
        {
            CompilerConfiguration = compilerConfiguration; 
            Name = name;

            Module = LLVMModuleRef.CreateWithName(name);
        }

        public CompilerConfiguration CompilerConfiguration { get; }
        public string Name { get; }

        public LLVMModuleRef Module { get; }

        // Egg - Sofie
    }
}