using System.Globalization;

namespace LLCS.Csv.Writer
{
    public sealed partial class CsvWriter : IDisposable
    {
        private readonly StreamWriter _stream;
        private readonly NumberFormatInfo _numberFormatInfo;
        private readonly char _separator;
        private char[] _bufferArray;
        private Memory<char> _buffer;

        public CsvWriter(StreamWriter stream) : this(stream, CultureInfo.CurrentCulture)
        { }

        public CsvWriter(StreamWriter stream, string culture) : this(stream, CultureInfo.GetCultureInfo(culture))
        { }

        public CsvWriter(StreamWriter stream, CultureInfo culture)
        {
            _stream = stream;
            _numberFormatInfo = culture.NumberFormat;
            _separator = culture.TextInfo.ListSeparator[0];
            _bufferArray = new char[1024 * 1024 * 16];
            _buffer = _bufferArray;
        }

        public static CsvWriter ToFile(string csvPath)
        {
            return new CsvWriter(new StreamWriter(csvPath));
        }

        public static CsvWriter ToFile(string csvPath, string culture)
        {
            return new CsvWriter(new StreamWriter(csvPath), culture);
        }

        public static CsvWriter ToStream(StreamWriter stream)
        {
            return new CsvWriter(stream);
        }

        public static CsvWriter ToStream(StreamWriter stream, string culture)
        {
            return new CsvWriter(stream, culture);
        }

        public void Write(string value)
        {
            Write(value.AsSpan());
        }

        public void Write(ReadOnlySpan<char> value)
        {
            if (value.Length > 32)
            {
                _stream.Write(value);
            }
            else
            {
                while (!value.TryCopyTo(_buffer.Span))
                {
                    WriteBufferToStreamMaybeExpandBuffer();
                }
            }

            _buffer = _buffer.Slice(value.Length);
        }

        public void WriteSeparator()
        {
            if (_buffer.Length < 1)
            {
                WriteBufferToStreamMaybeExpandBuffer();
            }

            _buffer.Span[0] = _separator;
            _buffer = _buffer.Slice(1);
        }

        public void WriteNewLine()
        {
            Write(Environment.NewLine);
        }

        private void WriteBufferToStreamMaybeExpandBuffer()
        {
            int usedBufferBytesCount = _bufferArray.Length - _buffer.Length;
            if (_buffer.Length < _bufferArray.Length)
            {
                _stream.Write(_bufferArray.AsSpan(0, usedBufferBytesCount));
                _buffer = _bufferArray;
            }
            else
            {
                Array.Resize(ref _bufferArray, _bufferArray.Length * 2);
                _buffer = _bufferArray.AsMemory(usedBufferBytesCount);
            }
        }

        public void Flush()
        {
            int usedBufferBytesCount = _bufferArray.Length - _buffer.Length;
            _stream.Write(_bufferArray.AsSpan(0, usedBufferBytesCount));
            _buffer = _bufferArray;
        }

        public void Dispose()
        {
            Flush();
            _stream.Flush();
            _stream.Dispose();
        }
    }
}