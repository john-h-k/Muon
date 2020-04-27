namespace Ultz.Muon.Representations.Types
{
    public interface ILowerable<out TLowered>
    {
        public TLowered Lower();
    }
}