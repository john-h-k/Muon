using System.Diagnostics;
using System.Reflection.Metadata;
using Ultz.Muon.Representations.Types;

namespace Ultz.Muon.Utils
{
    public static class BlobReaderExtensions
    {
    }
    public readonly struct CustomMod
    {
        public CustomMod(bool isRequired, NETType type)
        {
            IsRequired = isRequired;
            Type = type;
        }
        public bool IsRequired { get; }
        public NETType Type { get; }
    }
}