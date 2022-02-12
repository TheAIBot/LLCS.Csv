using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LLCS.Csv.Tests
{
    /// <summary>
    /// Fuuuuuck this bullshit related to casting nint and nuint.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class CastHelper<T>
    {
        internal static T CastTo<U>(U value)
        {
            if (typeof(U) == typeof(int))
            {
                return CastTo(Unsafe.As<U, int>(ref value));
            }
            else if (typeof(U) == typeof(T))
            {
                return Unsafe.As<U, T>(ref value);
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        internal static T CastTo(int value)
        {
            if (typeof(T) == typeof(nint))
            {
                long ptrValue = new IntPtr(value).ToInt64();
                return Unsafe.As<long, T>(ref ptrValue);
            }
            else if (typeof(T) == typeof(nuint))
            {
                ulong ptrValue = new UIntPtr((uint)value).ToUInt64();
                return Unsafe.As<ulong, T>(ref ptrValue);
            }
            return (T)(Convert.ChangeType(value, typeof(T)) ?? throw new InvalidCastException());
        }
    }
}
