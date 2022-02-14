using Microsoft.Toolkit.HighPerformance;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Enumerables;
using System.Text;

namespace LLCS.Csv
{

    public sealed class CsvReader : IDisposable
    {
        private readonly StreamReader _stream;
        private readonly CultureInfo _culture;
        private readonly NumberFormatInfo _numberFormatInfo;
        private readonly char _separator;
        private bool _endOfFile = false;
        private char[] _bufferArray;
        private Memory<char> _buffer;
        private const NumberStyles SignedIntegerParseStyle = NumberStyles.Integer;
        private const NumberStyles UnsignedIntegerParseStype = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowThousands;

        internal bool CanReadMore => !_endOfFile || _buffer.Length > 0;

        public CsvReader(StreamReader stream) : this(stream, CultureInfo.CurrentCulture)
        { }

        public CsvReader(StreamReader stream, string culture) : this(stream, CultureInfo.GetCultureInfo(culture))
        { }

        public CsvReader(StreamReader stream, CultureInfo culture)
        {
            _stream = stream;
            _culture = culture;
            _numberFormatInfo = culture.NumberFormat;
            _separator = culture.TextInfo.ListSeparator[0];
            _endOfFile = false;
            _bufferArray = new char[1024 * 16];
            _buffer = new Memory<char>();

            ReadIntoBuffer();
        }

        public static CsvReader FromFile(string csvPath)
        {
            return new CsvReader(new StreamReader(csvPath));
        }

        public static CsvReader FromStream(StreamReader stream)
        {
            return new CsvReader(stream);
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
            return new CsvReader(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(csv))), culture);
        }

        public long ReadLong(ref ReadOnlySpanTokenizer<char> tokens)
        {
            if (!TryReadLong(ref tokens, out long value))
            {
                ThrowParseException(tokens.Current, "long");
            }

            return value;
        }

        public int ReadInt(ref ReadOnlySpanTokenizer<char> tokens)
        {
            if (!TryReadInt(ref tokens, out int value))
            {
                ThrowParseException(tokens.Current, "int");
            }

            return value;
        }

        public short ReadShort(ref ReadOnlySpanTokenizer<char> tokens)
        {
            if (!TryReadShort(ref tokens, out short value))
            {
                ThrowParseException(tokens.Current, "short");
            }

            return value;
        }

        public byte ReadByte(ref ReadOnlySpanTokenizer<char> tokens)
        {
            if (!TryReadByte(ref tokens, out byte value))
            {
                ThrowParseException(tokens.Current, "byte");
            }

            return value;
        }

        public bool TryReadLong(ref ReadOnlySpanTokenizer<char> tokens, out long value)
        {
            tokens.MoveNext();
            return long.TryParse(tokens.Current, SignedIntegerParseStyle, _numberFormatInfo, out value);
        }

        public bool TryReadULong(ref ReadOnlySpanTokenizer<char> tokens, out ulong value)
        {
            tokens.MoveNext();
            return ulong.TryParse(tokens.Current, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        }

        public bool TryReadInt(ref ReadOnlySpanTokenizer<char> tokens, out int value)
        {
            tokens.MoveNext();
            return int.TryParse(tokens.Current, SignedIntegerParseStyle, _numberFormatInfo, out value);
        }

        public bool TryReadUInt(ref ReadOnlySpanTokenizer<char> tokens, out uint value)
        {
            tokens.MoveNext();
            return uint.TryParse(tokens.Current, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        }

        public bool TryReadNInt(ref ReadOnlySpanTokenizer<char> tokens, out nint value)
        {
            tokens.MoveNext();
            return nint.TryParse(tokens.Current, SignedIntegerParseStyle, _numberFormatInfo, out value);
        }

        public bool TryReadNUInt(ref ReadOnlySpanTokenizer<char> tokens, out nuint value)
        {
            tokens.MoveNext();
            return nuint.TryParse(tokens.Current, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        }

        public bool TryReadShort(ref ReadOnlySpanTokenizer<char> tokens, out short value)
        {
            tokens.MoveNext();
            return short.TryParse(tokens.Current, SignedIntegerParseStyle, _numberFormatInfo, out value);
        }

        public bool TryReadUShort(ref ReadOnlySpanTokenizer<char> tokens, out ushort value)
        {
            tokens.MoveNext();
            return ushort.TryParse(tokens.Current, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        }

        public bool TryReadSByte(ref ReadOnlySpanTokenizer<char> tokens, out sbyte value)
        {
            tokens.MoveNext();
            return sbyte.TryParse(tokens.Current, SignedIntegerParseStyle, _numberFormatInfo, out value);
        }

        public bool TryReadByte(ref ReadOnlySpanTokenizer<char> tokens, out byte value)
        {
            tokens.MoveNext();
            return byte.TryParse(tokens.Current, UnsignedIntegerParseStype, _numberFormatInfo, out value);
        }

        public bool TryRead(ref ReadOnlySpanTokenizer<char> tokens, out int value)
        {
            return TryReadInt(ref tokens, out value);
        }

        public bool TryRead<T>(ref ReadOnlySpanTokenizer<char> tokens, out T value)
        {
            if (typeof(T) == typeof(long))
            {
                long valueRead;
                bool couldRead = TryReadLong(ref tokens, out valueRead);
                value = Unsafe.As<long, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(ulong))
            {
                ulong valueRead;
                bool couldRead = TryReadULong(ref tokens, out valueRead);
                value = Unsafe.As<ulong, T>(ref valueRead);
                return couldRead;
            }
            /*else*/
            if (typeof(T) == typeof(int))
            {
                int valueRead;
                bool couldRead = TryReadInt(ref tokens, out valueRead);
                value = Unsafe.As<int, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(uint))
            {
                uint valueRead;
                bool couldRead = TryReadUInt(ref tokens, out valueRead);
                value = Unsafe.As<uint, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(nint))
            {
                nint valueRead;
                bool couldRead = TryReadNInt(ref tokens, out valueRead);
                value = Unsafe.As<nint, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(nuint))
            {
                nuint valueRead;
                bool couldRead = TryReadNUInt(ref tokens, out valueRead);
                value = Unsafe.As<nuint, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(short))
            {
                short valueRead;
                bool couldRead = TryReadShort(ref tokens, out valueRead);
                value = Unsafe.As<short, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(ushort))
            {
                ushort valueRead;
                bool couldRead = TryReadUShort(ref tokens, out valueRead);
                value = Unsafe.As<ushort, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(sbyte))
            {
                sbyte valueRead;
                bool couldRead = TryReadSByte(ref tokens, out valueRead);
                value = Unsafe.As<sbyte, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(byte))
            {
                byte valueRead;
                bool couldRead = TryReadByte(ref tokens, out valueRead);
                value = Unsafe.As<byte, T>(ref valueRead);
                return couldRead;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public bool TryReadRecord<T>(out T record) where T : ICsvSerializer, new()
        {
            ReadOnlySpan<char> recordChars = GetRecordChars();

            record = new T();
            var tokens = recordChars.Tokenize(_separator);
            bool couldParse = record.TrySerialize(this, ref tokens);
            AdvanceBuffer(recordChars);
            return couldParse;
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

        private void ThrowParseException(ReadOnlySpan<char> token, string type)
        {
            throw new InvalidDataException($"Failed to parse {type}. Text: \"{token}\"");
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }

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

        private struct CsvReaderIterator<U> : IEnumerator<U>
            where U : ICsvSerializer, new()
        {
            private readonly CsvReader _reader;
            public U Current { get; private set; }
            object IEnumerator.Current => Current;

            public CsvReaderIterator(CsvReader reader)
            {
                _reader = reader;
                Current = default(U)!;
            }

            public bool MoveNext()
            {
                if (!_reader.CanReadMore)
                {
                    return false;
                }

                U record;
                bool parsed = _reader.TryReadRecord(out record);

                Current = record;
                return parsed;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public void Dispose()
            {
            }
        }
    }
}