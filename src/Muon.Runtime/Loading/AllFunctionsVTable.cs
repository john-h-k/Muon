using System;
using System.Runtime.InteropServices;
using Ultz.Muon.Extensions;

namespace Muon.Runtime
{
    /// <summary>
    /// Contains the names and function pointers to every single function in the current library.
    /// Can be omitted, in which case the current library will not be usable via dynamic linking unless emits its own
    /// native callables.
    /// </summary>
    public static class AllFunctionsVTable
    {
        public static unsafe char** SlotNames;
        public static uint SlotCount;
        public static unsafe IntPtr* Slots;

        public static unsafe void Fill(ConstantData* ptr)
        {
            var length = (ptr->Length) / sizeof(char);
            var slotNames = (char*) ptr->Data;
            // TODO malloc a char** for the slot names
            // TODO copy 
        }
    }
}