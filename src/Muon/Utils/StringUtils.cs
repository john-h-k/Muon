using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Ultz.Muon.Utils
{
    internal static unsafe class StringUtils
    {
        public static void TruncateTranscodeAscii(string str, Span<sbyte> output)
            => TruncateTranscodeAscii(str, ref output.GetPinnableReference());

        public static void TruncateTranscodeAscii(string str, sbyte* output)
            => TruncateTranscodeAscii(str, ref *output);

        public static void TruncateTranscodeAscii(string str, ref sbyte output)
        {
#if !DEBUG
            var i = 0;
            for (; i < str.Length; i++)
            {
                var c = (int)str[i];
                Debug.Assert(c <= byte.MaxValue);
                Unsafe.Add(ref output, i) = (sbyte)c;
            }
            Unsafe.Add(ref output, i) = 0;
#else

#endif
        }
    }

    internal unsafe readonly struct NativeAsciiString : IDisposable
    {
        public static NativeAsciiString Create(int length)
        {
            return new NativeAsciiString((sbyte*)Marshal.AllocHGlobal(length) + 1, length);
        }

        private NativeAsciiString(sbyte* pointer, int length) 
        { 
            Pointer = pointer; 
            Length = length; 
        }

        public sbyte* Pointer { get; }
        public int Length { get; }

        public static implicit operator Span<sbyte>(NativeAsciiString nativeAsciiString) => new Span<sbyte>(nativeAsciiString.Pointer, nativeAsciiString.Length + 1);

        public void Dispose()
        {
            Marshal.FreeHGlobal((IntPtr)Pointer);
        }
    }
}
