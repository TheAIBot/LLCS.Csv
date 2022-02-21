using LLCS.Csv.Reader;
using Microsoft.Toolkit.HighPerformance.Enumerables;

namespace LLCS.Csv.Tests.Reader
{
    public record struct OneValueStruct<T>(T Value) : ICsvSerializer
    {
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