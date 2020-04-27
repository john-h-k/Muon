using System;
using System.IO;
using System.Text;
using Ultz.Muon.Representations;

#nullable enable

namespace Ultz.Muon.DebugTool.Disassembly
{
    public readonly struct IlDecompiler
    {
        public ReadOnlyMemory<Instruction> Instructions { get; }

        public int Offset { get; }


        public IlDecompiler(ReadOnlyMemory<byte> il, int offset = 0) : this(new InstructionReader(il).ReadAllInstructionsToArray().AsMemory(), offset)
        {
        }

        public IlDecompiler(ReadOnlyMemory<Instruction> instructions, int offset = 0)
        {
            Instructions = instructions;
            Offset = offset;
        }

        public IlDecompiler Slice(int offset)
        {
            return new IlDecompiler(Instructions, offset);
        }

        public string WriteIl()
        {
            StringBuilder builder = new StringBuilder(128);
            foreach (var instr in Instructions.Span)
            {
                builder.AppendLine(instr.ToString());
            }

            return builder.ToString();
        }
        
    }
}