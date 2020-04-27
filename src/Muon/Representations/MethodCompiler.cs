using System;
using LLVMSharp;
using LLVMSharp.Interop;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Ultz.Muon.Utils;

namespace Ultz.Muon.Representations
{
    public class MethodCompiler
    {
        public unsafe MethodCompiler(MethodForComp method, LLVMModuleRef module, LLVMTypeResolver resolver)
        {
            Instructions = new InstructionReader(method.Il).ReadAllInstructionsToArray(out var map);

            foreach (Instruction instr in Instructions)
            {
                if (!instr.IsBranch) continue;

                instr.BranchTarget = Instructions[map[instr.GetBranchTarget()]];
            }

            _module = module;

            var resolvedTypes = resolver.ResolveParams(method.ParamTypes, method.GetThisOrNullIfStatic());
            if (method.IsVarArg) throw new NotSupportedException();

            _func = LLVMTypeRef.CreateFunction(
                resolver.ResolveReturn(method.ReturnNetType),
                resolver.ResolveParams(method.ParamTypes, method.GetThisOrNullIfStatic())
            );
        }

        private LLVMModuleRef _module;
        private LLVMTypeRef _func;
        
        public Instruction[] Instructions { get; }

        public BasicBlock? First { get; }
    }
}