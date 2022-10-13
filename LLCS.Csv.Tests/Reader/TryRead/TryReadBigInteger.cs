using CommunityToolkit.HighPerformance.Enumerables;
using LLCS.Csv.Reader;
using LLCS.Csv.Tests.Reader.TryRead.CultureAndStyles;
using LLCS.Csv.Writer;
using System.Linq;
using System.Numerics;
using Xunit;

namespace LLCS.Csv.Tests.Reader.TryRead;

public sealed class TryReadBigInteger
{
    private sealed class BigIntegerStorage : ICsvSerializer
    {
        public BigInteger Value;

        public void Serialize(CsvWriter writer) => writer.Write(Value);

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens) => reader.TryReadBigInteger(ref tokens, out Value);
    }

    private sealed class BigIntegerStorageNumberStyleAndCulture<T> : ICsvSerializer where T : INumberStyleAndCulture, new()
    {
        public BigInteger Value;

        public void Serialize(CsvWriter writer) => writer.Write(Value);

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens)
        {
            T value = new T();
            return reader.TryReadBigInteger(ref tokens, value.NumberStyle, value.CultureInfo, out Value);
        }
    }

    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("23")]
    [InlineData("892323948723984239487234982374923847329847239482754923675801925764598756497856195874693")]
    [InlineData("-1")]
    [InlineData("-23")]
    [InlineData("-892323948723984239487234982374923847329847239482754923675801925764598756497856195874693")]
    public void ReadRecords_WithNumber_ExpectNumberParsed(string expectedNumberAsString)
    {
        BigInteger expectedNumber = BigInteger.Parse(expectedNumberAsString);
        string csv = @$"{expectedNumberAsString}";
        var csvReader = CsvReader<BigIntegerStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }

    [Theory]
    [InlineData("0\n0", "0", "0")]
    [InlineData("0\n0\n0\n0\n0", "0", "0", "0", "0", "0")]
    [InlineData("0\n1", "0", "1")]
    [InlineData("0\n1\n2\n3\n4", "0", "1", "2", "3", "4")]
    [InlineData("-2\n1", "-2", "1")]
    [InlineData("-2\n100\n92\n3\n44\n-70", "-2", "100", "92", "3", "44", "-70")]
    public void ReadRecords_WithMultipleRecordsWithANumber_ExpectNumbersParsed(string csv, params string[] expectedNumbersAsString)
    {
        BigInteger[] expectedNumbers = expectedNumbersAsString.Select(BigInteger.Parse).ToArray();
        var csvReader = CsvReader<BigIntegerStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers, actualValues);
    }

    [Theory]
    [InlineData("0\r\n1", "0", "1")]
    [InlineData("0\r\n1\r\n2\r\n3\r\n4", "0", "1", "2", "3", "4")]
    public void ReadRecords_WithMultipleRecordsWithANumberCarriageReturnNewline_ExpectNumbersParsed(string csv, params string[] expectedNumbersAsString)
    {
        BigInteger[] expectedNumbers = expectedNumbersAsString.Select(BigInteger.Parse).ToArray();
        var csvReader = CsvReader<BigIntegerStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers, actualValues);
    }

    [Theory]
    [InlineData("\n")]
    [InlineData("0\n", "0")]
    [InlineData("0\n1\n", "0", "1")]
    [InlineData("0\n1\n2\n3\n4\n", "0", "1", "2", "3", "4")]
    public void ReadRecords_WithMultipleRecordsWithANumberEndsWithNewline_ExpectNumbersParsed(string csv, params string[] expectedNumbersAsString)
    {
        BigInteger[] expectedNumbers = expectedNumbersAsString.Select(BigInteger.Parse).ToArray();
        var csvReader = CsvReader<BigIntegerStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers, actualValues);
    }

    [Theory]
    [InlineData("\r\n")]
    [InlineData("0\r\n", "0")]
    [InlineData("0\r\n1\r\n", "0", "1")]
    [InlineData("0\r\n1\r\n2\r\n3\r\n4\r\n", "0", "1", "2", "3", "4")]
    public void ReadRecords_WithMultipleRecordsWithANumberEndsWithCarriageReturnNewline_ExpectNumbersParsed(string csv, params string[] expectedNumbersAsString)
    {
        BigInteger[] expectedNumbers = expectedNumbersAsString.Select(BigInteger.Parse).ToArray();
        var csvReader = CsvReader<BigIntegerStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers, actualValues);
    }

    [Theory]
    [InlineData(" 1")]
    [InlineData("-1")]
    [InlineData("10.000")]
    public void ReadRecords_WithNoNumberStyleAndInvalidCsv_ExpectNoRecordsReturned(string invalidCsv)
    {
        var csvReader = CsvReader<BigIntegerStorageNumberStyleAndCulture<NoNumberStyleAndInvariantCulture>>.FromString(invalidCsv);

        var actualValues = csvReader.ReadRecords();

        Assert.Empty(actualValues);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    public void ReadRecords_WithNoNumberStyleAndValidCsv_ExpectSingleRecordReturned(string csv)
    {
        BigInteger expectedNumber = BigInteger.Parse(csv);
        var csvReader = CsvReader<BigIntegerStorageNumberStyleAndCulture<NoNumberStyleAndInvariantCulture>>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }

    [Theory]
    [InlineData("1,000", "1000")]
    [InlineData("10,000", "10000")]
    public void ReadRecords_WithInvariantCultureThousandSeparator_ExpectSingleRecordReturned(string csv, string expectedNumberAsString)
    {
        BigInteger expectedNumber = BigInteger.Parse(expectedNumberAsString);
        var csvReader = CsvReader<BigIntegerStorageNumberStyleAndCulture<AllowThousandSeparatorAndInvariantCulture>>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }

    [Theory]
    [InlineData("1 000", "1000")]
    [InlineData("10 000", "10000")]
    public void ReadRecords_WithFrenchCultureThousandSeparator_ExpectSingleRecordReturned(string csv, string expectedNumberAsString)
    {
        BigInteger expectedNumber = BigInteger.Parse(expectedNumberAsString);
        var csvReader = CsvReader<BigIntegerStorageNumberStyleAndCulture<AllowThousandSeparatorAndFrenchCulture>>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }
}