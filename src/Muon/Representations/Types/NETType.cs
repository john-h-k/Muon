using System.Collections.Generic;
using System.Reflection.Metadata;

namespace Ultz.Muon.Representations.Types
{
    public class NETType : ILowerable<HighLevelLLVMType>
    {
        public NETType(SignatureTypeCode elementType, TypeDefinitionHandle handle)
        {
            ElementType = elementType;
            Handle = handle;
        }

        public static class SpecialTypes
        {
            public static readonly NETType SByte;
            public static readonly NETType Int16;
            public static readonly NETType Int32;
            public static readonly NETType Int64;
            public static readonly NETType Byte;
            public static readonly NETType UInt16;
            public static readonly NETType UInt32;
            public static readonly NETType UInt64;
            public static readonly NETType Char;
            public static readonly NETType Object;
            public static readonly NETType Boolean;
            public static readonly NETType Single;
            public static readonly NETType Double;
        }

        public SignatureTypeCode ElementType { get; }
        public TypeDefinitionHandle Handle { get; }
        
        public HighLevelLLVMType Lower()
        {
            return GetOrLowerType(this);
        }

        private static readonly Dictionary<NETType, HighLevelLLVMType> LoweringMap =
            new Dictionary<NETType, HighLevelLLVMType>
            {
                [SpecialTypes.Boolean] = HighLevelLLVMType.SpecialTypes.Boolean,
                [SpecialTypes.SByte] = HighLevelLLVMType.SpecialTypes.SByte,
                [SpecialTypes.Int16] = HighLevelLLVMType.SpecialTypes.Int16,
                [SpecialTypes.Int32] = HighLevelLLVMType.SpecialTypes.Int32,
                [SpecialTypes.Int64] = HighLevelLLVMType.SpecialTypes.Int64,
                [SpecialTypes.Byte] = HighLevelLLVMType.SpecialTypes.Byte,
                [SpecialTypes.UInt16] = HighLevelLLVMType.SpecialTypes.UInt16,
                [SpecialTypes.UInt32] = HighLevelLLVMType.SpecialTypes.UInt32,
                [SpecialTypes.UInt64] = HighLevelLLVMType.SpecialTypes.UInt64,
                [SpecialTypes.Char]  = HighLevelLLVMType.SpecialTypes.Char,
                [SpecialTypes.Single] = HighLevelLLVMType.SpecialTypes.Single,
                [SpecialTypes.Double] = HighLevelLLVMType.SpecialTypes.Double,
                [SpecialTypes.Object] = HighLevelLLVMType.SpecialTypes.Object,
            };

        public static HighLevelLLVMType GetOrLowerType(NETType type)
        {
            if (LoweringMap.TryGetValue(type, out var value)) return value;

            return LowerNetType(type);
        }

        private static HighLevelLLVMType LowerNetType(NETType type)
        {
            throw new System.NotImplementedException();
        }
    }
}