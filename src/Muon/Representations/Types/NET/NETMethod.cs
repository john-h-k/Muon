using System;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Text;
using Ultz.Muon.Representations.Types;

namespace Ultz.Muon.Representations
{
    public class NETMethod
    {
        public NETMethod(ReaderSet readerSet, NETType owner, MethodDefinitionHandle handle)
        {
            var (reader, decoder, peReader) = readerSet;

            var def = reader.GetMethodDefinition(handle);
            Attributes = def.Attributes;
            ImplAttributes = def.ImplAttributes;
            Name = reader.GetString(def.Name);

            var blobReader = reader.GetBlobReader(def.Signature);
            Signature = decoder.DecodeMethodSignature(ref blobReader);

            Owner = owner;

            var methodData = peReader.GetSectionData(def.RelativeVirtualAddress);
            Body = MethodBodyBlock.Create(methodData.GetReader());
        }

        public MethodAttributes Attributes { get; }
        public MethodImplAttributes ImplAttributes { get; }
        public string Name { get; }
        public MethodSignature<NETType> Signature { get; }
        public NETType Owner { get; }
        public MethodBodyBlock Body { get; }

        public bool IsStatic => Attributes.HasFlag(MethodAttributes.Static);
        public bool IsVarArg => Signature.Header.CallingConvention == SignatureCallingConvention.VarArgs;
        public NETType? GetThisTypeOrNullIfStatic() => IsStatic ? null : Owner;
    }

    public sealed class ReaderSet
    {
        public ReaderSet(MetadataReader reader, SignatureDecoder<NETType, NETTypeGenericProvider> decoder, PEReader peReader)
        {
            Reader = reader;
            Decoder = decoder;
            PEReader = peReader;
        }

        public MetadataReader Reader { get; }
        public SignatureDecoder<NETType, NETTypeGenericProvider> Decoder { get; }
        public PEReader PEReader { get; }

        public void Deconstruct(
            out MetadataReader reader,
            out SignatureDecoder<NETType, NETTypeGenericProvider> decoder)
        {
            reader = Reader;
            decoder = Decoder;
        }
        
        public void Deconstruct(
            out MetadataReader reader,
            out SignatureDecoder<NETType, NETTypeGenericProvider> decoder, 
            out PEReader peReader)
        {
            reader = Reader;
            decoder = Decoder;
            peReader = PEReader;
        }
    }
}