using System;
using System.Reflection.Metadata;
using LLVMSharp.Interop;
using Ultz.Muon.Representations.Types;

namespace Ultz.Muon.Representations
{
    public class LLVMTypeResolver
    {
        public LLVMTypeRef ResolveReturn(NETType netType) => ResolveIndividualType(netType);

        public LLVMTypeRef[] ResolveParams(NETType[] parameters, NETType? @this = null)
        {
            LLVMTypeRef[] res = new LLVMTypeRef[parameters.Length + (@this is object ? 1 : 0)];

            for (var i = 0; i < parameters.Length; i++)
            {
                res[i] = ResolveIndividualType(parameters[i]);
            }

            if (@this is object)
            {
                res[^1] = ResolveIndividualType(@this);
            }

            return res;
        }

        private LLVMTypeRef ResolveIndividualType(NETType netType)
        {
            switch (netType.ElementType)
            {
                case SignatureTypeCode.Byte:
                case SignatureTypeCode.SByte:
                case SignatureTypeCode.Boolean:
                    return LLVMTypeRef.Int8;
                case SignatureTypeCode.Int16:
                case SignatureTypeCode.UInt16:
                case SignatureTypeCode.Char:
                    return LLVMTypeRef.Int16;
                case SignatureTypeCode.Int32:
                case SignatureTypeCode.UInt32:
                    return LLVMTypeRef.Int32;
                case SignatureTypeCode.Int64:
                case SignatureTypeCode.UInt64:
                    return LLVMTypeRef.Int64;
                case SignatureTypeCode.Single:
                    return LLVMTypeRef.Float;
                case SignatureTypeCode.Double:
                    return LLVMTypeRef.Double;
            }

            throw null!;
        }
    }
}