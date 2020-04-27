using System;
using System.Text;
using Ultz.Muon.Representations.Types;

namespace Ultz.Muon.Representations
{
    public class MethodForComp
    {
        public Memory<byte> Il { get; }
        public string Name { get; }
        public bool IsStatic { get; }
        public NETType EnclosingNetType { get; }
        public NETType ReturnNetType { get; }
        public NETType[] ParamTypes { get; }
        public NETType? GetThisOrNullIfStatic() => IsStatic ? null : EnclosingNetType;
        public bool IsVarArg { get; }
    }
}