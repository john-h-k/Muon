using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ultz.Muon.PELoader
{
    public static unsafe class MemoryExtensions
    {
        public static T Read<T>(this ref Memory<byte> mem) where T : unmanaged
        {
            T t = MemoryMarshal.Read<T>(mem.Span);
            mem = mem.Slice(sizeof(T));
            return t;
        }
    }
}
