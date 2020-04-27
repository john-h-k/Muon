using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using Ultz.Muon.Utils;
using static Ultz.Muon.Representations.OperandParams;

namespace Ultz.Muon.Representations
{
    public class Instruction
    {
        public Instruction(OpCode opCode, ReadOnlyMemory<byte> operand, int position, int offset)
        {
            OpCode = opCode;
            Operand = operand;
            Position = position;
            Offset = offset;
        }
        
        public OpCode OpCode { get; }

        public ReadOnlyMemory<byte> Operand { get; }

        public int Position { get; }

        public int Offset { get; }

        public bool IsBranch => OpCode.IsBranch;

        public int InstructionSize => OpCode.Size + Operand.Length;
        
        public Instruction? BranchTarget { get; set; }

        public int GetBranchTarget()
        {
            Debug.Assert(IsBranch);
            var span = Operand.Span;
            return OpCode.IlOpCode.GetBranchOperandSize() == 4
                ? MemoryMarshal.Read<int>(span)
                : MemoryMarshal.Read<sbyte>(span);
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(Offset.ToString("X8"));
            str.Append(" ");
            str.Append(OpCode.ToString());
            str.Append(" ");
            str.Append(FormatOperand());
            
            return str.ToString();
        }

        private string FormatOperand()
        {
            switch (OpCode.OperandParams)
            {
                case InlineNone:
                    return "";
                case ShortInlineVar:
                case ShortInlineI:
                case ShortInlineBrTarget:
                    return ReadOperandAs<byte>().ToString();
                case ShortInlineR:
                    return ReadOperandAs<float>().ToString(CultureInfo.InvariantCulture);
                case InlineSwitch:
                case InlineBrTarget:
                case InlineI:
                case InlineSig:
                case InlineMethod:
                case InlineType:
                case InlineString:
                case InlineField:
                case InlineTok:
                    return ReadOperandAs<int>().ToString();
                case InlineVar:
                    return ReadOperandAs<short>().ToString();
                case InlineI8:
                case InlineR:
                    return ReadOperandAs<double>().ToString(CultureInfo.InvariantCulture);
                    ;
                default:
                    ThrowHelper.ThrowArgumentException("Invalid enum value");
                    return default;
            }
        }

        private T ReadOperandAs<T>() where T : struct => MemoryMarshal.Read<T>(Operand.Span);
    }
}