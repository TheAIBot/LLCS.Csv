using System.Globalization;

namespace LLCS.Csv.Writer
{
    public sealed class CsvWriter<T> : IDisposable
            where T : ICsvSerializer
    {
        private readonly CsvWriter _writer;

        public CsvWriter(StreamWriter stream) : this(stream, CultureInfo.CurrentCulture)
        { }

        public CsvWriter(StreamWriter stream, string culture) : this(stream, CultureInfo.GetCultureInfo(culture))
        { }

        public CsvWriter(StreamWriter stream, CultureInfo culture)
        {
            _writer = new CsvWriter(stream, culture);
        }

        public static CsvWriter<T> ToFile(string csvPath)
        {
            return new CsvWriter<T>(new StreamWriter(csvPath));
        }

        public static CsvWriter<T> ToFile(string csvPath, string culture)
        {
            return new CsvWriter<T>(new StreamWriter(csvPath), culture);
        }

        public static CsvWriter<T> ToStream(StreamWriter stream)
        {
            return new CsvWriter<T>(stream);
        }

        public static CsvWriter<T> ToStream(StreamWriter stream, string culture)
        {
            return new CsvWriter<T>(stream, culture);
        }

        public static CsvWriter<T> ToStream(StreamWriter stream, CultureInfo culture)
        {
            return new CsvWriter<T>(stream, culture);
        }

        public void WriteRecord(T record)
        {
            record.Serialize(_writer);
            _writer.WriteNewLine();
        }

        public void Flush()
        {
            _writer.Flush();
        }

        public void Dispose()
        {
            Flush();
        }
    }
}
