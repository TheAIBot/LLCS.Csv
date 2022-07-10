using BenchmarkDotNet.Attributes;
using LLCS.Csv.Reader;
using LLCS.Csv.Writer;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace MyBenchmarks
{
    [MemoryDiagnoser]
    public class Csv10IntsRecordProfiler
    {
        private readonly Stream _csvStream;
        [AllowNull]
        private CsvReader<Csv10IntsRecord> _reader;

        public Csv10IntsRecordProfiler()
        {
            _csvStream = new MemoryStream();
            using var csvWriter = CsvWriter<Csv10IntsRecord>.ToStream(new StreamWriter(_csvStream), CultureInfo.InvariantCulture);
            for (int i = 0; i < 1_000_000; i++)
            {
                csvWriter.WriteRecord(new Csv10IntsRecord(i));
            }
        }

        [IterationSetup]
        public void CreateCsvReader()
        {
            _csvStream.Seek(0, SeekOrigin.Begin);
            _reader = CsvReader<Csv10IntsRecord>.FromStream(new StreamReader(_csvStream, leaveOpen: true), CultureInfo.InvariantCulture);
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

        [IterationCleanup]
        public void DisposeCsvReader()
        {
            _reader.Dispose();
        }
    }
}