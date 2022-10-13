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
        private CsvReader<SaledRecordTryRead> _readerTryRead;
        [AllowNull]
        private CsvReader<SaledRecordRead> _readerRead;
        [AllowNull]
        private CsvDataReader _sylvanReader;

        [IterationSetup]
        public void CreateCsvReader()
        {
            Stream _csvStreamTryRead = new MemoryStream(File.ReadAllBytes(".\\CsvFiles\\100000_Sales_Records.csv"));
            _readerTryRead = CsvReader<SaledRecordTryRead>.FromStream(new StreamReader(_csvStreamTryRead, leaveOpen: true), CultureInfo.InvariantCulture);

            Stream _csvStreamRead = new MemoryStream(File.ReadAllBytes(".\\CsvFiles\\100000_Sales_Records.csv"));
            _readerRead = CsvReader<SaledRecordRead>.FromStream(new StreamReader(_csvStreamRead, leaveOpen: true), CultureInfo.InvariantCulture);

            Stream _csvStream2 = new MemoryStream(File.ReadAllBytes(".\\CsvFiles\\100000_Sales_Records.csv"));
            _sylvanReader = CsvDataReader.Create(new StreamReader(_csvStream2, leaveOpen: true));
        }

        [Benchmark]
        public int ReadRecordsWithTryRead()
        {
            int count = 0;
            foreach (var record in _readerTryRead.ReadRecords())
            {
                count++;
            }

            return count;
        }

        [Benchmark]
        public async Task<int> ReadRecordsAsyncWithTryRead()
        {
            int count = 0;
            await foreach (var record in _readerTryRead)
            {
                count++;
            }

            return count;
        }

        [Benchmark]
        public int ReadRecordsWithRead()
        {
            int count = 0;
            foreach (var record in _readerRead.ReadRecords())
            {
                count++;
            }

            return count;
        }

        [Benchmark]
        public async Task<int> ReadRecordsAsyncWithRead()
        {
            int count = 0;
            await foreach (var record in _readerRead)
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
            _readerTryRead.Dispose();
            _readerRead.Dispose();
        }
    }
}