namespace LLCS.Csv
{
    public interface ICsvSerializer
    {
        bool TrySerialize(CsvReader reader);
    }
}