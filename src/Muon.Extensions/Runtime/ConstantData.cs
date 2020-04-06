namespace Ultz.Muon.Extensions
{
    public struct ConstantData
    {
        public const uint Size = 2 + 4;
        public ConstantDataType Type;
        public uint Length;
        public unsafe fixed byte Data[0];
    }
}