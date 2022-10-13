using CommunityToolkit.HighPerformance.Enumerables;
using System.Globalization;
using System.Numerics;

namespace LLCS.Csv.Reader
{
    public sealed partial class CsvReader
    {
        public long ReadLong(ref ReadOnlySpanTokenizer<char> tokens) => ReadLong(ref tokens, SignedIntegerParseStyle, _numberFormatInfo);
        public long ReadLong(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider)
        {
            if (!TryReadLong(ref tokens, style, provider, out long value))
            {
                ThrowParseException(tokens.Current, "long");
            }

            return value;
        }

        public ulong ReadULong(ref ReadOnlySpanTokenizer<char> tokens) => ReadULong(ref tokens, UnsignedIntegerParseStype, _numberFormatInfo);
        public ulong ReadULong(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider)
        {
            if (!TryReadULong(ref tokens, style, provider, out ulong value))
            {
                ThrowParseException(tokens.Current, "ulong");
            }

            return value;
        }

        public int ReadInt(ref ReadOnlySpanTokenizer<char> tokens) => ReadInt(ref tokens, SignedIntegerParseStyle, _numberFormatInfo);
        public int ReadInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider)
        {
            if (!TryReadInt(ref tokens, style, provider, out int value))
            {
                ThrowParseException(tokens.Current, "int");
            }

            return value;
        }

        public uint ReadUInt(ref ReadOnlySpanTokenizer<char> tokens) => ReadUInt(ref tokens, UnsignedIntegerParseStype, _numberFormatInfo);
        public uint ReadUInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider)
        {
            if (!TryReadUInt(ref tokens, style, provider, out uint value))
            {
                ThrowParseException(tokens.Current, "uint");
            }

            return value;
        }

        public nint ReadNInt(ref ReadOnlySpanTokenizer<char> tokens) => ReadNInt(ref tokens, SignedIntegerParseStyle, _numberFormatInfo);
        public nint ReadNInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider)
        {
            if (!TryReadNInt(ref tokens, style, provider, out nint value))
            {
                ThrowParseException(tokens.Current, "nint");
            }

            return value;
        }

        public nuint ReadNUInt(ref ReadOnlySpanTokenizer<char> tokens) => ReadNUInt(ref tokens, UnsignedIntegerParseStype, _numberFormatInfo);
        public nuint ReadNUInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider)
        {
            if (!TryReadNUInt(ref tokens, style, provider, out nuint value))
            {
                ThrowParseException(tokens.Current, "nuint");
            }

            return value;
        }

        public short ReadShort(ref ReadOnlySpanTokenizer<char> tokens) => ReadShort(ref tokens, SignedIntegerParseStyle, _numberFormatInfo);
        public short ReadShort(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider)
        {
            if (!TryReadShort(ref tokens, style, provider, out short value))
            {
                ThrowParseException(tokens.Current, "short");
            }

            return value;
        }

        public ushort ReadUShort(ref ReadOnlySpanTokenizer<char> tokens) => ReadUShort(ref tokens, UnsignedIntegerParseStype, _numberFormatInfo);
        public ushort ReadUShort(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider)
        {
            if (!TryReadUShort(ref tokens, style, provider, out ushort value))
            {
                ThrowParseException(tokens.Current, "ushort");
            }

            return value;
        }

        public sbyte ReadSByte(ref ReadOnlySpanTokenizer<char> tokens) => ReadSByte(ref tokens, SignedIntegerParseStyle, _numberFormatInfo);
        public sbyte ReadSByte(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider)
        {
            if (!TryReadSByte(ref tokens, style, provider, out sbyte value))
            {
                ThrowParseException(tokens.Current, "sbyte");
            }

            return value;
        }

        public byte ReadByte(ref ReadOnlySpanTokenizer<char> tokens) => ReadByte(ref tokens, UnsignedIntegerParseStype, _numberFormatInfo);
        public byte ReadByte(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider)
        {
            if (!TryReadByte(ref tokens, style, provider, out byte value))
            {
                ThrowParseException(tokens.Current, "byte");
            }

            return value;
        }

        public BigInteger ReadBigInteger(ref ReadOnlySpanTokenizer<char> tokens) => ReadBigInteger(ref tokens, SignedIntegerParseStyle, _numberFormatInfo);
        public BigInteger ReadBigInteger(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider)
        {
            if (!TryReadBigInteger(ref tokens, style, provider, out BigInteger value))
            {
                ThrowParseException(tokens.Current, "BigInteger");
            }

            return value;
        }
    }
}
