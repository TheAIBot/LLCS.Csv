using CommunityToolkit.HighPerformance.Enumerables;
using System.Globalization;

namespace LLCS.Csv.Reader
{
    public sealed partial class CsvReader
    {
        public bool TryReadDateTime(ref ReadOnlySpanTokenizer<char> tokens, out DateTime value) => TryReadDateTime(ref tokens, _dateTimeFormatInfo, DateParseStyle, out value);
        public bool TryReadDateTime(ref ReadOnlySpanTokenizer<char> tokens, DateTimeStyles styles, out DateTime value) => TryReadDateTime(ref tokens, _dateTimeFormatInfo, styles, out value);
        public bool TryReadDateTime(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, out DateTime value) => TryReadDateTime(ref tokens, provider, DateParseStyle, out value);
        public bool TryReadDateTime(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, DateTimeStyles styles, out DateTime value)
        {
            value = default;
            return tokens.MoveNext() && DateTime.TryParse(tokens.Current, provider, styles, out value);
        }

        public bool TryReadDateTimeOffset(ref ReadOnlySpanTokenizer<char> tokens, out DateTimeOffset value) => TryReadDateTimeOffset(ref tokens, _dateTimeFormatInfo, DateParseStyle, out value);
        public bool TryReadDateTimeOffset(ref ReadOnlySpanTokenizer<char> tokens, DateTimeStyles styles, out DateTimeOffset value) => TryReadDateTimeOffset(ref tokens, _dateTimeFormatInfo, styles, out value);
        public bool TryReadDateTimeOffset(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, out DateTimeOffset value) => TryReadDateTimeOffset(ref tokens, provider, DateParseStyle, out value);
        public bool TryReadDateTimeOffset(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, DateTimeStyles styles, out DateTimeOffset value)
        {
            value = default;
            return tokens.MoveNext() && DateTimeOffset.TryParse(tokens.Current, provider, styles, out value);
        }

        public bool TryReadDateOnly(ref ReadOnlySpanTokenizer<char> tokens, out DateOnly value) => TryReadDateOnly(ref tokens, _dateTimeFormatInfo, DateParseStyle, out value);
        public bool TryReadDateOnly(ref ReadOnlySpanTokenizer<char> tokens, DateTimeStyles styles, out DateOnly value) => TryReadDateOnly(ref tokens, _dateTimeFormatInfo, styles, out value);
        public bool TryReadDateOnly(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, out DateOnly value) => TryReadDateOnly(ref tokens, provider, DateParseStyle, out value);
        public bool TryReadDateOnly(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, DateTimeStyles styles, out DateOnly value)
        {
            value = default;
            return tokens.MoveNext() && DateOnly.TryParse(tokens.Current, provider, styles, out value);
        }

        public bool TryReadTimeOnly(ref ReadOnlySpanTokenizer<char> tokens, out TimeOnly value) => TryReadTimeOnly(ref tokens, _dateTimeFormatInfo, DateParseStyle, out value);
        public bool TryReadTimeOnly(ref ReadOnlySpanTokenizer<char> tokens, DateTimeStyles styles, out TimeOnly value) => TryReadTimeOnly(ref tokens, _dateTimeFormatInfo, styles, out value);
        public bool TryReadTimeOnly(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, out TimeOnly value) => TryReadTimeOnly(ref tokens, provider, DateParseStyle, out value);
        public bool TryReadTimeOnly(ref ReadOnlySpanTokenizer<char> tokens, DateTimeFormatInfo? provider, DateTimeStyles styles, out TimeOnly value)
        {
            value = default;
            return tokens.MoveNext() && TimeOnly.TryParse(tokens.Current, provider, styles, out value);
        }
    }
}
