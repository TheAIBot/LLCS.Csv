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