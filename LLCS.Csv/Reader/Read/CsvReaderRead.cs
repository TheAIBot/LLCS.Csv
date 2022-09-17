using CommunityToolkit.HighPerformance.Enumerables;

namespace LLCS.Csv.Reader
{
    public sealed partial class CsvReader
    {
        public long ReadLong(ref ReadOnlySpanTokenizer<char> tokens)
        {
            if (!TryReadLong(ref tokens, out long value))
            {
                ThrowParseException(tokens.Current, "long");
            }

            return value;
        }

        public int ReadInt(ref ReadOnlySpanTokenizer<char> tokens)
        {
            if (!TryReadInt(ref tokens, out int value))
            {
                ThrowParseException(tokens.Current, "int");
            }

            return value;
        }

        public short ReadShort(ref ReadOnlySpanTokenizer<char> tokens)
        {
            if (!TryReadShort(ref tokens, out short value))
            {
                ThrowParseException(tokens.Current, "short");
            }

            return value;
        }

        public byte ReadByte(ref ReadOnlySpanTokenizer<char> tokens)
        {
            if (!TryReadByte(ref tokens, out byte value))
            {
                ThrowParseException(tokens.Current, "byte");
            }

            return value;
        }
    }
}
