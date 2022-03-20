using LLCS.Csv.Reader;
using LLCS.Csv.Writer;
using Microsoft.Toolkit.HighPerformance.Enumerables;

namespace LLCS.Csv
{
    public interface ICsvSerializer
    {
        void Serialize(CsvWriter writer);
        bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens);
    }
}