using Microsoft.Toolkit.HighPerformance.Enumerables;

namespace LLCS.Csv
{
    public interface ICsvSerializer
    {
        bool TrySerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens);
    }
}