using LLCS.Csv.Reader;
using LLCS.Csv.Writer;
using Microsoft.Toolkit.HighPerformance.Enumerables;

namespace LLCS.Csv.Tests.GenericContainers
{
    public record struct OneValueStruct<T>(T Value) : ICsvSerializer
    {
        public void Serialize(CsvWriter writer)
        {
            writer.Write(Value);
        }

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens)
        {
            if (!reader.TryRead(ref tokens, out T value))
            {
                return false;
            }
            Value = value;

            return true;
        }
    }
}