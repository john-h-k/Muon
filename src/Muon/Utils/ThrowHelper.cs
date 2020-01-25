using System;
using System.Diagnostics.CodeAnalysis;

namespace Ultz.Muon.Utils
{
    internal static unsafe partial class ThrowHelper
    {
        public static void ThrowIfNotDefined<T>(T value, string paramName = "No parameter name provided") where T : struct, Enum
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                ThrowArgumentException(paramName);
            }
        }

        public static void ThrowIfNull<T>(T? obj, string paramName = "No parameter name provided") where T : class
        {
            if (obj is null)
            {
                ThrowArgumentNullException(paramName);
            }
        }
    }
}