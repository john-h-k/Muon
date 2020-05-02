using System;
using System.Reflection.Metadata;
using LLVMSharp.Interop;
using Ultz.Muon.Representations;

namespace Ultz.Muon.Transpilation
{
    public class MethodCompiler
    {
        public unsafe MethodCompiler(LLVMModuleRef module, LLVMTypeResolver resolver)
        {
            _module = module;
            _resolver = resolver;
        }

        public unsafe void Compile()
        {
            GenerateIlBBs();

            var method = _module.AddFunction(_method.Name, _func);

            var entry = method.AppendBasicBlock("entry");
            GenerateLocals(_method);
            EmitLocalsToEntryBB(entry);

            int i = 0;
            foreach (BasicBlock basicBlock in First)
            {
                var bb = method.AppendBasicBlock($"BB#{i++}:");

                EmitLLVMForBB(bb);
            }
        }

        private void CreateInstructionsAndBranchTargets()
        {
            Instructions = InstructionReader.ReadAllInstructions(_method.Body.GetILBytes(), out var map);
            foreach (var instr in Instructions)
            {
                if (!instr.IsBranch) continue;

                instr.BranchTarget = Instructions[map[instr.GetBranchTarget()]];
            }
        }

        private void BuildLLVMFunctionTy()
        {
            var resolvedTypes = _resolver.ResolveParams(_method.Signature.ParameterTypes,
                _method.GetThisTypeOrNullIfStatic());
            if (_method.Signature.Header.CallingConvention == SignatureCallingConvention.VarArgs)
                throw new NotSupportedException();

            _func = LLVMTypeRef.CreateFunction(
                _resolver.ResolveReturn(_method.Signature.ReturnType),
                resolvedTypes
            );

        }


        private LLVMModuleRef _module;
        private LLVMTypeRef _func;
        private NETMethod _method;
        private LLVMTypeResolver _resolver;

        public Instruction[] Instructions { get; private set; }

        public BasicBlock First { get; }
    }
}