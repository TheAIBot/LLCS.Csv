using CommunityToolkit.HighPerformance.Enumerables;
using LLCS.Csv.Reader;
using LLCS.Csv.Tests.CultureAndStyles;
using LLCS.Csv.Writer;
using System.IO;
using System.Linq;
using Xunit;

namespace LLCS.Csv.Tests.Reader.Read;

public sealed class ReadByte
{
    private sealed class ByteStorage : ICsvSerializer
    {
        public byte Value;

        public void Serialize(CsvWriter writer) => writer.Write(Value);

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens)
        {
            Value = reader.ReadByte(ref tokens);
            return true;
        }
    }

    private sealed class ByteStorageNumberStyleAndCulture<T> : ICsvSerializer where T : INumberStyleAndCulture, new()
    {
        public byte Value;

        public void Serialize(CsvWriter writer) => writer.Write(Value);

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens)
        {
            T value = new T();
            Value = reader.ReadByte(ref tokens, value.NumberStyle, value.CultureInfo);
            return true;
        }
    }

    [Theory]
    [InlineData(0u)]
    [InlineData(1u)]
    [InlineData(23u)]
    [InlineData(byte.MaxValue)]
    public void ReadRecords_WithNumber_ExpectNumberParsed(uint expectedNumber)
    {
        string csv = @$"{expectedNumber}";
        var csvReader = CsvReader<ByteStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }

    [Theory]
    [InlineData("0\n0", 0u, 0u)]
    [InlineData("0\n0\n0\n0\n0", 0u, 0u, 0u, 0u, 0u)]
    [InlineData("0\n1", 0u, 1u)]
    [InlineData("0\n1\n2\n3\n4", 0u, 1u, 2u, 3u, 4u)]
    [InlineData("2\n1", 2u, 1u)]
    [InlineData("2\n100\n92\n3\n44\n70", 2u, 100u, 92u, 3u, 44u, 70u)]
    public void ReadRecords_WithMultipleRecordsWithANumber_ExpectNumbersParsed(string csv, params uint[] expectedNumbers)
    {
        var csvReader = CsvReader<ByteStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers.Select(x => (byte)x), actualValues);
    }

    [Theory]
    [InlineData("0\r\n1", 0u, 1u)]
    [InlineData("0\r\n1\r\n2\r\n3\r\n4", 0u, 1u, 2u, 3u, 4u)]
    public void ReadRecords_WithMultipleRecordsWithANumberCarriageReturnNewline_ExpectNumbersParsed(string csv, params uint[] expectedNumbers)
    {
        var csvReader = CsvReader<ByteStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers.Select(x => (byte)x), actualValues);
    }

    [Theory]
    [InlineData("\n")]
    [InlineData("0\n", 0u)]
    [InlineData("0\n1\n", 0u, 1u)]
    [InlineData("0\n1\n2\n3\n4\n", 0u, 1u, 2u, 3u, 4u)]
    public void ReadRecords_WithMultipleRecordsWithANumberEndsWithNewline_ExpectNumbersParsed(string csv, params uint[] expectedNumbers)
    {
        var csvReader = CsvReader<ByteStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers.Select(x => (byte)x), actualValues);
    }

    [Theory]
    [InlineData("\r\n")]
    [InlineData("0\r\n", 0u)]
    [InlineData("0\r\n1\r\n", 0u, 1u)]
    [InlineData("0\r\n1\r\n2\r\n3\r\n4\r\n", 0u, 1u, 2u, 3u, 4u)]
    public void ReadRecords_WithMultipleRecordsWithANumberEndsWithCarriageReturnNewline_ExpectNumbersParsed(string csv, params uint[] expectedNumbers)
    {
        var csvReader = CsvReader<ByteStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers.Select(x => (byte)x), actualValues);
    }

    [Theory]
    [InlineData(" 1")]
    [InlineData("-1")]
    [InlineData("10.000")]
    [InlineData("10000000000000")]
    public void ReadRecords_WithNoNumberStyleAndInvalidCsv_ExpectNoRecordsReturned(string invalidCsv)
    {
        var csvReader = CsvReader<ByteStorageNumberStyleAndCulture<NoNumberStyleAndInvariantCulture>>.FromString(invalidCsv);

        Assert.Throws<InvalidDataException>(() => csvReader.ReadRecords().ToArray());
    }

    [Theory]
    [InlineData(0u)]
    [InlineData(1u)]
    public void ReadRecords_WithNoNumberStyleAndValidCsv_ExpectSingleRecordReturned(uint expectedNumber)
    {
        string csv = @$"{expectedNumber}";
        var csvReader = CsvReader<ByteStorageNumberStyleAndCulture<NoNumberStyleAndInvariantCulture>>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }
}