using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using Ultz.Muon.PELoader;

namespace Ultz.Muon
{
    public class CoreLibLoader
    {
        private const string CoreLibName = "System.Private.CoreLib"; // hard code because uhhhh lazy

        private CompilationAssembly _assembly;

        private static readonly HashSet<string> CorePrimitiveNames = new HashSet<string>
        {
            "Boolean",
            "Char",
        };

        private const string CorePrimitiveNamespace = "System";
        

        public void LoadCorePrimitives()
        {
            _assembly = new CompilationAssembly(File.OpenRead(CoreLibName));

            foreach (TypeDefinitionHandle def in _assembly.EnumerateTypeDefs())
            {
                
                var @namespace = _assembly.GetString(_assembly.Reader.GetTypeDefinition(def).Namespace);
                var name = _assembly.GetString(_assembly.Reader.GetTypeDefinition(def).Name);

                if (@namespace == CorePrimitiveNamespace && CorePrimitiveNames.Contains(name))
                {
                    LoadSingleCorePrimitive(def);
                }
            }
        }

        private void LoadSingleCorePrimitive(in TypeDefinitionHandle def)
        {
        }
    }
}