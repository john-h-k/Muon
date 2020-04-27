using System;
using System.Collections.Generic;
using LLVMSharp.Interop;

namespace Ultz.Muon.Representations.Types
{
    public class LoweredLLVMType
    {
        public LoweredLLVMType(LLVMTypeRef[] table, LLVMTypeRef[] orderedInstanceFields)
        {
            VTable = table;
            OrderedInstanceFields = orderedInstanceFields;
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