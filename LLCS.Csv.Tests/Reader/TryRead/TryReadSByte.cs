﻿using CommunityToolkit.HighPerformance.Enumerables;
using LLCS.Csv.Reader;
using LLCS.Csv.Tests.CultureAndStyles;
using LLCS.Csv.Writer;
using System.Linq;
using Xunit;

namespace LLCS.Csv.Tests.Reader.TryRead;

public sealed class TryReadSByte
{
    private sealed class SByteStorage : ICsvSerializer
    {
        public sbyte Value;

        public void Serialize(CsvWriter writer) => writer.Write(Value);

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens) => reader.TryReadSByte(ref tokens, out Value);
    }

    private sealed class SByteStorageNumberStyleAndCulture<T> : ICsvSerializer where T : INumberStyleAndCulture, new()
    {
        public sbyte Value;

        public void Serialize(CsvWriter writer) => writer.Write(Value);

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens)
        {
            T value = new T();
            return reader.TryReadSByte(ref tokens, value.NumberStyle, value.CultureInfo, out Value);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(23)]
    [InlineData(-1)]
    [InlineData(-23)]
    [InlineData(sbyte.MaxValue)]
    [InlineData(sbyte.MinValue)]
    public void ReadRecords_WithNumber_ExpectNumberParsed(int expectedNumber)
    {
        string csv = @$"{expectedNumber}";
        var csvReader = CsvReader<SByteStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }

    [Theory]
    [InlineData("0\n0", 0, 0)]
    [InlineData("0\n0\n0\n0\n0", 0, 0, 0, 0, 0)]
    [InlineData("0\n1", 0, 1)]
    [InlineData("0\n1\n2\n3\n4", 0, 1, 2, 3, 4)]
    [InlineData("-2\n1", -2, 1)]
    [InlineData("-2\n100\n92\n3\n44\n-70", -2, 100, 92, 3, 44, -70)]
    public void ReadRecords_WithMultipleRecordsWithANumber_ExpectNumbersParsed(string csv, params int[] expectedNumbers)
    {
        var csvReader = CsvReader<SByteStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers.Select(x => (sbyte)x), actualValues);
    }

    [Theory]
    [InlineData("0\r\n1", 0, 1)]
    [InlineData("0\r\n1\r\n2\r\n3\r\n4", 0, 1, 2, 3, 4)]
    public void ReadRecords_WithMultipleRecordsWithANumberCarriageReturnNewline_ExpectNumbersParsed(string csv, params int[] expectedNumbers)
    {
        var csvReader = CsvReader<SByteStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers.Select(x => (sbyte)x), actualValues);
    }

    [Theory]
    [InlineData("\n")]
    [InlineData("0\n", 0)]
    [InlineData("0\n1\n", 0, 1)]
    [InlineData("0\n1\n2\n3\n4\n", 0, 1, 2, 3, 4)]
    public void ReadRecords_WithMultipleRecordsWithANumberEndsWithNewline_ExpectNumbersParsed(string csv, params int[] expectedNumbers)
    {
        var csvReader = CsvReader<SByteStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers.Select(x => (sbyte)x), actualValues);
    }

    [Theory]
    [InlineData("\r\n")]
    [InlineData("0\r\n", 0)]
    [InlineData("0\r\n1\r\n", 0, 1)]
    [InlineData("0\r\n1\r\n2\r\n3\r\n4\r\n", 0, 1, 2, 3, 4)]
    public void ReadRecords_WithMultipleRecordsWithANumberEndsWithCarriageReturnNewline_ExpectNumbersParsed(string csv, params int[] expectedNumbers)
    {
        var csvReader = CsvReader<SByteStorage>.FromString(csv);

        var actualValues = csvReader.ReadRecords().Select(x => x.Value).ToArray();

        Assert.Equal(expectedNumbers.Select(x => (sbyte)x), actualValues);
    }

    [Theory]
    [InlineData(" 1")]
    [InlineData("-1")]
    [InlineData("10.000")]
    [InlineData("10000000000000")]
    public void ReadRecords_WithNoNumberStyleAndInvalidCsv_ExpectNoRecordsReturned(string invalidCsv)
    {
        var csvReader = CsvReader<SByteStorageNumberStyleAndCulture<NoNumberStyleAndInvariantCulture>>.FromString(invalidCsv);

        var actualValues = csvReader.ReadRecords();

        Assert.Empty(actualValues);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void ReadRecords_WithNoNumberStyleAndValidCsv_ExpectSingleRecordReturned(sbyte expectedNumber)
    {
        string csv = @$"{expectedNumber}";
        var csvReader = CsvReader<SByteStorageNumberStyleAndCulture<NoNumberStyleAndInvariantCulture>>.FromString(csv);

        var actualValues = csvReader.ReadRecords();

        var singleValue = Assert.Single(actualValues);
        Assert.Equal(expectedNumber, singleValue.Value);
    }
}