using Microsoft.Toolkit.HighPerformance;
using System.Globalization;
using System.Text;

namespace LLCS.Csv.Reader
{
    public sealed partial class CsvReader : IDisposable
    {
        private readonly StreamReader _stream;
        private readonly CultureInfo _cultureInfo;
        private readonly NumberFormatInfo _numberFormatInfo;
        private readonly DateTimeFormatInfo _dateTimeFormatInfo;
        private readonly char _separator;
        private bool _endOfFile = false;
        private char[] _bufferArray;
        private Memory<char> _buffer;
        private const NumberStyles SignedIntegerParseStyle = NumberStyles.Integer;
        private const NumberStyles UnsignedIntegerParseStype = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowThousands;
        private const NumberStyles FloatParseStyle = NumberStyles.Float | NumberStyles.AllowThousands;
        private const DateTimeStyles DateParseStyle = DateTimeStyles.None;

        internal bool CanReadMore => !_endOfFile || _buffer.Length > 0;

        public CsvReader(StreamReader stream) : this(stream, CultureInfo.CurrentCulture)
        { }

        public CsvReader(StreamReader stream, string culture) : this(stream, CultureInfo.GetCultureInfo(culture))
        { }

        public CsvReader(StreamReader stream, CultureInfo culture)
        {
            _stream = stream;
            _cultureInfo = culture;
            _numberFormatInfo = culture.NumberFormat;
            _dateTimeFormatInfo = culture.DateTimeFormat;
            _separator = culture.TextInfo.ListSeparator[0];
            _endOfFile = false;
            _bufferArray = new char[1024 * 16];
            _buffer = new Memory<char>();

            ReadIntoBuffer();
        }

        public static CsvReader FromFile(string csvPath)
        {
            return FromFile(csvPath, CultureInfo.CurrentCulture);
        }

        public static CsvReader FromFile(string csvPath, string culture)
        {
            return FromFile(csvPath, CultureInfo.GetCultureInfo(culture));
        }

        public static CsvReader FromFile(string csvPath, CultureInfo culture)
        {
            return FromStream(new StreamReader(csvPath), culture);
        }

        public static CsvReader FromString(string csv)
        {
            return FromString(csv, CultureInfo.CurrentCulture);
        }

        public static CsvReader FromString(string csv, string culture)
        {
            return FromString(csv, CultureInfo.GetCultureInfo(culture));
        }

        public static CsvReader FromString(string csv, CultureInfo culture)
        {
            return FromStream(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(csv))), culture);
        }

        public static CsvReader FromStream(StreamReader stream)
        {
            return FromStream(stream, CultureInfo.CurrentCulture);
        }

        public static CsvReader FromStream(StreamReader stream, string culture)
        {
            return FromStream(stream, CultureInfo.GetCultureInfo(culture));
        }

        public static CsvReader FromStream(StreamReader stream, CultureInfo culture)
        {
            return new CsvReader(stream, culture);
        }

        private void AdvanceBuffer(ReadOnlySpan<char> cell)
        {
            _buffer = _buffer.Slice(Math.Min(_buffer.Length, cell.Length + 1));
        }

        private ReadOnlySpan<char> GetRecordChars()
        {
            do
            {
                ReadOnlySpan<char> bufferData = _buffer.Span;
                int sepIndex = bufferData.IndexOf('\n');
                if (sepIndex != -1)
                {
                    return bufferData.Slice(0, sepIndex);
                }

            } while (ReadMoreData());

            // Last cell is what remains in the buffer
            return _buffer.Span;
        }

        private async ValueTask<ReadOnlyMemory<char>> GetRecordCharsAsync(CancellationToken cancellationToken)
        {
            do
            {
                int sepIndex = _buffer.Span.IndexOf('\n');
                if (sepIndex != -1)
                {
                    return _buffer.Slice(0, sepIndex);
                }

            } while (await ReadMoreDataAsync(cancellationToken));

            // Last cell is what remains in the buffer
            return _buffer;
        }

        private bool ReadMoreData()
        {
            if (_endOfFile)
            {
                return false;
            }

            if (_buffer.Length == _bufferArray.Length)
            {
                ExtendBuffer();
            }
            else
            {
                MoveDataToStartOfBufferBuffer();
            }

            ReadIntoBuffer();

            return true;
        }

        private async ValueTask<bool> ReadMoreDataAsync(CancellationToken cancellationToken)
        {
            if (_endOfFile)
            {
                return false;
            }

            if (_buffer.Length == _bufferArray.Length)
            {
                ExtendBuffer();
            }
            else
            {
                MoveDataToStartOfBufferBuffer();
            }

            await ReadIntoBufferAsync(cancellationToken);

            return true;
        }

        private void MoveDataToStartOfBufferBuffer()
        {
            Memory<char> currentDataAtStart = new Memory<char>(_bufferArray, 0, _buffer.Length);
            _buffer.CopyTo(currentDataAtStart);
            _buffer = currentDataAtStart;
        }

        private void ExtendBuffer()
        {
            // Move data to start of extended buffer which then replaced the current buffer
            char[] extendedBuffer = new char[_bufferArray.Length * 2];
            Memory<char> extendedBufferMem = new Memory<char>(extendedBuffer);
            _buffer.CopyTo(extendedBufferMem);

            _bufferArray = extendedBuffer;
            _buffer = extendedBufferMem.Slice(0, _buffer.Length);
        }

        private void ReadIntoBuffer()
        {
            // Read data into remaining space
            int charsToRead = _bufferArray.Length - _buffer.Length;
            int charsRead = _stream.Read(_bufferArray.AsSpan(_buffer.Length));
            if (charsRead < charsToRead)
            {
                _endOfFile = true;
            }

            // Extend buffer with newly read data
            _buffer = new Memory<char>(_bufferArray, 0, _buffer.Length + charsRead);
        }

        private async ValueTask ReadIntoBufferAsync(CancellationToken cancellationToken)
        {
            // Read data into remaining space
            int charsToRead = _bufferArray.Length - _buffer.Length;
            int charsRead = await _stream.ReadAsync(_bufferArray.AsMemory(_buffer.Length), cancellationToken);
            if (charsRead < charsToRead)
            {
                _endOfFile = true;
            }

            // Extend buffer with newly read data
            _buffer = new Memory<char>(_bufferArray, 0, _buffer.Length + charsRead);
        }

        private void ThrowParseException(ReadOnlySpan<char> token, string type)
        {
            throw new InvalidDataException($"Failed to parse {type}. Text: \"{token}\"");
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}