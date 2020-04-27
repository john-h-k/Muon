using System;

namespace Ultz.Muon.Representations
{
    [Flags]
    public enum PushBehaviour
    {
        Push0,
        Push1,
        Push1_Push1,
        PushI,
        PushRef,
        PushI4,
        PushI8,
        PushR4,
        PushR8,
        VarPush,
    }
}