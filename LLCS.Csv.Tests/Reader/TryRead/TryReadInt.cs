using CommunityToolkit.HighPerformance.Enumerables;
using LLCS.Csv.Reader;
using LLCS.Csv.Writer;
using System.Globalization;
using Xunit;

namespace LLCS.Csv.Tests.Reader.TryRead;

internal sealed class NoNumberStyleAndInvariantCulture : INumberStyleAndCulture
{
    public NumberStyles NumberStyle => NumberStyles.None;
    public CultureInfo? CultureInfo => CultureInfo.InvariantCulture;
}

internal sealed class AllowThousandSeparatorAndInvariantCulture : INumberStyleAndCulture
{
    public NumberStyles NumberStyle => NumberStyles.AllowThousands;
    public CultureInfo? CultureInfo => CultureInfo.InvariantCulture;
}

internal sealed class AllowThousandSeparatorAndFrenchCulture : INumberStyleAndCulture
{
    public NumberStyles NumberStyle => NumberStyles.AllowThousands;
    public CultureInfo? CultureInfo => CultureInfo.GetCultureInfo("fr-FR");
}

internal interface INumberStyleAndCulture
{
    NumberStyles NumberStyle { get; }
    CultureInfo? CultureInfo { get; }
}

public sealed class TryReadInt
{
    private sealed record IntStorage : ICsvSerializer
    {
        public int Value;

        public void Serialize(CsvWriter writer) => writer.Write(Value);

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens) => reader.TryReadInt(ref tokens, out Value);
    }

    private sealed record IntStorageNumberStyleAndCulture<T> : ICsvSerializer where T : INumberStyleAndCulture, new()
    {
        public int Value;

        public void Serialize(CsvWriter writer) => writer.Write(Value);

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens)
        {
            T value = new T();
            return reader.TryReadInt(ref tokens, value.NumberStyle, value.CultureInfo, out Value);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(23)]
    [InlineData(-1)]
    [InlineData(-23)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void ReadRecords_WithNumber_ExpectNumberParsed(int expectedNumber)
    {
        string csv = @$"{expectedNumber}";
        var csvReader = CsvReader<IntStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }

    [Theory]
    [InlineData(" 1")]
    [InlineData("-1")]
    [InlineData("10.000")]
    public void ReadRecords_WithNoNumberStyleAndInvalidCsv_ExpectNoRecordsReturned(string invalidCsv)
    {
        var csvReader = CsvReader<IntStorageNumberStyleAndCulture<NoNumberStyleAndInvariantCulture>>.FromString(invalidCsv);

        var actualValues = csvReader.ReadRecords();

        Assert.Empty(actualValues);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void ReadRecords_WithNoNumberStyleAndValidCsv_ExpectSingleRecordReturned(int expectedNumber)
    {
        string csv = @$"{expectedNumber}";
        var csvReader = CsvReader<IntStorageNumberStyleAndCulture<NoNumberStyleAndInvariantCulture>>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }

    [Theory]
    [InlineData("1,000", 1_000)]
    [InlineData("10,000", 10_000)]
    [InlineData("1,000,000", 1_000_000)]
    public void ReadRecords_WithInvariantCultureThousandSeparator_ExpectSingleRecordReturned(string csv, int expectedNumber)
    {
        var csvReader = CsvReader<IntStorageNumberStyleAndCulture<AllowThousandSeparatorAndInvariantCulture>>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }

    [Theory]
    [InlineData("1 000", 1_000)]
    [InlineData("10 000", 10_000)]
    [InlineData("1 000 000", 1_000_000)]
    public void ReadRecords_WithFrenchCultureThousandSeparator_ExpectSingleRecordReturned(string csv, int expectedNumber)
    {
        var csvReader = CsvReader<IntStorageNumberStyleAndCulture<AllowThousandSeparatorAndFrenchCulture>>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }
}
