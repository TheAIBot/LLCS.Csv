using System.Globalization;
using Microsoft.Toolkit.HighPerformance.Enumerables;

namespace LLCS.Csv.Reader
{
    public sealed partial class CsvReader
    {
        public bool TryReadFloat(ref ReadOnlySpanTokenizer<char> tokens, out float value) => TryReadFloat(ref tokens, FloatParseStyle, _numberFormatInfo, out value);
        public bool TryReadFloat(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out float value) => TryReadFloat(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadFloat(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out float value) => TryReadFloat(ref tokens, FloatParseStyle, provider, out value);
        public bool TryReadFloat(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out float value)
        {
            value = default;
            return tokens.MoveNext() && float.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadDouble(ref ReadOnlySpanTokenizer<char> tokens, out double value) => TryReadDouble(ref tokens, FloatParseStyle, _numberFormatInfo, out value);
        public bool TryReadDouble(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out double value) => TryReadDouble(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadDouble(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out double value) => TryReadDouble(ref tokens, FloatParseStyle, provider, out value);
        public bool TryReadDouble(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out double value)
        {
            value = default;
            return tokens.MoveNext() && double.TryParse(tokens.Current, style, provider, out value);
        }

        public bool TryReadDecimal(ref ReadOnlySpanTokenizer<char> tokens, out decimal value) => TryReadDecimal(ref tokens, FloatParseStyle, _numberFormatInfo, out value);
        public bool TryReadDecimal(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, out decimal value) => TryReadDecimal(ref tokens, style, _numberFormatInfo, out value);
        public bool TryReadDecimal(ref ReadOnlySpanTokenizer<char> tokens, IFormatProvider? provider, out decimal value) => TryReadDecimal(ref tokens, FloatParseStyle, provider, out value);
        public bool TryReadDecimal(ref ReadOnlySpanTokenizer<char> tokens, NumberStyles style, IFormatProvider? provider, out decimal value)
        {
            value = default;
            return tokens.MoveNext() && decimal.TryParse(tokens.Current, style, provider, out value);
        }
    }
}
