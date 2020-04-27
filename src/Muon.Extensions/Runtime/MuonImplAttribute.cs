using System;

namespace Ultz.Muon.Extensions
{
    /// <summary>
    /// Applicable to a method that the Muon AOT compiler knows how to implement.
    /// </summary>
    public class MuonImplAttribute : Attribute
    {
        public ImplDescriptor Descriptor;

        public MuonImplAttribute(ImplDescriptor descriptor)
        {
            Descriptor = descriptor;
        }
    }
}