using LLCS.Csv.Reader;
using LLCS.Csv.Tests.GenericContainers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace LLCS.Csv.Tests.Reader
{
    public abstract class ReadNumber<T> where T : IFormattable
    {
        private static string CreateCsv(string cultureName, params int[] values)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);
            return string.Join(culture.TextInfo.ListSeparator, values.Select(x => x.ToString(culture)));
        }

        private static string CombineCsvRecords(params string[] records)
        {
            return string.Join(Environment.NewLine, records);
        }

        public static TheoryData<int, string> DifferentRecordNumbersWithDifferentCultures()
        {
            string[] cultures = new string[]
            {
                "da-DK",
                "en-US",
            };

            int[] records = new int[]
            {
                1,
                10,
                10_000
            };

            var data = new TheoryData<int, string>();
            foreach (var culture in cultures)
            {
                foreach (var recordsCount in records)
                {
                    data.Add(recordsCount, culture);
                }
            }

            return data;
        }

        [Theory]
        [MemberData(nameof(DifferentRecordNumbersWithDifferentCultures))]
        public void ReadWithSerializerOneNumberPerRecord(int records, string culture)
        {
            string[] individualRecords = GetNumbers(records).Select(x => CreateCsv(culture, x)).ToArray();
            string csv = CombineCsvRecords(individualRecords);
            OneValueStruct<T>[] expectedValues = GetNumbers(records).Select(x => new OneValueStruct<T>(CastHelper<T>.CastTo(x))).ToArray();
            using CsvReader<OneValueStruct<T>> reader = CsvReader<OneValueStruct<T>>.FromString(csv, culture);

            OneValueStruct<T>[] actualValues = reader.ToArray();

            Assert.Equal(expectedValues, actualValues);
        }

        [Theory]
        [MemberData(nameof(DifferentRecordNumbersWithDifferentCultures))]
        public void ReadWithSerializerTwoNumbersPerRecord(int records, string culture)
        {
            string[] individualRecords = GetNumbers(records).Select(x => CreateCsv(culture, x, x + 1)).ToArray();
            string csv = CombineCsvRecords(individualRecords);
            TwoValueStruct<T, T>[] expectedValues = GetNumbers(records).Select(x => new TwoValueStruct<T, T>(CastHelper<T>.CastTo(x), CastHelper<T>.CastTo(x + 1))).ToArray();
            using CsvReader<TwoValueStruct<T, T>> reader = CsvReader<TwoValueStruct<T, T>>.FromString(csv, culture);

            TwoValueStruct<T, T>[] actualValues = reader.ToArray();

            Assert.Equal(expectedValues, actualValues);
        }

        private static IEnumerable<int> GetNumbers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return i % 100;
            }
        }
    }
}