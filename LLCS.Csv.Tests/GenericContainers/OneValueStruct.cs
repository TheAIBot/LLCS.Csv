namespace LLCS.Csv.Tests.Reader
{
    public record struct OneValueStruct<T>(T Value) : ICsvSerializer
    {
        public bool TrySerialize(CsvReader reader)
        {
            if (!reader.TryRead(true, out T value))
            {
                return false;
            }
            Value = value;

            return true;
        }
    }
}