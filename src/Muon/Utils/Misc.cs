using System;
using System.CommandLine.Rendering;
using System.Diagnostics.CodeAnalysis;

namespace Ultz.Muon.Utils
{
    public static class Misc
    {
        public static bool DefaultAndFalse<T>([MaybeNull] out T t)
        {
            t = default;
            return false;
        }
        
        public static bool ValueAndTrue<T>([MaybeNull] out T t, [MaybeNull] T val)
        {
            t = val;
            return true;
        }

        public static bool TryFormatSpace(ref Span<char> buffer, ref int charsWritten)
        {
            if (buffer.Length == 0) return false;
            buffer[0] = ' ';
            buffer.Slice(1);
            return true;
        }

        public static void AdjustBuffer(ref Span<char> buffer, ref int charsWritten, int newCharsWritten)
        {
            charsWritten += newCharsWritten;
            buffer.Slice(newCharsWritten);
        }


        public static bool AdjustAndTryFormatSpace(ref Span<char> buffer, ref int charsWritten, int newCharsWritten)
        {
            if (!TryFormatSpace(ref buffer, ref charsWritten)) return false;
            AdjustBuffer(ref buffer, ref charsWritten, newCharsWritten);
            return true;
        }
    }
}