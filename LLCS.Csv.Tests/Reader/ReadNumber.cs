using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;

namespace LLCS.Csv.Tests.Reader
{

    public abstract class ReadNumber<T> where T : IFormattable
    {
        public static IEnumerable<object[]> SingleNumber()
        {
            object[] TestCase<U>(U value)
            {
                ArgumentNullException.ThrowIfNull(value);
                T castValue = CastHelper<T>.CastTo(value);
                return new object[] { castValue.ToString(), castValue };
            }

            yield return TestCase(0);
            yield return TestCase(1);
            yield return TestCase(Numeric<T>.MinValue);
            yield return TestCase(Numeric<T>.MaxValue);
        }

        //[Theory]
        //[MemberData(nameof(SingleNumber))]
        //public void ParseSingleNumber(string csv, T expectedNumber)
        //{
        //    CsvReader reader = CsvReader.FromString(csv);

        //    T actualNumber;
        //    bool couldRead = reader.TryRead(true, out actualNumber);

        //    Assert.True(couldRead);
        //    Assert.Equal(expectedNumber, actualNumber);
        //}

        //public static IEnumerable<object[]> MultipleNumbers()
        //{
        //    string[] cultures = new string[] { "da-DK", "en-US", "fr-FR" };
        //    foreach (var culture in cultures)
        //    {
        //        yield return CreateRecord(culture, 1, 2);
        //        yield return CreateRecord(culture, 1, 2, 3);
        //        yield return CreateRecord(culture, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6);
        //    }
        //}

        //[Theory]
        //[MemberData(nameof(MultipleNumbers))]
        //public void ParseMultipleNumbers(string csv, string culture, T[] expectedNumbers)
        //{
        //    using CsvReader reader = CsvReader.FromString(csv, culture);

        //    T[] actualNumbers = new T[expectedNumbers.Length];
        //    bool[] couldReadNumber = new bool[actualNumbers.Length];
        //    for (int i = 0; i < actualNumbers.Length; i++)
        //    {
        //        bool lastCellInRecord = i + 1 == actualNumbers.Length;
        //        couldReadNumber[i] = reader.TryRead(lastCellInRecord, out actualNumbers[i]);
        //    }


        //    Assert.True(couldReadNumber.All(x => x));
        //    Assert.Equal(expectedNumbers, actualNumbers);
        //}

        private static object[] CreateRecord(string cultureName, params int[] intValues)
        {
            T[] values = intValues.Select(x => Clamp(x)).Select(x => CastHelper<T>.CastTo(x)).ToArray();
            CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);
            string csv = string.Join(culture.TextInfo.ListSeparator, values.Select(x => x.ToString("N0", culture)));

            return new object[] { csv, cultureName, values };
        }

        private static int Clamp(int value)
        {
            if (!Numeric<T>.IsSigned)
            {
                return 0;
            }

            return value;
        }

        private static string CreateCsv(string cultureName, params int[] values)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);
            return string.Join(culture.TextInfo.ListSeparator, values.Select(x => x.ToString(culture)));
        }

        private static string CombineCsvRecords(params string[] records)
        {
            return string.Join(Environment.NewLine, records);
        }

        [Fact]
        public void ReadWithSerializer1()
        {
            string culture = "da-DK";
            string csv = CombineCsvRecords(
                CreateCsv(culture, 1),
                CreateCsv(culture, 2),
                CreateCsv(culture, 3),
                CreateCsv(culture, 4),
                CreateCsv(culture, 5),
                CreateCsv(culture, 6));
            OneValueStruct<T>[] expectedValues = Enumerable.Range(1, 6).Select(x => new OneValueStruct<T>(CastHelper<T>.CastTo(x))).ToArray();
            using CsvReader<OneValueStruct<T>> reader = CsvReader<OneValueStruct<T>>.FromString(csv, culture);

            OneValueStruct<T>[] actualValues = reader.ToArray();

            Assert.Equal(expectedValues, actualValues);
        }

        [Fact]
        public void ReadWithSerializer2()
        {
            string culture = "da-DK";
            string csv = CombineCsvRecords(
                CreateCsv(culture, 1, 2),
                CreateCsv(culture, 2, 3),
                CreateCsv(culture, 3, 4),
                CreateCsv(culture, 4, 5),
                CreateCsv(culture, 5, 6),
                CreateCsv(culture, 6, 7));
            TwoValueStruct<T, T>[] expectedValues = Enumerable.Range(1, 6).Select(x => new TwoValueStruct<T, T>(CastHelper<T>.CastTo(x), CastHelper<T>.CastTo(x + 1))).ToArray();
            using CsvReader<TwoValueStruct<T, T>> reader = CsvReader<TwoValueStruct<T, T>>.FromString(csv, culture);

            TwoValueStruct<T, T>[] actualValues = reader.ToArray();

            Assert.Equal(expectedValues, actualValues);
        }
    }
}