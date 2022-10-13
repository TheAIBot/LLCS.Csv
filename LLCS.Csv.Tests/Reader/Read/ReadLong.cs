using CommunityToolkit.HighPerformance.Enumerables;
using LLCS.Csv.Reader;
using LLCS.Csv.Tests.CultureAndStyles;
using LLCS.Csv.Writer;
using System.IO;
using System.Linq;
using Xunit;

namespace LLCS.Csv.Tests.Reader.Read;

public sealed class ReadLong
{
    private sealed class LongStorage : ICsvSerializer
    {
        public long Value;

        public void Serialize(CsvWriter writer) => writer.Write(Value);

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens)
        {
            Value = reader.ReadLong(ref tokens);
            return true;
        }
    }

    private sealed class LongStorageNumberStyleAndCulture<T> : ICsvSerializer where T : INumberStyleAndCulture, new()
    {
        public long Value;

        public void Serialize(CsvWriter writer) => writer.Write(Value);

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens)
        {
            T value = new T();
            Value = reader.ReadLong(ref tokens, value.NumberStyle, value.CultureInfo);
            return true;
        }
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(1L)]
    [InlineData(23L)]
    [InlineData(-1L)]
    [InlineData(-23L)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    public void ReadRecords_WithNumber_ExpectNumberParsed(long expectedNumber)
    {
        string csv = @$"{expectedNumber}";
        var csvReader = CsvReader<LongStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }

    [Theory]
    [InlineData("0\n0", 0L, 0L)]
    [InlineData("0\n0\n0\n0\n0", 0L, 0L, 0L, 0L, 0L)]
    [InlineData("0\n1", 0L, 1L)]
    [InlineData("0\n1\n2\n3\n4", 0L, 1L, 2L, 3L, 4L)]
    [InlineData("-2\n1", -2L, 1L)]
    [InlineData("-2\n100\n92\n3\n44\n-70", -2L, 100L, 92L, 3L, 44L, -70L)]
    public void ReadRecords_WithMultipleRecordsWithANumber_ExpectNumbersParsed(string csv, params long[] expectedNumbers)
    {
        var csvReader = CsvReader<LongStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers, actualValues);
    }

    [Theory]
    [InlineData("0\r\n1", 0L, 1L)]
    [InlineData("0\r\n1\r\n2\r\n3\r\n4", 0L, 1L, 2L, 3L, 4L)]
    public void ReadRecords_WithMultipleRecordsWithANumberCarriageReturnNewline_ExpectNumbersParsed(string csv, params long[] expectedNumbers)
    {
        var csvReader = CsvReader<LongStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers, actualValues);
    }

    [Theory]
    [InlineData("\n")]
    [InlineData("0\n", 0L)]
    [InlineData("0\n1\n", 0L, 1L)]
    [InlineData("0\n1\n2\n3\n4\n", 0L, 1L, 2L, 3L, 4L)]
    public void ReadRecords_WithMultipleRecordsWithANumberEndsWithNewline_ExpectNumbersParsed(string csv, params long[] expectedNumbers)
    {
        var csvReader = CsvReader<LongStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers, actualValues);
    }

    [Theory]
    [InlineData("\r\n")]
    [InlineData("0\r\n", 0L)]
    [InlineData("0\r\n1\r\n", 0L, 1L)]
    [InlineData("0\r\n1\r\n2\r\n3\r\n4\r\n", 0L, 1L, 2L, 3L, 4L)]
    public void ReadRecords_WithMultipleRecordsWithANumberEndsWithCarriageReturnNewline_ExpectNumbersParsed(string csv, params long[] expectedNumbers)
    {
        var csvReader = CsvReader<LongStorage>.FromString(csv);

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
        var csvReader = CsvReader<LongStorageNumberStyleAndCulture<NoNumberStyleAndInvariantCulture>>.FromString(invalidCsv);

        Assert.Throws<InvalidDataException>(() => csvReader.ReadRecords().ToArray());
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(1L)]
    public void ReadRecords_WithNoNumberStyleAndValidCsv_ExpectSingleRecordReturned(long expectedNumber)
    {
        string csv = @$"{expectedNumber}";
        var csvReader = CsvReader<LongStorageNumberStyleAndCulture<NoNumberStyleAndInvariantCulture>>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }

    [Theory]
    [InlineData("1,000", 1_000L)]
    [InlineData("10,000", 10_000L)]
    public void ReadRecords_WithInvariantCultureThousandSeparator_ExpectSingleRecordReturned(string csv, long expectedNumber)
    {
        var csvReader = CsvReader<LongStorageNumberStyleAndCulture<AllowThousandSeparatorAndInvariantCulture>>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }

    [Theory]
    [InlineData("1 000", 1_000L)]
    [InlineData("10 000", 10_000L)]
    public void ReadRecords_WithFrenchCultureThousandSeparator_ExpectSingleRecordReturned(string csv, long expectedNumber)
    {
        var csvReader = CsvReader<LongStorageNumberStyleAndCulture<AllowThousandSeparatorAndFrenchCulture>>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }
}
