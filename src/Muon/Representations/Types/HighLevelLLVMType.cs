using System;
using System.Collections.Generic;
using LLVMSharp.Interop;

namespace Ultz.Muon.Representations.Types
{
    public partial class HighLevelLLVMType : ILowerable<LoweredLLVMType>
    {
        public HighLevelLLVMType(HighLevelLLVMType[] instanceFields, HighLevelLLVMType[] instanceMethods, HighLevelLLVMType[] staticFields, HighLevelLLVMType[] staticMethods)
        {
            InstanceFields = instanceFields;
            InstanceMethods = instanceMethods;
            StaticFields = staticFields;
            StaticMethods = staticMethods;
        }

        public HighLevelLLVMType[] InstanceFields { get; }
        public HighLevelLLVMType[] InstanceMethods { get; }
        
        public HighLevelLLVMType[] StaticFields { get; }
        public HighLevelLLVMType[] StaticMethods { get; }
        public bool IsReferenceType { get; }

        public static partial class SpecialTypes
        {
            public static readonly HighLevelLLVMType SByte ;
            public static readonly HighLevelLLVMType Int16;
            public static readonly HighLevelLLVMType Int32;
            public static readonly HighLevelLLVMType Int64;
            public static readonly HighLevelLLVMType Byte;
            public static readonly HighLevelLLVMType UInt16;
            public static readonly HighLevelLLVMType UInt32;
            public static readonly HighLevelLLVMType UInt64;
            public static readonly HighLevelLLVMType Char;
            public static readonly HighLevelLLVMType Object;
            public static readonly HighLevelLLVMType Boolean;
            public static readonly HighLevelLLVMType Single;
            public static readonly HighLevelLLVMType Double;
        }
        
        public LoweredLLVMType Lower()
        {
            return GetOrLowerType(this);
        }

        private static readonly Dictionary<HighLevelLLVMType, LoweredLLVMType> LoweringMap =
            new Dictionary<HighLevelLLVMType, LoweredLLVMType>
            {
            };

        private static LoweredLLVMType GetOrLowerType(HighLevelLLVMType type)
        {
            if (LoweringMap.TryGetValue(type, out var value)) return value;

            LoweredLLVMType result;
            if (!type.IsReferenceType)
            {
                var layout = new LLVMTypeRef[type.InstanceFields.Length];

                int i = 0;
                foreach (HighLevelLLVMType field in type.InstanceFields)
                {
                    var lowered = field.Lower();
                    layout[i++] = lowered.TypeHandle;
                }

                // The vtable used when boxed
                var vtable = CreateVtableForValueType(type.ImplementingInterfaces);
                
                result = new LoweredLLVMType(vtable, layout);
            }
        }

    }
}