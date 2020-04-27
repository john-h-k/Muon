using System.IO;
using LLVMSharp.Interop;
using Ultz.Muon.PELoader;
using Ultz.Muon.Utils;

namespace Ultz.Muon
{
    public class CompilationUnit
    {
        public unsafe CompilationUnit(CompilerConfiguration compilerConfiguration, string name)
        {
            CompilerConfiguration = compilerConfiguration; 
            Name = name;

            var filestream = File.OpenRead(name);
            CompilationAssembly = new CompilationAssembly(filestream);

            Module = LLVMModuleRef.CreateWithName(name);
        }

        public CompilerConfiguration CompilerConfiguration { get; }
        public CompilationAssembly CompilationAssembly { get; }
        public string Name { get; }

        public LLVMModuleRef Module { get; }

        // Egg - Sofie
    }
}