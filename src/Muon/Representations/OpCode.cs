using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Ultz.Muon.Utils;
using static Ultz.Muon.Representations.OpCode;
using static Ultz.Muon.Representations.OperandParams;
using static Ultz.Muon.Representations.PopBehaviour;
using static Ultz.Muon.Representations.PushBehaviour;
using static Ultz.Muon.Representations.ControlFlowKind;
using static Ultz.Muon.Representations.OpCodeKind;

namespace Ultz.Muon.Representations
{
    using static OpCode;

    public readonly struct OpCode
    {
        internal OpCode(
            string name,
            ILOpCode ilOpCode,
            OperandParams operandParams,
            PopBehaviour popBehaviour,
            PushBehaviour pushBehaviour,
            ControlFlowKind controlFlowKind,
            OpCodeKind opCodeKind
        )
        {
            Name = name;
            IlOpCode = ilOpCode;
            OperandParams = operandParams;
            PopBehaviour = popBehaviour;
            PushBehaviour = pushBehaviour;
            ControlFlowKind = controlFlowKind;
            OpCodeKind = opCodeKind;
            EncodingMap[IlOpCode] = this;
        }

        public static OpCode CreateNewOpCode(
            string name,
            PopBehaviour popBehaviour,
            PushBehaviour pushBehaviour,
            OperandParams operandParams,
            OpCodeKind opCodeKind,
            byte firstByte,
            byte secondByte,
            ControlFlowKind controlFlowKind
        )
            => new OpCode(name, (ILOpCode) ((firstByte << 8) | secondByte), operandParams, popBehaviour, pushBehaviour,
                controlFlowKind, opCodeKind);

        public string Name { get; }
        public ILOpCode IlOpCode { get; }
        public OperandParams OperandParams { get; }
        public PopBehaviour PopBehaviour { get; }
        public PushBehaviour PushBehaviour { get; }
        public ControlFlowKind ControlFlowKind { get; }
        public OpCodeKind OpCodeKind { get; }

        public int Size => (int) IlOpCode <= 0xFF ? 1 : 2;

        public bool IsBranch => IlOpCode.IsBranch();

        internal static readonly Dictionary<ILOpCode, OpCode> EncodingMap = new Dictionary<ILOpCode, OpCode>();

        public override string ToString() => Name;

        public bool TryFormat(Span<char> buffer, out int charWritten)
        {
            if (buffer.Length < Name.Length) return Misc.DefaultAndFalse(out charWritten);

            Name.AsSpan().CopyTo(buffer);
            return Misc.ValueAndTrue(out charWritten, Name.Length);
        }
        
        public int OperandSize
        {
            get
            {
                switch (OperandParams)
                {
                    case InlineNone:
                        return 0;
                    case ShortInlineVar:
                    case ShortInlineI:
                    case ShortInlineBrTarget:
                        return 1;
                    case ShortInlineR:
                    case InlineSwitch:
                    case InlineBrTarget:
                    case InlineI:
                    case InlineSig:
                    case InlineMethod:
                    case InlineType:
                    case InlineString:
                    case InlineField:
                    case InlineTok:
                        return 4;
                    case InlineVar:
                        return 2;
                    case InlineI8:
                    case InlineR:
                        return 8;
                    default:
                        ThrowHelper.ThrowArgumentException("Invalid enum value");
                        return default;
                }
            }
        }
    }

    // ReSharper disable StringLiteralTypo
    // ReSharper disable IdentifierTypo

    public static class OpCodes
    {
        public static bool TryReadOpCode(ReadOnlySpan<byte> span, out OpCode opCode)
        {
            if (span.IsEmpty)
            {
                opCode = default;
                return false;
            }

            ILOpCode ilOpCode;

            if (span[0] == 0xFE)
            {
                if (span.Length < 2)
                {
                    opCode = default;
                    return false;
                }
                ilOpCode = (ILOpCode)((span[0] << 8) | span[1]);
            }
            else
            {
                ilOpCode = (ILOpCode)span[0];
            }

            opCode = FromEncoding(ilOpCode);

            return true;
        }

        public static OpCode FromEncoding(ILOpCode ilOpCode) => EncodingMap[ilOpCode];

        public static readonly OpCode Nop = CreateNewOpCode("nop", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0x00,
            Next);

        public static readonly OpCode Break = CreateNewOpCode("break", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0x01,
            ControlFlowKind.Break);

        public static readonly OpCode Ldarg_0 =
            CreateNewOpCode("ldarg.0", Pop0, Push1, InlineNone, IMacro, 0x00, 0x02, Next);

        public static readonly OpCode Ldarg_1 =
            CreateNewOpCode("ldarg.1", Pop0, Push1, InlineNone, IMacro, 0x00, 0x03, Next);

        public static readonly OpCode Ldarg_2 =
            CreateNewOpCode("ldarg.2", Pop0, Push1, InlineNone, IMacro, 0x00, 0x04, Next);

        public static readonly OpCode Ldarg_3 =
            CreateNewOpCode("ldarg.3", Pop0, Push1, InlineNone, IMacro, 0x00, 0x05, Next);

        public static readonly OpCode Ldloc_0 =
            CreateNewOpCode("ldloc.0", Pop0, Push1, InlineNone, IMacro, 0x00, 0x06, Next);

        public static readonly OpCode Ldloc_1 =
            CreateNewOpCode("ldloc.1", Pop0, Push1, InlineNone, IMacro, 0x00, 0x07, Next);

        public static readonly OpCode Ldloc_2 =
            CreateNewOpCode("ldloc.2", Pop0, Push1, InlineNone, IMacro, 0x00, 0x08, Next);

        public static readonly OpCode Ldloc_3 =
            CreateNewOpCode("ldloc.3", Pop0, Push1, InlineNone, IMacro, 0x00, 0x09, Next);

        public static readonly OpCode Stloc_0 =
            CreateNewOpCode("stloc.0", Pop1, Push0, InlineNone, IMacro, 0x00, 0x0A, Next);

        public static readonly OpCode Stloc_1 =
            CreateNewOpCode("stloc.1", Pop1, Push0, InlineNone, IMacro, 0x00, 0x0B, Next);

        public static readonly OpCode Stloc_2 =
            CreateNewOpCode("stloc.2", Pop1, Push0, InlineNone, IMacro, 0x00, 0x0C, Next);

        public static readonly OpCode Stloc_3 =
            CreateNewOpCode("stloc.3", Pop1, Push0, InlineNone, IMacro, 0x00, 0x0D, Next);

        public static readonly OpCode Ldarg_S =
            CreateNewOpCode("ldarg.s", Pop0, Push1, ShortInlineVar, IMacro, 0x00, 0x0E, Next);

        public static readonly OpCode Ldarga_S =
            CreateNewOpCode("ldarga.s", Pop0, PushI, ShortInlineVar, IMacro, 0x00, 0x0F, Next);

        public static readonly OpCode Starg_S =
            CreateNewOpCode("starg.s", Pop1, Push0, ShortInlineVar, IMacro, 0x00, 0x10, Next);

        public static readonly OpCode Ldloc_S =
            CreateNewOpCode("ldloc.s", Pop0, Push1, ShortInlineVar, IMacro, 0x00, 0x11, Next);

        public static readonly OpCode Ldloca_S =
            CreateNewOpCode("ldloca.s", Pop0, PushI, ShortInlineVar, IMacro, 0x00, 0x12, Next);

        public static readonly OpCode Stloc_S =
            CreateNewOpCode("stloc.s", Pop1, Push0, ShortInlineVar, IMacro, 0x00, 0x13, Next);

        public static readonly OpCode Ldnull =
            CreateNewOpCode("ldnull", Pop0, PushRef, InlineNone, IPrimitive, 0x00, 0x14, Next);

        public static readonly OpCode Ldc_I4_M1 =
            CreateNewOpCode("ldc.i4.m1", Pop0, PushI, InlineNone, IMacro, 0x00, 0x15, Next);

        public static readonly OpCode Ldc_I4_0 =
            CreateNewOpCode("ldc.i4.0", Pop0, PushI, InlineNone, IMacro, 0x00, 0x16, Next);

        public static readonly OpCode Ldc_I4_1 =
            CreateNewOpCode("ldc.i4.1", Pop0, PushI, InlineNone, IMacro, 0x00, 0x17, Next);

        public static readonly OpCode Ldc_I4_2 =
            CreateNewOpCode("ldc.i4.2", Pop0, PushI, InlineNone, IMacro, 0x00, 0x18, Next);

        public static readonly OpCode Ldc_I4_3 =
            CreateNewOpCode("ldc.i4.3", Pop0, PushI, InlineNone, IMacro, 0x00, 0x19, Next);

        public static readonly OpCode Ldc_I4_4 =
            CreateNewOpCode("ldc.i4.4", Pop0, PushI, InlineNone, IMacro, 0x00, 0x1A, Next);

        public static readonly OpCode Ldc_I4_5 =
            CreateNewOpCode("ldc.i4.5", Pop0, PushI, InlineNone, IMacro, 0x00, 0x1B, Next);

        public static readonly OpCode Ldc_I4_6 =
            CreateNewOpCode("ldc.i4.6", Pop0, PushI, InlineNone, IMacro, 0x00, 0x1C, Next);

        public static readonly OpCode Ldc_I4_7 =
            CreateNewOpCode("ldc.i4.7", Pop0, PushI, InlineNone, IMacro, 0x00, 0x1D, Next);

        public static readonly OpCode Ldc_I4_8 =
            CreateNewOpCode("ldc.i4.8", Pop0, PushI, InlineNone, IMacro, 0x00, 0x1E, Next);

        public static readonly OpCode Ldc_I4_S =
            CreateNewOpCode("ldc.i4.s", Pop0, PushI, ShortInlineI, IMacro, 0x00, 0x1F, Next);

        public static readonly OpCode Ldc_I4 =
            CreateNewOpCode("ldc.i4", Pop0, PushI, InlineI, IPrimitive, 0x00, 0x20, Next);

        public static readonly OpCode Ldc_I8 =
            CreateNewOpCode("ldc.i8", Pop0, PushI8, InlineI8, IPrimitive, 0x00, 0x21, Next);

        public static readonly OpCode Ldc_R4 =
            CreateNewOpCode("ldc.r4", Pop0, PushR4, ShortInlineR, IPrimitive, 0x00, 0x22, Next);

        public static readonly OpCode Ldc_R8 =
            CreateNewOpCode("ldc.r8", Pop0, PushR8, InlineR, IPrimitive, 0x00, 0x23, Next);

        public static readonly OpCode Unused49 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0x24, Next);

        public static readonly OpCode Dup = CreateNewOpCode("dup", Pop1, Push1_Push1, InlineNone, IPrimitive, 0x00,
            0x25, Next);

        public static readonly OpCode Pop = CreateNewOpCode("pop", Pop1, Push0, InlineNone, IPrimitive, 0x00, 0x26,
            Next);

        public static readonly OpCode Jmp = CreateNewOpCode("jmp", Pop0, Push0, InlineMethod, IPrimitive, 0x00, 0x27,
            ControlFlowKind.Call);

        public static readonly OpCode Call = CreateNewOpCode("call", VarPop, VarPush, InlineMethod, IPrimitive, 0x00,
            0x28, ControlFlowKind.Call);

        public static readonly OpCode Calli = CreateNewOpCode("calli", VarPop, VarPush, InlineSig, IPrimitive, 0x00,
            0x29, ControlFlowKind.Call);

        public static readonly OpCode Ret = CreateNewOpCode("ret", VarPop, Push0, InlineNone, IPrimitive, 0x00, 0x2A,
            Return);

        public static readonly OpCode Br_S =
            CreateNewOpCode("br.s", Pop0, Push0, ShortInlineBrTarget, IMacro, 0x00, 0x2B, Branch);

        public static readonly OpCode Brfalse_S = CreateNewOpCode("brfalse.s", PopI, Push0, ShortInlineBrTarget, IMacro,
            0x00, 0x2C, ConditionalBranch);

        public static readonly OpCode Brtrue_S = CreateNewOpCode("brtrue.s", PopI, Push0, ShortInlineBrTarget, IMacro,
            0x00, 0x2D, ConditionalBranch);

        public static readonly OpCode Beq_S = CreateNewOpCode("beq.s", Pop1_Pop1, Push0, ShortInlineBrTarget, IMacro,
            0x00, 0x2E, ConditionalBranch);

        public static readonly OpCode Bge_S = CreateNewOpCode("bge.s", Pop1_Pop1, Push0, ShortInlineBrTarget, IMacro,
            0x00, 0x2F, ConditionalBranch);

        public static readonly OpCode Bgt_S = CreateNewOpCode("bgt.s", Pop1_Pop1, Push0, ShortInlineBrTarget, IMacro,
            0x00, 0x30, ConditionalBranch);

        public static readonly OpCode Ble_S = CreateNewOpCode("ble.s", Pop1_Pop1, Push0, ShortInlineBrTarget, IMacro,
            0x00, 0x31, ConditionalBranch);

        public static readonly OpCode Blt_S = CreateNewOpCode("blt.s", Pop1_Pop1, Push0, ShortInlineBrTarget, IMacro,
            0x00, 0x32, ConditionalBranch);

        public static readonly OpCode Bne_Un_S = CreateNewOpCode("bne.un.s", Pop1_Pop1, Push0, ShortInlineBrTarget,
            IMacro, 0x00, 0x33, ConditionalBranch);

        public static readonly OpCode Bge_Un_S = CreateNewOpCode("bge.un.s", Pop1_Pop1, Push0, ShortInlineBrTarget,
            IMacro, 0x00, 0x34, ConditionalBranch);

        public static readonly OpCode Bgt_Un_S = CreateNewOpCode("bgt.un.s", Pop1_Pop1, Push0, ShortInlineBrTarget,
            IMacro, 0x00, 0x35, ConditionalBranch);

        public static readonly OpCode Ble_Un_S = CreateNewOpCode("ble.un.s", Pop1_Pop1, Push0, ShortInlineBrTarget,
            IMacro, 0x00, 0x36, ConditionalBranch);

        public static readonly OpCode Blt_Un_S = CreateNewOpCode("blt.un.s", Pop1_Pop1, Push0, ShortInlineBrTarget,
            IMacro, 0x00, 0x37, ConditionalBranch);

        public static readonly OpCode Br = CreateNewOpCode("br", Pop0, Push0, InlineBrTarget, IPrimitive, 0x00, 0x38,
            Branch);

        public static readonly OpCode Brfalse = CreateNewOpCode("brfalse", PopI, Push0, InlineBrTarget, IPrimitive,
            0x00, 0x39, ConditionalBranch);

        public static readonly OpCode Brtrue = CreateNewOpCode("brtrue", PopI, Push0, InlineBrTarget, IPrimitive, 0x00,
            0x3A, ConditionalBranch);

        public static readonly OpCode Beq = CreateNewOpCode("beq", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 0x00, 0x3B,
            ConditionalBranch);

        public static readonly OpCode Bge = CreateNewOpCode("bge", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 0x00, 0x3C,
            ConditionalBranch);

        public static readonly OpCode Bgt = CreateNewOpCode("bgt", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 0x00, 0x3D,
            ConditionalBranch);

        public static readonly OpCode Ble = CreateNewOpCode("ble", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 0x00, 0x3E,
            ConditionalBranch);

        public static readonly OpCode Blt = CreateNewOpCode("blt", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 0x00, 0x3F,
            ConditionalBranch);

        public static readonly OpCode Bne_Un = CreateNewOpCode("bne.un", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 0x00,
            0x40, ConditionalBranch);

        public static readonly OpCode Bge_Un = CreateNewOpCode("bge.un", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 0x00,
            0x41, ConditionalBranch);

        public static readonly OpCode Bgt_Un = CreateNewOpCode("bgt.un", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 0x00,
            0x42, ConditionalBranch);

        public static readonly OpCode Ble_Un = CreateNewOpCode("ble.un", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 0x00,
            0x43, ConditionalBranch);

        public static readonly OpCode Blt_Un = CreateNewOpCode("blt.un", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 0x00,
            0x44, ConditionalBranch);

        public static readonly OpCode Switch = CreateNewOpCode("switch", PopI, Push0, InlineSwitch, IPrimitive, 0x00,
            0x45, ConditionalBranch);

        public static readonly OpCode Ldind_I1 =
            CreateNewOpCode("ldind.i1", PopI, PushI, InlineNone, IPrimitive, 0x00, 0x46, Next);

        public static readonly OpCode Ldind_U1 =
            CreateNewOpCode("ldind.u1", PopI, PushI, InlineNone, IPrimitive, 0x00, 0x47, Next);

        public static readonly OpCode Ldind_I2 =
            CreateNewOpCode("ldind.i2", PopI, PushI, InlineNone, IPrimitive, 0x00, 0x48, Next);

        public static readonly OpCode Ldind_U2 =
            CreateNewOpCode("ldind.u2", PopI, PushI, InlineNone, IPrimitive, 0x00, 0x49, Next);

        public static readonly OpCode Ldind_I4 =
            CreateNewOpCode("ldind.i4", PopI, PushI, InlineNone, IPrimitive, 0x00, 0x4A, Next);

        public static readonly OpCode Ldind_U4 =
            CreateNewOpCode("ldind.u4", PopI, PushI, InlineNone, IPrimitive, 0x00, 0x4B, Next);

        public static readonly OpCode Ldind_I8 =
            CreateNewOpCode("ldind.i8", PopI, PushI8, InlineNone, IPrimitive, 0x00, 0x4C, Next);

        public static readonly OpCode Ldind_I =
            CreateNewOpCode("ldind.i", PopI, PushI, InlineNone, IPrimitive, 0x00, 0x4D, Next);

        public static readonly OpCode Ldind_R4 =
            CreateNewOpCode("ldind.r4", PopI, PushR4, InlineNone, IPrimitive, 0x00, 0x4E, Next);

        public static readonly OpCode Ldind_R8 =
            CreateNewOpCode("ldind.r8", PopI, PushR8, InlineNone, IPrimitive, 0x00, 0x4F, Next);

        public static readonly OpCode Ldind_Ref =
            CreateNewOpCode("ldind.ref", PopI, PushRef, InlineNone, IPrimitive, 0x00, 0x50, Next);

        public static readonly OpCode Stind_Ref =
            CreateNewOpCode("stind.ref", PopI_PopI, Push0, InlineNone, IPrimitive, 0x00, 0x51, Next);

        public static readonly OpCode Stind_I1 =
            CreateNewOpCode("stind.i1", PopI_PopI, Push0, InlineNone, IPrimitive, 0x00, 0x52, Next);

        public static readonly OpCode Stind_I2 =
            CreateNewOpCode("stind.i2", PopI_PopI, Push0, InlineNone, IPrimitive, 0x00, 0x53, Next);

        public static readonly OpCode Stind_I4 =
            CreateNewOpCode("stind.i4", PopI_PopI, Push0, InlineNone, IPrimitive, 0x00, 0x54, Next);

        public static readonly OpCode Stind_I8 =
            CreateNewOpCode("stind.i8", PopI_PopI8, Push0, InlineNone, IPrimitive, 0x00, 0x55, Next);

        public static readonly OpCode Stind_R4 =
            CreateNewOpCode("stind.r4", PopI_PopR4, Push0, InlineNone, IPrimitive, 0x00, 0x56, Next);

        public static readonly OpCode Stind_R8 =
            CreateNewOpCode("stind.r8", PopI_PopR8, Push0, InlineNone, IPrimitive, 0x00, 0x57, Next);

        public static readonly OpCode Add = CreateNewOpCode("add", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0x58,
            Next);

        public static readonly OpCode Sub = CreateNewOpCode("sub", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0x59,
            Next);

        public static readonly OpCode Mul = CreateNewOpCode("mul", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0x5A,
            Next);

        public static readonly OpCode Div = CreateNewOpCode("div", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0x5B,
            Next);

        public static readonly OpCode Div_Un =
            CreateNewOpCode("div.un", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0x5C, Next);

        public static readonly OpCode Rem = CreateNewOpCode("rem", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0x5D,
            Next);

        public static readonly OpCode Rem_Un =
            CreateNewOpCode("rem.un", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0x5E, Next);

        public static readonly OpCode And = CreateNewOpCode("and", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0x5F,
            Next);

        public static readonly OpCode Or = CreateNewOpCode("or", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0x60,
            Next);

        public static readonly OpCode Xor = CreateNewOpCode("xor", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0x61,
            Next);

        public static readonly OpCode Shl = CreateNewOpCode("shl", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0x62,
            Next);

        public static readonly OpCode Shr = CreateNewOpCode("shr", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0x63,
            Next);

        public static readonly OpCode Shr_Un =
            CreateNewOpCode("shr.un", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0x64, Next);

        public static readonly OpCode Neg = CreateNewOpCode("neg", Pop1, Push1, InlineNone, IPrimitive, 0x00, 0x65,
            Next);

        public static readonly OpCode Not = CreateNewOpCode("not", Pop1, Push1, InlineNone, IPrimitive, 0x00, 0x66,
            Next);

        public static readonly OpCode Conv_I1 =
            CreateNewOpCode("conv.i1", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0x67, Next);

        public static readonly OpCode Conv_I2 =
            CreateNewOpCode("conv.i2", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0x68, Next);

        public static readonly OpCode Conv_I4 =
            CreateNewOpCode("conv.i4", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0x69, Next);

        public static readonly OpCode Conv_I8 =
            CreateNewOpCode("conv.i8", Pop1, PushI8, InlineNone, IPrimitive, 0x00, 0x6A, Next);

        public static readonly OpCode Conv_R4 =
            CreateNewOpCode("conv.r4", Pop1, PushR4, InlineNone, IPrimitive, 0x00, 0x6B, Next);

        public static readonly OpCode Conv_R8 =
            CreateNewOpCode("conv.r8", Pop1, PushR8, InlineNone, IPrimitive, 0x00, 0x6C, Next);

        public static readonly OpCode Conv_U4 =
            CreateNewOpCode("conv.u4", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0x6D, Next);

        public static readonly OpCode Conv_U8 =
            CreateNewOpCode("conv.u8", Pop1, PushI8, InlineNone, IPrimitive, 0x00, 0x6E, Next);

        public static readonly OpCode Callvirt = CreateNewOpCode("callvirt", VarPop, VarPush, InlineMethod, IObjModel,
            0x00, 0x6F, ControlFlowKind.Call);

        public static readonly OpCode Cpobj =
            CreateNewOpCode("cpobj", PopI_PopI, Push0, InlineType, IObjModel, 0x00, 0x70, Next);

        public static readonly OpCode Ldobj =
            CreateNewOpCode("ldobj", PopI, Push1, InlineType, IObjModel, 0x00, 0x71, Next);

        public static readonly OpCode Ldstr =
            CreateNewOpCode("ldstr", Pop0, PushRef, InlineString, IObjModel, 0x00, 0x72, Next);

        public static readonly OpCode Newobj = CreateNewOpCode("newobj", VarPop, PushRef, InlineMethod, IObjModel, 0x00,
            0x73, ControlFlowKind.Call);

        public static readonly OpCode Castclass =
            CreateNewOpCode("castclass", PopRef, PushRef, InlineType, IObjModel, 0x00, 0x74, Next);

        public static readonly OpCode Isinst =
            CreateNewOpCode("isinst", PopRef, PushI, InlineType, IObjModel, 0x00, 0x75, Next);

        public static readonly OpCode Conv_R_Un =
            CreateNewOpCode("conv.r.un", Pop1, PushR8, InlineNone, IPrimitive, 0x00, 0x76, Next);

        public static readonly OpCode Unused58 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0x77, Next);

        public static readonly OpCode Unused1 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0x78, Next);

        public static readonly OpCode Unbox =
            CreateNewOpCode("unbox", PopRef, PushI, InlineType, IPrimitive, 0x00, 0x79, Next);

        public static readonly OpCode Throw = CreateNewOpCode("throw", PopRef, Push0, InlineNone, IObjModel, 0x00, 0x7A,
            ControlFlowKind.Throw);

        public static readonly OpCode Ldfld =
            CreateNewOpCode("ldfld", PopRef, Push1, InlineField, IObjModel, 0x00, 0x7B, Next);

        public static readonly OpCode Ldflda =
            CreateNewOpCode("ldflda", PopRef, PushI, InlineField, IObjModel, 0x00, 0x7C, Next);

        public static readonly OpCode Stfld =
            CreateNewOpCode("stfld", PopRef_Pop1, Push0, InlineField, IObjModel, 0x00, 0x7D, Next);

        public static readonly OpCode Ldsfld =
            CreateNewOpCode("ldsfld", Pop0, Push1, InlineField, IObjModel, 0x00, 0x7E, Next);

        public static readonly OpCode Ldsflda =
            CreateNewOpCode("ldsflda", Pop0, PushI, InlineField, IObjModel, 0x00, 0x7F, Next);

        public static readonly OpCode Stsfld =
            CreateNewOpCode("stsfld", Pop1, Push0, InlineField, IObjModel, 0x00, 0x80, Next);

        public static readonly OpCode Stobj =
            CreateNewOpCode("stobj", PopI_Pop1, Push0, InlineType, IPrimitive, 0x00, 0x81, Next);

        public static readonly OpCode Conv_Ovf_I1_Un =
            CreateNewOpCode("conv.ovf.i1.un", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0x82, Next);

        public static readonly OpCode Conv_Ovf_I2_Un =
            CreateNewOpCode("conv.ovf.i2.un", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0x83, Next);

        public static readonly OpCode Conv_Ovf_I4_Un =
            CreateNewOpCode("conv.ovf.i4.un", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0x84, Next);

        public static readonly OpCode Conv_Ovf_I8_Un =
            CreateNewOpCode("conv.ovf.i8.un", Pop1, PushI8, InlineNone, IPrimitive, 0x00, 0x85, Next);

        public static readonly OpCode Conv_Ovf_U1_Un =
            CreateNewOpCode("conv.ovf.u1.un", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0x86, Next);

        public static readonly OpCode Conv_Ovf_U2_Un =
            CreateNewOpCode("conv.ovf.u2.un", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0x87, Next);

        public static readonly OpCode Conv_Ovf_U4_Un =
            CreateNewOpCode("conv.ovf.u4.un", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0x88, Next);

        public static readonly OpCode Conv_Ovf_U8_Un =
            CreateNewOpCode("conv.ovf.u8.un", Pop1, PushI8, InlineNone, IPrimitive, 0x00, 0x89, Next);

        public static readonly OpCode Conv_Ovf_I_Un =
            CreateNewOpCode("conv.ovf.i.un", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0x8A, Next);

        public static readonly OpCode Conv_Ovf_U_Un =
            CreateNewOpCode("conv.ovf.u.un", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0x8B, Next);

        public static readonly OpCode Box = CreateNewOpCode("box", Pop1, PushRef, InlineType, IPrimitive, 0x00, 0x8C,
            Next);

        public static readonly OpCode Newarr =
            CreateNewOpCode("newarr", PopI, PushRef, InlineType, IObjModel, 0x00, 0x8D, Next);

        public static readonly OpCode Ldlen =
            CreateNewOpCode("ldlen", PopRef, PushI, InlineNone, IObjModel, 0x00, 0x8E, Next);

        public static readonly OpCode Ldelema =
            CreateNewOpCode("ldelema", PopRef_PopI, PushI, InlineType, IObjModel, 0x00, 0x8F, Next);

        public static readonly OpCode Ldelem_I1 =
            CreateNewOpCode("ldelem.i1", PopRef_PopI, PushI, InlineNone, IObjModel, 0x00, 0x90, Next);

        public static readonly OpCode Ldelem_U1 =
            CreateNewOpCode("ldelem.u1", PopRef_PopI, PushI, InlineNone, IObjModel, 0x00, 0x91, Next);

        public static readonly OpCode Ldelem_I2 =
            CreateNewOpCode("ldelem.i2", PopRef_PopI, PushI, InlineNone, IObjModel, 0x00, 0x92, Next);

        public static readonly OpCode Ldelem_U2 =
            CreateNewOpCode("ldelem.u2", PopRef_PopI, PushI, InlineNone, IObjModel, 0x00, 0x93, Next);

        public static readonly OpCode Ldelem_I4 =
            CreateNewOpCode("ldelem.i4", PopRef_PopI, PushI, InlineNone, IObjModel, 0x00, 0x94, Next);

        public static readonly OpCode Ldelem_U4 =
            CreateNewOpCode("ldelem.u4", PopRef_PopI, PushI, InlineNone, IObjModel, 0x00, 0x95, Next);

        public static readonly OpCode Ldelem_I8 =
            CreateNewOpCode("ldelem.i8", PopRef_PopI, PushI8, InlineNone, IObjModel, 0x00, 0x96, Next);

        public static readonly OpCode Ldelem_I =
            CreateNewOpCode("ldelem.i", PopRef_PopI, PushI, InlineNone, IObjModel, 0x00, 0x97, Next);

        public static readonly OpCode Ldelem_R4 =
            CreateNewOpCode("ldelem.r4", PopRef_PopI, PushR4, InlineNone, IObjModel, 0x00, 0x98, Next);

        public static readonly OpCode Ldelem_R8 =
            CreateNewOpCode("ldelem.r8", PopRef_PopI, PushR8, InlineNone, IObjModel, 0x00, 0x99, Next);

        public static readonly OpCode Ldelem_Ref =
            CreateNewOpCode("ldelem.ref", PopRef_PopI, PushRef, InlineNone, IObjModel, 0x00, 0x9A, Next);

        public static readonly OpCode Stelem_I =
            CreateNewOpCode("stelem.i", PopRef_PopI_PopI, Push0, InlineNone, IObjModel, 0x00, 0x9B, Next);

        public static readonly OpCode Stelem_I1 = CreateNewOpCode("stelem.i1", PopRef_PopI_PopI, Push0, InlineNone,
            IObjModel, 0x00, 0x9C, Next);

        public static readonly OpCode Stelem_I2 = CreateNewOpCode("stelem.i2", PopRef_PopI_PopI, Push0, InlineNone,
            IObjModel, 0x00, 0x9D, Next);

        public static readonly OpCode Stelem_I4 = CreateNewOpCode("stelem.i4", PopRef_PopI_PopI, Push0, InlineNone,
            IObjModel, 0x00, 0x9E, Next);

        public static readonly OpCode Stelem_I8 = CreateNewOpCode("stelem.i8", PopRef_PopI_PopI8, Push0, InlineNone,
            IObjModel, 0x00, 0x9F, Next);

        public static readonly OpCode Stelem_R4 = CreateNewOpCode("stelem.r4", PopRef_PopI_PopR4, Push0, InlineNone,
            IObjModel, 0x00, 0xA0, Next);

        public static readonly OpCode Stelem_R8 = CreateNewOpCode("stelem.r8", PopRef_PopI_PopR8, Push0, InlineNone,
            IObjModel, 0x00, 0xA1, Next);

        public static readonly OpCode Stelem_Ref = CreateNewOpCode("stelem.ref", PopRef_PopI_PopRef, Push0, InlineNone,
            IObjModel, 0x00, 0xA2, Next);

        public static readonly OpCode Ldelem =
            CreateNewOpCode("ldelem", PopRef_PopI, Push1, InlineType, IObjModel, 0x00, 0xA3, Next);

        public static readonly OpCode Stelem =
            CreateNewOpCode("stelem", PopRef_PopI_Pop1, Push0, InlineType, IObjModel, 0x00, 0xA4, Next);

        public static readonly OpCode Unbox_Any =
            CreateNewOpCode("unbox.any", PopRef, Push1, InlineType, IObjModel, 0x00, 0xA5, Next);

        public static readonly OpCode Unused5 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xA6, Next);

        public static readonly OpCode Unused6 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xA7, Next);

        public static readonly OpCode Unused7 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xA8, Next);

        public static readonly OpCode Unused8 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xA9, Next);

        public static readonly OpCode Unused9 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xAA, Next);

        public static readonly OpCode Unused10 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xAB, Next);

        public static readonly OpCode Unused11 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xAC, Next);

        public static readonly OpCode Unused12 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xAD, Next);

        public static readonly OpCode Unused13 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xAE, Next);

        public static readonly OpCode Unused14 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xAF, Next);

        public static readonly OpCode Unused15 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xB0, Next);

        public static readonly OpCode Unused16 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xB1, Next);

        public static readonly OpCode Unused17 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xB2, Next);

        public static readonly OpCode Conv_Ovf_I1 =
            CreateNewOpCode("conv.ovf.i1", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0xB3, Next);

        public static readonly OpCode Conv_Ovf_U1 =
            CreateNewOpCode("conv.ovf.u1", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0xB4, Next);

        public static readonly OpCode Conv_Ovf_I2 =
            CreateNewOpCode("conv.ovf.i2", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0xB5, Next);

        public static readonly OpCode Conv_Ovf_U2 =
            CreateNewOpCode("conv.ovf.u2", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0xB6, Next);

        public static readonly OpCode Conv_Ovf_I4 =
            CreateNewOpCode("conv.ovf.i4", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0xB7, Next);

        public static readonly OpCode Conv_Ovf_U4 =
            CreateNewOpCode("conv.ovf.u4", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0xB8, Next);

        public static readonly OpCode Conv_Ovf_I8 =
            CreateNewOpCode("conv.ovf.i8", Pop1, PushI8, InlineNone, IPrimitive, 0x00, 0xB9, Next);

        public static readonly OpCode Conv_Ovf_U8 =
            CreateNewOpCode("conv.ovf.u8", Pop1, PushI8, InlineNone, IPrimitive, 0x00, 0xBA, Next);

        public static readonly OpCode Unused50 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xBB, Next);

        public static readonly OpCode Unused18 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xBC, Next);

        public static readonly OpCode Unused19 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xBD, Next);

        public static readonly OpCode Unused20 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xBE, Next);

        public static readonly OpCode Unused21 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xBF, Next);

        public static readonly OpCode Unused22 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xC0, Next);

        public static readonly OpCode Unused23 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xC1, Next);

        public static readonly OpCode Refanyval =
            CreateNewOpCode("refanyval", Pop1, PushI, InlineType, IPrimitive, 0x00, 0xC2, Next);

        public static readonly OpCode Ckfinite =
            CreateNewOpCode("ckfinite", Pop1, PushR8, InlineNone, IPrimitive, 0x00, 0xC3, Next);

        public static readonly OpCode Unused24 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xC4, Next);

        public static readonly OpCode Unused25 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xC5, Next);

        public static readonly OpCode Mkrefany =
            CreateNewOpCode("mkrefany", PopI, Push1, InlineType, IPrimitive, 0x00, 0xC6, Next);

        public static readonly OpCode Unused59 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xC7, Next);

        public static readonly OpCode Unused60 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xC8, Next);

        public static readonly OpCode Unused61 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xC9, Next);

        public static readonly OpCode Unused62 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xCA, Next);

        public static readonly OpCode Unused63 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xCB, Next);

        public static readonly OpCode Unused64 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xCC, Next);

        public static readonly OpCode Unused65 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xCD, Next);

        public static readonly OpCode Unused66 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xCE, Next);

        public static readonly OpCode Unused67 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xCF, Next);

        public static readonly OpCode Ldtoken =
            CreateNewOpCode("ldtoken", Pop0, PushI, InlineTok, IPrimitive, 0x00, 0xD0, Next);

        public static readonly OpCode Conv_U2 =
            CreateNewOpCode("conv.u2", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0xD1, Next);

        public static readonly OpCode Conv_U1 =
            CreateNewOpCode("conv.u1", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0xD2, Next);

        public static readonly OpCode Conv_I =
            CreateNewOpCode("conv.i", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0xD3, Next);

        public static readonly OpCode Conv_Ovf_I =
            CreateNewOpCode("conv.ovf.i", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0xD4, Next);

        public static readonly OpCode Conv_Ovf_U =
            CreateNewOpCode("conv.ovf.u", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0xD5, Next);

        public static readonly OpCode Add_Ovf =
            CreateNewOpCode("add.ovf", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0xD6, Next);

        public static readonly OpCode Add_Ovf_Un =
            CreateNewOpCode("add.ovf.un", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0xD7, Next);

        public static readonly OpCode Mul_Ovf =
            CreateNewOpCode("mul.ovf", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0xD8, Next);

        public static readonly OpCode Mul_Ovf_Un =
            CreateNewOpCode("mul.ovf.un", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0xD9, Next);

        public static readonly OpCode Sub_Ovf =
            CreateNewOpCode("sub.ovf", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0xDA, Next);

        public static readonly OpCode Sub_Ovf_Un =
            CreateNewOpCode("sub.ovf.un", Pop1_Pop1, Push1, InlineNone, IPrimitive, 0x00, 0xDB, Next);

        public static readonly OpCode Endfinally =
            CreateNewOpCode("endfinally", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xDC, Return);

        public static readonly OpCode Leave =
            CreateNewOpCode("leave", Pop0, Push0, InlineBrTarget, IPrimitive, 0x00, 0xDD, Branch);

        public static readonly OpCode Leave_S =
            CreateNewOpCode("leave.s", Pop0, Push0, ShortInlineBrTarget, IPrimitive, 0x00, 0xDE, Branch);

        public static readonly OpCode Stind_I =
            CreateNewOpCode("stind.i", PopI_PopI, Push0, InlineNone, IPrimitive, 0x00, 0xDF, Next);

        public static readonly OpCode Conv_U =
            CreateNewOpCode("conv.u", Pop1, PushI, InlineNone, IPrimitive, 0x00, 0xE0, Next);

        public static readonly OpCode Unused26 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xE1, Next);

        public static readonly OpCode Unused27 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xE2, Next);

        public static readonly OpCode Unused28 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xE3, Next);

        public static readonly OpCode Unused29 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xE4, Next);

        public static readonly OpCode Unused30 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xE5, Next);

        public static readonly OpCode Unused31 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xE6, Next);

        public static readonly OpCode Unused32 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xE7, Next);

        public static readonly OpCode Unused33 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xE8, Next);

        public static readonly OpCode Unused34 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xE9, Next);

        public static readonly OpCode Unused35 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xEA, Next);

        public static readonly OpCode Unused36 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xEB, Next);

        public static readonly OpCode Unused37 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xEC, Next);

        public static readonly OpCode Unused38 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xED, Next);

        public static readonly OpCode Unused39 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xEE, Next);

        public static readonly OpCode Unused40 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xEF, Next);

        public static readonly OpCode Unused41 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xF0, Next);

        public static readonly OpCode Unused42 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xF1, Next);

        public static readonly OpCode Unused43 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xF2, Next);

        public static readonly OpCode Unused44 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xF3, Next);

        public static readonly OpCode Unused45 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xF4, Next);

        public static readonly OpCode Unused46 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xF5, Next);

        public static readonly OpCode Unused47 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xF6, Next);

        public static readonly OpCode Unused48 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0x00, 0xF7, Next);

        public static readonly OpCode Prefix7 =
            CreateNewOpCode("prefix7", Pop0, Push0, InlineNone, IInternal, 0x00, 0xF8, Meta);

        public static readonly OpCode Prefix6 =
            CreateNewOpCode("prefix6", Pop0, Push0, InlineNone, IInternal, 0x00, 0xF9, Meta);

        public static readonly OpCode Prefix5 =
            CreateNewOpCode("prefix5", Pop0, Push0, InlineNone, IInternal, 0x00, 0xFA, Meta);

        public static readonly OpCode Prefix4 =
            CreateNewOpCode("prefix4", Pop0, Push0, InlineNone, IInternal, 0x00, 0xFB, Meta);

        public static readonly OpCode Prefix3 =
            CreateNewOpCode("prefix3", Pop0, Push0, InlineNone, IInternal, 0x00, 0xFC, Meta);

        public static readonly OpCode Prefix2 =
            CreateNewOpCode("prefix2", Pop0, Push0, InlineNone, IInternal, 0x00, 0xFD, Meta);

        public static readonly OpCode Prefix1 =
            CreateNewOpCode("prefix1", Pop0, Push0, InlineNone, IInternal, 0x00, 0xFE, Meta);

        public static readonly OpCode Prefixref =
            CreateNewOpCode("prefixref", Pop0, Push0, InlineNone, IInternal, 0x00, 0xFF, Meta);

        public static readonly OpCode Arglist =
            CreateNewOpCode("arglist", Pop0, PushI, InlineNone, IPrimitive, 0xFE, 0x00, Next);

        public static readonly OpCode Ceq = CreateNewOpCode("ceq", Pop1_Pop1, PushI, InlineNone, IPrimitive, 0xFE, 0x01,
            Next);

        public static readonly OpCode Cgt = CreateNewOpCode("cgt", Pop1_Pop1, PushI, InlineNone, IPrimitive, 0xFE, 0x02,
            Next);

        public static readonly OpCode Cgt_Un =
            CreateNewOpCode("cgt.un", Pop1_Pop1, PushI, InlineNone, IPrimitive, 0xFE, 0x03, Next);

        public static readonly OpCode Clt = CreateNewOpCode("clt", Pop1_Pop1, PushI, InlineNone, IPrimitive, 0xFE, 0x04,
            Next);

        public static readonly OpCode Clt_Un =
            CreateNewOpCode("clt.un", Pop1_Pop1, PushI, InlineNone, IPrimitive, 0xFE, 0x05, Next);

        public static readonly OpCode Ldftn =
            CreateNewOpCode("ldftn", Pop0, PushI, InlineMethod, IPrimitive, 0xFE, 0x06, Next);

        public static readonly OpCode Ldvirtftn =
            CreateNewOpCode("ldvirtftn", PopRef, PushI, InlineMethod, IPrimitive, 0xFE, 0x07, Next);

        public static readonly OpCode Unused56 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0xFE, 0x08, Next);

        public static readonly OpCode Ldarg =
            CreateNewOpCode("ldarg", Pop0, Push1, InlineVar, IPrimitive, 0xFE, 0x09, Next);

        public static readonly OpCode Ldarga =
            CreateNewOpCode("ldarga", Pop0, PushI, InlineVar, IPrimitive, 0xFE, 0x0A, Next);

        public static readonly OpCode Starg =
            CreateNewOpCode("starg", Pop1, Push0, InlineVar, IPrimitive, 0xFE, 0x0B, Next);

        public static readonly OpCode Ldloc =
            CreateNewOpCode("ldloc", Pop0, Push1, InlineVar, IPrimitive, 0xFE, 0x0C, Next);

        public static readonly OpCode Ldloca =
            CreateNewOpCode("ldloca", Pop0, PushI, InlineVar, IPrimitive, 0xFE, 0x0D, Next);

        public static readonly OpCode Stloc =
            CreateNewOpCode("stloc", Pop1, Push0, InlineVar, IPrimitive, 0xFE, 0x0E, Next);

        public static readonly OpCode Localloc =
            CreateNewOpCode("localloc", PopI, PushI, InlineNone, IPrimitive, 0xFE, 0x0F, Next);

        public static readonly OpCode Unused57 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0xFE, 0x10, Next);

        public static readonly OpCode Endfilter =
            CreateNewOpCode("endfilter", PopI, Push0, InlineNone, IPrimitive, 0xFE, 0x11, Return);

        public static readonly OpCode Unaligned =
            CreateNewOpCode("unaligned.", Pop0, Push0, ShortInlineI, IPrefix, 0xFE, 0x12, Meta);

        public static readonly OpCode Volatile =
            CreateNewOpCode("volatile.", Pop0, Push0, InlineNone, IPrefix, 0xFE, 0x13, Meta);

        public static readonly OpCode Tailcall =
            CreateNewOpCode("tail.", Pop0, Push0, InlineNone, IPrefix, 0xFE, 0x14, Meta);

        public static readonly OpCode Initobj =
            CreateNewOpCode("initobj", PopI, Push0, InlineType, IObjModel, 0xFE, 0x15, Next);

        public static readonly OpCode Constrained =
            CreateNewOpCode("constrained.", Pop0, Push0, InlineType, IPrefix, 0xFE, 0x16, Meta);

        public static readonly OpCode Cpblk =
            CreateNewOpCode("cpblk", PopI_PopI_PopI, Push0, InlineNone, IPrimitive, 0xFE, 0x17, Next);

        public static readonly OpCode Initblk =
            CreateNewOpCode("initblk", PopI_PopI_PopI, Push0, InlineNone, IPrimitive, 0xFE, 0x18, Next);

        public static readonly OpCode Unused69 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0xFE, 0x19, Next);

        public static readonly OpCode Rethrow = CreateNewOpCode("rethrow", Pop0, Push0, InlineNone, IObjModel, 0xFE,
            0x1A, ControlFlowKind.Throw);

        public static readonly OpCode Unused51 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0xFE, 0x1B, Next);

        public static readonly OpCode Sizeof =
            CreateNewOpCode("sizeof", Pop0, PushI, InlineType, IPrimitive, 0xFE, 0x1C, Next);

        public static readonly OpCode Refanytype =
            CreateNewOpCode("refanytype", Pop1, PushI, InlineNone, IPrimitive, 0xFE, 0x1D, Next);

        public static readonly OpCode Readonly =
            CreateNewOpCode("readonly.", Pop0, Push0, InlineNone, IPrefix, 0xFE, 0x1E, Meta);

        public static readonly OpCode Unused53 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0xFE, 0x1F, Next);

        public static readonly OpCode Unused54 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0xFE, 0x20, Next);

        public static readonly OpCode Unused55 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0xFE, 0x21, Next);

        public static readonly OpCode Unused70 =
            CreateNewOpCode("unused", Pop0, Push0, InlineNone, IPrimitive, 0xFE, 0x22, Next);
    }
}