using BenchmarkDotNet.Attributes;
using LLCS.Csv.Reader;
using LLCS.Csv.Writer;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MyBenchmarks
{
    [MemoryDiagnoser]
    public class Csv10IntsRecordProfiler
    {
        private const string _fileName = @"E:\\Github\\LLCS.Csv\\LLCS.Csv.Profiler\\bin\\Release\\net7.0csvFile.csv";
        [AllowNull]
        private StreamReader _csvStream;
        [AllowNull]
        private CsvReader<Csv10IntsRecordTryRead> _readerTryRead;
        [AllowNull]
        private CsvReader<Csv10IntsRecordRead> _readerRead;

        [GlobalSetup]
        public void Setup()
        {
            if (File.Exists(_fileName))
            {
                return;
            }
            using var csvWriter = CsvWriter<Csv10IntsRecordTryRead>.ToStream(new StreamWriter(_fileName), CultureInfo.InvariantCulture);
            for (int i = 0; i < 1_000_000_000; i++)
            {
                csvWriter.WriteRecord(new Csv10IntsRecordTryRead(i));
            }
        }

        [IterationSetup(Targets = new[] { "TryReadRecords", "TryReadRecordsAsync" })]
        public void CreateCsvReaderTryRead()
        {
            _csvStream = new StreamReader(_fileName);
            _readerTryRead = CsvReader<Csv10IntsRecordTryRead>.FromStream(_csvStream, CultureInfo.InvariantCulture);
        }

        [Benchmark]
        public int TryReadRecords()
        {
            int count = 0;
            foreach (var record in _readerTryRead.ReadRecords())
            {
                count++;
            }

            return count;
        }

        [Benchmark]
        public async Task<int> TryReadRecordsAsync()
        {
            int count = 0;
            await foreach (var record in _readerTryRead)
            {
                count++;
            }

            return count;
        }

        [IterationCleanup(Targets = new[] { "TryReadRecords", "TryReadRecordsAsync" })]
        public void DisposeCsvReaderTryRead()
        {
            _readerTryRead.Dispose();
        }

        [IterationSetup(Targets = new[] { "ReadRecords", "ReadRecordsAsync" })]
        public void CreateCsvReaderRead()
        {
            _csvStream = new StreamReader(_fileName);
            _readerRead = CsvReader<Csv10IntsRecordRead>.FromStream(_csvStream, CultureInfo.InvariantCulture);
        }

        [Benchmark]
        public int ReadRecords()
        {
            int count = 0;
            foreach (var record in _readerRead.ReadRecords())
            {
                count++;
            }

            return count;
        }

        [Benchmark]
        public async Task<int> ReadRecordsAsync()
        {
            int count = 0;
            await foreach (var record in _readerRead)
            {
                count++;
            }

            return count;
        }

        [IterationCleanup(Targets = new[] { "ReadRecords", "ReadRecordsAsync" })]
        public void DisposeCsvReaderRead()
        {
            _readerRead.Dispose();
        }
    }
}