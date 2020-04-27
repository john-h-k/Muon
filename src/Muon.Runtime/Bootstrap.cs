using System;
using Ultz.Muon.Extensions;

namespace Muon.Runtime
{
    public static class Bootstrap
    {
        /// <summary>
        /// Gets compile-time constants that Muon has set.
        /// </summary>
        /// <remarks>
        /// This data will be used to initialize the Muon runtime in its entirety.<br />
        /// <br />
        /// The preface's format is as follows:<br />
        /// - 4 bytes describing how many bytes remain (uint)<br />
        /// <br />
        /// The format is:<br />
        ///<br />
        /// - 2 bytes for the ConstantDataType (ushort)<br />
        /// - 4 bytes for the data length n (uint)<br />
        /// - n bytes of data<br />
        ///<br />
        /// </remarks>
        /// <returns></returns>
        [MuonImpl(ImplDescriptor.GetCompileTimeConstants)]
        static extern unsafe byte* GetCompileTimeConstants();

        public static unsafe void BootRuntime()
        {
            var compileTimeConstants = GetCompileTimeConstants();
            var length = (uint) (compileTimeConstants[0] << 24 | compileTimeConstants[1] << 16 |
                                 compileTimeConstants[2] << 8 | compileTimeConstants[3]);
            var currentIndex = 0u;
            while (currentIndex < length)
            {
                var data = ParseNextData(compileTimeConstants, ref currentIndex);
                switch (data->Type)
                {
                    case ConstantDataType.Invalid:
                    {
                        // TODO can we throw exceptions yet?
                        throw new InvalidProgramException();
                    }
                    case ConstantDataType.VTableNames:
                    {
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static unsafe ConstantData* ParseNextData(byte* constants, ref uint currentIndex)
        {
            var ret = (ConstantData*) (constants + currentIndex);
            currentIndex += ret->Length + ConstantData.Size;
            return ret;
        }
    }
}