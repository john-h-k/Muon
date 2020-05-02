using System;
using System.Collections.Immutable;
using System.Reflection.Metadata;
using LLVMSharp.Interop;
using Ultz.Muon.Representations.Types;

namespace Ultz.Muon.Representations
{
    public class LLVMTypeResolver
    {
        public LLVMTypeRef ResolveReturn(NETType netType) => ResolveIndividualType(netType);

        public LLVMTypeRef[] ResolveParams(ImmutableArray<NETType> parameters, NETType? @this = null)
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
            throw new NotImplementedException();
        }
    }
}