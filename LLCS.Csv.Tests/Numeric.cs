using System;
using System.Runtime.CompilerServices;

namespace LLCS.Csv.Tests.Reader
{
    internal static class Numeric<T>
    {
        internal static readonly T MinValue;
        internal static readonly T MaxValue;
        internal static readonly bool IsSigned =
            typeof(T) == typeof(long) ||
            typeof(T) == typeof(int) ||
            typeof(T) == typeof(short) ||
            typeof(T) == typeof(sbyte) ||
            typeof(T) == typeof(nint) ||
            typeof(T) == typeof(double) ||
            typeof(T) == typeof(float);

        static Numeric()
        {
            if (typeof(T) == typeof(nint))
            {
                IntPtr minValue = IntPtr.MinValue;
                IntPtr maxValue = IntPtr.MaxValue;
                MinValue = Unsafe.As<IntPtr, T>(ref minValue);
                MaxValue = Unsafe.As<IntPtr, T>(ref maxValue);
            }
            else if (typeof(T) == typeof(nuint))
            {
                UIntPtr minValue = UIntPtr.MinValue;
                UIntPtr maxValue = UIntPtr.MaxValue;
                MinValue = Unsafe.As<UIntPtr, T>(ref minValue);
                MaxValue = Unsafe.As<UIntPtr, T>(ref maxValue);
            }
            else
            {
                MinValue = (T)(typeof(T).GetField(nameof(int.MinValue))?.GetValue(null) ?? throw new TypeAccessException());
                MaxValue = (T)(typeof(T).GetField(nameof(int.MaxValue))?.GetValue(null) ?? throw new TypeAccessException());
            }
        }
    }
}