using System;
using System.Collections.Generic;
using System.CommandLine.Binding;
using System.Linq;
using System.Reflection.Metadata;
using Ultz.Muon.Utils;

namespace Ultz.Muon.Representations.Types
{
    public partial class NETType : ILowerable<HighLevelLLVMType>
    {
        public NETType(ReaderSet readerSet, TypeDefinitionHandle handle)
        {
            var (reader, _) = readerSet;
            Handle = handle;
            var def = reader.GetTypeDefinition(handle);

            var methods = def.GetMethods();

            var netMethods = new NETMethod[methods.Count];
            int i = 0;
            
            foreach (var method in methods)
            {
                netMethods[i++] = new NETMethod(readerSet, this, method);
            }

            var fields = def.GetFields();

            var netFields = new NETField[fields.Count];
            i = 0;
            foreach (var field in fields)
            {
                netFields[i++] = new NETField(readerSet, this, field);
            }

            NETMethods = netMethods;
            NETFields = netFields;
        }

        public static partial class SpecialTypes
        {
        }

        public static NETType MakePointerType(NETType type)
        {
            NETType newType = new NETType
            {
                Handle = type.Handle,
                UnderlyingType = type,
                IsPointer = true,
                NETMethods = PointerMethods,
                NETFields = PointerFields
            };

            return newType;
        }

        public static NETType MakeByRefType(NETType type)
        {
            var ptr = MakePointerType(type);
            ptr.IsPointer = false;
            ptr.IsByRef = true;
            return ptr;
        }

        public static NETType MakeModified(NETType type, CustomMod modifier)
        {
            type.Modifier = modifier;
            return type;
        }

        private static readonly NETMethod[] PointerMethods = Array.Empty<NETMethod>();
        private static readonly NETField[] PointerFields = Array.Empty<NETField>();

#pragma warning disable 8618
        private NETType() { }
#pragma warning restore 8618
        
        public TypeDefinitionHandle Handle { get; private set; }
        public CustomMod Modifier { get; private set; }
        public NETMethod[] NETMethods { get; private set; }
        public NETField[] NETFields { get; private set; }
        public bool IsPointer { get; private set; }
        public bool IsArray { get; private set; }
        public bool IsByRef { get; private set; }
        public bool HasUnderlyingType => IsPointer || IsArray || IsByRef;
        
        public NETType? UnderlyingType { get; private set; }

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
                [SpecialTypes.Char] = HighLevelLLVMType.SpecialTypes.Char,
                [SpecialTypes.Single] = HighLevelLLVMType.SpecialTypes.Single,
                [SpecialTypes.Double] = HighLevelLLVMType.SpecialTypes.Double,
                [SpecialTypes.Object] = HighLevelLLVMType.SpecialTypes.Object,
                [SpecialTypes.String] = HighLevelLLVMType.SpecialTypes.String,
                [SpecialTypes.Void] = HighLevelLLVMType.SpecialTypes.Void,
                [SpecialTypes.IntPtr] = HighLevelLLVMType.SpecialTypes.IntPtr,
                [SpecialTypes.UIntPtr] = HighLevelLLVMType.SpecialTypes.IntPtr,
                [SpecialTypes.TypedReference] = HighLevelLLVMType.SpecialTypes.TypedReference
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