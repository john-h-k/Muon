using System;

namespace Ultz.Muon.Representations
{
    [Flags]
    public enum PopBehaviour
    {
        Pop0,
        Pop1,
        Pop1_Pop1,
        PopI,
        PopI_Pop1,
        PopI4,
        PopI8,
        PopR4,
        PopR8,
        PopRef,
        VarPop,
        PopI_PopI,
        PopI_PopRef,
        PopI_PopR4,
        PopI_PopR8,
        PopI_PopI8,
        PopRef_PopI,
        PopRef_PopI_PopI,
        PopRef_PopI_PopR4,
        PopRef_PopI_PopR8,
        PopRef_PopI_PopRef,
        PopRef_PopI_PopI8,
        PopI_PopI_PopI,
        PopRef_Pop1,
        PopRef_PopI_Pop1
    }
}