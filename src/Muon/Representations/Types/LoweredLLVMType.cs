using System;
using System.Collections.Generic;
using LLVMSharp.Interop;

namespace Ultz.Muon.Representations.Types
{
    public class LoweredLLVMType
    {
        public LoweredLLVMType(LLVMTypeRef[] table, LLVMTypeRef[] orderedInstanceFields, LLVMTypeRef[] staticMethods, LLVMTypeRef[] instanceMethods, LLVMTypeRef[] staticFields)
        {
            VTable = table;
            OrderedInstanceFields = orderedInstanceFields;
            StaticMethods = staticMethods;
            InstanceMethods = instanceMethods;
            StaticFields = staticFields;
        }
        
        public LLVMTypeRef[] TypeLayout { get; }

        public ArraySegment<LLVMTypeRef> VTable { get; }
        public ArraySegment<LLVMTypeRef> OrderedInstanceFields { get; }
        public LLVMTypeRef TypeHandle { get; }
        
        public LLVMTypeRef[] StaticMethods { get; }
        public LLVMTypeRef[] InstanceMethods { get; }
        public LLVMTypeRef[] StaticFields { get; }
    }
}