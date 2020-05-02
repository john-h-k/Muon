using System;
using System.Collections.Immutable;
using System.Reflection.Metadata;
using Ultz.Muon.Representations.Types;
using Ultz.Muon.Utils;

namespace Ultz.Muon.Representations
{
    public class NETTypeProvider : ISignatureTypeProvider<NETType, NETTypeGenericProvider>
    {
        public NETType GetSZArrayType(NETType elementType)
        {
            throw new NotImplementedException();
        }

        public NETType GetArrayType(NETType elementType, ArrayShape shape)
        {
            throw new NotImplementedException();
        }

        public NETType GetByReferenceType(NETType elementType)
        {
            return NETType.MakeByRefType(elementType);
        }

        public NETType GetGenericInstantiation(NETType genericType, ImmutableArray<NETType> typeArguments)
        {
            throw new NotImplementedException();
        }

        public NETType GetPointerType(NETType elementType)
        {
            return NETType.MakePointerType(elementType);
        }

        public NETType GetPrimitiveType(PrimitiveTypeCode typeCode)
        {
            switch (typeCode)
            {
                case PrimitiveTypeCode.Void:
                    return NETType.SpecialTypes.Void;
                case PrimitiveTypeCode.Boolean:
                    return NETType.SpecialTypes.Boolean;
                case PrimitiveTypeCode.Char:
                    return NETType.SpecialTypes.Char;
                case PrimitiveTypeCode.SByte:
                    return NETType.SpecialTypes.SByte;
                case PrimitiveTypeCode.Byte:
                    return NETType.SpecialTypes.Byte;
                case PrimitiveTypeCode.Int16:
                    return NETType.SpecialTypes.Int16;
                case PrimitiveTypeCode.UInt16:
                    return NETType.SpecialTypes.UInt16;
                case PrimitiveTypeCode.Int32:
                    return NETType.SpecialTypes.Int32;
                case PrimitiveTypeCode.UInt32:
                    return NETType.SpecialTypes.UInt32;
                case PrimitiveTypeCode.Int64:
                    return NETType.SpecialTypes.Int64;
                case PrimitiveTypeCode.UInt64:
                    return NETType.SpecialTypes.UInt64;
                case PrimitiveTypeCode.Single:
                    return NETType.SpecialTypes.Single;
                case PrimitiveTypeCode.Double:
                    return NETType.SpecialTypes.Double;
                case PrimitiveTypeCode.String:
                    return NETType.SpecialTypes.String;
                case PrimitiveTypeCode.TypedReference:
                    return NETType.SpecialTypes.TypedReference;
                case PrimitiveTypeCode.IntPtr:
                    return NETType.SpecialTypes.IntPtr;
                case PrimitiveTypeCode.UIntPtr:
                    return NETType.SpecialTypes.UIntPtr;
                case PrimitiveTypeCode.Object:
                    return NETType.SpecialTypes.Object;

                default:
                    throw new ArgumentOutOfRangeException(nameof(typeCode), typeCode, null);
            }
        }

        public NETType GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            throw new NotImplementedException();
        }

        public NETType GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            throw new NotImplementedException();
        }

        public NETType GetFunctionPointerType(MethodSignature<NETType> signature)
        {
            throw new NotImplementedException();
        }

        public NETType GetGenericMethodParameter(NETTypeGenericProvider genericContext, int index)
        {
            throw new NotImplementedException();
        }

        public NETType GetGenericTypeParameter(NETTypeGenericProvider genericContext, int index)
        {
            throw new NotImplementedException();
        }

        public NETType GetModifiedType(NETType modifier, NETType unmodifiedType, bool isRequired)
        {
            return NETType.MakeModified(unmodifiedType, new CustomMod(isRequired, modifier));
        }

        public NETType GetPinnedType(NETType elementType)
        {
            throw new NotImplementedException();
        }

        public NETType GetTypeFromSpecification(MetadataReader reader, NETTypeGenericProvider genericContext,
            TypeSpecificationHandle handle, byte rawTypeKind)
        {
            throw new NotImplementedException();
        }
    }

    public class NETTypeGenericProvider
    {
    }
}