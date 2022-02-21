using Microsoft.Toolkit.HighPerformance.Enumerables;

namespace LLCS.Csv
{
    public interface ICsvSerializer
    {
        bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens);
    }
}