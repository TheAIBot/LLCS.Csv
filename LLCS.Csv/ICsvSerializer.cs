using CommunityToolkit.HighPerformance.Enumerables;
using LLCS.Csv.Reader;
using LLCS.Csv.Writer;

namespace LLCS.Csv
{
    public interface ICsvSerializer
    {
        void Serialize(CsvWriter writer);
        bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens);
    }
}