﻿using CommunityToolkit.HighPerformance.Enumerables;
using System.Globalization;
using System.Numerics;

namespace LLCS.Csv.Reader
{
    public sealed partial class CsvReader
    {
        public bool TryReadLong(ref ReadOnlySpanTokenizer<char> tokens, out long value) => TryReadLong(ref tokens, SignedIntegerParseStyle, _numberFormatInfo, out value);
        public bool TryReadLong(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out long value)
        {
            value = default;
            return tokens.MoveNext() && long.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadULong(ref ReadOnlySpanTokenizer<char> tokens, out ulong value) => TryReadULong(ref tokens, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        public bool TryReadULong(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out ulong value)
        {
            value = default;
            return tokens.MoveNext() && ulong.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadInt(ref ReadOnlySpanTokenizer<char> tokens, out int value) => TryReadInt(ref tokens, SignedIntegerParseStyle, _numberFormatInfo, out value);
        public bool TryReadInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out int value)
        {
            value = default;
            return tokens.MoveNext() && int.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadUInt(ref ReadOnlySpanTokenizer<char> tokens, out uint value) => TryReadUInt(ref tokens, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        public bool TryReadUInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out uint value)
        {
            value = default;
            return tokens.MoveNext() && uint.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadNInt(ref ReadOnlySpanTokenizer<char> tokens, out nint value) => TryReadNInt(ref tokens, SignedIntegerParseStyle, _numberFormatInfo, out value);
        public bool TryReadNInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out nint value)
        {
            value = default;
            return tokens.MoveNext() && nint.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadNUInt(ref ReadOnlySpanTokenizer<char> tokens, out nuint value) => TryReadNUInt(ref tokens, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        public bool TryReadNUInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out nuint value)
        {
            value = default;
            return tokens.MoveNext() && nuint.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadShort(ref ReadOnlySpanTokenizer<char> tokens, out short value) => TryReadShort(ref tokens, SignedIntegerParseStyle, _numberFormatInfo, out value);
        public bool TryReadShort(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out short value)
        {
            value = default;
            return tokens.MoveNext() && short.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadUShort(ref ReadOnlySpanTokenizer<char> tokens, out ushort value) => TryReadUShort(ref tokens, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        public bool TryReadUShort(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out ushort value)
        {
            value = default;
            return tokens.MoveNext() && ushort.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadSByte(ref ReadOnlySpanTokenizer<char> tokens, out sbyte value) => TryReadSByte(ref tokens, SignedIntegerParseStyle, _numberFormatInfo, out value);
        public bool TryReadSByte(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out sbyte value)
        {
            value = default;
            return tokens.MoveNext() && sbyte.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadByte(ref ReadOnlySpanTokenizer<char> tokens, out byte value) => TryReadByte(ref tokens, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        public bool TryReadByte(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out byte value)
        {
            value = default;
            return tokens.MoveNext() && byte.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadBigInteger(ref ReadOnlySpanTokenizer<char> tokens, out BigInteger value) => TryReadBigInteger(ref tokens, SignedIntegerParseStyle, _numberFormatInfo, out value);
        public bool TryReadBigInteger(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out BigInteger value)
        {
            value = default;
            return tokens.MoveNext() && BigInteger.TryParse(tokens.Current, style, provider, out value);
        }
    }
}
