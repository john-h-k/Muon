using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Ultz.Muon.Utils;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Ultz.Muon.PELoader
{
    public enum OutputType
    {
        Exe,
        Dll
    }

    public class CompilationAssembly
    {
        public OutputType OutputType { get; }
        public MetadataReader Reader { get; }
        
        public CompilationAssembly(Stream memory)
        {
            using var peReader = new PEReader(memory);
            var headers = peReader.PEHeaders;

            VerifyHeaders(headers, out var outputType);
            OutputType = outputType;

            Reader = peReader.GetMetadataReader();
        }

        void VerifyHeaders(PEHeaders headers, out OutputType outputType)
        {
            var coffHeader = headers.CoffHeader;

            outputType = coffHeader.Characteristics.HasFlag(Characteristics.Dll) ? OutputType.Dll : OutputType.Exe;
        }
    }
}