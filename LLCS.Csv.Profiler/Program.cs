using BenchmarkDotNet.Attributes;
using Microsoft.Toolkit.HighPerformance.Enumerables;
using LLCS.Csv;
using LLCS.Csv.Reader;
using LLCS.Csv.Writer;

namespace MyBenchmarks
{
    public class CsvIntProfiler
    {
        private readonly string FilePath;
        private CsvReader<Csv10IntsRecord> _reader;

        public CsvIntProfiler()
        {
            FilePath = "./csv_ints_profile_file.csv";
            if (!File.Exists(FilePath))
            {
                File.WriteAllLines(FilePath, Enumerable.Range(0, 1_000_000_000).Select(x => string.Join(";", Enumerable.Range(x, 10))));
            }
        }

        [IterationSetup]
        public void CreateCsvReader()
        {
            _reader = CsvReader<Csv10IntsRecord>.FromFile(FilePath);
        }

        [Benchmark]
        public int ReadRecords()
        {
            int count = 0;
            foreach (var record in _reader)
            {
                count++;
            }
            Console.WriteLine(count);

            return count;
        }

        [IterationCleanup]
        public void DisposeCsvReader()
        {
            _reader.Dispose();
        }
    }

    //DOTNET_ReadyToRun=0,DOTNET_TC_QuickJitForLoops=1,DOTNET_TieredPGO=1
    public class Program
    {
        public static void Main(string[] args)
        {
            var FilePath = @"Q:\Github\LLCS.Csv\LLCS.Csv.Profiler\bin\Release\net6.0\bf0b42a8-fe2f-4b8c-b313-0d258a9fb237\bin\Release\net6.0\csv_ints_profile_file.csv";
            using var _reader = CsvReader<Csv10IntsRecord>.FromFile(FilePath);


            int count = 0;
            using CancellationTokenSource cancelSource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                int previousCount = 0;
                using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
                while (!cancelSource.IsCancellationRequested && await timer.WaitForNextTickAsync(cancelSource.Token))
                {
                    int change = count - previousCount;
                    Console.WriteLine(change.ToString("N0"));

                    previousCount = count;
                }
            });




            foreach (var record in _reader)
            {
                count += 10;
            }
            cancelSource.Cancel();
            Console.WriteLine(count);

            //var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }

    internal struct Csv10IntsRecord : ICsvSerializer
    {
        public int Value1;
        public int Value2;
        public int Value3;
        public int Value4;
        public int Value5;
        public int Value6;
        public int Value7;
        public int Value8;
        public int Value9;
        public int Value10;

        public void Serialize(CsvWriter writer)
        {
            writer.Write(Value1);
            writer.Write(Value2);
            writer.Write(Value3);
            writer.Write(Value4);
            writer.Write(Value5);
            writer.Write(Value6);
            writer.Write(Value7);
            writer.Write(Value8);
            writer.Write(Value9);
            writer.Write(Value10);
        }

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens)
        {
            //reader.TryRead<string>(ref tokens, out string _);
            return reader.TryRead(ref tokens, out Value1) &&
                   reader.TryRead(ref tokens, out Value2) &&
                   reader.TryRead(ref tokens, out Value3) &&
                   reader.TryRead(ref tokens, out Value4) &&
                   reader.TryRead(ref tokens, out Value5) &&
                   reader.TryRead(ref tokens, out Value6) &&
                   reader.TryRead(ref tokens, out Value7) &&
                   reader.TryRead(ref tokens, out Value8) &&
                   reader.TryRead(ref tokens, out Value9) &&
                   reader.TryRead(ref tokens, out Value10);
        }
    }
}