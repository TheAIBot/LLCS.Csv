using BenchmarkDotNet.Attributes;
using LLCS.Csv.Reader;
using Sylvan.Data;
using Sylvan.Data.Csv;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MyBenchmarks
{
    [MemoryDiagnoser, LongRunJob, InProcess]
    public class CsvSalesRecordProfiler
    {
        [AllowNull]
        private CsvReader<SaledRecord> _reader;
        [AllowNull]
        private CsvDataReader _sylvanReader;

        [IterationSetup]
        public void CreateCsvReader()
        {
            Stream _csvStream = new MemoryStream(File.ReadAllBytes(".\\CsvFiles\\100000_Sales_Records.csv"));
            _reader = CsvReader<SaledRecord>.FromStream(new StreamReader(_csvStream, leaveOpen: true), CultureInfo.InvariantCulture);

            Stream _csvStream2 = new MemoryStream(File.ReadAllBytes(".\\CsvFiles\\100000_Sales_Records.csv"));
            _sylvanReader = CsvDataReader.Create(new StreamReader(_csvStream2, leaveOpen: true));
        }

        [Benchmark]
        public int ReadRecords()
        {
            int count = 0;
            foreach (var record in _reader.ReadRecords())
            {
                count++;
            }

            return count;
        }

        [Benchmark]
        public async Task<int> ReadRecordsAsync()
        {
            int count = 0;
            await foreach (var record in _reader)
            {
                count++;
            }

            return count;
        }

        [Benchmark]
        public int SylvanReadRecords()
        {
            int count = 0;
            foreach (var record in _sylvanReader.GetRecords<SaledRecord2>())
            {
                count++;
            }

            return count;
        }

        [IterationCleanup]
        public void DisposeCsvReader()
        {
            _reader.Dispose();
        }
    }
}