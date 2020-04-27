using System;

namespace Ultz.Muon.Extensions
{
    // Used to statically link to libraries. Example usage would look like this:
    // [StaticImport("kernel32.lib")]
    // [DllImport("__Internal", EntryPoint = "HeapAlloc")]
    // extern IntPtr Malloc(IntPtr size);
    public class StaticImportAttribute : Attribute
    {
        public StaticImportAttribute(string lib)
        {
            Library = lib;
        }

        public readonly string Library;
    }
}