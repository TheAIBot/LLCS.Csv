using CommunityToolkit.HighPerformance.Enumerables;
using LLCS.Csv.Reader;
using LLCS.Csv.Tests.Reader.TryRead.CultureAndStyles;
using LLCS.Csv.Writer;
using System.Linq;
using Xunit;

namespace LLCS.Csv.Tests.Reader.TryRead;

public sealed class TryReadULong
{
    private sealed class ULongStorage : ICsvSerializer
    {
        public ulong Value;

        public void Serialize(CsvWriter writer) => writer.Write(Value);

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens) => reader.TryReadULong(ref tokens, out Value);
    }

    private sealed class ULongStorageNumberStyleAndCulture<T> : ICsvSerializer where T : INumberStyleAndCulture, new()
    {
        public ulong Value;

        public void Serialize(CsvWriter writer) => writer.Write(Value);

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens)
        {
            T value = new T();
            return reader.TryReadULong(ref tokens, value.NumberStyle, value.CultureInfo, out Value);
        }
    }

    [Theory]
    [InlineData(0UL)]
    [InlineData(1UL)]
    [InlineData(23UL)]
    [InlineData(ulong.MaxValue)]
    public void ReadRecords_WithNumber_ExpectNumberParsed(ulong expectedNumber)
    {
        string csv = @$"{expectedNumber}";
        var csvReader = CsvReader<ULongStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }

    [Theory]
    [InlineData("0\n0", 0UL, 0UL)]
    [InlineData("0\n0\n0\n0\n0", 0UL, 0UL, 0UL, 0UL, 0UL)]
    [InlineData("0\n1", 0UL, 1UL)]
    [InlineData("0\n1\n2\n3\n4", 0UL, 1UL, 2UL, 3UL, 4UL)]
    [InlineData("2\n1", 2UL, 1UL)]
    [InlineData("2\n100\n92\n3\n44\n70", 2UL, 100UL, 92UL, 3UL, 44UL, 70UL)]
    public void ReadRecords_WithMultipleRecordsWithANumber_ExpectNumbersParsed(string csv, params ulong[] expectedNumbers)
    {
        var csvReader = CsvReader<ULongStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers, actualValues);
    }

    [Theory]
    [InlineData("0\r\n1", 0UL, 1UL)]
    [InlineData("0\r\n1\r\n2\r\n3\r\n4", 0UL, 1UL, 2UL, 3UL, 4UL)]
    public void ReadRecords_WithMultipleRecordsWithANumberCarriageReturnNewline_ExpectNumbersParsed(string csv, params ulong[] expectedNumbers)
    {
        var csvReader = CsvReader<ULongStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers, actualValues);
    }

    [Theory]
    [InlineData("\n")]
    [InlineData("0\n", 0UL)]
    [InlineData("0\n1\n", 0UL, 1UL)]
    [InlineData("0\n1\n2\n3\n4\n", 0UL, 1UL, 2UL, 3UL, 4UL)]
    public void ReadRecords_WithMultipleRecordsWithANumberEndsWithNewline_ExpectNumbersParsed(string csv, params ulong[] expectedNumbers)
    {
        var csvReader = CsvReader<ULongStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers, actualValues);
    }

    [Theory]
    [InlineData("\r\n")]
    [InlineData("0\r\n", 0UL)]
    [InlineData("0\r\n1\r\n", 0UL, 1UL)]
    [InlineData("0\r\n1\r\n2\r\n3\r\n4\r\n", 0UL, 1UL, 2UL, 3UL, 4UL)]
    public void ReadRecords_WithMultipleRecordsWithANumberEndsWithCarriageReturnNewline_ExpectNumbersParsed(string csv, params ulong[] expectedNumbers)
    {
        var csvReader = CsvReader<ULongStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers, actualValues);
    }

    [Theory]
    [InlineData(" 1")]
    [InlineData("-1")]
    [InlineData("10.000")]
    [InlineData("1000000000000000000000000000000")]
    public void ReadRecords_WithNoNumberStyleAndInvalidCsv_ExpectNoRecordsReturned(string invalidCsv)
    {
        var csvReader = CsvReader<ULongStorageNumberStyleAndCulture<NoNumberStyleAndInvariantCulture>>.FromString(invalidCsv);

        var actualValues = csvReader.ReadRecords();

        Assert.Empty(actualValues);
    }

    [Theory]
    [InlineData(0UL)]
    [InlineData(1UL)]
    public void ReadRecords_WithNoNumberStyleAndValidCsv_ExpectSingleRecordReturned(ulong expectedNumber)
    {
        string csv = @$"{expectedNumber}";
        var csvReader = CsvReader<ULongStorageNumberStyleAndCulture<NoNumberStyleAndInvariantCulture>>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }

    [Theory]
    [InlineData("1,000", 1_000UL)]
    [InlineData("10,000", 10_000UL)]
    public void ReadRecords_WithInvariantCultureThousandSeparator_ExpectSingleRecordReturned(string csv, ulong expectedNumber)
    {
        var csvReader = CsvReader<ULongStorageNumberStyleAndCulture<AllowThousandSeparatorAndInvariantCulture>>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }

    [Theory]
    [InlineData("1 000", 1_000UL)]
    [InlineData("10 000", 10_000UL)]
    public void ReadRecords_WithFrenchCultureThousandSeparator_ExpectSingleRecordReturned(string csv, ulong expectedNumber)
    {
        var csvReader = CsvReader<ULongStorageNumberStyleAndCulture<AllowThousandSeparatorAndFrenchCulture>>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }
}