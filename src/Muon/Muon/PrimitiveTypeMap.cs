using LLVMSharp;

namespace Ultz.Muon
{
    public static unsafe class PrimitiveTypeMap
    {
        public static LLVMOpaqueType* Int8 = LLVM.Int8Type();
        public static LLVMOpaqueType* Int16 = LLVM.Int16Type();
        public static LLVMOpaqueType* Int32 = LLVM.Int32Type();
        public static LLVMOpaqueType* Int64 = LLVM.Int64Type();

        public static LLVMOpaqueType* UInt8 = LLVM.Int8Type();
        public static LLVMOpaqueType* UInt16 = LLVM.Int16Type();
        public static LLVMOpaqueType* UInt32 = LLVM.Int32Type();
        public static LLVMOpaqueType* UInt64 = LLVM.Int64Type();
    }
}