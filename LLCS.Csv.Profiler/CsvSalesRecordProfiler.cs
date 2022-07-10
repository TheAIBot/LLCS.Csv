using BenchmarkDotNet.Attributes;
using LLCS.Csv.Reader;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace MyBenchmarks
{
    [MemoryDiagnoser]
    public class CsvSalesRecordProfiler
    {
        private readonly Stream _csvStream;
        [AllowNull]
        private CsvReader<SaledRecord> _reader;

        public CsvSalesRecordProfiler()
        {
            _csvStream = new MemoryStream(File.ReadAllBytes("C:\\Users\\Andreas\\Desktop\\100000-Sales-Records\\100000 Sales Records.csv"));
        }

        [IterationSetup]
        public void CreateCsvReader()
        {
            _csvStream.Seek(0, SeekOrigin.Begin);
            _reader = CsvReader<SaledRecord>.FromStream(new StreamReader(_csvStream,leaveOpen: true ), CultureInfo.InvariantCulture);
        }

        [Benchmark]
        public int ReadRecords()
        {
            int count = 0;
            foreach (var record in _reader)
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

        [IterationCleanup]
        public void DisposeCsvReader()
        {
            _reader.Dispose();
        }
    }
}