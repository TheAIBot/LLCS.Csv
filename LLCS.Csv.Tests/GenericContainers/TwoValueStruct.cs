namespace LLCS.Csv.Tests.Reader
{
    public record struct TwoValueStruct<T1, T2>(T1 Value1, T2 Value2) : ICsvSerializer
    {
        public bool TrySerialize(CsvReader reader)
        {
            if (!reader.TryRead(false, out T1 value1))
            {
                return false;
            }
            Value1 = value1;

            if (!reader.TryRead(true, out T2 value2))
            {
                return false;
            }
            Value2 = value2;

            return true;
        }
    }
}