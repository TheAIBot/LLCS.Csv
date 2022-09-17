namespace LLCS.Csv.Writer
{
    public sealed partial class CsvWriter : IDisposable
    {
        public void Write<T>(T value) where T : ISpanFormattable => Write(value, default, _numberFormatInfo);

        public void Write<T>(T value, ReadOnlySpan<char> format) where T : ISpanFormattable => Write(value, format, _numberFormatInfo);

        public void Write<T>(T value, IFormatProvider? provider) where T : ISpanFormattable => Write(value, default, provider);

        public void Write<T>(T value, ReadOnlySpan<char> format, IFormatProvider? provider) where T : ISpanFormattable
        {
            int charsWritten;
            while (!value.TryFormat(_buffer.Span, out charsWritten, format, provider))
            {
                WriteBufferToStreamMaybeExpandBuffer();
            }

            _buffer = _buffer.Slice(charsWritten);
        }
    }
}
