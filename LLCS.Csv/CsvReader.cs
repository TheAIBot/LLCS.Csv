using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace LLCS.Csv
{

    public sealed class CsvReader : IDisposable
    {
        private readonly StreamReader _stream;
        private readonly CultureInfo _culture;
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
            _separator = culture.TextInfo.ListSeparator[0];
            _endOfFile = false;
            _bufferArray = new char[1024];
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

        public long ReadLong(bool lastCellInRecord)
        {
            if (!TryReadLong(lastCellInRecord, out long value))
            {
                ThrowParseException(lastCellInRecord, "long");
            }

            return value;
        }
        public int ReadInt(bool lastCellInRecord)
        {
            if (!TryReadInt(lastCellInRecord, out int value))
            {
                ThrowParseException(lastCellInRecord, "int");
            }

            return value;
        }
        public short ReadShort(bool lastCellInRecord)
        {
            if (!TryReadShort(lastCellInRecord, out short value))
            {
                ThrowParseException(lastCellInRecord, "short");
            }

            return value;
        }
        public byte ReadByte(bool lastCellInRecord)
        {
            if (!TryReadByte(lastCellInRecord, out byte value))
            {
                ThrowParseException(lastCellInRecord, "byte");
            }

            return value;
        }

        public bool TryReadLong(bool lastCellInRecord, out long value)
        {
            ReadOnlySpan<char> cell = ReadCell(lastCellInRecord);
            if (long.TryParse(cell, SignedIntegerParseStyle, _culture, out value))
            {
                AdvanceBuffer(cell);
                return true;
            }

            return false;
        }

        public bool TryReadULong(bool lastCellInRecord, out ulong value)
        {
            ReadOnlySpan<char> cell = ReadCell(lastCellInRecord);
            if (ulong.TryParse(cell, UnsignedIntegerParseStype, _culture, out value))
            {
                AdvanceBuffer(cell);
                return true;
            }

            return false;
        }

        public bool TryReadInt(bool lastCellInRecord, out int value)
        {
            ReadOnlySpan<char> cell = ReadCell(lastCellInRecord);
            if (int.TryParse(cell, SignedIntegerParseStyle, _culture, out value))
            {
                AdvanceBuffer(cell);
                return true;
            }

            return false;
        }

        public bool TryReadUInt(bool lastCellInRecord, out uint value)
        {
            ReadOnlySpan<char> cell = ReadCell(lastCellInRecord);
            if (uint.TryParse(cell, UnsignedIntegerParseStype, _culture, out value))
            {
                AdvanceBuffer(cell);
                return true;
            }

            return false;
        }

        public bool TryReadNInt(bool lastCellInRecord, out nint value)
        {
            ReadOnlySpan<char> cell = ReadCell(lastCellInRecord);
            if (nint.TryParse(cell, SignedIntegerParseStyle, _culture, out value))
            {
                AdvanceBuffer(cell);
                return true;
            }

            return false;
        }

        public bool TryReadNUInt(bool lastCellInRecord, out nuint value)
        {
            ReadOnlySpan<char> cell = ReadCell(lastCellInRecord);
            if (nuint.TryParse(cell, UnsignedIntegerParseStype, _culture, out value))
            {
                AdvanceBuffer(cell);
                return true;
            }

            return false;
        }

        public bool TryReadShort(bool lastCellInRecord, out short value)
        {
            ReadOnlySpan<char> cell = ReadCell(lastCellInRecord);
            if (short.TryParse(cell, SignedIntegerParseStyle, _culture, out value))
            {
                AdvanceBuffer(cell);
                return true;
            }

            return false;
        }

        public bool TryReadUShort(bool lastCellInRecord, out ushort value)
        {
            ReadOnlySpan<char> cell = ReadCell(lastCellInRecord);
            if (ushort.TryParse(cell, UnsignedIntegerParseStype, _culture, out value))
            {
                AdvanceBuffer(cell);
                return true;
            }

            return false;
        }

        public bool TryReadSByte(bool lastCellInRecord, out sbyte value)
        {
            ReadOnlySpan<char> cell = ReadCell(lastCellInRecord);
            if (sbyte.TryParse(cell, SignedIntegerParseStyle, _culture, out value))
            {
                AdvanceBuffer(cell);
                return true;
            }

            return false;
        }

        public bool TryReadByte(bool lastCellInRecord, out byte value)
        {
            ReadOnlySpan<char> cell = ReadCell(lastCellInRecord);
            if (byte.TryParse(cell, UnsignedIntegerParseStype, _culture, out value))
            {
                AdvanceBuffer(cell);
                return true;
            }

            return false;
        }

        public bool TryRead(bool lastCellInRecord, out int value)
        {
            return TryReadInt(lastCellInRecord, out value);
        }

        public bool TryRead<T>(bool lastCellInRecord, out T value)
        {
            if (typeof(T) == typeof(long))
            {
                long valueRead;
                bool couldRead = TryReadLong(lastCellInRecord, out valueRead);
                value = Unsafe.As<long, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(ulong))
            {
                ulong valueRead;
                bool couldRead = TryReadULong(lastCellInRecord, out valueRead);
                value = Unsafe.As<ulong, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(int)) 
            {
                int valueRead;
                bool couldRead = TryReadInt(lastCellInRecord, out valueRead);
                value = Unsafe.As<int, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(uint))
            {
                uint valueRead;
                bool couldRead = TryReadUInt(lastCellInRecord, out valueRead);
                value = Unsafe.As<uint, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(nint))
            {
                nint valueRead;
                bool couldRead = TryReadNInt(lastCellInRecord, out valueRead);
                value = Unsafe.As<nint, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(nuint))
            {
                nuint valueRead;
                bool couldRead = TryReadNUInt(lastCellInRecord, out valueRead);
                value = Unsafe.As<nuint, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(short))
            {
                short valueRead;
                bool couldRead = TryReadShort(lastCellInRecord, out valueRead);
                value = Unsafe.As<short, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(ushort))
            {
                ushort valueRead;
                bool couldRead = TryReadUShort(lastCellInRecord, out valueRead);
                value = Unsafe.As<ushort, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(sbyte))
            {
                sbyte valueRead;
                bool couldRead = TryReadSByte(lastCellInRecord, out valueRead);
                value = Unsafe.As<sbyte, T>(ref valueRead);
                return couldRead;
            }
            else if (typeof(T) == typeof(byte))
            {
                byte valueRead;
                bool couldRead = TryReadByte(lastCellInRecord, out valueRead);
                value = Unsafe.As<byte, T>(ref valueRead);
                return couldRead;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private void AdvanceBuffer(ReadOnlySpan<char> cell)
        {
            _buffer = _buffer.Slice(Math.Min(_buffer.Length, cell.Length + 1));
        }

        private ReadOnlySpan<char> ReadCell(bool lastCellInRecord)
        {
            char separator = lastCellInRecord ? '\n' : _separator;
            do
            {
                ReadOnlySpan<char> bufferData = _buffer.Span;
                int sepIndex = bufferData.IndexOf(separator);
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

        private void ThrowParseException(bool lastCellInRecord, string type)
        {
            ReadOnlySpan<char> cell = ReadCell(lastCellInRecord);
            throw new InvalidDataException($"Failed to parse {type}. Text: \"{cell}\"");
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

        public bool TryReadRecord(out T record)
        {
            record = new T();
            return record.TrySerialize(_reader);
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
            return new CsvReaderIterator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CsvReaderIterator<T>(this);
        }

        private struct CsvReaderIterator<U> : IEnumerator<U>
            where U : ICsvSerializer, new()
        {
            private readonly CsvReader<U> _reader;
            public U Current { get; private set; }
            object IEnumerator.Current => Current;

            public CsvReaderIterator(CsvReader<U> reader)
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