using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Ultz.Muon.Utils;

// ReSharper disable InconsistentNaming

namespace Ultz.Muon.Representations
{
    public struct InstructionReader : IEnumerable<Instruction>, IEnumerator<Instruction>
    {
        private readonly List<Instruction> _instructions;
        
        public static Instruction[] ReadAllInstructions(ReadOnlyMemory<byte> il) 
            => new InstructionReader(il).ReadAllInstructionsToArray();
    
        public static Instruction[] ReadAllInstructions(ReadOnlyMemory<byte> il, out IDictionary<int, int> positionToIndexDictionary) 
            => new InstructionReader(il).ReadAllInstructionsToArray(out positionToIndexDictionary);
    
        public InstructionReader(ReadOnlyMemory<byte> il)
        {
            Il = il;
            __backing_field__position = 0;
            _instructions = new List<Instruction>(il.Length / 2); // Guess each instruction is 2 bytes
                                                                          // (if this changes, keep it a pow of 2 for perf. this must be a fast type!!
            Current = default;
        }
    
        public static bool TryFindTarget(Instruction branch, Span<Instruction> instructions, out Instruction target)
        {
            foreach (Instruction instr in instructions)
            {
                if (instr.Position == branch.GetBranchTarget() + branch.Position + branch.InstructionSize)
                {
                    target = instr;
                    return true;
                }
            }
    
            target = default;
            return false;
        }
    
        public ReadOnlyMemory<byte> Il { get; }
    
        object IEnumerator.Current => Current;
    
        private ReadOnlyMemory<byte> CurrentIl => Il.Slice(Position);
    
        private int __backing_field__position;
    
        public int Position
        {
            get => __backing_field__position;
            set
            {
                if (value < 0) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
    
                __backing_field__position = value;
            }
        }
    
        public void Advance(int count)
        {
            if (count < 0) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(count));
    
            Position += count;
        }
    
        public bool TryReadInstruction(out Instruction instr)
        {
            int opPosition = Position;
            if (!OpCodes.TryReadOpCode(CurrentIl.Span, out OpCode opCode))
            {
                instr = default;
                return false;
            }
    
            Advance(opCode.Size);
    
            if (CurrentIl.Length < opCode.OperandSize)
            {
                instr = default;
                return false;
            }
    
            ReadOnlyMemory<byte> operand = CurrentIl.Slice(0, opCode.OperandSize);
            Advance(opCode.OperandSize);
    
            instr = new Instruction(opCode, operand, opCode.Size + opCode.OperandSize, opPosition);
    
            Current = instr;
    
            return true;
        }
    
        public bool MoveNext()
        {
            return TryReadInstruction(out _);
        }
    
        public void Reset()
        {
            Position = 0;
        }
    
        public Instruction Current { get; private set; }
    
        public Instruction[] ReadAllInstructionsToArray()
            => ReadAllInstructionsToArray(out _);
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Instruction[] ReadAllInstructionsToArray(out IDictionary<int, int> positionToIndex)
        {
            positionToIndex = new Dictionary<int, int>(16); // guesstimate 16 opcodes
    
            int i = 0;
            foreach (Instruction instr in this)
            {
                _instructions.Add(instr);
                positionToIndex[instr.Position] = i;
                i++;
            }
    
            return _instructions.ToArray();
        }
    
        void IDisposable.Dispose()
        {
    
        }
    
        public InstructionReader GetEnumerator() => this;
        IEnumerator<Instruction> IEnumerable<Instruction>.GetEnumerator() => GetEnumerator();
    
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }


}