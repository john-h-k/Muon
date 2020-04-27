namespace Ultz.Muon.Representations
{
    public enum ControlFlowKind
    {
        Next = 0,
        Branch = 1,
        ConditionalBranch = 2,
        Call = 3,
        Return = 4,
        Meta = 5,
        Throw = 6,
        Break = 7
    }
}