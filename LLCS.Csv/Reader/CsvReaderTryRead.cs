using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance;
using Microsoft.Toolkit.HighPerformance.Enumerables;

namespace LLCS.Csv.Reader
{
    public sealed partial class CsvReader
    {
        public bool TryReadLong(ref ReadOnlySpanTokenizer<char> tokens, out long value)
        {
            tokens.MoveNext();
            return long.TryParse(tokens.Current, SignedIntegerParseStyle, _numberFormatInfo, out value);
        }

        public bool TryReadULong(ref ReadOnlySpanTokenizer<char> tokens, out ulong value)
        {
            tokens.MoveNext();
            return ulong.TryParse(tokens.Current, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        }

        public bool TryReadInt(ref ReadOnlySpanTokenizer<char> tokens, out int value)
        {
            tokens.MoveNext();
            return int.TryParse(tokens.Current, SignedIntegerParseStyle, _numberFormatInfo, out value);
        }

        public bool TryReadUInt(ref ReadOnlySpanTokenizer<char> tokens, out uint value)
        {
            tokens.MoveNext();
            return uint.TryParse(tokens.Current, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        }

        public bool TryReadNInt(ref ReadOnlySpanTokenizer<char> tokens, out nint value)
        {
            tokens.MoveNext();
            return nint.TryParse(tokens.Current, SignedIntegerParseStyle, _numberFormatInfo, out value);
        }

        public bool TryReadNUInt(ref ReadOnlySpanTokenizer<char> tokens, out nuint value)
        {
            tokens.MoveNext();
            return nuint.TryParse(tokens.Current, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        }

        public bool TryReadShort(ref ReadOnlySpanTokenizer<char> tokens, out short value)
        {
            tokens.MoveNext();
            return short.TryParse(tokens.Current, SignedIntegerParseStyle, _numberFormatInfo, out value);
        }

        public bool TryReadUShort(ref ReadOnlySpanTokenizer<char> tokens, out ushort value)
        {
            tokens.MoveNext();
            return ushort.TryParse(tokens.Current, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        }

        public bool TryReadSByte(ref ReadOnlySpanTokenizer<char> tokens, out sbyte value)
        {
            tokens.MoveNext();
            return sbyte.TryParse(tokens.Current, SignedIntegerParseStyle, _numberFormatInfo, out value);
        }

        public bool TryReadByte(ref ReadOnlySpanTokenizer<char> tokens, out byte value)
        {
            tokens.MoveNext();
            return byte.TryParse(tokens.Current, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        }

        public bool TryReadString(ref ReadOnlySpanTokenizer<char> tokens, out string value)
        {
            tokens.MoveNext();
            value = tokens.Current.ToString();
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

        public async ValueTask<(T Value, bool couldRead)> TryReadRecordAsync<T>() where T : ICsvSerializer, new()
        {
            ReadOnlyMemory<char> recordChars = await GetRecordCharsAsync();
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
