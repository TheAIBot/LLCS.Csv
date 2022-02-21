using Microsoft.Toolkit.HighPerformance.Enumerables;

namespace LLCS.Csv.Tests.Reader
{
    public record struct TwoValueStruct<T1, T2>(T1 Value1, T2 Value2) : ICsvSerializer
    {
        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens)
        {
            if (!reader.TryRead(ref tokens, out T1 value1))
            {
                return false;
            }
            Value1 = value1;

            if (!reader.TryRead(ref tokens, out T2 value2))
            {
                return false;
            }
            Value2 = value2;

            return true;
        }
    }
}