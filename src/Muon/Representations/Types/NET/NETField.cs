using System.Reflection;
using System.Reflection.Metadata;

namespace Ultz.Muon.Representations.Types
{
    public class NETField
    {
        public NETField(ReaderSet readerSet, NETType owner, FieldDefinitionHandle handle)
        {
            var (reader, decoder) = readerSet;
            var def = reader.GetFieldDefinition(handle);

            Name = reader.GetString(def.Name);
            Attributes = def.Attributes;

            var blob = reader.GetBlobReader(def.Signature);

            Type = decoder.DecodeFieldSignature(ref blob);
            Owner = owner;
        }
        
        public string Name { get; }
        public FieldAttributes Attributes { get; }
        public NETType Type { get; }
        public NETType Owner { get; }
    }
}