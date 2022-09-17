using Microsoft.Toolkit.HighPerformance;
using Microsoft.Toolkit.HighPerformance.Enumerables;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace LLCS.Csv.Reader
{
    public sealed partial class CsvReader
    {
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
