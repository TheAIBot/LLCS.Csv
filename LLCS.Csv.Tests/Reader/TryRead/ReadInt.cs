using LLCS.Csv.Reader;
using LLCS.Csv.Writer;
using Microsoft.Toolkit.HighPerformance.Enumerables;

namespace LLCS.Csv.Tests.Reader.TryRead;

public sealed class TryReadInt
{
    private sealed record IntStorage : ICsvSerializer
    {
        public int Value;

        public void Serialize(CsvWriter writer) => writer.Write(Value);

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens) => reader.TryReadInt(ref tokens, out Value);
    }
}
