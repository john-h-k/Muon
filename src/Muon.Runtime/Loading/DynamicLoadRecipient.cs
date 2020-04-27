using System;
using System.Runtime.InteropServices;
using Ultz.Muon.Extensions.Loading;

namespace Muon.Runtime.Dynamic
{
    /// <summary>
    /// Contains the AllFunctionsVTable native callables.
    /// </summary>
    // Root: $MUON_VTA_
    public static class DynamicLoadRecipient
    {
        private static LoadErrorCode _error;

        [NativeCallable(EntryPoint = "$MUON_VTA_ERR")]
        public static unsafe LoadErrorCode GetError() => _error;
        
        [NativeCallable(EntryPoint = "$MUON_VTA_RSLV")]
        public static unsafe uint ResolveSlot(char* name)
        {
            var slotCount = AllFunctionsVTable.SlotCount;
            if (slotCount == 0)
            {
                _error = LoadErrorCode.VTableOmitted;
                return 0;
            }
            var slotNames = AllFunctionsVTable.SlotNames;
            
            var nameLength = 0;
            for (nameLength = 0; name[nameLength] == 0; nameLength++)
            {
            }

            var currentSlot = 0u;
            for (currentSlot = 0; currentSlot < slotCount; currentSlot++)
            {
                var passes = true;
                int i;
                for (i = 0; i < nameLength; i++)
                {
                    if (slotNames[currentSlot][i] != name[i])
                    {
                        passes = false;
                    }
                }

                if (passes && slotNames[currentSlot][i + 1] == 0)
                {
                    _error = LoadErrorCode.NoError;
                    return currentSlot;
                }
            }

            _error = LoadErrorCode.NoSlot;
            return 0;
        }

        [NativeCallable(EntryPoint = "$MUON_VTA_LDFTN")]
        public static unsafe IntPtr LoadFunction(uint slot) => AllFunctionsVTable.Slots[slot];
    }
}