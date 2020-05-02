using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Ultz.Muon.Utils;

// ReSharper disable InconsistentNaming

namespace Ultz.Muon.Representations
{
    public partial struct Instruction
    {
        public readonly struct InstructionSet : IEnumerable<Instruction>
        {
            private readonly ImmutableArray<Instruction> _instructions;

            public Reader(ReadOnlyMemory<byte> ilMemory)
            {
                var il = ilMemory.Span;
                var builder = ImmutableArray.CreateBuilder<Instruction>(il.Length / 2); // Guess each instruction is 2 bytes

                for (var i = 0; ; i++)
                {
                    if (!OpCodes.TryReadOpCode(il, out var opcode))
                    {
                        break;
                    }
                    
                    if (il.Length < opcode.OperandSize + opcode.Size)
                    {
                        break;
                    }
    
                    ReadOnlyMemory<byte> operand = ilMemory.Slice(0, opcode.OperandSize);
    
                    var instr = new Instruction(opcode, operand, i);

                    int advance = instr.InstructionSize;
                    il = il.Slice(advance);
                    ilMemory = ilMemory.Slice(advance);
                    
                    
                    builder.Add(instr);
                }
            }
            
            public ImmutableArray<Instruction>.Enumerator  GetEnumerator() => _instructions.GetEnumerator();

            IEnumerator<Instruction> IEnumerable<Instruction>.GetEnumerator() => ((IEnumerable<Instruction>) _instructions).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Instruction>) this).GetEnumerator();
        }
    }
}