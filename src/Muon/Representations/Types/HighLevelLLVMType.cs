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

        public HighLevelLLVMType[] ImplementingInterfaces { get; }

        public HighLevelLLVMType[] InstanceFields { get; }
        public HighLevelLLVMType[] InstanceMethods { get; }
        
        public HighLevelLLVMType[] StaticFields { get; }
        public HighLevelLLVMType[] StaticMethods { get; }
        public bool IsReferenceType { get; }

        public static partial class SpecialTypes
        {
        }
        
        public LoweredLLVMType Lower()
        {
            return GetOrLowerType(this);
        }

        private static readonly Dictionary<HighLevelLLVMType, LoweredLLVMType> LoweringMap = new Dictionary<HighLevelLLVMType, LoweredLLVMType>();
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
                var vtable = CreateVTableForValueType(type.ImplementingInterfaces);
                
                //result = new LoweredLLVMType(vtable, layout, );
                throw new NotImplementedException();
            }
            else
            {
                
            }
        }

        private static LLVMTypeRef[] CreateVTableForValueType(HighLevelLLVMType[] implementingInterfaces)
        {
            if (implementingInterfaces != null && implementingInterfaces.Length != 0)
                throw new NotSupportedException();
            
            
        }
        
        
    }
}