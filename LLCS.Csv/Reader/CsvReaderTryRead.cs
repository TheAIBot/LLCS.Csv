using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance;
using Microsoft.Toolkit.HighPerformance.Enumerables;

namespace LLCS.Csv.Reader
{
    public sealed partial class CsvReader
    {
        public bool TryReadLong(ref ReadOnlySpanTokenizer<char> tokens, out long value) => TryReadLong(ref tokens, SignedIntegerParseStyle, _numberFormatInfo, out value);
        public bool TryReadLong(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out long value) => TryReadLong(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadLong(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out long value) => TryReadLong(ref tokens, SignedIntegerParseStyle, provider, out value);
        public bool TryReadLong(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out long value)
        {
            value = default;
            return tokens.MoveNext() && long.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadULong(ref ReadOnlySpanTokenizer<char> tokens, out ulong value) => TryReadULong(ref tokens, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        public bool TryReadULong(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out ulong value) => TryReadULong(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadULong(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out ulong value) => TryReadULong(ref tokens, UnsignedIntegerParseStype, provider, out value);
        public bool TryReadULong(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out ulong value)
        {
            value = default;
            return tokens.MoveNext() && ulong.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadInt(ref ReadOnlySpanTokenizer<char> tokens, out int value) => TryReadInt(ref tokens, SignedIntegerParseStyle, _numberFormatInfo, out value);
        public bool TryReadInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out int value) => TryReadInt(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadInt(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out int value) => TryReadInt(ref tokens, SignedIntegerParseStyle, provider, out value);
        public bool TryReadInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out int value)
        {
            value = default;
            return tokens.MoveNext() && int.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadUInt(ref ReadOnlySpanTokenizer<char> tokens, out uint value) => TryReadUInt(ref tokens, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        public bool TryReadUInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out uint value) => TryReadUInt(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadUInt(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out uint value) => TryReadUInt(ref tokens, UnsignedIntegerParseStype, provider, out value);
        public bool TryReadUInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out uint value)
        {
            value = default;
            return tokens.MoveNext() && uint.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadNInt(ref ReadOnlySpanTokenizer<char> tokens, out nint value) => TryReadNInt(ref tokens, SignedIntegerParseStyle, _numberFormatInfo, out value);
        public bool TryReadNInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out nint value) => TryReadNInt(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadNInt(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out nint value) => TryReadNInt(ref tokens, SignedIntegerParseStyle, provider, out value);
        public bool TryReadNInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out nint value)
        {
            value = default;
            return tokens.MoveNext() && nint.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadNUInt(ref ReadOnlySpanTokenizer<char> tokens, out nuint value) => TryReadNUInt(ref tokens, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        public bool TryReadNUInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out nuint value) => TryReadNUInt(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadNUInt(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out nuint value) => TryReadNUInt(ref tokens, UnsignedIntegerParseStype, provider, out value);
        public bool TryReadNUInt(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out nuint value)
        {
            value = default;
            return tokens.MoveNext() && nuint.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadShort(ref ReadOnlySpanTokenizer<char> tokens, out short value) => TryReadShort(ref tokens, SignedIntegerParseStyle, _numberFormatInfo, out value);
        public bool TryReadShort(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out short value) => TryReadShort(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadShort(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out short value) => TryReadShort(ref tokens, SignedIntegerParseStyle, provider, out value);
        public bool TryReadShort(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out short value)
        {
            value = default;
            return tokens.MoveNext() && short.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadUShort(ref ReadOnlySpanTokenizer<char> tokens, out ushort value) => TryReadUShort(ref tokens, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        public bool TryReadUShort(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out ushort value) => TryReadUShort(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadUShort(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out ushort value) => TryReadUShort(ref tokens, UnsignedIntegerParseStype, provider, out value);
        public bool TryReadUShort(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out ushort value)
        {
            value = default;
            return tokens.MoveNext() && ushort.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadSByte(ref ReadOnlySpanTokenizer<char> tokens, out sbyte value) => TryReadSByte(ref tokens, SignedIntegerParseStyle, _numberFormatInfo, out value);
        public bool TryReadSByte(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out sbyte value) => TryReadSByte(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadSByte(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out sbyte value) => TryReadSByte(ref tokens, SignedIntegerParseStyle, provider, out value);
        public bool TryReadSByte(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out sbyte value)
        {
            value = default;
            return tokens.MoveNext() && sbyte.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadByte(ref ReadOnlySpanTokenizer<char> tokens, out byte value) => TryReadByte(ref tokens, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        public bool TryReadByte(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out byte value) => TryReadByte(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadByte(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out byte value) => TryReadByte(ref tokens, UnsignedIntegerParseStype, provider, out value);
        public bool TryReadByte(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out byte value)
        {
            value = default;
            return tokens.MoveNext() && byte.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadFloat(ref ReadOnlySpanTokenizer<char> tokens, out float value) => TryReadFloat(ref tokens, FloatParseStyle, _numberFormatInfo, out value);
        public bool TryReadFloat(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out float value) => TryReadFloat(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadFloat(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out float value) => TryReadFloat(ref tokens, FloatParseStyle, provider, out value);
        public bool TryReadFloat(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out float value)
        {
            value = default;
            return tokens.MoveNext() && float.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadDouble(ref ReadOnlySpanTokenizer<char> tokens, out double value) => TryReadDouble(ref tokens, FloatParseStyle, _numberFormatInfo, out value);
        public bool TryReadDouble(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out double value) => TryReadDouble(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadDouble(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out double value) => TryReadDouble(ref tokens, FloatParseStyle, provider, out value);
        public bool TryReadDouble(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out double value)
        {
            value = default;
            return tokens.MoveNext() && double.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadDecimal(ref ReadOnlySpanTokenizer<char> tokens, out decimal value) => TryReadDecimal(ref tokens, FloatParseStyle, _numberFormatInfo, out value);
        public bool TryReadDecimal(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out decimal value) => TryReadDecimal(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadDecimal(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out decimal value) => TryReadDecimal(ref tokens, FloatParseStyle, provider, out value);
        public bool TryReadDecimal(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out decimal value)
        {
            value = default;
            return tokens.MoveNext() && decimal.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadBigInteger(ref ReadOnlySpanTokenizer<char> tokens, out BigInteger value) => TryReadBigInteger(ref tokens, SignedIntegerParseStyle, _numberFormatInfo, out value);
        public bool TryReadBigInteger(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out BigInteger value) => TryReadBigInteger(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadBigInteger(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out BigInteger value) => TryReadBigInteger(ref tokens, SignedIntegerParseStyle, provider, out value);
        public bool TryReadBigInteger(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out BigInteger value)
        {
            value = default;
            return tokens.MoveNext() && BigInteger.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadDateTime(ref ReadOnlySpanTokenizer<char> tokens, out DateTime value) => TryReadDateTime(ref tokens, _dateTimeFormatInfo, DateParseStyle, out value);
        public bool TryReadDateTime(ref ReadOnlySpanTokenizer<char> tokens, DateTimeStyles styles, out DateTime value) => TryReadDateTime(ref tokens, _dateTimeFormatInfo, styles, out value);
        public bool TryReadDateTime(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, out DateTime value) => TryReadDateTime(ref tokens, provider, DateParseStyle, out value);
        public bool TryReadDateTime(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, DateTimeStyles styles, out DateTime value)
        {
            value = default;
            return tokens.MoveNext() && DateTime.TryParse(tokens.Current, provider, styles, out value);
        }

        public bool TryReadDateTimeOffset(ref ReadOnlySpanTokenizer<char> tokens, out DateTimeOffset value) => TryReadDateTimeOffset(ref tokens, _dateTimeFormatInfo, DateParseStyle, out value);
        public bool TryReadDateTimeOffset(ref ReadOnlySpanTokenizer<char> tokens, DateTimeStyles styles, out DateTimeOffset value) => TryReadDateTimeOffset(ref tokens, _dateTimeFormatInfo, styles, out value);
        public bool TryReadDateTimeOffset(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, out DateTimeOffset value) => TryReadDateTimeOffset(ref tokens, provider, DateParseStyle, out value);
        public bool TryReadDateTimeOffset(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, DateTimeStyles styles, out DateTimeOffset value)
        {
            value = default;
            return tokens.MoveNext() && DateTimeOffset.TryParse(tokens.Current, provider, styles, out value);
        }

        public bool TryReadDateOnly(ref ReadOnlySpanTokenizer<char> tokens, out DateOnly value) => TryReadDateOnly(ref tokens, _dateTimeFormatInfo, DateParseStyle, out value);
        public bool TryReadDateOnly(ref ReadOnlySpanTokenizer<char> tokens, DateTimeStyles styles, out DateOnly value) => TryReadDateOnly(ref tokens, _dateTimeFormatInfo, styles, out value);
        public bool TryReadDateOnly(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, out DateOnly value) => TryReadDateOnly(ref tokens, provider, DateParseStyle, out value);
        public bool TryReadDateOnly(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, DateTimeStyles styles, out DateOnly value)
        {
            value = default;
            return tokens.MoveNext() && DateOnly.TryParse(tokens.Current, provider, styles, out value);
        }

        public bool TryReadTimeOnly(ref ReadOnlySpanTokenizer<char> tokens, out TimeOnly value) => TryReadTimeOnly(ref tokens, _dateTimeFormatInfo, DateParseStyle, out value);
        public bool TryReadTimeOnly(ref ReadOnlySpanTokenizer<char> tokens, DateTimeStyles styles, out TimeOnly value) => TryReadTimeOnly(ref tokens, _dateTimeFormatInfo, styles, out value);
        public bool TryReadTimeOnly(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, out TimeOnly value) => TryReadTimeOnly(ref tokens, provider, DateParseStyle, out value);
        public bool TryReadTimeOnly(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, DateTimeStyles styles, out TimeOnly value)
        {
            value = default;
            return tokens.MoveNext() && TimeOnly.TryParse(tokens.Current, provider, styles, out value);
        }

        public bool TryReadString(ref ReadOnlySpanTokenizer<char> tokens, [NotNullWhen(true)] out string? value)
        {
            if (!tokens.MoveNext())
            {
                value = null;
                return false;
            }

            value = tokens.Current.ToString();
            return true;
        }

        public bool TryReadSpan(ref ReadOnlySpanTokenizer<char> tokens, out ReadOnlySpan<char> value)
        {
            if (!tokens.MoveNext())
            {
                value = null;
                return false;
            }

            value = tokens.Current;
            return true;
        }

        public bool TryRead<T>(ref ReadOnlySpanTokenizer<char> tokens, out T value)
        {
            if (IsSame<T, long>.Value)
            {
                long valueRead;
                bool couldRead = TryReadLong(ref tokens, out valueRead);
                value = Unsafe.As<long, T>(ref valueRead);
                return couldRead;
            }
            else if (IsSame<T, ulong>.Value)
            {
                ulong valueRead;
                bool couldRead = TryReadULong(ref tokens, out valueRead);
                value = Unsafe.As<ulong, T>(ref valueRead);
                return couldRead;
            }
            else if (IsSame<T, int>.Value)
            {
                int valueRead;
                bool couldRead = TryReadInt(ref tokens, out valueRead);
                value = Unsafe.As<int, T>(ref valueRead);
                return couldRead;
            }
            else if (IsSame<T, uint>.Value)
            {
                uint valueRead;
                bool couldRead = TryReadUInt(ref tokens, out valueRead);
                value = Unsafe.As<uint, T>(ref valueRead);
                return couldRead;
            }
            else if (IsSame<T, nint>.Value)
            {
                nint valueRead;
                bool couldRead = TryReadNInt(ref tokens, out valueRead);
                value = Unsafe.As<nint, T>(ref valueRead);
                return couldRead;
            }
            else if (IsSame<T, nuint>.Value)
            {
                nuint valueRead;
                bool couldRead = TryReadNUInt(ref tokens, out valueRead);
                value = Unsafe.As<nuint, T>(ref valueRead);
                return couldRead;
            }
            else if (IsSame<T, short>.Value)
            {
                short valueRead;
                bool couldRead = TryReadShort(ref tokens, out valueRead);
                value = Unsafe.As<short, T>(ref valueRead);
                return couldRead;
            }
            else if (IsSame<T, ushort>.Value)
            {
                ushort valueRead;
                bool couldRead = TryReadUShort(ref tokens, out valueRead);
                value = Unsafe.As<ushort, T>(ref valueRead);
                return couldRead;
            }
            else if (IsSame<T, sbyte>.Value)
            {
                sbyte valueRead;
                bool couldRead = TryReadSByte(ref tokens, out valueRead);
                value = Unsafe.As<sbyte, T>(ref valueRead);
                return couldRead;
            }
            else if (IsSame<T, byte>.Value)
            {
                byte valueRead;
                bool couldRead = TryReadByte(ref tokens, out valueRead);
                value = Unsafe.As<byte, T>(ref valueRead);
                return couldRead;
            }
            else if (IsSame<T, string>.Value)
            {
                var lol = tokens.Current.ToString();
                value = Unsafe.As<string, T>(ref lol);
                return true;
            }
            else
            {
                throw new Exception();
            }
        }

        public bool TryReadRecord<T>(out T record) where T : ICsvSerializer, new()
        {
            ReadOnlySpan<char> recordChars = GetRecordChars();

            record = new T();
            var tokens = recordChars.Tokenize(_separator);
            bool couldParse = record.TryDeSerialize(this, ref tokens);
            AdvanceBuffer(recordChars);
            return couldParse;
        }

        public async ValueTask<(T Value, bool couldRead)> TryReadRecordAsync<T>(CancellationToken cancellationToken) where T : ICsvSerializer, new()
        {
            ReadOnlyMemory<char> recordChars = await GetRecordCharsAsync(cancellationToken);
            return TryDeSerializeRecord<T>(recordChars.Span);
        }

        private (T Value, bool couldRead) TryDeSerializeRecord<T>(ReadOnlySpan<char> recordChars) where T : ICsvSerializer, new()
        {
            var record = new T();
            var tokens = recordChars.Tokenize(_separator);
            bool couldParse = record.TryDeSerialize(this, ref tokens);
            AdvanceBuffer(recordChars);
            return (record, couldParse);
        }
    }
}
