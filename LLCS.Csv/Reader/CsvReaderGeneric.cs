using System.Collections;
using System.Globalization;
using System.Text;

namespace LLCS.Csv.Reader
{
    public sealed class CsvReader<T> : IEnumerable<T>, IDisposable
        where T : ICsvSerializer, new()
    {
        private readonly CsvReader _reader;

        internal bool CanReadMore => _reader.CanReadMore;

        public CsvReader(StreamReader stream) : this(stream, CultureInfo.CurrentCulture)
        { }

        public CsvReader(StreamReader stream, string culture) : this(stream, CultureInfo.GetCultureInfo(culture))
        { }

        public CsvReader(StreamReader stream, CultureInfo culture)
        {
            _reader = new CsvReader(stream, culture);
        }

        public static CsvReader<T> FromFile(string csvPath)
        {
            return new CsvReader<T>(new StreamReader(csvPath));
        }

        public static CsvReader<T> FromStream(StreamReader stream)
        {
            return new CsvReader<T>(stream);
        }

        public static CsvReader<T> FromString(string csv)
        {
            return FromString(csv, CultureInfo.CurrentCulture);
        }

        public static CsvReader<T> FromString(string csv, string culture)
        {
            return FromString(csv, CultureInfo.GetCultureInfo(culture));
        }

        public static CsvReader<T> FromString(string csv, CultureInfo culture)
        {
            return new CsvReader<T>(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(csv))), culture);
        }

        public ValueTask<T> ReadRecordAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new CsvReaderIterator<T>(_reader);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CsvReaderIterator<T>(_reader);
        }
    }
}
