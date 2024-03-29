﻿using CommunityToolkit.HighPerformance.Enumerables;

namespace LLCS.Csv.Reader
{
    public sealed partial class CsvReader
    {
        public string ReadString(ref ReadOnlySpanTokenizer<char> tokens)
        {
            if (!TryReadString(ref tokens, out string? value))
            {
                ThrowParseException(tokens.Current, "string");
            }

            return value;
        }

        public ReadOnlySpan<char> ReadSpan(ref ReadOnlySpanTokenizer<char> tokens)
        {
            if (!TryReadSpan(ref tokens, out ReadOnlySpan<char> value))
            {
                ThrowParseException(tokens.Current, "ReadOnlySpan<char>");
            }

            return value;
        }
    }
}
